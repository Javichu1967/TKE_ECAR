using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TK_ECAR.Application_Services;
using TK_ECAR.Application_Services.DTOs;
using TK_ECAR.Aspects;
using TK_ECAR.Content.resources;
using TK_ECAR.Framework;

using TK_ECAR.Models;
using TK_ECAR.Utils;

namespace TK_ECAR.Controllers
{
    public class AlertasController : BaseController
    {
        public ActionResult Index(int? idtipoAlerta)
        {
            var ListaPerfiles = Global.GetLiteralesPerfilesSeparadosPorComas();

            ViewBag.ListaPerfilesDescripcion = ListaPerfiles;

            ViewBag.IdTipoAlerta = idtipoAlerta;

            var service = new AlertasService();

            if (idtipoAlerta.HasValue)
            {
                var values = service.GetEstadosPendienteByTipoAlerta(idtipoAlerta ?? 0)
                            .Select(x => x.ID)
                            .ToArray();

                ViewBag.IdTiposEstadosAlerta = string.Join(",", values);
            }
           

            return View();
        }
            

        
        public ActionResult ConductoresJSON(int? IdAlerta)
        {
           
            var conductores = new List<ConductorDataTableModel>();

            //si ya tiene el conductor se le pasa la alerta y
            //devolvemos su conductor 
            if (IdAlerta.HasValue )
            {
                var alerta = new AlertasService().GetAlerta(IdAlerta.Value);

                var conductor = new ConductorDataTableModel
                {
                    Cod_Conductor = alerta.ConductorConfirmadoRenting.CodigoConductor,
                    Cod_Postal = alerta.ConductorConfirmadoRenting.CodigoPostal,
                    Direccion = alerta.ConductorConfirmadoRenting.Direccion,
                    DNI = alerta.ConductorConfirmadoRenting.DNI,
                    Fecha_Nacimiento = alerta.ConductorConfirmadoRenting.FechaNacimiento,
                    Fecha_Vencimiento_Carnet = alerta.ConductorConfirmadoRenting.FechaVencimientoCarnet,
                    Nombre = alerta.ConductorConfirmadoRenting.Nombre,
                    Poblacion = alerta.ConductorConfirmadoRenting.Poblacion,
                    Provincia = alerta.ConductorConfirmadoRenting.Provincia
                };

                conductores.Add(conductor);
            }
            else
            {//sino todos para que seleccione
                conductores = new ConductoresService().AllConductoresDataTable();
            }
            

            var data = new
            {
                data = conductores,
                draw = 1,
                recordsFiltered = conductores.Count,
                recordsTotal = conductores.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);

        }
       
            

        public ActionResult NuevaSolicitud(int tipoAlerta, int baja)
        {
            ViewBag.MatriculasDeBaja = Convert.ToBoolean(baja);

            switch (tipoAlerta)
            {
                case (int)EnumTipoAlerta.Multa:                  
                    return PartialView("_DatosMulta", new DatosMultaModel());
                case (int)EnumTipoAlerta.CambioConductor:
                    return PartialView("_CambioConductor", new DatosCambioConductorModel());
                case (int)EnumTipoAlerta.TarjetaSOLRED:
                    return PartialView("_SolicitudSOLRED", new DatosSOLREDModel());
                case (int)EnumTipoAlerta.Robos:
                    return PartialView("_NotificarRobo", new DatosRoboModel());             
                case (int)EnumTipoAlerta.Otras:
                    return PartialView("_OtraNotificacion", new DatosOtraNotificacionModel());
                default:
                    return new EmptyResult();
            }
            
        }

