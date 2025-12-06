using Practica3_Grupo3.EF;
using Practica3_Grupo3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Practica3_Grupo3.Controllers
{
    public class PrincipalController : Controller
    {
        [HttpGet]
        public ActionResult Consulta()
        {
            var resultado = ConsultarCompras();
            return View(resultado);
        }

        private List<PrincipalModel> ConsultarCompras()
        {
            using (var context = new PracticaS13Entities())
            {
                var datos = context.Principal
                    .ToList()
                    .Select(x => new PrincipalModel
                    {
                        Id_Compra = x.Id_Compra,
                        Descripcion = x.Descripcion,
                        Precio = x.Precio,
                        Saldo = x.Saldo,
                        Estado = (x.Estado.ToUpper() == "CANCELADO") ? "Cancelado" : "Pendiente"
                    })
                    .OrderBy(x => x.Estado == "Cancelado")  // Primero los que aun esten pendientes
                    .ThenBy(x => x.Id_Compra)
                    .ToList();

                return datos;
            }
        }
    }
}

