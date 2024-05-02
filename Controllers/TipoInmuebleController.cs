using inmobiliaria.Models;
using inmobiliaria.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria.Controllers
{
    [ApiController]
    [Route("[controller]")]
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