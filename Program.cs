using System.Configuration;
using System.Security.Claims;
using System.Text;
using inmobiliaria.Models;
using inmobiliaria.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using inmobiliaria.Servicio;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001", "http://*:5000", "https://*:5001");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Obtener la cadena de conexión de appsettings.json
var connectionString = builder.Configuration.GetConnectionString("Mysql");

// Agregar el DbContext utilizando la cadena de conexión
#pragma warning disable CS8604 // Posible argumento de referencia nulo
builder.Services.AddDbContext<DataContext>(options => options.UseMySQL(connectionString));
#pragma warning restore CS8604 // Posible argumento de referencia nulo
builder.Services.AddScoped<RepositorioInquilino>();
builder.Services.AddScoped<RepositorioInmueble>();
builder.Services.AddScoped<RepositorioPropietario>();
builder.Services.AddScoped<RepositorioContrato>();
builder.Services.AddScoped<RepositorioPago>();
builder.Services.AddScoped<RepositorioTipoInmuebles>();
builder.Services.AddScoped<Auth>();
builder.Services.AddScoped<EmailSender>();
builder.Services.AddScoped<RepositorioUsuario>();
builder.Services.AddSwaggerGen();
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
{
    Console.WriteLine("Error: Clave, emisor o audiencia del token JWT no configurados correctamente");
}
else
{
    builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        var key = Encoding.ASCII.GetBytes(jwtKey);

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false, //este en falso para las pruebas
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

}
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Propietario", policy => policy.RequireRole("Propietario"));

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001", "http://*:5000", "https://*:5001");
    app.UseSwagger();
    app.UseSwaggerUI();
}
//habilita el cors
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseAuthentication();
//se usa para habilitar la redirección HTTPS - comentada para pobrar desde el movil
// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.Run();
