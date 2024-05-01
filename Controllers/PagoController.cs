using inmobiliaria.Controllers;
using inmobiliaria.Repositorios;
using Microsoft.AspNetCore.Mvc;
namespace inmobiliaria.Models
{

    public class PagoController : Controller, IControladorBase<Pago>
    {
        private readonly RepositorioPago repositorioPago;
        public PagoController(RepositorioPago repo)
        {
            this.repositorioPago = repo;
        }

        public ActionResult<Pago> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ActionResult<List<Pago>> Get()
        {
            throw new NotImplementedException();
        }

        public ActionResult<Pago> Get(int id)
        {
            throw new NotImplementedException();
        }

        public ActionResult<Pago> Post(Pago t)
        {
            throw new NotImplementedException();
        }

        public ActionResult<Pago> Put(int id, Pago t)
        {
            throw new NotImplementedException();
        }
    }



}
