using inmobiliaria.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace inmobiliaria.Models
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Propietario")]
    public class InmuebleController : ControllerBase
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly RepositorioInmueble repositorioInmueble;
        public InmuebleController(RepositorioInmueble repositorioInmueble, IWebHostEnvironment env)
        {
            this.repositorioInmueble = repositorioInmueble;
            this.hostingEnvironment = env;
        }
        private int GetPropietarioId()
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            var userIdClaim = User.FindFirst("id").Value;
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
            int userId = int.Parse(userIdClaim);
            return userId;
        }
        [HttpGet] //devuelve todos los inmuebles del propietario logueado
        public ActionResult<List<Inmueble>> Get()
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.

            //este listado deberia estar en un gridLayout y al tocar en uno
            //deberia acceder el endpoint siguiente: api/inmueble/propietario/{id}
            var id = User.FindFirst("id").Value;
            var inmuebles = repositorioInmueble.InmueblesDePropietario(int.Parse(id));
            if (inmuebles == null)
            {
                return NotFound();
            }
            return inmuebles;
        }
        [HttpGet("{id}")]
        public ActionResult<Inmueble> Get(int id)
        {
            //esto devuelve un inmueble por id, de un propietario logueado
            var userId = User.FindFirst("id")?.Value;
            var inmueble = repositorioInmueble.BuscarPorId(id);
            if (inmueble == null || inmueble.PropietarioId.ToString() != userId)
            {
                return NotFound();
            }

            return inmueble;
        }

        [HttpPost("guardar")]
        public ActionResult<Inmueble> Post([FromForm] Inmueble inmueble, [FromForm] IFormFile img)
        {
            var userId = User.FindFirst("id")?.Value;
            if (!ModelState.IsValid)
            {
                Console.WriteLine(ModelState.ErrorCount);

                Console.WriteLine("No se pudo agregar el inmueble");
                return BadRequest(ModelState);
            }
            if (img != null)
            {
                if (!ImagenValida(img))
                {
                    return BadRequest("El archivo proporcionado no es una imagen válida.");
                }
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "img", "uploads");
                Directory.CreateDirectory(uploadsFolder); // Crear la carpeta si no existe
                var fileName = img.FileName;
                var filePath = Path.Combine(uploadsFolder, fileName);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        img.CopyTo(stream);
                    }
                    inmueble.avatarUrl = fileName;
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error al guardar el archivo: {ex.Message}");
                }
            }

#pragma warning disable CS8604 // Posible argumento de referencia nulo
            inmueble.PropietarioId = int.Parse(userId);
            inmueble.estado = "Retirado";
#pragma warning restore CS8604 // Posible argumento de referencia nulo
            repositorioInmueble.Crear(inmueble);
            return Ok(inmueble);
        }

        [HttpPut("actualizar/{id}")]
        public ActionResult<Inmueble> Put(int id, Inmueble inmueble)
        {
            //esto actualiza un inmueble unicamente del propietario logueado
            var userId = User.FindFirst("id")?.Value;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState + ": El modelo no coincide con el esperado");
            }
            if (id != inmueble.id)
            {
                return BadRequest("El ID proporcionado no coincide con el ID del Inmueble");
            }

            var inmuebleExistente = repositorioInmueble.BuscarPorId(id);
            if (inmuebleExistente == null && inmuebleExistente.PropietarioId.ToString() != userId)
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


        [HttpPatch("actualizar/avatar/{id}")]
        public ActionResult<Inmueble> CambiarAvatar(int id, IFormFile avatarFile)
        {
            if (avatarFile == null)
            {
                return BadRequest("Debe proporcionar un archivo de imagen.");
            }

            var inmuebleExistente = repositorioInmueble.BuscarPorId(id);
            if (inmuebleExistente == null)
            {
                return NotFound();
            }

            if (!ImagenValida(avatarFile))
            {
                return BadRequest("El archivo proporcionado no es una imagen válida.");
            }

            string imgFolderPath = Path.Combine(hostingEnvironment.ContentRootPath, "img");
            string folderPath = Path.Combine(imgFolderPath, "uploads");
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(avatarFile.FileName);

            try
            {
                Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    avatarFile.CopyTo(stream);
                }
                inmuebleExistente.avatarUrl = Path.Combine("img", "uploads", fileName);
                bool exito = repositorioInmueble.CambiarAvatar(id, inmuebleExistente.avatarUrl);

                if (!exito)
                {
                    return StatusCode(500, "Error al actualizar el avatar en la base de datos");
                }

                return Ok(inmuebleExistente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardar el archivo: {ex.Message}");
            }
        }
        private bool ImagenValida(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            // Validacion de las extensiones 
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            return allowedExtensions.Contains(extension);
        }

        [HttpPatch("habilitar/{id}")]
        public IActionResult habilitar(int id)
        {
            var inmueble = repositorioInmueble.habilitar(id);
            if (inmueble == null)
            {
                return NotFound("No se encontró el inmueble, intente de");
            }
            return Ok(inmueble);
        }

        [HttpGet("alquilados")]
        public ActionResult<List<Inmueble>> GetAlquilados()
        {
            var userId = GetPropietarioId();
            if (userId == 0)
            {
                return BadRequest("Usuario invalido.");
            }
            var inmuebles = repositorioInmueble.InmueblesAlquilados(userId);
            if (inmuebles == null || inmuebles.Count == 0)
            {
                return NotFound();
            }
            return inmuebles;
        }
    }
}