using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Repositorios;
using inmobiliaria.Models;
using inmobiliaria.Repositorios;
using Microsoft.AspNetCore.Authorization;
using inmobiliaria.Servicio;

namespace inmobiliaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly RepositorioPropietario _repositorio;
        private readonly Auth _auth;
        public LoginController(Auth auth, RepositorioPropietario repositorio)
        {
            _auth = auth;
            _repositorio = repositorio;
        }
        [HttpPost]
        public IActionResult Post(LoginModel loginModel)
        {
#pragma warning disable CS8604 // Posible argumento de referencia nulo
            var p = _repositorio.ObtenerPorEmail(loginModel.Email);

            if (p == null)
            {
                return NotFound(new { message = "Ocurrio un error intente de nuevo" });
            }
            if (HashPass.VerificarPassword(loginModel.Password, p.password))
            {
                var tokenGenerado = _auth.GenerarToken(p);
                return Ok(new { tokenGenerado });
            }
            return NotFound(new { message = "Correo electrónico o contraseña incorrectos" });
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            HttpContext.Response.Cookies.Delete("jwt");
            return Ok("Sesión cerrada");
        }
    }
}