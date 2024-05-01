using inmobiliaria.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace inmobiliaria.Models
{
    [ApiController]
    [Route("[controller]")]
    public class PropietarioController : ControllerBase
    {
        private readonly RepositorioPropietario repositorioPropietario;

        public PropietarioController(RepositorioPropietario repo)
        {
            repositorioPropietario = repo;

        }
        [HttpGet]
        public ActionResult<List<Propietario>> Get()
        {
            var propietarios = repositorioPropietario.ObtenerActivos();
            if (propietarios == null)
            {
                return NotFound();
            }
            return propietarios;
        }

        [HttpGet("{id}")]
        public ActionResult<Propietario> Get(int id)
        {
            var propietario = repositorioPropietario.BuscarPorId(id);
            if (propietario == null)
            {
                return NotFound();
            }
            return propietario;
        }

        [HttpPost]
        public ActionResult<Propietario> Post(Propietario propietario)
        {
            bool exito = repositorioPropietario.Crear(propietario);
            if (!exito)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut("actualizar/{id}")]
        public ActionResult<Propietario> Put(int id, Propietario propietario)
        {
            if (id != propietario.id)
            {
                return BadRequest("El ID proporcionado no coincide con el ID del Propietario");
            }
            var propietarioExistente = repositorioPropietario.BuscarPorId(id);
            if (propietarioExistente == null)
            {
                return NotFound();
            }

            // Actualizar solo los campos que se envían
            propietarioExistente.nombre = propietario.nombre ?? propietarioExistente.nombre;
            propietarioExistente.apellido = propietario.apellido ?? propietarioExistente.apellido;
            propietarioExistente.email = propietario.email ?? propietarioExistente.email;
            propietarioExistente.dni = propietario.dni ?? propietarioExistente.dni;
            propietarioExistente.telefono = propietario.telefono ?? propietarioExistente.telefono;
            bool exito = repositorioPropietario.Actualizar(propietarioExistente);
            if (!exito)
            {
                return StatusCode(500, "Error al actualizar el propietario");
            }

            return Ok(propietarioExistente);
        }

        [HttpDelete("borrar/{id}")]
        public ActionResult<Propietario> Delete(int id)
        {
            var propietario = repositorioPropietario.BuscarPorId(id);
            if (propietario == null)
            {
                return NotFound();
            }
            var exito = repositorioPropietario.EliminadoLogico(id);
            if (!exito)
            {
                return StatusCode(500, "Error al eliminar");
            }

            return NoContent();
        }

    }
}