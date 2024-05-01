using inmobiliaria.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace inmobiliaria.Models
{
    [ApiController]
    [Route("[controller]")]
    public class InmuebleController : ControllerBase
    {
        private readonly RepositorioInmueble repositorioInmueble;
        public InmuebleController(RepositorioInmueble repositorioInmueble)
        {
            this.repositorioInmueble = repositorioInmueble;
        }
        [HttpGet]
        public ActionResult<List<Inmueble>> Get()
        {
            var inmuebles = repositorioInmueble.ObtenerActivos();
            if (inmuebles == null)
            {
                return NotFound();
            }
            return inmuebles;
        }


        [HttpGet("{id}")]
        public ActionResult<Inmueble> Get(int id)
        {
            var inmueble = repositorioInmueble.BuscarPorId(id);
            if (inmueble == null)
            {
                return NotFound();
            }
            return inmueble;
        }

        [HttpPost("guardar")]
        public ActionResult<Inmueble> Post(Inmueble inmueble)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            repositorioInmueble.Crear(inmueble);
            return Ok(inmueble);
        }

        [HttpPut("actualizar/{id}")]
        public ActionResult<Inmueble> Put(int id, Inmueble inmueble)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState + ": El modelo no coincide con el esperado");
            }
            if (id != inmueble.id)
            {
                return BadRequest("El ID proporcionado no coincide con el ID del Inmueble");
            }
            var inmuebleExistente = repositorioInmueble.BuscarPorId(id);
            if (inmuebleExistente == null)
            {
                return NotFound();
            }
            //hago esto para actualizar unicamente un campo del inmueble, que me envien en el formulario
            inmuebleExistente.ambientes = inmueble.ambientes > 0 ? inmueble.ambientes : inmuebleExistente.ambientes;
            inmuebleExistente.tipoInmuebleid = inmueble.tipoInmuebleid > 0 ? inmueble.tipoInmuebleid : inmuebleExistente.tipoInmuebleid;
            inmuebleExistente.coordenadas = !string.IsNullOrWhiteSpace(inmueble.coordenadas) ? inmueble.coordenadas : inmuebleExistente.coordenadas;
            inmuebleExistente.precio = inmueble.precio > 0 ? inmueble.precio : inmuebleExistente.precio;
            inmuebleExistente.PropietarioId = inmueble.PropietarioId > 0 ? inmueble.PropietarioId : inmuebleExistente.PropietarioId;
            inmuebleExistente.estado = !string.IsNullOrWhiteSpace(inmueble.estado) ? inmueble.estado : inmuebleExistente.estado;
            inmuebleExistente.borrado = inmueble.borrado;
            inmuebleExistente.descripcion = !string.IsNullOrWhiteSpace(inmueble.descripcion) ? inmueble.descripcion : inmuebleExistente.descripcion;
            inmuebleExistente.uso = !string.IsNullOrWhiteSpace(inmueble.uso) ? inmueble.uso : inmuebleExistente.uso;
            repositorioInmueble.Actualizar(inmuebleExistente);
            return Ok(inmuebleExistente);
        }

        [HttpDelete("borrar/{id}")]
        public ActionResult<Inmueble> Delete(int id)
        {
            var inmueble = repositorioInmueble.BuscarPorId(id);
            if (inmueble == null)
            {
                return NotFound();
            }

            var exito = repositorioInmueble.EliminadoLogico(id);
            if (!exito)
            {
                return StatusCode(500, "Error al eliminar");
            }

            return NoContent();
        }

        [HttpGet("propietario/{id}")]
        public ActionResult<List<Inmueble>> GetPropietario(int id)
        {
            var inmuebles = repositorioInmueble.InmueblesDePropietario(id);
            if (inmuebles == null)
            {
                return NotFound();
            }
            return inmuebles;
        }

    }
}