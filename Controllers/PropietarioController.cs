using inmobiliaria.Repositorios;
using inmobiliaria.Servicio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Linq;

namespace inmobiliaria.Models
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PropietarioController : ControllerBase
    {
        private readonly RepositorioPropietario repositorioPropietario;
        private readonly IWebHostEnvironment hostingEnvironment;
        public PropietarioController(RepositorioPropietario repo, IWebHostEnvironment env)
        {
            repositorioPropietario = repo;
            hostingEnvironment = env;
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
        public ActionResult<Propietario> Get()
        {
            var userId = GetPropietarioId();
            var propietario = repositorioPropietario.BuscarPorId(userId);

            if (propietario == null)
            {
                return NotFound();
            }
            propietario.password = null;
            return propietario;
        }

        [HttpPut("actualizar")]
        public ActionResult<Propietario> ActualizarPropietario([FromBody] Propietario propietario)
        {
            var id = GetPropietarioId();
            var propietarioExistente = repositorioPropietario.BuscarPorId(id);
            if (propietarioExistente == null)
            {
                return NotFound();
            }
            propietarioExistente.nombre = propietario.nombre ?? propietarioExistente.nombre;
            propietarioExistente.apellido = propietario.apellido ?? propietarioExistente.apellido;
            propietarioExistente.dni = propietario.dni ?? propietarioExistente.dni;
            propietarioExistente.telefono = propietario.telefono ?? propietarioExistente.telefono;

            bool exito = repositorioPropietario.Actualizar(propietarioExistente);
            if (!exito)
            {
                return StatusCode(500, "Error al actualizar el propietario");
            }
            propietarioExistente.password = null;
            return Ok(propietarioExistente);
        }

        [HttpPost("actualizar/avatar")]
        public ActionResult<Propietario> ActualizarAvatar([FromForm] IFormFile nuevoAvatarFile)
        {
            var id = GetPropietarioId();
            var propietarioExistente = repositorioPropietario.BuscarPorId(id);
            if (propietarioExistente == null)
            {
                return NotFound();
            }
            if (!ImagenValida(nuevoAvatarFile))
            {
                return BadRequest("Imagen no valida");
            }
            if (nuevoAvatarFile != null)
            {
                string imgFolderPath = Path.Combine(hostingEnvironment.WebRootPath, "img");
                string folderPath = Path.Combine(imgFolderPath, "uploads");
                Directory.CreateDirectory(imgFolderPath);
                Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, nuevoAvatarFile.FileName);
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        nuevoAvatarFile.CopyTo(stream);
                    }
                    propietarioExistente.avatarUrl = Path.GetFileName(filePath);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error al guardar el archivo: {ex.Message}");
                }
            }
            bool exito = repositorioPropietario.Actualizar(propietarioExistente);
            if (!exito)
            {
                return StatusCode(500, "Error al actualizar el avatar del propietario");
            }
            return Ok(propietarioExistente);
        }

        [HttpPatch("actualizar/pass")]
        public string CambiarPass([FromForm] string pass)
        {
            var id = GetPropietarioId();
            var usuarioExistente = repositorioPropietario.BuscarPorId(id);

            if (usuarioExistente == null)
            {
                return "Usuario no encontrado.";
            }

            if (string.IsNullOrEmpty(pass) || pass.Length < 8)
            {
                return "La contraseña debe tener al menos 8 caracteres.";
            }

            bool exito = repositorioPropietario.CambiarPass(usuarioExistente.id, pass);
            if (!exito)
            {
                return "Error al actualizar la contraseña.";
            }

            usuarioExistente.password = null;
            return "La contraseña se cambió correctamente, vuelva a iniciar sesión.";
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