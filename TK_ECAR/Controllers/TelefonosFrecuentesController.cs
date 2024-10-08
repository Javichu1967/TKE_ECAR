using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Application_Services;
using TK_ECAR.Models;
using HelperExtensionsNameSpace;
using Newtonsoft.Json;

namespace TK_ECAR.Controllers
{
    public class TelefonosFrecuentesController : BaseController
    {
        public ActionResult Index()
        {
            List<int> empresas = new UsersService().GetEmpresasUsuario().ToList();
            var telefonos = new TelefonosService().GetAllTelefonos(empresas);

            return View(telefonos);
            //return View();
        }

        public ActionResult NuevoTelefono()
        {
            TelefonosFrecuentesModels telefono = new TelefonosFrecuentesModels();
            telefono.Accion = Framework.EnumAccionEntity.Alta;
            return PartialView("_MantenimientoTelefonos", telefono);
        }

        public ActionResult BorrarTelefono(string numTelefono)
        {

            TelefonosService serviceTelefonos = new TelefonosService();
            var telefono = serviceTelefonos.GetTelefono(numTelefono);
            telefono.Accion = Framework.EnumAccionEntity.Baja;

            return PartialView("_BorrarTelefono", telefono);
        }

        public ActionResult EditarTelefono(string numTelefono)
        {

            TelefonosService serviceTelefonos = new TelefonosService();
            var telefono = serviceTelefonos.GetTelefono(numTelefono);
            telefono.Accion = Framework.EnumAccionEntity.Modificacion;

            return PartialView("_MantenimientoTelefonos", telefono);
        }


        [ActionName("GuardarTelefono")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarTelefono(TelefonosFrecuentesModels modelo)
        {
            TelefonosService serviceTelefonos = new TelefonosService();

            if (modelo.Accion == Framework.EnumAccionEntity.Alta || modelo.Accion == Framework.EnumAccionEntity.Modificacion)
            {
                serviceTelefonos.SaveTelefono(modelo);
            }
            else if (modelo.Accion == Framework.EnumAccionEntity.Baja)
            {
                serviceTelefonos.DeleteTelefono(modelo.NUMERO_TELEFONO);
            }

            return Json("Success", JsonRequestBehavior.AllowGet);
        }


        #region CargaDatatable
        public ActionResult TelefonosJson()
        {
            List<int> empresas = new UsersService().GetEmpresasUsuario().ToList();
            var telefonos = new TelefonosService().GetAllTelefonos(empresas);

            var data = new
            {
                data = telefonos,
                draw = 1,
                recordsFiltered = telefonos.Count,
                recordsTotal = telefonos.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}