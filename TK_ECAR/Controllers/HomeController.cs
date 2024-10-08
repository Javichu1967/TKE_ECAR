using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TK_ECAR.Application_Services;
using TK_ECAR.Models;
using TK_ECAR.Infraestructure;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Application_Services.DTOs;
using System.Data.Entity.SqlServer;
using TKUtilidades;
using TK_ECAR.Filters;
using TK_ECAR.Utils;
using System.Globalization;
using System.Threading;
using System.Resources;
using System;
using System.Web;
using System.IO;
using TK_ECAR.Framework.Application_Services;

namespace TK_ECAR.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //List<int> estadosExcluir = new List<int>{ (int)EnumEstadoAlerta.Atendida, (int)EnumEstadoAlerta.Cancelada, (int)EnumEstadoAlerta.RentingRechazado };

            //ViewData["DescripcionPerfilUsuarioActivo"] = EnumUtils<EnumTipoPerfil>.GetDescription((EnumTipoPerfil)UserModel.IdPerfil);

            //Idioma
            string sCulture = Global.IdiomaPorDefecto();
            if (Request.QueryString["culture"] != null)
            {
                sCulture = Request.QueryString["culture"] as string;
            }
            else if (Session != null && Session[Constants.LANG] != null)
            {
                sCulture = Session[Constants.LANG] as string;
            }
            LocalizationAttribute.SetCultureOnThread(sCulture);
            HttpContext.Session[Constants.LANG] = sCulture;

            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;

            //Alertas pendientes
            var perfilUsuario = ((UserModel)Util.GetItemFromMemory("userProfile")).IdPerfil;
            var TextoPerfil = Global.GetLiteralesPerfilesSeparadosPorComas().Split(',').ElementAt(perfilUsuario - 1);

            Util.RemoveItemFromMemory("TextoPerfil");
            Util.AddItemToMemory("TextoPerfil", TextoPerfil);

            var service = new AlertasService();

            List<AlertaDataTableModel> alertasPendientes = service.GetAlertasPendientes(CecosUser, true)
                                        .OrderBy(x => x.IdTipoAlerta).ToList();

            List<SelectListItem> tiposAlerta = (from x in service.AllTipoAlertas()
                                                select new SelectListItem
                                                {
                                                    Text = x.DESCRPICION,
                                                    Value = x.ID.ToString()

                                                }).ToList();

            var totalAlertasPendientesPerfilUsuario = alertasPendientes.Where(x => x.IdPerfil == perfilUsuario).Count();
            List<int> resumenAlertasPendientes = new List<int>();
            List<int> numeroAlertasPendientesTipoAlertaPerfil = new List<int>();
            foreach (var item in tiposAlerta)
            {
                resumenAlertasPendientes.Add(0);
                numeroAlertasPendientesTipoAlertaPerfil.Add(alertasPendientes.Where(x => x.IdPerfil == perfilUsuario && x.IdTipoAlerta.ToString() == item.Value).Count());
            }

            foreach (var item in alertasPendientes)
            {
                resumenAlertasPendientes[item.IdTipoAlerta - 1]++;
            }

            ViewData["totalAlertasPendientesPerfilUsuario"] = totalAlertasPendientesPerfilUsuario;

            ViewData["numeroAlertasPendientesTipoAlertaPerfil"] = numeroAlertasPendientesTipoAlertaPerfil;

            ViewData["ResumenAlertasPendientes"] = resumenAlertasPendientes;

            ViewData["TotalAlertasPendientes"] = alertasPendientes.Count;

            ViewData["TiposAlertas"] = tiposAlerta;

            return View();
        }

        public JsonResult GetMatriculas(string term, int matriculasDeBaja = 0)
        {
            var matriculas = new FlotaService().GetMatriculas(term, CecosUser, Convert.ToBoolean(matriculasDeBaja));

            return Json(matriculas, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMatriculas_Chosen(string term, string empresasSel, string delegacionSel)
        {
            List<string> Cecos = new List<string>();

            if (!string.IsNullOrEmpty(delegacionSel))
            {
                Cecos = UserModel.CecosModelList.Where(x => x.IdDelegacion == delegacionSel).Select(x => x.IdCeco).ToList();
            }
            else if(!string.IsNullOrEmpty(empresasSel))
            {
                var emp = Convert.ToInt32(empresasSel);
                Cecos = UserModel.CecosModelList.Where(x => x.CodigoEmpresa == emp).Select(x => x.IdCeco).ToList();
            }
            else
            {
                Cecos = CecosUser;
            }

            var matriculas = (from matricula in new FlotaService().GetMatriculas(term, Cecos)
                              select new SelectChosen
                              {
                                  PonerValuePorDelanteDeTexto = false,
                                  DevolverValueFormateado = false,
                                  text = matricula,
                                  value = matricula,
                              }).ToList();

            return Json(matriculas, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetTelefonosFrecuentes()
        {
            List<int> empresas = new UsersService().GetEmpresasUsuario().ToList();
            var telefonos = new TelefonosService().GetAllTelefonos(empresas);

            return Json(telefonos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmpleados(string term)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                 
                SAPHR_UsuariosSAPSpecification  spec = new SAPHR_UsuariosSAPSpecification
                {
                    IdCecoIN = CecosUserFormatted,                    

                    Baja = false

                };
                var empleados = unitOfWork.RepositorySAPHR_UsuariosSAP
                    .Where(spec)
                    .Where(x=>  SqlFunctions.StringConvert((decimal?)x.NumeroEmpleado).StartsWith(term))
                    .Select(x=>x.NumeroEmpleado ).ToList();

                return Json(empleados, JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult GetNombreEmpleados(string term)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                SAPHR_UsuariosSAPSpecification spec = new SAPHR_UsuariosSAPSpecification
                {
                    IdCecoIN = CecosUserFormatted,

                    Baja = false,
                    

                };
                var empleados = unitOfWork.RepositorySAPHR_UsuariosSAP
                    .Where(spec)
                    .Where(uSAP => uSAP.IdUniOrganizativa != "50003566" && uSAP.IdDivision != "FE01") //Jubilados
                    .Where(x => (x.Nombre + " " + x.Apellido1 + " " + x.Apellido2).Contains(term))
                    .OrderBy(x => (x.Nombre + " " + x.Apellido1 + " " + x.Apellido2))
                    .Select(x => (x.Nombre + " " + x.Apellido1 + " " + x.Apellido2)).Take(100).ToList();

                return Json(empleados, JsonRequestBehavior.AllowGet);

            }
        }


        public JsonResult GetConductor(string matricula)
        {

            var service = new ConductoresService();
            var conductor = service.GetConductor(matricula);
            return Json(conductor, JsonRequestBehavior.AllowGet);

             
        }
        public JsonResult GetEmpleado(int numEmpleado)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                SAPHR_UsuariosSAPSpecification spec = new SAPHR_UsuariosSAPSpecification
                {
                     NumeroEmpleado = numEmpleado

                };
                var empleado = unitOfWork.RepositorySAPHR_UsuariosSAP
                    .Where(spec)
                    .Select(x => new
                    {
                        Nombre = x.Nombre + " " + x.Apellido1 + " " + x.Apellido2,
                        x.NumeroEmpleado,
                        x.Poblacion,
                        x.Provincia,
                        x.Domicilio,
                        x.Dni,
                        x.FecNacimiento,
                        x.CodPostal,

                    }).FirstOrDefault();
                    

                return Json(empleado, JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult GetNombreEmpleado(string nombreEmpleado)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                //SAPHR_UsuariosSAPSpecification spec = new SAPHR_UsuariosSAPSpecification
                //{
                //    NumeroEmpleado = nombreEmpleado

                //};
                var empleado = unitOfWork.RepositorySAPHR_UsuariosSAP.Fetch()                  
                    .Where(x => (x.Nombre + " " + x.Apellido1 + " " + x.Apellido2).Equals(nombreEmpleado))
                    .Select(x => new
                    {
                        Nombre = x.Nombre + " " + x.Apellido1 + " " + x.Apellido2,
                        x.NumeroEmpleado,
                        x.Poblacion,
                        x.Provincia,
                        x.Domicilio,
                        x.Dni,
                        x.FecNacimiento,
                        x.CodPostal,

                    }).FirstOrDefault();


                return Json(empleado, JsonRequestBehavior.AllowGet);

            }
        }

        public FileResult DescargarPDF()
        {

            string filename = System.Configuration.ConfigurationManager.AppSettings["userManual"];

            string contentType = "application/pdf";

            var path = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["pathManual"]);

            var fileNameServer = System.IO.Path.Combine(path, filename);


            return File(fileNameServer, contentType, filename);
        }


        [ActionName("OpenInc")]
        [HttpPost]
        public ActionResult GenerarIncidencia()
        {


            ServiceIncidentManagement.WSIncidentManagementSoapClient client =
               new ServiceIncidentManagement.WSIncidentManagementSoapClient("WSIncidentManagementSoap");

            var key = System.Configuration.ConfigurationManager.AppSettings["keyApp"];
            var app = System.Configuration.ConfigurationManager.AppSettings["appCode"];


            UserModel user = (UserModel)Util.GetItemFromMemory("userProfile");

            var login = user.Login;
            string mail = user.Email;

            var res = client.GetInfo(app, key, mail, login);

            var url = res.PuedePonerIncidencias ? res.URLNuevaIncidencia : string.Empty;

            return Json(url, JsonRequestBehavior.AllowGet);
           

        }

        [HttpPost]
        public ActionResult CambiaIdioma(string idioma)
        {
            HttpContext.Session[Constants.LANG] = idioma;
            LocalizationAttribute.SetCultureOnThread(idioma);

            return Json("Success");
        }

        #region Incidencia
        /// <summary>
        /// Añade los datos al modal de "Abrir incidencia"
        /// </summary>
        /// <returns></returns>
        public ActionResult AbrirIncidencia()
        {
            SapHrWebApiService servApi = new SapHrWebApiService();

            UserModel user = (UserModel)Util.GetItemFromMemory("userProfile");

            IncidenciaModel model = new IncidenciaModel
            {
                LoginUsuario = user.Login,
                NombreUsuario = user.Nombre,
                CorreoUsuario = user.Email != null && user.Email != "" ? user.Email : servApi.obtenerCorreoUsuario(user.Login),
                NombreAplicacion = System.Configuration.ConfigurationManager.AppSettings["appname"].ToString(),
                CodigoAplicacion = System.Configuration.ConfigurationManager.AppSettings["appCode"].ToString(),
                CorreoCAU = System.Configuration.ConfigurationManager.AppSettings["correoCAU"].ToString()
            };

            return PartialView("_DatosIncidencia", model);
        }

        /// <summary>
        /// Obtenemos los datos del formulario "AbrirIncidencia" y si son correctos enviamos email
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult ComprobarDatosIncidencia(FormCollection form)
        {
            string resultado = "";
            EmailService emailServ = new EmailService();

            var loginUsuario = form["LoginUsuario"] != null ? form["LoginUsuario"] : "";
            var nombreUsuario = form["NombreUsuario"] != null ? form["NombreUsuario"] : "";
            var correoUsuario = form["CorreoUsuario"] != null ? form["CorreoUsuario"] : "";
            var correoCAU = form["CorreoCAU"] != null ? form["CorreoCAU"] : "";
            var nombreApp = form["NombreAplicacion"] != null ? form["NombreAplicacion"] : "";
            var codigoApp = form["CodigoAplicacion"] != null ? form["CodigoAplicacion"] : "";
            var descripcionIncidencia = form["DescripcionIncidencia"] != null ? form["DescripcionIncidencia"] : "";
            var descripcion = descripcionIncidencia.Length > 0 ? descripcionIncidencia.Substring(0, descripcionIncidencia.Length - 1) : "";

            if (descripcion == null || descripcion.Trim() == "")
            {
                return Json("FaltaComentario");
            }

            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["FileToImport"];

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    //ruta de nuestros archivos
                    string rutaArchivos = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["rutaArchivos"]);

                    //si no existe el directorio lo creamos
                    if (!Directory.Exists(rutaArchivos))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(rutaArchivos);
                    }

                    //guardamos el archivo
                    file.SaveAs(rutaArchivos + file.FileName);

                    resultado = emailServ.EnviarEmail(loginUsuario, nombreUsuario, correoUsuario, correoCAU, codigoApp, nombreApp, descripcion, rutaArchivos + file.FileName);

                    //finalmente obtenemos el archivo del directorio y lo borramos
                    string[] filePaths = Directory.GetFiles(rutaArchivos);

                    foreach (string filePath in filePaths)
                    {
                        System.IO.File.Delete(filePath);
                    }

                    return Json(resultado);
                }
                else
                {
                    //Enviamos el correo sin adjuntos
                    resultado = emailServ.EnviarEmail(loginUsuario, nombreUsuario, correoUsuario, correoCAU, codigoApp, nombreApp, descripcion, "");
                    return Json(resultado);
                }
            }
            else
            {
                return Json("Error");
            }
        }
        #endregion Incidencia

    }


}
