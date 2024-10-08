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

namespace TK_ECAR.Controllers
{
    public class ImportarViaVerdeController : BaseController
    {
        private IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<ECAR_ProgressHub>();
        private string msgImportacion = string.Empty;

        public ActionResult Index()
        {
            Session["incidencias"] = new ResumenImportacionModels();
            ((ResumenImportacionModels)Session["incidencias"]).ListadoResumen = new List<Incidencia>();
            ImportacionDatosModels modelo = new ImportacionDatosModels();
            modelo.IDEmpresa = Constants.CODIGO_EMPRESA_PORTUGAL;
            modelo.Empresa = Constants.CODIGO_EMPRESA_PORTUGAL;
            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ImportarDatosViaVerde(ImportacionDatosModels modelo)
        {
            var result = "OK";
            //int fileProgress = 0;

            Session["incidencias"] = new ResumenImportacionModels();
            if (!new GlobalProcesosSignalR().ImportarViaVerde(modelo, ((ResumenImportacionModels)Session["incidencias"]), hubContext))
            {
                result = "ERROR";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ArchivoYaProcesado(string archivo)
        {
            var result = "NO";

            if (new ImportacionPortugalService().Archivo_VIAVERDE_ImportadoConAnterioridad(archivo))
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