        [HttpPost]
        [ActionName("Ver")]
        public ActionResult VerAlerta(int idAlerta, bool? modoEdicion = false )
        {

            AlertasService serviceAlertas = new AlertasService();

            var alerta = serviceAlertas.GetAlerta(idAlerta);

            ViewBag.ModoEdicion = modoEdicion;

            switch (alerta.IdTipoAlerta)
            {
                case (int)EnumTipoAlerta.Multa:

                    var modeloMultas = getDatosMultaModel(alerta);
                    return PartialView("_DatosMulta", modeloMultas);

                case (int)EnumTipoAlerta.CambioConductor:

                    var modeloConductor = getDatosCambioConductorModel(alerta);
                    return PartialView("_CambioConductor", modeloConductor);

                case (int)EnumTipoAlerta.TarjetaSOLRED:

                    var modeloSOLRED = getDatosSOLREDModel(alerta);                   
                    return PartialView("_SolicitudSOLRED", modeloSOLRED);

                case (int)EnumTipoAlerta.Robos:

                    var modeloRobo = getDatosRoboModel(alerta);
                    return PartialView("_NotificarRobo", modeloRobo);

                case (int)EnumTipoAlerta.Otras:
                   
                    var model = getDatosOtraNotificacionModel(alerta) ;
                    return PartialView("_OtraNotificacion", model);

                case (int)EnumTipoAlerta.ITV:
                    var modelRenovacionITV = getDatosRenovacionITVModel(alerta);
                    return PartialView("_RenovarITV", modelRenovacionITV);

                case (int)EnumTipoAlerta.Carnet:
                    var modelRenovacionCarnet = getDatosRenovacionCarnetModel(alerta);
                    return PartialView("_RenovarCarnet", modelRenovacionCarnet);

                case (int)EnumTipoAlerta.Renting:
                    var modelRenovacionRenting = getDatosRenovacionRentingModel(alerta);
                    return PartialView("_NotificarRenting", modelRenovacionRenting);
                default:
                    return new EmptyResult();
            }

        }


        private AlertaModel getAlertaModel(DatosAlerta alerta)
        {
            return new DatosOtraNotificacionModel
            {
                 IdAlerta = alerta.IdAlerta,
                 Matricula = alerta.Matricula,
                 Observaciones = alerta.Observaciones,
                 FileDownload =   alerta.Fichero

            };
        }

         
        private DatosMultaModel getDatosMultaModel(DatosAlerta alerta)
        {
            return  new DatosMultaModel
            {
                Expediente = alerta.DatosMulta.Expendiente,
                FechaDenuncia = alerta.DatosMulta.FechaDenuncia,
                HoraDenuncia = alerta.DatosMulta.HoraDenuncia,
                MinutosDenuncia = alerta.DatosMulta.MinutosDenuncia,
                Importe = alerta.DatosMulta.Importe,
                Infraccion = alerta.DatosMulta.Infracion,
                Lugar = alerta.DatosMulta.Lugar,
                Matricula = alerta.Matricula,
                Observaciones = alerta.Observaciones,
                IdAlerta = alerta.IdAlerta,
                FileDownload = alerta.Fichero

            };
        }
        private DatosCambioConductorModel getDatosCambioConductorModel(DatosAlerta alerta)
        {
            return new DatosCambioConductorModel
            {
                
                CodigoPostal = alerta.DatosCambioConductor.CodigoPostal,
                Domicilio = alerta.DatosCambioConductor.Domicilio,
                IdAlerta = alerta.IdAlerta,
                Matricula = alerta.Matricula,
                Observaciones = alerta.Observaciones,
                Dni = alerta.DatosCambioConductor.Dni,
                FechaNacimiento = alerta.DatosCambioConductor.FechaNacimiento,
                FechaVencimientoCarnet = alerta.DatosCambioConductor.FechaVencimientoCarnet,
                NumEmpleado = alerta.DatosCambioConductor.NumEmpleado,
                Empleado = alerta.DatosCambioConductor.Nombre,         
                Poblacion = alerta.DatosCambioConductor.Poblacion,
                Provincia = alerta.DatosCambioConductor.Provincia,               
                FileDownload = alerta.Fichero,
                Motivo = alerta.DatosCambioConductor.Motivo,
                FileDownloadEmiteRenting = alerta.DatosCambioConductor.FicheroEmiteRenting,
                ObservacionesValidarSolicitud = alerta.DatosCambioConductor.Observaciones,
                FechaEfecto = alerta.DatosCambioConductor.FechaEfecto,

            };
        }

        private DatosSOLREDModel getDatosSOLREDModel(DatosAlerta alerta)
        {
            return new DatosSOLREDModel
            {
                FechaSolicitud = alerta.DatosSolicitudSOLRED.FechaSolicitud,
                IdAlerta =alerta.IdAlerta,
                IdMotivoSolicitud = alerta.DatosSolicitudSOLRED.IdTipoSolicitud,
                Matricula = alerta.Matricula,
                Observaciones = alerta.Observaciones,
                FileDownload = alerta.Fichero
            };
        }

        private DatosRoboModel getDatosRoboModel(DatosAlerta alerta)
        {
          
            return new DatosRoboModel
            {
                 FechaRobo = alerta.DatosRobo.FECHA_ROBO,
                 Matricula = alerta.Matricula,
                 IdAlerta = alerta.IdAlerta,
                 Observaciones = alerta.Observaciones,
                FileDownload = alerta.Fichero
            };
        }

