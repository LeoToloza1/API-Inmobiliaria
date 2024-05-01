using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositorios
{
    public interface IRepositorio<T>
    {
        //crud basico
        List<T> ObtenerTodos();
        List<T> ObtenerActivos();
        bool Crear(T entity);
        bool Actualizar(T entity);
        bool EliminadoLogico(int id);
        T BuscarPorId(int id);
    }

}