using System.Linq;
using System.Web.Mvc;
using TK_ECAR.Utils;
using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.IO;
using System.Threading;
using TK_ECAR.Models;
using System.Collections.Generic;
using TK_ECAR.Content.resources;
using TK_ECAR.ImportacionFlota.ApplicationServices;
using TK_ECAR.Framework;
using TK_ECAR.Application_Services;
using TK_ECAR.PortugalImportacion.ApplicationServices;
using TK_ECAR.ApplicationServices;

namespace TK_ECAR.Controllers
{
    public class ImportarLeasingController : BaseController
    {
        private IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<ECAR_ProgressHub>();
        private string msgImportacion = string.Empty;

        public ActionResult Index()
        {
            Session["incidencias"] = new ResumenImportacionModels();
            ((ResumenImportacionModels)Session["incidencias"]).ListadoResumen = new List<Incidencia>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult ImportarFlotaDesdeExcel(HttpPostedFileBase[] filesUpload)
        public JsonResult ImportarFacturacion(ImportacionLeasingModels modelo)
        {
            var result = "OK";
            //int fileProgress = 0;

            Session["incidencias"] = new ResumenImportacionModels();
            if (!new GlobalProcesosSignalR().ImportarLEASING(modelo, ((ResumenImportacionModels)Session["incidencias"]), hubContext))
            {
                result = "ERROR";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ArchivoYaProcesado(string archivo, int empresa)
        {
            var result = "NO";

            //switch (empresa)
            //    case 


            if (new ImportacionLeasingService().Archivo_LEASING_ImportadoConAnterioridad(archivo, empresa))
            {
                result = "SI";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        #region carga DataTable
        public ActionResult DatosImportacionJson()
        {
            var incidenciasJson = ((ResumenImportacionModels)Session["incidencias"]);

            if (incidenciasJson == null)
            {
                incidenciasJson = new ResumenImportacionModels();
            }

            if (incidenciasJson.ListadoResumen == null)
            {
                incidenciasJson.ListadoResumen = new List<Incidencia>();
            }

            var data = new
            {
                data = incidenciasJson.ListadoResumen,
                draw = 1,
                recordsFiltered = incidenciasJson.ListadoResumen.Count,
                recordsTotal = incidenciasJson.ListadoResumen.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
