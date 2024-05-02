using inmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositorios;

namespace inmobiliaria.Repositorios
{
    public class RepositorioTipoInmuebles : Controller
    {
        private readonly DataContext _contexto;

        public RepositorioTipoInmuebles(DataContext contexto)
        {
            _contexto = contexto;
        }

        public List<TipoInmueble> ObtenerTodos()
        {
            return _contexto.TipoInmueble.ToList();
        }

        public bool Crear(TipoInmueble tipoInmueble)
        {
            try
            {
                _contexto.TipoInmueble.Add(tipoInmueble);
                _contexto.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Eliminar(int id)
        {
            var tipo = _contexto.TipoInmueble.FirstOrDefault(i => i.id == id);
            if (tipo != null)
            {
                tipo.borrado = true;
                _contexto.SaveChanges();
                return true;
            }
            return false;
        }

        public TipoInmueble ObtenerPorId(int id)
        {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return _contexto.TipoInmueble.FirstOrDefault(i => i.id == id);
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }

    }
}