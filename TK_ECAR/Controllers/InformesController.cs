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

namespace TK_ECAR.Controllers
{
    public class InformesController : BaseController
    {
        private IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<ECAR_ProgressHub>();
        private string msgImportacion = string.Empty;

        public ActionResult InfAltaBajaFechaContrato()
        {
            //Session["incidencias"] = new ResumenImportacionModels();
            //((ResumenImportacionModels)Session["incidencias"]).ListadoResumen = new List<Incidencia>();
            return View("InfAltaBajaFechaContrato");
        }

        [HttpPost]
        public JsonResult InfAltaBajaFechaContrato(FilterInformeFlotaModel modelo)
        {
            var result = "OK";
            MemoryStream msExcel = new MemoryStream();

            Session["InfAltaBajaFechaContrato"] = null;

            try
            {
                var lCeCo = CecosUserByFilter(modelo);
                var vehiculos = new InformeFlotaService().InformeFlotaPeriodoAltaBajaFechaContrato(lCeCo, modelo);
                if (vehiculos.vehiculosLeasing.Count > 0)
                {
                    msExcel = new InformeFlotaService().ExportReportContratosRentingToExcel(vehiculos);
                    msExcel.Position = 0;
                    Session["InfAltaBajaFechaContrato"] = msExcel;

                    //using (FileStream file = new FileStream("c://borrar//borrar//InformeFlota_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx", FileMode.Create, FileAccess.Write))
                    //{
                    //    msExcel.WriteTo(file);
                    //}
                }
                else
                {
                    result = "EMPTY";
                }

            }
            catch (Exception ex)
            {
                result = $"ERROR Se ha producido un error en la obtención del informe. {Environment.NewLine} {ex.Message}";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileResult DescargaInformeAltaBajaFechaContrato()
        {
            MemoryStream ms = (MemoryStream)Session["InfAltaBajaFechaContrato"];
            Session["InfAltaBajaFechaContrato"] = null;
            return File(ms, System.Net.Mime.MediaTypeNames.Application.Octet, "InfFlotaAltaBajaFechaContrato.xlsx");
        }



        //[HttpPost]
        ////public ActionResult ImportarFlotaDesdeExcel(HttpPostedFileBase[] filesUpload)
        //public JsonResult ImportarFlotaDesdeExcel(ImportacionFlotaModels modelo)
        //{
        //    var result = "OK";
        //    //int fileProgress = 0;

        //    Session["incidencias"] = new ResumenImportacionModels();
        //    if (!new GlobalProcesosSignalR().ImportarFlota(modelo, ((ResumenImportacionModels)Session["incidencias"]), hubContext, UserModel.Login))
        //    {
        //        result = "ERROR";
        //    }

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult ValidaIdentificadorUnico(string identificador)
        //{
        //    bool valorReturn = true;

        //    var vehiculo = new VehiculoService().GetVehiculoByIdentificadorImportacion(identificador);

        //    if (vehiculo != null)
        //    {
        //        valorReturn = false;
        //    }


        //    return Json(valorReturn, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult ArchivoYaProcesado(string archivo)
        //{
        //    var result = "NO";

        //    if (new ImportarFlotaService().ArchivoImportadoConAnterioridad(archivo))
        //    {
        //        result = "SI";
        //    }

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}



        //#region carga DataTable
        //public ActionResult DatosImportacionJson()
        //{
        //    var incidenciasJson = ((ResumenImportacionModels)Session["incidencias"]);

        //    var data = new
        //    {
        //        data = incidenciasJson.ListadoResumen,
        //        draw = 1,
        //        recordsFiltered = incidenciasJson.ListadoResumen.Count,
        //        recordsTotal = incidenciasJson.ListadoResumen.Count
        //    };

        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        //#endregion

    }
}
