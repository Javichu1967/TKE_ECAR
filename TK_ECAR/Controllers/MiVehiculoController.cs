using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Models;
using TK_ECAR.Application_Services;
using System.IO;
using TK_ECAR.Utils;
using TK_ECAR.Framework;

namespace TK_ECAR.Controllers
{
    public class MiVehiculoController : BaseController
    {
        public ActionResult Index()
        {
            DatosVehiculoModel datosVista = new DatosVehiculoModel();
            string Matricula = new MiVehiculoService().GetMatriculaVehiculoActivo(UserModel.Dni); //40965660-S

            if (!string.IsNullOrEmpty(Matricula))
            {
                var miVehiculo = new FlotaService().GetDatosVehiculo(Matricula);
                datosVista = miVehiculo;
            }

            ViewData["Matricula"] = Matricula;

            return View(datosVista);
        }

        public PartialViewResult VerVehiculo(string matricula)
        {

            ViewBag.FichaConductor = false;

            var miVehiculo = new FlotaService().GetDatosVehiculo(matricula);

            ViewData["Matricula"] = matricula;
            ViewData["Sociedad"] = miVehiculo.DatosGenerales_Vehiculo.Empresa;

            return  PartialView("_Vehiculo", miVehiculo) ;
        }

        #region DocumentosVehiculo
        public ActionResult NuevoDocumentoVehiculo(string matricula)
        {
            DatosVehiculoDocumentacionModel documentacionVehiculo = new DatosVehiculoDocumentacionModel();
            documentacionVehiculo.Matricula = matricula;
            return PartialView("_DocumentacionVehiculoAlta", documentacionVehiculo);
        }

        public FileResult DowmnLoadDocumentoVehiculo(int idDocumento, string matricula, int idAlerta, int idCatergoria)
        {
            string fileToDownload = string.Empty;
            if (idCatergoria == (int)EnumTipoAlerta.ITV)
            {
                var documento = new VehiculoService().GetListDatosITV_ECAR(matricula).Where(x=> x.ID == idDocumento).FirstOrDefault();
                fileToDownload = $"{Global.PathToUploadDocumentITV}{matricula}/{documento.Documento}";
            }
            else if (idAlerta == 0)
            {
                var documentos = new FlotaService().GetDatosDocumentacionVehiculo(idDocumento, matricula);
                fileToDownload = Global.GetPathToUploadDocumentMiVehiculo(matricula, "-", "_") + documentos.Documento;
            }
            else
            {
                var alerta = new AlertasService().GetAlerta(idAlerta);
                fileToDownload = Global.PathToUploadDocumentAlertas + idAlerta.ToString() + "/" + alerta.Fichero;
            }

            //byte[] fileBytes = System.IO.File.ReadAllBytes(@documentos.Ruta);

            return File(System.Web.HttpContext.Current.Server.MapPath(fileToDownload), System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(System.Web.HttpContext.Current.Server.MapPath(fileToDownload)));
        }

        public ActionResult CompruebaDocumentoVehiculo(string nombreArchivo, string matricula)
        {
            bool valorReturn = false;

            valorReturn = FileUtilities.ExisteDocumentoToUploadEnDisco(Global.GetPathToUploadDocumentMiVehiculo(matricula, "-", "_") + nombreArchivo);


            return Json(valorReturn, JsonRequestBehavior.AllowGet);
        }

        #region mantenimiento documentación
        [HttpPost]
        public ActionResult BorrarDocumentoVehiculo(int idDocumento, string matricula)
        {
            FlotaService flota = new FlotaService();

            flota.DeleteDocumentacion(idDocumento, matricula);

            return Json("Success");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GrabaDocumentoVehiculo(DatosVehiculoDocumentacionModel modelo)
        {
            
            FlotaService flota = new FlotaService();
            
            flota.SaveDocumentacionVehiculo(modelo);

            return Json("Success", JsonRequestBehavior.AllowGet);

            //var miflota = new List<DatosVehiculoDocumentacionModel>();

            //miflota = flota.GetDatosVehiculoDocumentacion(modelo.Matricula).ToList();

            //return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region imagen carnet conducir
        public ActionResult GetImgCarnetConducir(string nombreArchivo, int idAlerta)
        {
            FileStream fs = null;

            var archivoCarnet = Global.PathToUploadDocumentAlertas + idAlerta.ToString() + "/" + nombreArchivo;

            try
            {
                fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath(archivoCarnet), FileMode.Open, FileAccess.Read);
            }

            catch
            {
                fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/Application/CarnetConducirVacio.jpg"), FileMode.Open, FileAccess.Read);
            }
            return File(fs, "image/jpeg");
        }
        #endregion

        #region CargaDatatable
        public ActionResult DocumentacionVehiculoJson(string matricula)
        {
            var miflota = new FlotaService().GetDatosVehiculoDocumentacion(matricula).ToList();

            var data = new
            {
                data = miflota,
                draw = 1,
                recordsFiltered = miflota.Count,
                recordsTotal = miflota.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CargaDatatable
        public ActionResult ConductorJson(string matricula)
        {
            var datos = new FlotaService().GetDatosConductoresVehiculo(matricula).ListaConductores;

            var data = new
            {
                data = datos,
                draw = 1,
                recordsFiltered = datos.Count,
                recordsTotal = datos.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SOLREDJson(string matricula)
        {
            var datos = new FlotaService().GetDatosVehiculoConsumoCombustible(matricula).ToList();

            var data = new
            {
                data = datos,
                draw = 1,
                recordsFiltered = datos.Count,
                recordsTotal = datos.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MultasJson(string matricula)
        {
            var datos = new FlotaService().GetDatosVehiculoMultas(matricula).ToList();

            var data = new
            {
                data = datos,
                draw = 1,
                recordsFiltered = datos.Count,
                recordsTotal = datos.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RentingJson(string matricula)
        {
            var datos = new FlotaService().GetDatosVehiculoLeasePlan(matricula).ToList();

            var data = new
            {
                data = datos,
                draw = 1,
                recordsFiltered = datos.Count,
                recordsTotal = datos.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViaVerdeJson(string matricula)
        {
            var datos = new FlotaService().GetViaVerdeDataTableByMatricula(matricula).LineasDataTable;

            if (datos == null)
            {
                datos = new List<ViaVerdeLineaDatatable>();
            }

            var data = new
            {
                data = datos,
                draw = 1,
                recordsFiltered = datos.Count,
                recordsTotal = datos.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        #endregion

    }
}