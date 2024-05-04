using inmobiliaria.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace inmobiliaria.Models
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Propietario")]
    public class InquilinoController : Controller
    {
        private readonly RepositorioInquilino repositorioInquilino;

        public InquilinoController(RepositorioInquilino repositorioInquilino)
        {
            this.repositorioInquilino = repositorioInquilino;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Inquilino> inquilinos = repositorioInquilino.ObtenerActivos();
            if (inquilinos.Count == 0)
            {
                return NotFound();
            }
            return Ok(inquilinos);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Inquilino inquilino = repositorioInquilino.BuscarPorId(id);
            if (inquilino == null)
            {
                return NotFound();
            }
            return Ok(inquilino);
        }

        [HttpPost("guardar")]
        public IActionResult Post(Inquilino inquilino)
        {
            if (inquilino == null)
            {
                return BadRequest("El inquilino no puede ser nulo");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool guardado = repositorioInquilino.Crear(inquilino);
            if (guardado)
            {
                return CreatedAtAction(nameof(Get), new { id = inquilino.id }, inquilino);
            }
            else
            {
                return StatusCode(500, "Hubo un error al intentar crear el inquilino");
            }
        }

        [HttpPut("actualizar/{id}")]
        public IActionResult Put(int id, Inquilino inquilino)
        {
            if (id != inquilino.id)
            {
                return BadRequest("El ID proporcionado no coincide con el ID del inquilino");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState + ": El modelo no coincide con el esperado");
            }
            var inquilinoExistente = repositorioInquilino.BuscarPorId(id);
            if (inquilinoExistente == null)
            {
                return NotFound();
            }
            inquilinoExistente.nombre = inquilino.nombre ?? inquilinoExistente.nombre;
            inquilinoExistente.apellido = inquilino.apellido ?? inquilinoExistente.apellido;
            inquilinoExistente.dni = inquilino.dni ?? inquilinoExistente.dni;
            inquilinoExistente.email = inquilino.email ?? inquilinoExistente.email;
            inquilinoExistente.telefono = inquilino.telefono ?? inquilinoExistente.telefono;

            bool actualizado = repositorioInquilino.Actualizar(inquilinoExistente);
            if (actualizado)
            {
                return NoContent(); // esto indica que la actualización fue exitosa
            }
            else
            {
                return StatusCode(500, "Hubo un error al intentar actualizar el inquilino");
            }
        }

        [HttpDelete("borrar/{id}")]
        public ActionResult<Inmueble> Delete(int id)
        {
            var inmueble = repositorioInquilino.BuscarPorId(id);
            if (inmueble == null)
            {
                return NotFound();
            }

            var exito = repositorioInquilino.EliminadoLogico(id);
            if (!exito)
            {
                return StatusCode(500, "Error al eliminar");
            }

            return NoContent();
        }

    }
}