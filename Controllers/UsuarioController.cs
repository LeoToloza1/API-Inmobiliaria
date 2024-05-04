using inmobiliaria.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositorios;
using System.Linq;

namespace inmobiliaria.Models
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Administrador")]
    public class UsuarioController : ControllerBase, IControladorBase<Usuario>
    {

        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly RepositorioUsuario repositorioUsuario;

        public UsuarioController(RepositorioUsuario repositorioUsuario, IWebHostEnvironment _hostingEnvironment)
        {
            this.repositorioUsuario = repositorioUsuario;
            hostingEnvironment = _hostingEnvironment;
        }
        [HttpDelete("borrar/{id}")]
        public ActionResult<Usuario> Delete(int id)
        {
            var propietario = repositorioUsuario.BuscarPorId(id);
            if (propietario == null)
            {
                return NotFound();
            }
            var exito = repositorioUsuario.EliminadoLogico(id);
            if (!exito)
            {
                return StatusCode(500, "Error al eliminar");
            }

            return NoContent();
        }
        [HttpGet]
        public ActionResult<List<Usuario>> Get()
        {
            return repositorioUsuario.ObtenerActivos();
        }
        [HttpGet("{id}")]
        public ActionResult<Usuario> Get(int id)
        {
            return repositorioUsuario.BuscarPorId(id);
        }
        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public ActionResult<Usuario> Post(Usuario u)
        {

            bool exito = repositorioUsuario.Crear(u);
            if (!exito)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPut("actualizar/{id}")]
        public ActionResult<Usuario> Put(int id, Usuario u)
        {
            if (id != u.id)
            {
                return BadRequest("El ID proporcionado no coincide con el ID del Usuario");
            }
            var usuarioExistente = repositorioUsuario.BuscarPorId(id);
            if (usuarioExistente == null)
            {
                return NotFound();
            }
            usuarioExistente.nombre = u.nombre ?? usuarioExistente.nombre;
            usuarioExistente.apellido = u.apellido ?? usuarioExistente.apellido;
            usuarioExistente.email = u.email ?? usuarioExistente.email;
            usuarioExistente.dni = u.dni ?? usuarioExistente.dni;
            bool exito = repositorioUsuario.Actualizar(usuarioExistente);
            if (!exito)
            {
                return StatusCode(500, "Error al actualizar el propietario");
            }

            return Ok(usuarioExistente);

        }

        [HttpPatch("actualizar/pass/{id}")]
        public ActionResult<Usuario> CambiarPass(int id, string pass)
        {
            var usuarioExistente = repositorioUsuario.BuscarPorId(id);
            if (usuarioExistente == null)
            {
                return NotFound();
            }
            if (string.IsNullOrEmpty(pass) || pass.Length < 8)
            {
                return BadRequest("La contraseña debe tener al menos 8 caracteres.");
            }
            usuarioExistente.password = pass;
            bool exito = repositorioUsuario.CambiarPass(usuarioExistente.id, pass);
            if (!exito)
            {
                return StatusCode(500, "Error al actualizar la contraseña");
            }
            return Ok(usuarioExistente);
        }

        [HttpPatch("actualizar/avatar/{id}")]
        public ActionResult<Usuario> CambiarAvatar(int id, string avatarUrl, IFormFile avatarFile)
        {
            string imgFolderPath = Path.Combine(hostingEnvironment.WebRootPath, "img");
            string folderPath = Path.Combine(imgFolderPath, "uploads");
            var usuarioExistente = repositorioUsuario.BuscarPorId(id);
            if (usuarioExistente == null)
            {
                return NotFound();
            }
            if (avatarFile != null)
            {
                if (!ImagenValida(avatarFile))
                {
                    return BadRequest("El archivo proporcionado no es una imagen válida.");
                }
                Directory.CreateDirectory(imgFolderPath);
                Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, avatarFile.FileName);
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        avatarFile.CopyTo(stream);
                    }
                    usuarioExistente.avatarUrl = Path.GetFileName(filePath);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error al guardar el archivo: {ex.Message}");
                }
            }
            else
            {
                usuarioExistente.avatarUrl = avatarUrl;
            }
            bool exito = repositorioUsuario.CambiarAvatar(id, usuarioExistente.avatarUrl);
            if (!exito)
            {
                return StatusCode(500, "Error al actualizar el avatar en la base de datos");
            }
            return Ok(usuarioExistente);
        }
        private bool ImagenValida(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            // Validacion de las extensiones 
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            return allowedExtensions.Contains(extension);
        }


    }
}