        private DatosOtraNotificacionModel getDatosOtraNotificacionModel(DatosAlerta alerta)
        {

            return new DatosOtraNotificacionModel
            {
                IdTipoClasificacion = alerta.DatosOtraNotificacion.IdTipoClasificacion,
                Matricula = alerta.Matricula,
                IdAlerta = alerta.IdAlerta,
                Observaciones = alerta.Observaciones,
                FileDownload = alerta.Fichero,
                FileDownloadRespuesta = alerta.DatosOtraNotificacion.Fichero,
                ObservacionesRespuesta = alerta.DatosOtraNotificacion.Observaciones
            };
        }

        private RenovarITVModel getDatosRenovacionITVModel(DatosAlerta alerta)
        {
            var model = new RenovarITVModel
            {

                IdAlerta = alerta.IdAlerta,
                IdTipoAlerta = alerta.IdTipoAlerta,
                Matricula = alerta.Matricula,
                IdEstado = alerta.IdEstado,
                
            };
            if (alerta.DatosRenovacionITV != null)
            {
                model.FechaCaducidadITV = alerta.DatosRenovacionITV.FechaCaducidadITV;
                model.FechaITV = alerta.DatosRenovacionITV.FechaITV;
                model.ExisteRenovacion = true;
                
                model.FileDownload = alerta.DatosRenovacionITV.FicheroITV;
            }

            return model;
               
        }

        private RenovarCarnetModel getDatosRenovacionCarnetModel(DatosAlerta alerta)
        {
            var model = new RenovarCarnetModel
            {
                
                IdAlerta = alerta.IdAlerta,
                IdTipoAlerta = alerta.IdTipoAlerta,
                Matricula = alerta.Matricula,                
                IdEstado = alerta.IdEstado
            };

            if (alerta.DatosRenovacionCarnet != null)
            {
                model.CodigoConductor = alerta.DatosRenovacionCarnet.CodigoConductor;
                model.Conductor = alerta.DatosRenovacionCarnet.Conductor;
                model.DNI = alerta.DatosRenovacionCarnet.DNI;
                model.FechaCaducidadCarnet = alerta.DatosRenovacionCarnet.FechaCaducidadCarnet;
                model.FileDownload = alerta.DatosRenovacionCarnet.FicheroCarnet;
                model.ExisteRenovacion = true;
            }
            else
            {
                var conductor = new ConductoresService().GetConductor(alerta.Matricula);
                if (conductor == null)
                    throw new ApplicationException("No existe el conductor para la matrícula de la alerta.");
                model.CodigoConductor = conductor.Cod_Conductor;
                model.Conductor = conductor.Nombre;
                model.DNI = conductor.DNI; 
            }
            return model;
           
        }
        private RenovarRentingModel getDatosRenovacionRentingModel(DatosAlerta alerta)
        {
            var model = new RenovarRentingModel
            {

                IdAlerta = alerta.IdAlerta,
                IdTipoAlerta = alerta.IdTipoAlerta,
                Matricula = alerta.Matricula,
                IdEstado = alerta.IdEstado,
                
            };

            if (alerta.ConductorConfirmadoRenting != null)
            {
                model.CodConductor = alerta.ConductorConfirmadoRenting.CodigoConductor;

                model.ExisteRenovacion = true;    
               
            }

            model.Renovar = model.IdEstado == (int)EnumEstadoAlerta.RentingRechazado ?
                false : true;

            
        
            return model;

        }


