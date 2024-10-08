using System;
using System.Linq;
using System.Web.Mvc;
using TK_ECAR.Utils;
using Microsoft.AspNet.SignalR;
using System.IO;
using System.Collections.Generic;
using TK_ECAR.Framework.Models;
using TK_ECAR.Framework.PDF;
using System.Configuration;
using System.Globalization;
using TK_ECAR.Models;
using static TK_ECAR.Utils.FileUtilities;

namespace TK_ECAR.Controllers
{
    public class FacturacionController : BaseController
    {
        private IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<ECAR_ProgressHub>();

        public ActionResult Index()
        {
            Session["incidencias"] = new ResumenImportacionModels();
            ((ResumenImportacionModels)Session["incidencias"]).ListadoResumen = new List<Incidencia>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GenerarFacturacion(GeneracionFacturacionModels modelo)
        {
             var result = "OK";

            List<FacturaModels> FacturasLeasing = new List<FacturaModels>();
            List<FacturaRepartoModels> RepartoFacturasLeasing = new List<FacturaRepartoModels>();

            Session["incidencias"] = new ResumenImportacionModels();

            if (!new GlobalProcesosSignalR().GenerarFacturacion(modelo, ((ResumenImportacionModels)Session["incidencias"]), hubContext, 
                                                                FacturasLeasing, RepartoFacturasLeasing))
            {
                result = "ERROR";
            }
            else
            {
                if (FacturasLeasing.Count > 0)
                {
                    //Se ordenan las listas de facturas y reparto.
                    Session["FacturasLeasing"] = FacturasLeasing.OrderBy(x=>x.NombreEmpresaFatura).ThenBy(x => x.Canarias).ThenBy(x => x.NumeroFactura).ToList();
                    Session["RepartoFacturasLeasing"] = RepartoFacturasLeasing.OrderBy(x => x.IDEmpresaFatura).ThenBy(x => x.Canarias).ThenBy(x => x.NombreDelegacionFatura).ThenBy(x => x.NombreCentroCoste).ThenBy(x => x.Matricula).ToList();
                }
                else
                {
                    result = "EMPTY";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GeneraArchivosFacturacion()// GeneracionFacturacionModels modelo
        {
            var result = "OK";

            Session["NombreArchivosFacturacion"] = null;
            Session["NombreArchivoRepartoFacturas"] = null;
            Session["NombreArchivoRepartoSAP"] = null;
            Session["DirectorioArchivosFacturacion"] = null;
            Session["MesFacturacion"] = null;

            try
            {
                if (Session["FacturasLeasing"] != null)
                {
                    if (((List<FacturaModels>)Session["FacturasLeasing"]).Count > 0)
                    {
                        List<string> ListaNombresArchivosFacturacion = new List<string>();
                        List<string> ListaNombresRepartoFacturas = new List<string>();
                        List<string> ListaNombresArchivosRepartoSAP = new List<string>();

                        var Factura = ((List<FacturaModels>)Session["FacturasLeasing"]).FirstOrDefault();
                        Session["DirectorioArchivosFacturacion"] = $"{Global.PathToUploadDocumentFacturacion}/{Factura.AñoMesFactura.Replace(" ", ".")}/";
                        Session["MesFacturacion"] = Factura.AñoMesFactura.Replace(" ", ".");

                        List<int> empresasFactura = ((List<FacturaModels>)Session["FacturasLeasing"]).Select(x => x.IDEmpresaFatura).Distinct().ToList();

                        FileUtilities.DeleteFilesFromDirectory(Session["DirectorioArchivosFacturacion"].ToString());

                        foreach (int emp in empresasFactura)
                        {
                            //Factura-Asientos.
                            var facturaPDF = new FacturaPDFGenerator().ExportFacturaPDF(
                                ((List<FacturaModels>)Session["FacturasLeasing"]).Where(x => x.IDEmpresaFatura == emp).OrderBy(x => x.NumeroFactura).ToList(), PDFConstants.OrientationPDF.VERTICAL,
                                $"{ConfigurationManager.AppSettings["baseUrl"]}/Content/img/layout/Logo-TK_2x_mobile_blue.png",
                                CultureInfo.CurrentCulture);
                            facturaPDF.Position = 0;
                            var NombreArchivoFactura = $"Asientos_{emp}_{Factura.NombreEmpresaLeasing}_Mes_{Factura.AñoMesFactura.Replace(" ", ".")}.pdf";
                            ListaNombresArchivosFacturacion.Add(NombreArchivoFactura);
                            FileUtilities.SaveFileFromMemoryStream(facturaPDF, Session["DirectorioArchivosFacturacion"].ToString(), NombreArchivoFactura);
                            //******Factura-Asientos.

                            //Factura-Reparto.
                            var repartoDF = new FacturaPDFGenerator().ExportRepartofacturaPDF(
                                ((List<FacturaRepartoModels>)Session["RepartoFacturasLeasing"]).Where(x => x.IDEmpresaFatura == emp).OrderBy(x => x.NumFactura).ToList(), PDFConstants.OrientationPDF.VERTICAL,
                                $"{ConfigurationManager.AppSettings["baseUrl"]}/Content/img/layout/Logo-TK_2x_mobile_blue.png",
                                CultureInfo.CurrentCulture);
                            repartoDF.Position = 0;
                            var NombreArchivoReparto = $"Reparto_{emp}_{Factura.NombreEmpresaLeasing}_Mes_{Factura.AñoMesFactura.Replace(" ", ".")}.pdf";
                            ListaNombresRepartoFacturas.Add(NombreArchivoReparto);
                            FileUtilities.SaveFileFromMemoryStream(repartoDF, Session["DirectorioArchivosFacturacion"].ToString(), NombreArchivoReparto);
                            //******Factura-Reparto.

                            //Factura-SAP.
                            var repartoSAP = new FacturaPDFGenerator().ExportRepartofacturaSAP_TXT(((List<FacturaRepartoModels>)Session["RepartoFacturasLeasing"]).Where(x => x.IDEmpresaFatura == emp).OrderBy(x => x.NumFactura).ToList());
                            repartoSAP.Position = 0;
                            var NombreArchivoRepartoSAP = $"SAP_ECAR_{emp}_{Factura.NombreEmpresaLeasing}_Mes_{Factura.AñoMesFactura.Replace(" ", ".")}.txt";
                            ListaNombresArchivosRepartoSAP.Add(NombreArchivoRepartoSAP);
                            FileUtilities.SaveFileFromMemoryStream(repartoSAP, Session["DirectorioArchivosFacturacion"].ToString(), NombreArchivoRepartoSAP);
                            //******Factura-SAP.
                        }
                        Session["NombreArchivosFacturacion"] = ListaNombresArchivosFacturacion;
                        Session["NombreArchivoRepartoFacturas"] = ListaNombresRepartoFacturas;
                        Session["NombreArchivoRepartoSAP"] = ListaNombresArchivosRepartoSAP;

                        //using (FileStream file = new FileStream("c://borrar//borrar//repartoPDF_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf", FileMode.Create, FileAccess.Write))
                        //{
                        //    repartoDF.Position = 0;
                        //    repartoDF.WriteTo(file);
                        //}
                    }
                }
                else
                {
                    result = "SESSIONEMPTY";
                }

            }

            catch (Exception ex)
            {
                result = $"ERROR {ex.Message}";
            }
            //TODO Generar PDFs de facturas, reparto y archivos txt para SAP. Guradrlos en un directorio.
            //Los archivos para SAP se podrían guardar en variable de sesión del tipo FileContentResult.
            // Para luego leerlo en el método DescargaArchivosFacturacion.
            // O bien, guardarlos en disco y después leerlos.
            //Si diera algún tipo de error, se copiarán en un directorio donde tenga acceso SAP o se bajan y los copia el usuario.
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileResult DescargaArchivosFacturacion()
        {
            List<EntryFiles> archivosParaBajar = new List<EntryFiles>();

            var nombreArchivoCompleto = string.Empty;

            foreach (string nomArchivo in ((List<string>)Session["NombreArchivosFacturacion"]))
            {
                nombreArchivoCompleto = HttpContext.Server.MapPath($"{Session["DirectorioArchivosFacturacion"].ToString()}/{nomArchivo}");
                archivosParaBajar.Add(new EntryFiles
                {
                    NameEntry = nomArchivo,
                    Path = nombreArchivoCompleto
                });
            }
            foreach (string nomArchivo in ((List<string>)Session["NombreArchivoRepartoFacturas"]))
            {
                nombreArchivoCompleto = HttpContext.Server.MapPath($"{Session["DirectorioArchivosFacturacion"].ToString()}/{nomArchivo}");
                archivosParaBajar.Add(new EntryFiles
                {
                    NameEntry = nomArchivo,
                    Path = nombreArchivoCompleto
                });
            }
            foreach (string nomArchivo in ((List<string>)Session["NombreArchivoRepartoSAP"]))
            {
                nombreArchivoCompleto = HttpContext.Server.MapPath($"{Session["DirectorioArchivosFacturacion"].ToString()}/{nomArchivo}");
                archivosParaBajar.Add(new EntryFiles
                {
                    NameEntry = nomArchivo,
                    Path = nombreArchivoCompleto
                });
            }

            string nombreArchivoZip = $"ArchivosFacturación_{Session["MesFacturacion"].ToString()}";

            var resul = CompressionFiles(archivosParaBajar);

            return File(resul, "application/zip", nombreArchivoZip + ".zip");

            //return File((MemoryStream)Session["ArchivoFacturaPDF"], System.Net.Mime.MediaTypeNames.Application.Pdf, Session["NombreArchivoFacturaPDF"].ToString());
        }

        public JsonResult LimpiarSesionFacturacion()
        {
            var result = "OK";

            Session["incidencias"] = new ResumenImportacionModels();
            Session["FacturasLeasing"] = new List<FacturaModels>();
            Session["RepartoFacturasLeasing"] = new List<FacturaRepartoModels>();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region carga DataTables
        public ActionResult ResumenProcesoFacturacionJson()
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

        public ActionResult DatosResumenFacturasJson()
        {
            var resumenFacturasJson = new List<Framework.Models.FacturaDataTableModels>();

            if (Session["FacturasLeasing"] != null)
            {
                foreach(FacturaModels factura in ((List<FacturaModels>)Session["FacturasLeasing"])
                                                .OrderBy(x=> x.NombreEmpresaLeasing)
                                                .ThenBy(x => x.IDEmpresaFatura)
                                                .ThenBy(x=>x.FechaFactura)
                                                .ThenBy(x=>x.NumeroFactura))
                {
                    var resumenFac = new Framework.Models.FacturaDataTableModels
                    {
                        EmpresaFactura = factura.NombreEmpresaFatura,
                        EmpresaLeasing = factura.NombreEmpresaLeasing,
                        NumFactura = factura.NumeroFactura,
                        FechaFactura = factura.FechaFactura,
                        BaseFactura = factura.Base,
                        ImpuestoFactura = factura.Impuesto,
                        TotalFactura = factura.TotalFactura,
                    };
                    resumenFacturasJson.Add(resumenFac);
                }
            }

            var data = new
            {
                data = resumenFacturasJson,
                draw = 1,
                recordsFiltered = resumenFacturasJson.Count,
                recordsTotal = resumenFacturasJson.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion carga DataTables

    }
}
