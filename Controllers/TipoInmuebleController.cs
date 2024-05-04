using System.ComponentModel.DataAnnotations;
using inmobiliaria.Models;
using inmobiliaria.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Propietario")]
    public class TipoInmuebleController : Controller
    {
        private readonly RepositorioTipoInmuebles repositorioTipoInmuebles;

        public TipoInmuebleController(RepositorioTipoInmuebles repositorioTipoInmuebles)
        {
            this.repositorioTipoInmuebles = repositorioTipoInmuebles;
        }

        [HttpGet]

        public ActionResult<List<TipoInmueble>> Get()
        {
            Console.WriteLine("Acceso autorizado para Propietario.");
            return repositorioTipoInmuebles.ObtenerTodos();
        }
        [HttpPost("guardar")]

        public ActionResult<TipoInmueble> Post(TipoInmueble tipoInmueble)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            repositorioTipoInmuebles.Crear(tipoInmueble);
            return Ok(tipoInmueble);
        }

        [HttpDelete("borrar/{id}")]
        public ActionResult<TipoInmueble> Delete(int id)
        {
            var tipoInmueble = repositorioTipoInmuebles.ObtenerPorId(id);
            if (tipoInmueble == null)
            {
                return NotFound();
            }
            repositorioTipoInmuebles.Eliminar(id);
            return NoContent();

        }
    }

}