        [HttpPost]
        public ActionResult VerConfirmarConductorMulta(int idAlerta)
        {

            var alerta = new AlertasService().GetAlerta(idAlerta);
            var modelo = new ConfirmarConductorMultaModel();

            modelo.IdAlerta = alerta.IdAlerta;
            modelo.IdEstado = alerta.IdEstado;

            if (alerta.ConductorConfirmadoMulta != null)
            {
                modelo.AutorizacionPermiso = alerta.ConductorConfirmadoMulta.AutorizacionPermiso ?? true;
                modelo.CodigoPostal = alerta.ConductorConfirmadoMulta.CodigoPostal;
                modelo.ValidezPermisoESP = alerta.ConductorConfirmadoMulta.ValidezPermisoESP ?? true;
                modelo.Domicilio = alerta.ConductorConfirmadoMulta.Domicilio;
                modelo.NacionalidadPermiso = alerta.ConductorConfirmadoMulta.NacionalidadPermiso;
                modelo.NumPermisoConducir = alerta.ConductorConfirmadoMulta.NumPermisoConducir;
                modelo.Nombre = alerta.ConductorConfirmadoMulta.Nombre;
                modelo.DNI = alerta.ConductorConfirmadoMulta.DNI;
                modelo.Poblacion = alerta.ConductorConfirmadoMulta.Poblacion;
                modelo.Provincia = alerta.ConductorConfirmadoMulta.Provincia;
                modelo.Pais = alerta.ConductorConfirmadoMulta.Pais;
                modelo.FileDownloadCarnet = alerta.ConductorConfirmadoMulta.FicheroCarnet;
            }
            else
            {
                var conductor = new ConductoresECARService().GetConductorByMatricula(alerta.Matricula);
                if (conductor != null)
                {
                    modelo.Nombre = conductor.Nombre;
                    modelo.CodigoPostal = conductor.Cod_Postal;
                    modelo.Domicilio = conductor.Direccion;
                    modelo.Poblacion = conductor.Poblacion;
                    modelo.Provincia = conductor.Provincia;
                    modelo.DNI = conductor.DNI;
                    modelo.NumPermisoConducir = conductor.NumeroCarnetConducir;
                }
            }




            return PartialView("_ConfirmarConductorMulta", modelo);


        }

        [HttpPost]   
        [ValidateAntiForgeryToken]         
        public ActionResult GuardarSolicitud(AlertaModel modelo)
        {           
            AlertasService serviceAlertas = new AlertasService();

            serviceAlertas.AddAlerta(modelo);

            return Json("Success"); 
        }


        [HttpPost]
        public ActionResult ReenviarCorreo(int IdAlerta)
        {
            new EmailService().SendEmailAlerta(IdAlerta);

            return Json("Success");
        }

        //private AlertaEmail getAlertaSolicitada(UserModel user)
        //{
        //    using (var unitOfWork = new UnitOfWork())
        //    {


