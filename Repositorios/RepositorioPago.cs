using inmobiliaria.Models;
using Microsoft.EntityFrameworkCore;
using Repositorios;

namespace inmobiliaria.Repositorios
{
    public class RepositorioPago
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
                .Where(p => p.id == id)
                .Select(c => new Pago
                {
                    id = c.id,
                    contratoid = c.contratoid,
                    fecha_pago = new DateOnly(c.fecha_pago.Year, c.fecha_pago.Month, c.fecha_pago.Day),
                    importe = c.importe,
                    estado = c.estado,
                    numero_pago = c.numero_pago,
                    detalle = c.detalle,
                    Contrato = c.Contrato != null ? new Contrato
                    {
                        id = c.Contrato.id,
                        inquilinoid = c.Contrato.inquilinoid,
                        inmuebleid = c.Contrato.inmuebleid,
                        fecha_inicio = new DateOnly(c.Contrato.fecha_inicio.Year, c.Contrato.fecha_inicio.Month, c.Contrato.fecha_inicio.Day),
                        fecha_fin = new DateOnly(c.Contrato.fecha_fin.Year, c.Contrato.fecha_fin.Month, c.Contrato.fecha_fin.Day),
                        monto = c.Contrato.monto,
                        inquilino = c.Contrato.inquilino,
                        inmueble = c.Contrato.inmueble
                    } : null
                })
                .FirstOrDefault();
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

        public List<Pago> ObtenerTodos(int propietarioId)
        {
            return _contexto.Pago
                .Where(p => p.Contrato != null && p.Contrato.inmueble != null && p.Contrato.inmueble.PropietarioId == propietarioId)
                .Select(c => new Pago
                {
                    id = c.id,
                    contratoid = c.contratoid,
                    fecha_pago = new DateOnly(c.fecha_pago.Year, c.fecha_pago.Month, c.fecha_pago.Day),
                    importe = c.importe,
                    estado = c.estado,
                    numero_pago = c.numero_pago,
                    detalle = c.detalle,
                    Contrato = c.Contrato != null ? new Contrato
                    {
                        id = c.Contrato.id,
                        inquilinoid = c.Contrato.inquilinoid,
                        inmuebleid = c.Contrato.inmuebleid,
                        fecha_inicio = new DateOnly(c.Contrato.fecha_inicio.Year, c.Contrato.fecha_inicio.Month, c.Contrato.fecha_inicio.Day),
                        fecha_fin = new DateOnly(c.Contrato.fecha_fin.Year, c.Contrato.fecha_fin.Month, c.Contrato.fecha_fin.Day),
                        monto = c.Contrato.monto,
                        inquilino = c.Contrato.inquilino,
                        inmueble = c.Contrato.inmueble

                    } : null
                }).ToList();
        }

    }
}