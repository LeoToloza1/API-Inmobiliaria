using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace inmobiliaria.Models
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly DataContext _contexto;

        public UsuarioController(DataContext contexto)
        {
            _contexto = contexto;
        }

        // [HttpGet("{id}")]
        // public IActionResult ObtenerUsuario(int id)
        // {
        //     var usuario = _contexto.Usuario.FirstOrDefault(u => u.id == id);
        //     if (usuario == null)
        //     {
        //         return NotFound();
        //     }
        //     return Ok(usuario);
        // }
        /*
       Esto se usa para rutear por atributos
       */
        // [HttpGet("nombre/{nombre}")]
        // public IActionResult ObtenerUsuarioPorNombre(string nombre)
        // {
        //     var usuario = _contexto.Usuario.FirstOrDefault(u => u.nombre == nombre);
        //     if (usuario == null)
        //     {
        //         return NotFound();
        //     }
        //     return Ok(usuario);
        // }
    }
}
