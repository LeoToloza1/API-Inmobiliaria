using inmobiliaria.Controllers;
using inmobiliaria.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace inmobiliaria.Models
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Propietario")]
    public class PagoController : Controller
    {
        private readonly RepositorioPago repositorioPago;
        public PagoController(RepositorioPago repo)
        {
            this.repositorioPago = repo;
        }
        private int GetPropietarioId()
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            var userIdClaim = User.FindFirst("id").Value;
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
            int userId = int.Parse(userIdClaim);
            return userId;
        }
        [HttpGet]
        public ActionResult<List<Pago>> Get()
        {
            var propietarioId = GetPropietarioId();
            if (propietarioId == -1)
            {
                return BadRequest("No se pudo obtener el ID del propietario.");
            }

            var pagos = repositorioPago.ObtenerTodos(propietarioId);
            if (pagos == null)
            {
                return NotFound();
            }
            return pagos;
        }

        [HttpGet("{id}")]
        public ActionResult<Pago> Get(int id)
        {
            var propietarioId = GetPropietarioId();
            if (propietarioId == -1)
            {
                return BadRequest("No se pudo obtener el ID del propietario.");
            }

            var pago = repositorioPago.BuscarPorId(id);
            if (pago == null || pago.Contrato?.inmueble?.PropietarioId != propietarioId)
            {
                return NotFound();
            }
            return pago;
        }

        [HttpPost("guardar")]
        public ActionResult<Pago> Post(Pago pago)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            repositorioPago.Crear(pago);
            return Ok(pago);
        }

        [HttpPut("actualizar/{id}")]
        public ActionResult<Pago> Put(int id, Pago pago)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState + ": El modelo no coincide con el esperado");
            }

            var pagoExistente = repositorioPago.BuscarPorId(id);
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            if (pagoExistente == null || pagoExistente.Contrato.inmueble.PropietarioId != GetPropietarioId())
            {
                return NotFound();
            }
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.

            if (pagoExistente.fecha_pago != new DateOnly())
            {
                return BadRequest("No se puede actualizar el pago, solo es posible unicamente el mismo día de pago");
            }

            pagoExistente.importe = pago.importe != 0 ? pago.importe : pagoExistente.importe;
            pagoExistente.estado = pago.estado;
            pagoExistente.detalle = pago.detalle ?? pagoExistente.detalle;

            repositorioPago.Actualizar(pagoExistente);
            return Ok(pagoExistente);
        }
    }
}