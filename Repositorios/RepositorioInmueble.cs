using inmobiliaria.Models;
using Microsoft.EntityFrameworkCore;
using Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace inmobiliaria.Repositorios
{
    public class RepositorioInmueble
    {
        private readonly DataContext _contexto;

        public RepositorioInmueble(DataContext contexto)
        {
            _contexto = contexto;
        }

        public bool Actualizar(Inmueble inmueble)
        {
            try
            {
                _contexto.Inmueble.Update(inmueble);
                _contexto.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el inmueble: {ex.Message}");
                return false;
            }
        }

        public Inmueble BuscarPorId(int id)
        {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return _contexto.Inmueble
                .Include(i => i.tipoInmueble)
                .Include(i => i.propietario)
                .FirstOrDefault(i => i.id == id);
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }

        public bool Crear(Inmueble inmueble)
        {
            try
            {
                _contexto.Inmueble.Add(inmueble);
                _contexto.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el inmueble: {ex.Message}");
                return false;
            }
        }
        public List<Inmueble> InmueblesDePropietario(int propietarioId)
        {
            var inmuebles = _contexto.Inmueble
       .Where(i => i.PropietarioId == propietarioId)
       .Include(i => i.propietario)
       .Include(i => i.tipoInmueble)
       .ToList();
            foreach (var inmueble in inmuebles)
            {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                inmueble.propietario.password = null;
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.

            }

            return inmuebles;
        }


        public bool CambiarAvatar(int inmuebleId, string nuevoAvatar)
        {
            try
            {
                var inmueble = _contexto.Inmueble.FirstOrDefault(u => u.id == inmuebleId);
                if (inmueble == null)
                {
                    Console.WriteLine("No se encontró el inmueble.");
                    return false;
                }
                inmueble.avatarUrl = nuevoAvatar;
                _contexto.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cambiar el avatar: {ex.Message}");
                return false;
            }
        }

        public Inmueble habilitar(int id)
        {
            var inmueble = _contexto.Inmueble.FirstOrDefault(i => i.id == id);
            if (inmueble?.estado == "Retirado")
            {
                inmueble.estado = "Disponible";
            }
            else
            {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                inmueble.estado = "Retirado";
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
            }
            _contexto.SaveChanges();
            return inmueble;
        }

        public List<Inmueble> InmueblesAlquilados(int propietarioId)
        {

            var inmueblesAlquilados = _contexto.Inmueble
       .Where(i => i.PropietarioId == propietarioId)
       .Include(i => i.propietario)
       .Include(i => i.tipoInmueble)
       .ToList();
            foreach (var inmueble in inmueblesAlquilados)
            {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                inmueble.propietario.password = null;
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.

            }

            return inmueblesAlquilados;
        }

    }
}
