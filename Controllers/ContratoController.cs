using inmobiliaria.Models;
using inmobiliaria.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContratoController : ControllerBase, IControladorBase<Contrato>
    {
        private readonly RepositorioContrato repositorioContrato;
        public ContratoController(RepositorioContrato repo)
        {
            this.repositorioContrato = repo;
        }
        [HttpDelete("borrar/{id}")]
        public ActionResult<Contrato> Delete(int id)
        {
            var contrato = repositorioContrato.BuscarPorId(id);
            if (contrato == null)
            {
                return NotFound();
            }

            var exito = repositorioContrato.EliminadoLogico(id);
            if (!exito)
            {
                return StatusCode(500, "Error al eliminar");
            }

            return NoContent();

        }
        [HttpGet]
        public ActionResult<List<Contrato>> Get()
        {
            var contratos = repositorioContrato.ObtenerActivos();
            if (contratos == null)
            {
                return NotFound();
            }
            return contratos;
        }
        [HttpGet("{id}")]
        public ActionResult<Contrato> Get(int id)
        {
            var contrato = repositorioContrato.BuscarPorId(id);
            if (contrato == null)
            {
                return NotFound();
            }
            return contrato;
        }
        [HttpPost("guardar")]
        public ActionResult<Contrato> Post(Contrato contrato)
        {
            //las fechas tienen que venir desde el formulario asi:
            //como si fuera un string
            //"fecha_inicio": "2024-05-01"
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            repositorioContrato.Crear(contrato);
            return Ok(contrato);
        }

        [HttpPut("actualizar/{id}")]
        public ActionResult<Contrato> Put(int id, Contrato contrato)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState + ": El modelo no coincide con el esperado");
            }

            var contratoExistente = repositorioContrato.BuscarPorId(id);
            if (contratoExistente == null)
            {
                return NotFound();
            }

            if (contratoExistente.fecha_inicio == DateOnly.FromDateTime(DateTime.Today) || contrato.fecha_inicio != default(DateOnly))
            {
                contratoExistente.fecha_inicio = contrato.fecha_inicio != default(DateOnly) ? contrato.fecha_inicio : contratoExistente.fecha_inicio;
                contratoExistente.fecha_efectiva = contrato.fecha_efectiva != default(DateOnly) ? contrato.fecha_efectiva : contratoExistente.fecha_efectiva;
                contratoExistente.monto = contrato.monto > 0 ? contrato.monto : contratoExistente.monto;

                repositorioContrato.Actualizar(contratoExistente);
                return Ok(contratoExistente);
            }
            else
            {
                return BadRequest("No se puede actualizar el contrato porque la fecha de inicio no es hoy.");
            }
        }


    }



}