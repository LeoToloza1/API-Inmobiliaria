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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PropietarioController : ControllerBase
    {
        private readonly RepositorioPropietario repositorioPropietario;
        private readonly IWebHostEnvironment hostingEnvironment;
        public PropietarioController(RepositorioPropietario repo, IWebHostEnvironment env)
        {
            repositorioPropietario = repo;
            hostingEnvironment = env;

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
        [AllowAnonymous]
        public ActionResult<Propietario> Post([FromForm] Propietario propietario, [FromForm] IFormFile avatarFile)
        {
#pragma warning disable CS8604 // Posible argumento de referencia nulo
            string hashedPassword = HashPass.HashearPass(propietario.password);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
            propietario.password = hashedPassword;
            if (avatarFile != null && avatarFile.Length > 0)
            {
                try
                {
                    // Ruta donde se guardarán las imágenes (en la carpeta 'img/uploads' dentro del directorio de la aplicación)
                    string imgFolderPath = Path.Combine(AppContext.BaseDirectory, "img");
                    string folderPath = Path.Combine(imgFolderPath, "uploads");
                    Directory.CreateDirectory(folderPath);
                    string filePath = Path.Combine(folderPath, avatarFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        avatarFile.CopyTo(stream);
                    }
                    propietario.avatarUrl = avatarFile.FileName;
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error al guardar la imagen de avatar: {ex.Message}");
                }
            }
            else
            {
                propietario.avatarUrl = string.Empty;
            }
            bool exito = repositorioPropietario.Crear(propietario);
            if (!exito)
            {
                return BadRequest("Error al crear el propietario.");
            }
            return Ok(propietario);
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
        [Authorize(Policy = "Administrador")]
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
        [HttpPatch("actualizar/pass/{id}")]
        public ActionResult<Propietario> CambiarPass(int id, string pass)
        {
            var usuarioExistente = repositorioPropietario.BuscarPorId(id);
            if (usuarioExistente == null)
            {
                return NotFound();
            }
            if (string.IsNullOrEmpty(pass) || pass.Length < 8)
            {
                return BadRequest("La contraseña debe tener al menos 8 caracteres.");
            }
            usuarioExistente.password = pass;
            bool exito = repositorioPropietario.CambiarPass(usuarioExistente.id, pass);
            if (!exito)
            {
                return StatusCode(500, "Error al actualizar la contraseña");
            }
            return Ok(usuarioExistente);
        }
        [HttpPatch("actualizar/avatar/{id}")]
        public ActionResult<Propietario> CambiarAvatar(int id, string avatarUrl, IFormFile avatarFile)
        {
            string imgFolderPath = Path.Combine(hostingEnvironment.WebRootPath, "img");
            string folderPath = Path.Combine(imgFolderPath, "uploads");
            var usuarioExistente = repositorioPropietario.BuscarPorId(id);
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
            bool exito = repositorioPropietario.CambiarAvatar(id, usuarioExistente.avatarUrl);
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