using inmobiliaria.Models;
using Microsoft.EntityFrameworkCore;
using Repositorios;

namespace inmobiliaria.Repositorios
{
    public class RepositorioPago : IRepositorio<Pago>
    {
        private readonly DataContext _contexto;
        public RepositorioPago(DataContext contexto)
        {
            _contexto = contexto;
        }

        public bool Actualizar(Pago pago)
        {
            try
            {
                _contexto.Pago.Update(pago);
                _contexto.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el pago: {ex.Message}");
                return false;
            }
        }

        public Pago BuscarPorId(int id)
        {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return _contexto.Pago
                .Include(p => p.Contrato)
                .FirstOrDefault(p => p.id == id);
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }

        public bool Crear(Pago pago)
        {
            try
            {
                _contexto.Pago.Add(pago);
                _contexto.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el Pago: {ex.Message}");
                return false;
            }
        }

        public bool EliminadoLogico(int id)
        {

            throw new NotImplementedException("El pago no se puede eliminar");
        }

        public List<Pago> ObtenerActivos()
        {
            //podria filtrarse por estado...
            throw new NotImplementedException();
        }

        public List<Pago> ObtenerTodos()
        {
            return _contexto.Pago.ToList();
        }
    }
}