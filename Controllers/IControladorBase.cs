using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria.Controllers
{

    public interface IControladorBase<T>
    {
        ActionResult<List<T>> Get();
        ActionResult<T> Get(int id);
        ActionResult<T> Post(T t);
        ActionResult<T> Put(int id, T t);
        ActionResult<T> Delete(int id);
    }

}
