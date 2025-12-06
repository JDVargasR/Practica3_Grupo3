using Practica3_Grupo3.EF;
using Practica3_Grupo3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Practica3_Grupo3.Controllers
{
    public class AbonosController : Controller
    {
        [HttpGet]
        public ActionResult Registro()
        {
            CargarComprasPendientes();
            return View();
        }

        [HttpPost]
        public ActionResult Registro(AbonoModel modelo)
        {
            using (var context = new PracticaS13Entities())
            {
                var compra = context.Principal
                                    .Where(x => x.Id_Compra == modelo.Id_Compra)
                                    .FirstOrDefault();

                if (compra == null)
                {
                    ViewBag.Mensaje = "No se encontró la compra.";
                    CargarComprasPendientes();
                    return View(modelo);
                }

                // Validación
                if (modelo.MontoAbono > compra.Saldo)
                {
                    ViewBag.Mensaje = "El abono no puede ser mayor al saldo.";
                    CargarComprasPendientes();
                    return View(modelo);
                }

                // Registrar abono
                var nuevoAbono = new Abonos
                {
                    Id_Compra = modelo.Id_Compra,
                    Monto = modelo.MontoAbono,
                    Fecha = DateTime.Now
                };

                context.Abonos.Add(nuevoAbono);

                // Actualizar saldo
                compra.Saldo -= modelo.MontoAbono;
                if (compra.Saldo == 0)
                    compra.Estado = "Cancelado";

                var resultado = context.SaveChanges();

                if (resultado > 0)
                    return RedirectToAction("Consulta", "Principal");

                ViewBag.Mensaje = "No se pudo registrar el abono.";
                CargarComprasPendientes();
                return View(modelo);
            }
        }

        [HttpGet]
        public JsonResult ObtenerSaldo(int id)
        {
            using (var context = new PracticaS13Entities())
            {
                var compra = context.Principal.Find(id);
                return Json(compra?.Saldo ?? 0, JsonRequestBehavior.AllowGet);
            }
        }

        private void CargarComprasPendientes()
        {
            using (var context = new PracticaS13Entities())
            {
                var resultado = context.Principal
                                       .Where(x => x.Estado == "Pendiente")
                                       .ToList();

                var datos = resultado.Select(c => new SelectListItem
                {
                    Value = c.Id_Compra.ToString(),
                    Text = c.Descripcion
                }).ToList();

                datos.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = "Seleccione"
                });

                ViewBag.ListaCompras = datos;
            }
        }
    }
}
