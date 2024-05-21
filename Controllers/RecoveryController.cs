using inmobiliaria.Models;
using inmobiliaria.Repositorios;
using inmobiliaria.Servicio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace inmobiliaria.Models
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class RecoveryController : ControllerBase
    {
        private readonly RepositorioPropietario _repositorio;
        private readonly EmailSender _emailSender;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public RecoveryController(RepositorioPropietario repositorio, IWebHostEnvironment hostingEnvironment, EmailSender emailSender)
        {
            _repositorio = repositorio;
            _emailSender = emailSender;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public IActionResult Recovery([FromForm] string email)
        {
            try
            {
                var propietario = _repositorio.ObtenerPorEmail(email);
                var dominio = _hostingEnvironment.IsDevelopment() ? HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() : "www.inmobiliaria.com";

                if (propietario != null)
                {
                    string token = GeneratePasswordResetToken();
                    string templatePath = Path.Combine(_hostingEnvironment.ContentRootPath, "EmailTemplate.html");
                    string templateContent = System.IO.File.ReadAllText(templatePath);
                    string mensajeHtml = templateContent.Replace("{{Token}}", token).Replace("{{Nombre}}", propietario.nombre);
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                    bool enviado = _emailSender.SendEmail(propietario.email, "Restablecer Contraseña", mensajeHtml);
                    if (enviado)
                    {
                        _repositorio.CambiarPass(propietario.id, token);
                        return Ok("Se ha enviado un correo electrónico con una nueva contraseña, por favor revise su correo.");
                    }
                    else
                    {
                        return BadRequest("Error al enviar el correo electrónico para restablecer la contraseña.");
                    }
                }
                else
                {
                    return NotFound("No se encontró ningún propietario con la dirección de correo electrónico proporcionada.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al procesar la solicitud: " + ex.Message);
            }
        }

        private string GeneratePasswordResetToken()
        {
            Random rand = new Random(Environment.TickCount);
            string randomChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
            string nuevaClave = "";
            for (int i = 0; i < 8; i++)
            {
                nuevaClave += randomChars[rand.Next(0, randomChars.Length)];
            }
            return nuevaClave;
        }
    }
}
