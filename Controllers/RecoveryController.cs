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

        [HttpPost("recovery")]
        public IActionResult Recovery(string email)
        {
            try
            {
                var entidad = _repositorio.ObtenerPorEmail(email);
                var dominio = _hostingEnvironment.IsDevelopment() ? HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() : "www.misitio.com";

                if (entidad != null)
                {
                    string token = GeneratePasswordResetToken();
                    string resetPasswordLink = $"http://{dominio}/reset-password?token={token}";
                    string mensajeHtml = $@"<h1>Restablecer contraseña</h1>
                                    <p>Hemos recibido una solicitud para restablecer tu contraseña. Haz clic en el siguiente enlace para crear una nueva contraseña:</p>
                                    <a href='{resetPasswordLink}'>Restablecer contraseña</a>";
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                    bool enviado = _emailSender.SendEmail(entidad.email, "Restablecer Contraseña", mensajeHtml);
#pragma warning restore CS8604 // Posible argumento de referencia nulo

                    if (enviado)
                    {
                        return Ok("Se ha enviado un correo electrónico con instrucciones para restablecer la contraseña.");
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
