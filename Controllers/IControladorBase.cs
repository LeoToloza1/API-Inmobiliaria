using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria.Controllers
{

    public interface IControladorBase<T>
    {
        ActionResult<List<T>> Get(); //no es necesario
        ActionResult<T> Get(int id); //solo devuelve el usuario logueado
        ActionResult<T> Post(T t); //no hay registros en la app
        ActionResult<T> Put(int id, T t); //si permito actualizar
        ActionResult<T> Delete(int id); //no deberia poder permitir borrar, a excepcion de los inmuebles del propietario
    }

}
