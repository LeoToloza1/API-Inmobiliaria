using inmobiliaria.Models;
using Microsoft.EntityFrameworkCore;
using Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
namespace inmobiliaria.Repositorios
{
    public class RepositorioPropietario : IRepositorio<Propietario>
    {
        private readonly DataContext _contexto;
        public RepositorioPropietario(DataContext dataContext)
        {
            _contexto = dataContext;
        }

        public bool Actualizar(Propietario propietario)
        {
            try
            {
                _contexto.Propietario.Update(propietario);
                _contexto.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el propietario: {ex.Message}");
                return false;
            }
        }

        public Propietario BuscarPorId(int id)
        {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return _contexto.Propietario
                .FirstOrDefault(p => p.id == id);
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }

        public bool Crear(Propietario propietario)
        {
            try
            {
                _contexto.Propietario.Add(propietario);
                _contexto.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el propietario: {ex.Message}");
                return false;
            }
        }


        public bool EliminadoLogico(int id)
        {
            try
            {
                var propietario = _contexto.Propietario.FirstOrDefault(i => i.id == id);
                if (propietario != null)
                {
                    string uuid = Guid.NewGuid().ToString();
                    propietario.borrado = true;
                    propietario.email = $"borrado-{uuid}";
                    propietario.dni += "-borrado";
                    _contexto.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el propietario: {ex.Message}");
                return false;
            }
        }

        public List<Propietario> ObtenerTodos()
        {
            return _contexto.Propietario.ToList();
        }

        public List<Propietario> ObtenerActivos()
        {
            return _contexto.Propietario.Where(p => !p.borrado).ToList();
        }
    }

}