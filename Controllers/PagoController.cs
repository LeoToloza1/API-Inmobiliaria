using inmobiliaria.Controllers;
using inmobiliaria.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace inmobiliaria.Models
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Propietario")]
    public class PagoController : Controller, IControladorBase<Pago>
    {
        private readonly RepositorioPago repositorioPago;
        public PagoController(RepositorioPago repo)
        {
            this.repositorioPago = repo;
        }
        [HttpDelete("borrar/{id}")]
        public ActionResult<Pago> Delete(int id)
        {
            return NotFound("Los pagos no se pueden eliminar");
        }

        [HttpGet]
        public ActionResult<List<Pago>> Get()
        {
            var pagos = repositorioPago.ObtenerTodos();
            if (pagos == null)
            {
                return NotFound();
            }
            return pagos;
        }
        [HttpGet("{id}")]
        public ActionResult<Pago> Get(int id)
        {
            var pago = repositorioPago.BuscarPorId(id);
            if (pago == null)
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
            if (pagoExistente == null)
            {
                return NotFound();
            }

            if (pagoExistente.fecha_pago != DateOnly.FromDateTime(DateTime.Today))
            {
                return BadRequest("No se puede actualizar el pago porque la fecha de pago ya pasó.");
            }

            pagoExistente.importe = pago.importe != 0 ? pago.importe : pagoExistente.importe;
            pagoExistente.estado = pago.estado;
            pagoExistente.detalle = pago.detalle ?? pagoExistente.detalle;

            repositorioPago.Actualizar(pagoExistente);
            return Ok(pagoExistente);
        }


    }
}