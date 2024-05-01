using inmobiliaria.Models;
using inmobiliaria.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repositorios;

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

//falta implementar:
// builder.Services.AddScoped<RepositorioTipoInmueble>();
// builder.Services.AddScoped<RepositorioCiudad>();
// builder.Services.AddScoped<RepositorioZona>();
// builder.Services.AddScoped<RepositorioUsuario>();
// builder.Services.AddScoped<EmailSender>();

builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//habilita el cors
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

//se usa para habilitar la redirección HTTPS - comentada para pobrar desde el movil
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