        //        return (from x in unitOfWork.RepositoryT_G_ALERTAS.Fetch()
        //                where x.USUARIO_CREACION.Equals(user.Login)
        //                orderby x.ID_ALERTA descending
        //                select new AlertaEmail
        //                {
        //                    Tipo = x.T_M_TIPOS_ALERTAS.DESCRIPCION,
        //                    Estado = x.T_M_ESTADOS.DESCRIPCION,
        //                    Matricula = x.MATRICULA,
        //                    Accion = x.T_M_ACCIONES.DESCRIPCION,
        //                    Modelo = x.MODELO,
        //                    Ceco = x.ID_CECO,
        //                    Prioridad = x.T_M_TIPOS_ALERTAS.PRIORIDAD,
        //                    Usuario = user.Nombre
        //                }).FirstOrDefault();
        //    }
        //}



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NotificarRespuesta(DatosOtraNotificacionModel modelo)
        {
            AlertasService serviceAlertas = new AlertasService();

            serviceAlertas.NotifyRespuesta(modelo);

            return Json("Success");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ValidarSolicitud(DatosCambioConductorModel modelo)
        {
            AlertasService serviceAlertas = new AlertasService();

            serviceAlertas.NotifyValidarSolicitud(modelo);

            return Json("Success");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarConductorMulta(ConfirmarConductorMultaModel modelo)
        {
        
            var service = new AlertasService();

            service.NotifyConductorMulta(modelo);

            return Json("Success");


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarRenovacion(RenovarModel modelo)
        {
            var service = new AlertasService();

            service.NotifyRenovacion(modelo);

            return Json("Success");
        }

        //public enum EnumVerAlertas
        //{
        //     Pendientes = 0,
        //     Atendidas = 1,
        //     Canceladas = 2
        //}

        public ActionResult AlertasSAPJson(FilterAlertaModel modelo  )
        {
            var nohayFilterSeleccion = modelo.EmpresasSeleccionadas == null
                && modelo.DireccionesTerritorialesSeleccionadas == null
                 && modelo.DelegacionesSeleccionadas == null
                  && modelo.CentrosCosteSeleccionados == null
                  && modelo.AgrupacionesFiltroSeleccionadas == null
                  && modelo.TiposAlertasSeleccionadas == null
                  && modelo.TiposEstadosAlertasSeleccionadas == null
                  && modelo.TiposAccionAlertaSeleccionadas == null
                  && modelo.FechaCreacionAlertaDesde == null
                  && modelo.FechaCreacionAlertaHasta == null;
            var service = new AlertasService();

            List<AlertaDataTableModel> alertas = new List<AlertaDataTableModel>();
            if (!nohayFilterSeleccion)
            {
                
                var cecos = CecosUserByFilter(modelo);


                
                //if (tipoEstado.Equals((int)EnumVerAlertas.Pendientes))
                //{
                //    alertas = service.GetAlertasPendientes(cecos);
                //}
                //else 
                //{
                //    var estados = tipoEstado.Equals((int)EnumVerAlertas.Atendidas) ?
                //        new List<int> { (int)EnumEstadoAlerta.Atendida } :
                //        new List<int> { (int)EnumEstadoAlerta.Cancelada, (int)EnumEstadoAlerta.RentingRechazado };

                //    alertas = service.GetAlertas(cecos, estados);
                //}

                List<int> tiposAlertas = null;
                List<int> tiposEstadosAlertas = null;
                List<int> tiposAccionesAlertas = null;
                if (modelo.TiposAlertasSeleccionadas != null)
                    tiposAlertas = modelo.TiposAlertasSeleccionadas.Split(',').Select(x => int.Parse(x)).ToList();

                if (modelo.TiposEstadosAlertasSeleccionadas != null)
                    tiposEstadosAlertas = modelo.TiposEstadosAlertasSeleccionadas.Split(',').Select(x => int.Parse(x)).ToList();

                if (modelo.TiposAccionAlertaSeleccionadas != null)
                    tiposAccionesAlertas = modelo.TiposAccionAlertaSeleccionadas.Split(',').Select(x => int.Parse(x)).ToList();

                //Global.EscribeLogApp(Global.TipoDeLog.DEBUG, $"Desde {modelo.FechaCreacionAlertaDesde.ToString()} - Hasta {modelo.FechaCreacionAlertaHasta.ToString()}");

                //alertas = true ? service.GetAlertasPendientes(cecos ) :
                alertas = service.GetAlertas(cecos, tiposEstadosAlertas, tiposAlertas, tiposAccionesAlertas, modelo.FechaCreacionAlertaDesde, modelo.FechaCreacionAlertaHasta);
            }
            var data = new
            {
                data = alertas,
                draw = 1,
                recordsFiltered = alertas.Count,
                recordsTotal = alertas.Count
            };

            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(data),
                ContentType = "application/json"
            };

            return result; // Json(data, JsonRequestBehavior.AllowGet);
        }

       

        [HttpPost]
        [ActionName("Confirmar")]     
        [ValidateAntiForgeryToken]  
        public ActionResult ConfirmarRenting(RenovarRentingModel modelo)
        {
         
            var service = new AlertasService();

            if (modelo.Renovar)
            {
                service.NotifyConductorRenting(modelo);
            }
            else { service.RemoveAlerta(modelo.IdAlerta); }

            return Json("Success");

        }


        public ActionResult EjecutarAccion(int idAlerta, int IdEstado)
        {
           
            var service = new AlertasService();

            service.RunAccion(idAlerta, IdEstado);

            return Json("Success");           
        }

        [HttpPost]
        [ActionName("Cancelar")]
        public ActionResult CancelarAlerta(int idAlerta )
        {

            var service = new AlertasService();

            service.RemoveAlerta (idAlerta);

            return Json("Success");
        }

        [HttpPost]
        [ActionName("Rechazar")]
        public ActionResult Rechazar(int idAlerta, string textoRechazo)
        {

            var service = new AlertasService();

            service.RemoveAlerta(idAlerta);

            new EmailService().SendEmailrechazoAlerta(idAlerta, "Rechazada", textoRechazo);

            return Json("Success");
        }

        public FileResult DownloadFile(int idAlerta, string fichero)
        {
            var directorio = Server.MapPath(Global.PathToUploadDocumentAlertas  + idAlerta);

            var path = System.IO.Path.Combine(directorio, fichero);
            
            return File(path, System.Net.Mime.MediaTypeNames.Application.Octet, fichero);
        }

        public ActionResult BuscaConductorByMatricula(string matricula)
        {

            ConductorECARModel conductor = new ConductoresECARService().GetConductorByMatricula(matricula);

            return Json(conductor, JsonRequestBehavior.AllowGet);
        }

        //private class AlertaEmail
        //{
        //    public string Tipo { get; set; }
        //    public string Estado { get; set; }
        //    public string Matricula { get; set; }
        //    public string Accion { get; set; }
        //    public string Modelo { get; set; }
        //    public string Ceco { get; set; }
        //    public int Prioridad { get; set; }

        //    public string Usuario { get; set; }

        //}

    }



}
