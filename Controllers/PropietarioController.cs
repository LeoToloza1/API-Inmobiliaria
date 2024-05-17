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

        //         [HttpPost]
        //         public ActionResult<Propietario> Post([FromForm] Propietario propietario, [FromForm] IFormFile avatarFile)
        //         {
        // #pragma warning disable CS8604 // Posible argumento de referencia nulo
        //             string hashedPassword = HashPass.HashearPass(propietario.password);
        // #pragma warning restore CS8604 // Posible argumento de referencia nulo
        //             propietario.password = hashedPassword;
        //             if (avatarFile != null && avatarFile.Length > 0)
        //             {
        //                 try
        //                 {
        //                     // Ruta donde se guardarán las imágenes (en la carpeta 'img/uploads' dentro del directorio de la aplicación)
        //                     string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "img", "uploads");
        //                     Directory.CreateDirectory(uploadsFolder); // Crear la carpeta si no existe
        //                     var fileName = Guid.NewGuid().ToString() + Path.GetExtension(avatarFile.FileName);
        //                     var filePath = Path.Combine(uploadsFolder, fileName);
        //                     using (var stream = new FileStream(filePath, FileMode.Create))
        //                     {
        //                         avatarFile.CopyTo(stream);
        //                     }
        //                     propietario.avatarUrl = Path.Combine("img", "uploads", fileName);
        //                 }
        //                 catch (Exception ex)
        //                 {
        //                     return StatusCode(500, $"Error al guardar la imagen de avatar: {ex.Message}");
        //                 }
        //             }
        //             else
        //             {
        //                 propietario.avatarUrl = string.Empty;
        //             }
        //             bool exito = repositorioPropietario.Crear(propietario);
        //             if (!exito)
        //             {
        //                 return BadRequest("Error al crear el propietario.");
        //             }
        //             return Ok(propietario);
        //         }

        [HttpPut("actualizar")]
        public ActionResult<Propietario> ActualizarPropietario([FromBody] Propietario propietario)
        {
            var id = GetPropietarioId();
            var propietarioExistente = repositorioPropietario.BuscarPorId(id);
            if (propietarioExistente == null)
            {
                return NotFound();
            }

            Console.WriteLine("PROPIETARIO ENVIADO ---->" + id);
            Console.WriteLine("PROPIETARIO ENVIADO ---->" + propietario.nombre);
            Console.WriteLine("PROPIETARIO recibido ---->" + propietarioExistente.nombre);
            Console.WriteLine("PROPIETARIO recibido ---->" + propietarioExistente.apellido);
            Console.WriteLine("PROPIETARIO recibido ---->" + propietarioExistente.dni);
            Console.WriteLine("PROPIETARIO recibido ---->" + propietarioExistente.telefono);

            // Actualizar solo los campos que se envían
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

        // [HttpDelete("borrar/{id}")]
        // [Authorize(Policy = "Administrador")]
        // public ActionResult<Propietario> Delete(int id)
        // {
        //     var propietario = repositorioPropietario.BuscarPorId(id);
        //     if (propietario == null)
        //     {
        //         return NotFound();
        //     }
        //     var exito = repositorioPropietario.EliminadoLogico(id);
        //     if (!exito)
        //     {
        //         return StatusCode(500, "Error al eliminar");
        //     }

        //     return NoContent();
        // }
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

        private bool ImagenValida(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            // Validacion de las extensiones 
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            return allowedExtensions.Contains(extension);
        }


    }
}