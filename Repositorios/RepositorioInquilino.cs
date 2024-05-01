using inmobiliaria.Models;
using Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace inmobiliaria.Repositorios
{
    public class RepositorioInquilino : IRepositorio<Inquilino>
    {
        private readonly DataContext _contexto;

        public RepositorioInquilino(DataContext contexto)
        {
            _contexto = contexto;
        }

        public bool Actualizar(Inquilino inqulino)
        {
            try
            {
                _contexto.Inquilino.Update(inqulino);
                _contexto.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Inquilino BuscarPorId(int id)
        {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return _contexto.Inquilino.FirstOrDefault(i => i.id == id);
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }

        public bool Crear(Inquilino entity)
        {
            try
            {
                _contexto.Inquilino.Add(entity);
                _contexto.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EliminadoLogico(int id)
        {
            try
            {
                var inquilino = _contexto.Inquilino.FirstOrDefault(i => i.id == id);
                if (inquilino != null)
                {
                    inquilino.borrado = true;
                    _contexto.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Inquilino> ObtenerActivos()
        {
            return _contexto.Inquilino.Where(i => !i.borrado).ToList();
        }

        public List<Inquilino> ObtenerTodos()
        {
            return _contexto.Inquilino.ToList();
        }
    }
}
