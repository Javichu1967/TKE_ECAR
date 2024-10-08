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
    public class PreavisosAlertasController : BaseController
    {
        public ActionResult Index()
        {
            if (Session["PreavisoActualizado"] == null)
            {
                Session["PreavisoActualizado"] = false;
                ViewBag.PreavisoActualizado = false;
            }
            else
            {
                ViewBag.PreavisoActualizado = Convert.ToBoolean(Session["PreavisoActualizado"].ToString());
            }

            var preavisos = new PreavisosAlertasService().GetTiposAlertasAutomaticasDatatable();

            return View(preavisos);
        }

        public ActionResult EditarPreaviso(int idPreaviso)
        {

            PreavisosAlertasService servicePreavisos = new PreavisosAlertasService();
            var tipoalerta = servicePreavisos.GetTipoAlertas(idPreaviso);

            return PartialView("_MantenimientoPreaviso", tipoalerta);
        }


        [ActionName("GuardarPreaviso")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarPreaviso(PreavisosAlertasModel modelo)
        {
            PreavisosAlertasService servicePreavisos = new PreavisosAlertasService();

            servicePreavisos.SaveTipoAlerta(modelo);

            Session["PreavisoActualizado"] = true;

            return Json("Success", JsonRequestBehavior.AllowGet);
        }
    }
}