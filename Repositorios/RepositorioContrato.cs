using inmobiliaria.Models;
using Microsoft.EntityFrameworkCore;
using Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
namespace inmobiliaria.Repositorios
{
    public class RepositorioContrato : IRepositorio<Contrato>
    {
        private readonly DataContext _contexto;

        public RepositorioContrato(DataContext contexto)
        {
            _contexto = contexto;
        }

        public bool Actualizar(Contrato contrato)
        {
            try
            {
                _contexto.Contrato.Update(contrato);
                _contexto.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el contrato: {ex.Message}");
                return false;
            }
        }

        public Contrato BuscarPorId(int id)
        {
            var contrato = _contexto.Contrato
                .Where(c => c.id == id && !c.borrado)
                .Select(c => new Contrato
                {
                    id = c.id,
                    inquilinoid = c.inquilinoid,
                    inmuebleid = c.inmuebleid,
                    fecha_inicio = new DateOnly(c.fecha_inicio.Year, c.fecha_inicio.Month, c.fecha_inicio.Day),
                    fecha_fin = new DateOnly(c.fecha_fin.Year, c.fecha_fin.Month, c.fecha_fin.Day),
                    fecha_efectiva = new DateOnly(c.fecha_efectiva.Year, c.fecha_efectiva.Month, c.fecha_efectiva.Day),
                    monto = c.monto,
                    borrado = c.borrado,
                    inquilino = c.inquilino,
                    inmueble = c.inmueble
                })
                .FirstOrDefault();

#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return contrato;
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }

        public bool Crear(Contrato contrato)
        {
            try
            {
                _contexto.Contrato.Add(contrato);
                _contexto.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el contrato: {ex.Message}");
                return false;
            }
        }

        public bool EliminadoLogico(int id)
        {
            try
            {
                var query = $"UPDATE contrato SET borrado = 1 WHERE id = {id}";
                _contexto.Database.ExecuteSqlRaw(query);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar lógicamente el contrato: {ex.Message}");
                return false;
            }
        }

        public List<Contrato> ObtenerActivos()
        {
            return _contexto.Contrato
                .Where(c => !c.borrado)
                .Select(c => new Contrato
                {
                    id = c.id,
                    inquilinoid = c.inquilinoid,
                    inmuebleid = c.inmuebleid,
                    fecha_inicio = new DateOnly(c.fecha_inicio.Year, c.fecha_inicio.Month, c.fecha_inicio.Day),
                    fecha_fin = new DateOnly(c.fecha_fin.Year, c.fecha_fin.Month, c.fecha_fin.Day),
                    fecha_efectiva = new DateOnly(c.fecha_efectiva.Year, c.fecha_efectiva.Month, c.fecha_efectiva.Day),
                    monto = c.monto,
                    borrado = c.borrado,
                    inquilino = c.inquilino,
                    inmueble = c.inmueble
                }).ToList();
        }

        public List<Contrato> ObtenerTodos()
        {
            return _contexto.Contrato
                .Select(c => new Contrato
                {
                    id = c.id,
                    inquilinoid = c.inquilinoid,
                    inmuebleid = c.inmuebleid,
                    fecha_inicio = new DateOnly(c.fecha_inicio.Year, c.fecha_inicio.Month, c.fecha_inicio.Day),
                    fecha_fin = new DateOnly(c.fecha_fin.Year, c.fecha_fin.Month, c.fecha_fin.Day),
                    fecha_efectiva = new DateOnly(c.fecha_efectiva.Year, c.fecha_efectiva.Month, c.fecha_efectiva.Day),
                    monto = c.monto,
                    borrado = c.borrado,
                    inquilino = c.inquilino,
                    inmueble = c.inmueble
                }).ToList();
        }
    }


}