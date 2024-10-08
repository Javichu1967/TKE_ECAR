using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using TK_ECAR.Application_Services.DTOs;
using TK_ECAR.Aspects;
using TK_ECAR.Content.resources;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Framework;
using TK_ECAR.Framework.Exceptions;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;
using TK_ECAR.Utils;
using TKClasesGenericas.Mail;
using TKUtilidades;

namespace TK_ECAR.Application_Services
{

    public class AlertasService
    {

        readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string logonUser
        {
            get { return ((UserModel)Util.GetItemFromMemory("userProfile")).Login; }
        }
        private int PerfiUserSesion
        {
            get { return ((UserModel)Util.GetItemFromMemory("userProfile")).IdPerfil; }
        }

        private int PerfilUser(string logon)
        {
            var usuario = new UsersService().GetUser(logon);
            if (usuario != null)
            {
                return usuario.IdPerfil;
            }
            else
            {
                return 0;
            }
        }

        private string NombreUsuarioByLogin(string logon)
        {
            var usuario = new UsersService().GetUser(logon);
            if (usuario != null)
            {
                return usuario.Nombre;
            }
            else
            {
                return "";
            }
        }
        [SendEmailAlerta]
        public void AddAlerta(AlertaModel modelo)
        {
            var vehiculo = getVehiculoAlerta(modelo.Matricula);

            using (var scope = new TransactionScope())
            {
                using (var unitOfWork = new UnitOfWork())
                {

                    var estadoAccion = unitOfWork.RepositoryT_R_ESTADOS_ACCION.First(modelo.IdTipoAlerta);

                    string descModelo = null;

                    if (vehiculo.Modelo != null)
                    {
                        var modeloVehiculo = unitOfWork.RepositoryT_M_MODELOS_VEHICULO.Fetch().Where(x => x.ID_MODELO == vehiculo.Modelo).FirstOrDefault();
                        if (modeloVehiculo !=null)
                        {
                            descModelo = modeloVehiculo.DESCRIPCION;
                        }
                    }

                    var filename = modelo.FileUpload == null ? null : FileUtilities.GetFileName(modelo.FileUpload);
                    var alerta = new T_G_ALERTAS
                    {
                        ID_TIPO_ALERTA = modelo.IdTipoAlerta,
                        MATRICULA = modelo.Matricula,
                        OBSERVACIONES = modelo.Observaciones,
                        FICHERO = filename,
                        ID_ACCION = estadoAccion.ID_ACCION,
                        ID_ESTADO = estadoAccion.ID_ESTADO,
                        MODELO = descModelo, //vehiculo.T_M_MODELOS_VEHICULO == null ? null : vehiculo.T_M_MODELOS_VEHICULO.DESCRIPCION,
                        ID_CECO = vehiculo.CC,
                        USUARIO_CREACION = logonUser,

                    };



                    switch (modelo.IdTipoAlerta)
                    {
                        case (int)EnumTipoAlerta.Multa:
                            addEntityTypeAlerta((DatosMultaModel)modelo, alerta);
                            break;
                        case (int)EnumTipoAlerta.TarjetaSOLRED:
                            addEntityTypeAlerta((DatosSOLREDModel)modelo, alerta);
                            break;
                        case (int)EnumTipoAlerta.Otras:
                            addEntityTypeAlerta((DatosOtraNotificacionModel)modelo, alerta);
                            break;
                        case (int)EnumTipoAlerta.Robos:
                            addEntityTypeAlerta((DatosRoboModel)modelo, alerta);
                            break;
                        case (int)EnumTipoAlerta.CambioConductor:
                            addEntityTypeAlerta((DatosCambioConductorModel)modelo, alerta);
                            break;
                        default:
                            throw new InvalidOperationException("No se ha podido evaluar el tipo de alerta.");
                    }


                    unitOfWork.RepositoryT_G_ALERTAS.Insert(alerta);

                    unitOfWork.Commit();

                    saveFile(modelo.FileUpload, alerta.ID_ALERTA, filename);



                }
                scope.Complete();

            }
        }


        private ECAR_Datos_Vehiculo getVehiculoAlerta(string matricula)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                return unitOfWork.RepositoryECAR_Datos_Vehiculo.FindOne(x => x.Matricula.Equals(matricula));
            }
        }
        public DatosAlerta GetAlerta(int idAlerta)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                var alerta = unitOfWork.RepositoryT_G_ALERTAS
                    .Include(x => x.T_G_ALERTAS_CAMBIO_CONDUCTOR,
                            x => x.T_G_ALERTAS_MULTA,
                            x => x.T_G_ALERTAS_ROBO,
                            x => x.T_G_ALERTAS_SOLICITUD_SOLRED,
                            x => x.T_G_ALERTAS_RENOVACION_CARNET,
                            x => x.T_G_ALERTAS_RENOVACION_ITV,
                            x => x.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR,
                            x => x.T_G_ALERTAS_OTRAS_NOTIFICACIONES)
                    .FirstOrDefault(x => x.ID_ALERTA.Equals(idAlerta));


                var datosAlerta = new DatosAlerta
                {
                    IdAlerta = alerta.ID_ALERTA,
                    IdTipoAlerta = alerta.ID_TIPO_ALERTA,
                    IdEstado = alerta.ID_ESTADO,
                    Matricula = alerta.MATRICULA,
                    Observaciones = alerta.OBSERVACIONES,
                    Fichero = alerta.FICHERO
                };

                if (alerta.T_G_ALERTAS_CAMBIO_CONDUCTOR != null)
                {
                    datosAlerta.DatosCambioConductor = new DatosCambioConductor
                    {
                        Dni = alerta.T_G_ALERTAS_CAMBIO_CONDUCTOR.DNI,
                        FechaNacimiento = alerta.T_G_ALERTAS_CAMBIO_CONDUCTOR.FECHA_NACIMIENTO,
                        FechaVencimientoCarnet = alerta.T_G_ALERTAS_CAMBIO_CONDUCTOR.FECHA_VENCIMIENTO_CARNET,
                        NumEmpleado = alerta.T_G_ALERTAS_CAMBIO_CONDUCTOR.NUM_EMPLEADO,
                        CodigoPostal = alerta.T_G_ALERTAS_CAMBIO_CONDUCTOR.CP,
                        Domicilio = alerta.T_G_ALERTAS_CAMBIO_CONDUCTOR.DOMICILIO,
                        Nombre = alerta.T_G_ALERTAS_CAMBIO_CONDUCTOR.NOMBRE,
                        Provincia = alerta.T_G_ALERTAS_CAMBIO_CONDUCTOR.PROVINCIA,
                        Poblacion = alerta.T_G_ALERTAS_CAMBIO_CONDUCTOR.POBLACION,
                        Motivo = alerta.T_G_ALERTAS_CAMBIO_CONDUCTOR.MOTIVO,
                        FicheroEmiteRenting = alerta.T_G_ALERTAS_CAMBIO_CONDUCTOR.FICHERO_RENTING,
                        Observaciones = alerta.T_G_ALERTAS_CAMBIO_CONDUCTOR.OBSERVACIONES,
                        FechaEfecto = alerta.T_G_ALERTAS_CAMBIO_CONDUCTOR.FECHA_EFECTO,
                    };
                }
                if (alerta.T_G_ALERTAS_MULTA != null)
                {

                    datosAlerta.DatosMulta = new DatosMulta
                    {
                        Expendiente = alerta.T_G_ALERTAS_MULTA.EXPEDIENTE,
                        FechaDenuncia = alerta.T_G_ALERTAS_MULTA.FECHA_DENUNCIA,
                        Infracion = alerta.T_G_ALERTAS_MULTA.INFRACCION,
                        Lugar = alerta.T_G_ALERTAS_MULTA.LUGAR,
                        Importe = alerta.T_G_ALERTAS_MULTA.IMPORTE,
                        HoraDenuncia = alerta.T_G_ALERTAS_MULTA.HORA_DENUNCIA,
                        MinutosDenuncia = alerta.T_G_ALERTAS_MULTA.MINUTOS_DENUNCIA,
                    };
                }
                if (alerta.T_G_ALERTAS_SOLICITUD_SOLRED != null)
                {
                    datosAlerta.DatosSolicitudSOLRED = new DatosSolicitudSOLRED
                    {
                        FechaSolicitud = alerta.T_G_ALERTAS_SOLICITUD_SOLRED.FECHA_SOLICITUD,
                        IdTipoSolicitud = alerta.T_G_ALERTAS_SOLICITUD_SOLRED.ID_TIPO_SOLICITUD_SOLRED,
                        Descripcion = alerta.T_G_ALERTAS_SOLICITUD_SOLRED.T_M_TIPOS_SOLICITUD_SOLRED.DESCRIPCION,
                    };
                }

                if (alerta.T_G_ALERTAS_ROBO != null)
                {
                    datosAlerta.DatosRobo = new DatosRobo
                    {
                        FECHA_ROBO = alerta.T_G_ALERTAS_ROBO.FECHA_ROBO
                    };
                }
                if (alerta.T_G_ALERTAS_OTRAS_NOTIFICACIONES != null)
                {
                    datosAlerta.DatosOtraNotificacion = new DatosOtraNotificacion
                    {
                        IdTipoClasificacion = alerta.T_G_ALERTAS_OTRAS_NOTIFICACIONES.ID_TIPO_CLASIFICACION,
                        Observaciones = alerta.T_G_ALERTAS_OTRAS_NOTIFICACIONES.OBSERVACIONES,
                        Fichero = alerta.T_G_ALERTAS_OTRAS_NOTIFICACIONES.FICHERO
                    };
                }
                if (alerta.T_G_ALERTAS_RENOVACION_CARNET != null)
                {
                    datosAlerta.DatosRenovacionCarnet = new DatosRenovacionCarnet
                    {
                        Conductor = alerta.T_G_ALERTAS_RENOVACION_CARNET.CONDUCTOR,
                        CodigoConductor = alerta.T_G_ALERTAS_RENOVACION_CARNET.COD_CONDUCTOR,
                        DNI = alerta.T_G_ALERTAS_RENOVACION_CARNET.DNI,
                        FicheroCarnet = alerta.T_G_ALERTAS_RENOVACION_CARNET.FICHERO_CARNET,
                        FechaCaducidadCarnet = alerta.T_G_ALERTAS_RENOVACION_CARNET.FECHA_CADUCIDAD_CARNET,
                    };
                }
                if (alerta.T_G_ALERTAS_RENOVACION_ITV != null)
                {
                    datosAlerta.DatosRenovacionITV = new DatosRenovacionITV
                    {
                        FechaCaducidadITV = alerta.T_G_ALERTAS_RENOVACION_ITV.FECHA_CADUCIDAD_ITV,
                        FechaITV = alerta.T_G_ALERTAS_RENOVACION_ITV.FECHA_ITV,
                        FicheroITV = alerta.T_G_ALERTAS_RENOVACION_ITV.FICHERO_ITV,
                    };
                }
                if (alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR != null)
                {
                    datosAlerta.ConductorConfirmadoMulta = new ConductorConfirmadoMulta
                    {
                        AutorizacionPermiso = alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.B_AUTORIZACION_PERMISO,
                        CodigoPostal = alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.COD_POSTAL,
                        ValidezPermisoESP = alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.B_VALIDEZ_PERMISO,
                        Domicilio = alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.DOMICILIO,
                        NacionalidadPermiso = alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.NACIONALIDAD_PERMISO,
                        NumPermisoConducir = alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.NUM_PERMISO_CONDUCIR,
                        Nombre = alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.NOMBRE,
                        DNI = alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.DNI,
                        Poblacion = alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.POBLACION,
                        Provincia = alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.PROVINCIA,
                        Pais = alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.PAIS,
                        FicheroCarnet = alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.FICHERO_CARNET,

                    };
                }
                if (alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR != null)
                {
                    datosAlerta.ConductorConfirmadoRenting = new ConductorConfirmadoRenting
                    {
                        CodigoConductor = alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.COD_CONDUCTOR,
                        CodigoPostal = alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.CP,
                        DNI = alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.DNI,
                        FechaNacimiento = alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.FECHA_NACIMIENTO,
                        FechaVencimientoCarnet = alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.FECHA_VENCIMIENTO_CARNET,
                        Direccion = alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.DIRECCION,
                        Nombre = alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.NOMBRE,
                        Poblacion = alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.POBLACION,
                        Provincia = alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.PROVINCIA,

                    };
                }


                return datosAlerta;
            }
        }


        private void saveFile(HttpPostedFileBase file, int idalerta, string namefile)
        {
            if (file != null)
            {
                var path = Global.PathToUploadDocumentAlertas + idalerta;

                FileUtilities.UploadFile(file, path, namefile);

            }
        }

        public List<AlertaDataTableModel> GetAlertasPendientes(List<string> codigoCecos, bool panelInicial)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var query = unitOfWork.RepositoryT_G_ALERTAS.GetPendientes(codigoCecos);

                return getListAlertasDataTable(unitOfWork, query, panelInicial);
            }
        }

        public List<AlertaDataTableModel> GetAlertas(List<string> codigoCecos, List<int> estados = null, List<int> tipos = null, List<int> acciones = null, DateTime? FechaDesde = null, DateTime? FechaHasta = null)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                T_G_ALERTASSpecification spec = new T_G_ALERTASSpecification
                {
                    ID_CECOIN = codigoCecos,
                    ID_ESTADOIN = estados != null ? estados.Cast<int?>() : null,
                    ID_TIPO_ALERTAIN = tipos != null ? tipos.Cast<int?>() : null,
                    ID_ACCIONIN = acciones != null ? acciones.Cast<int?>() : null,
                };
                //var query = new IQueryable<T_G_ALERTAS>();

                var query = unitOfWork.RepositoryT_G_ALERTAS.Where(spec);
                if (FechaDesde != null)
                {
                    query = query.Where(x => x.FECHA_CREACION >= FechaDesde);
                }
                if (FechaHasta != null)
                {
                    query = query.Where(x => x.FECHA_CREACION <= FechaHasta);
                }
                return getListAlertasDataTable(unitOfWork, query);
            }
        }

        private List<AlertaDataTableModel> getListAlertasDataTable(UnitOfWork unitOfWork, IQueryable<T_G_ALERTAS> query, bool panelInicial = false)
        {
            var alertas = (from alerta in query
                           join
                           ceco in unitOfWork.RepositoryV_ALERTAS_CECOS_DELEGACION.Fetch() on alerta.ID_ALERTA equals ceco.ID_ALERTA
                           //join
                           //perfil in unitOfWork.RepositoryT_R_ESTADOS_ACCION.Fetch() on alerta.ID_TIPO_ALERTA equals perfil.ID_TIPO_ALERTA
                           //where perfil.ID_ACCION == alerta.ID_ACCION && perfil.ID_ESTADO == alerta.ID_ESTADO

                           let perfil = alerta.T_M_ACCIONES.T_R_ESTADOS_ACCION.Where(x => x.ID_TIPO_ALERTA == alerta.ID_TIPO_ALERTA
                                                && x.ID_ACCION == alerta.ID_ACCION && x.ID_ESTADO == alerta.ID_ESTADO).FirstOrDefault()


                           let hasConductorMulta = alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR != null && alerta.ID_ESTADO != (int)EnumEstadoAlerta.Cancelada
                           let hasConductorRenting = alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR != null && alerta.ID_ESTADO != (int)EnumEstadoAlerta.RentingRechazado

                           select new AlertaDataTableModel
                           {
                               Prioridad = alerta.T_M_TIPOS_ALERTAS.PRIORIDAD,
                               Matricula = alerta.MATRICULA,
                               Modelo = alerta.MODELO,
                               Ceco = alerta.ID_CECO,
                               FechaVencimiento = alerta.FECHA_VENCIMIENTO,
                               IdTipoAlerta = alerta.ID_TIPO_ALERTA,
                               Alerta = alerta.T_M_TIPOS_ALERTAS.DESCRIPCION,
                               Accion = alerta.T_M_ACCIONES.DESCRIPCION ?? string.Empty,
                               Estado = alerta.T_M_ESTADOS.DESCRIPCION,
                               IdAlerta = alerta.ID_ALERTA,
                               IdAccion = alerta.ID_ACCION,
                               ConductorConfirmado = hasConductorMulta ? alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.NOMBRE :
                                                hasConductorRenting ? alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.NOMBRE : null,

                               IdEstado = alerta.T_M_ESTADOS.ID_ESTADO,
                               //se puede eliminar si se encuentra en su estado inicial y el usuario que la creo es el mismo que el usuario que consulta
                               EnableCancelar = alerta.T_G_HIST_ALERTAS.Count == 1 &&
                                                !alerta.T_M_TIPOS_ALERTAS.B_AUTOMATICA &&
                                        alerta.USUARIO_CREACION == logonUser,
                               EnableRechazar = alerta.T_G_HIST_ALERTAS.Count == 1 &&
                                                !alerta.T_M_TIPOS_ALERTAS.B_AUTOMATICA &&
                                                PerfiUserSesion == (int)EnumTipoPerfil.SuperUsuario,
                               UsuarioCreacion = alerta.USUARIO_CREACION,
                               EstadoInicial = alerta.T_G_HIST_ALERTAS.Count == 1,
                               Automatica = alerta.T_M_TIPOS_ALERTAS.B_AUTOMATICA,
                               FechaCreacion = alerta.FECHA_CREACION,
                               idDelegacion = ceco.NUM_DEL,
                               DescDelegacion = ceco.NOMBRE_DELEGACION,
                               Sociedad = alerta.ECAR_Datos_Vehiculo.Sociedad,
                               IdPerfil = perfil.ID_PERFIL == null ? 0 : (int)perfil.ID_PERFIL,

                           });


            var result = alertas.OrderBy(x => x.Prioridad).ThenByDescending(x => x.IdAlerta).ToList();

            if (!panelInicial) //Le pongo esto para que, en la carga inicial, no haga este proceso y sea mas ágil.
            {
                Dictionary<string, string> usuariosCreacion = new Dictionary<string, string>();
                foreach (AlertaDataTableModel alerta in result)
                {
                    if (alerta.IdAccion != null)
                    {
                        alerta.IdPerfil = (int)unitOfWork.RepositoryT_R_ESTADOS_ACCION.Fetch().Where
                            (o => o.ID_TIPO_ALERTA == alerta.IdTipoAlerta &&
                            o.ID_ACCION == (int)alerta.IdAccion &&
                            o.ID_ESTADO == alerta.IdEstado).FirstOrDefault().ID_PERFIL;

                        if (!alerta.EnableCancelar)
                        {
                            alerta.EnableCancelar = alerta.EstadoInicial && !alerta.Automatica &&
                                                    ((alerta.UsuarioCreacion.ToUpper() == logonUser.ToUpper()) ||
                                                    (PerfilUser(alerta.UsuarioCreacion) == (int)EnumTipoPerfil.SuperUsuario &&
                                                    PerfilUser(logonUser) == (int)EnumTipoPerfil.SuperUsuario));
                        }
                    }
                    if (alerta.Automatica)
                    {
                        alerta.NombreUsuarioCreacion = "Automatica";
                    }
                    else
                    {
                        if (!usuariosCreacion.ContainsKey(alerta.UsuarioCreacion.ToUpper()))
                        {
                            alerta.NombreUsuarioCreacion = NombreUsuarioByLogin(alerta.UsuarioCreacion);
                            usuariosCreacion.Add(alerta.UsuarioCreacion.ToUpper(), alerta.NombreUsuarioCreacion);
                        }
                        else
                        {
                            alerta.NombreUsuarioCreacion = usuariosCreacion[alerta.UsuarioCreacion.ToUpper()];
                        }
                    }
                    //var descDelegacion = string.Empty;
                    //var idDelegacion = string.Empty;
                    //var ceco = unitOfWork.RepositorySAPHR_CentrosCoste.Fetch().Where(x => x.IdCeco.Contains(alerta.Ceco)).FirstOrDefault();
                    //if (ceco != null)
                    //{
                    //    if (string.IsNullOrEmpty(ceco.IdDelegacion))
                    //    {
                    //        descDelegacion = ceco.SAPHR_Empresas.Nombre;
                    //        idDelegacion = ceco.SAPHR_Empresas.CodigoEmpresa.ToString();
                    //    }
                    //    else
                    //    {
                    //        descDelegacion = ceco.SAPHR_Delegaciones.Nombre;
                    //        idDelegacion = ceco.SAPHR_Delegaciones.IdDelegacion;
                    //    }
                    //}
                    //alerta.idDelegacion = idDelegacion;
                    //alerta.DescDelegacion = descDelegacion;
                }
            }
            return result;
        }


        public bool RunAccion(int idAlerta, int idEstado)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var alerta = unitOfWork.RepositoryT_G_ALERTAS.FindOne(x => x.ID_ALERTA.Equals(idAlerta));


                modifiedEstadoAlerta(unitOfWork, alerta, idEstado);


                unitOfWork.Commit();
            }
            return true;
        }

        public bool NotifyRespuesta(DatosOtraNotificacionModel model)
        {
            string paso = "";
            try
            {
                using (var scope = new TransactionScope())
                {

                    using (var unitOfWork = new UnitOfWork())
                    {
                        paso = "Busca alerta";
                        var alerta = unitOfWork.RepositoryT_G_ALERTAS.FindOne(x => x.ID_ALERTA.Equals(model.IdAlerta));

                        paso = "Busca fichero";
                        var namefile = model.FileUploadRespuesta == null ? null : FileUtilities.GetFileName(model.FileUploadRespuesta);

                        paso = "Compara fichero";
                        namefile = compareFileName(alerta.FICHERO, namefile);

                        paso = "Asigna fichero";
                        alerta.T_G_ALERTAS_OTRAS_NOTIFICACIONES.FICHERO = namefile;

                        paso = "Asigna observaciones";
                        alerta.T_G_ALERTAS_OTRAS_NOTIFICACIONES.OBSERVACIONES = model.ObservacionesRespuesta;

                        paso = "Busca estado";
                        var idestado = unitOfWork.RepositoryT_R_ESTADOS_ACCION.GetByAccion(alerta.ID_TIPO_ALERTA, (int)EnumTipoAccion.DarRespuesta).Select(x => x.ID_ESTADO).First();

                        paso = "modifica estado alerta";
                        modifiedEstadoAlerta(unitOfWork, alerta, idestado);

                        paso = "commit";
                        unitOfWork.Commit();

                        paso = "guarda fichero";
                        saveFile(model.FileUploadRespuesta, model.IdAlerta, namefile);
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                logger.Error("ERROR -> " + paso + ". " + ex.Message, ex);
                return false;
            }
            return true;
        }

        public bool NotifyValidarSolicitud(DatosCambioConductorModel model)
        {
            string paso = string.Empty;

            bool valorReturn = true;
            try
            {
                using (var scope = new TransactionScope())
                {

                    using (var unitOfWork = new UnitOfWork())
                    {
                        var alerta = unitOfWork.RepositoryT_G_ALERTAS.FindOne(x => x.ID_ALERTA.Equals(model.IdAlerta));

                        alerta.T_G_ALERTAS_CAMBIO_CONDUCTOR.OBSERVACIONES = model.ObservacionesValidarSolicitud;

                        var namefile = model.FileUploadEmiteRenting == null ? null : FileUtilities.GetFileName(model.FileUploadEmiteRenting);

                        namefile = compareFileName(alerta.FICHERO, namefile);

                        alerta.T_G_ALERTAS_CAMBIO_CONDUCTOR.FICHERO_RENTING = namefile;


                        var idestado = unitOfWork.RepositoryT_R_ESTADOS_ACCION.GetByAccion(alerta.ID_TIPO_ALERTA, (int)EnumTipoAccion.ValidarSolicitud).Select(x => x.ID_ESTADO).First();

                        modifiedEstadoAlerta(unitOfWork, alerta, idestado);

                        paso = $"Buscando vehículo ({alerta.MATRICULA})";
                        //Actualizar el conductor del vehículo en la tabla ECARDatos_Vehículo.
                        ECAR_Datos_Vehiculo vehiculo = unitOfWork.RepositoryECAR_Datos_Vehiculo.FindOne(x => x.Matricula == alerta.MATRICULA);
                        paso = $"Buscando conductor ({model.NumEmpleado.ToString()})";
                        ECAR_Datos_Conductor conductor = unitOfWork.RepositoryECAR_Datos_Conductor.FindOne(x => x.Num_Empleado == model.NumEmpleado.ToString());
                        var conductorInicial = vehiculo.Conductor;
                        vehiculo.Conductor = conductor.Cod_Conductor;
                        unitOfWork.RepositoryECAR_Datos_Vehiculo.Update(vehiculo);

                        paso = $"SaveHistoricoCambiosConductor ({alerta.MATRICULA})";
                        new VehiculoService().SaveHistoricoCambiosConductor(unitOfWork, conductorInicial,
                                                        conductor.Cod_Conductor, alerta.MATRICULA, logonUser);
                        paso = "";


                        unitOfWork.Commit();

                        saveFile(model.FileUploadEmiteRenting, model.IdAlerta, namefile);
                    }

                    scope.Complete();
                }
            }
            catch(Exception ex)
            {
                valorReturn = false;

                if (paso != "")
                {
                    throw new InvalidOperationException($"Error al actualizar ECAR_Datos_Vehiculo. {paso}. {ex.Message}");
                }
                else
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }
            return valorReturn;
        }

        public bool NotifyConductorMulta(ConfirmarConductorMultaModel conductor)
        {
            using (var scope = new TransactionScope())
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var alerta = unitOfWork.RepositoryT_G_ALERTAS.FindOne(x => x.ID_ALERTA.Equals(conductor.IdAlerta));

                    var idestado = unitOfWork.RepositoryT_R_ESTADOS_ACCION.GetByAccion(alerta.ID_TIPO_ALERTA, (int)EnumTipoAccion.IdentificarConductor).Select(x => x.ID_ESTADO).First();

                    modifiedEstadoAlerta(unitOfWork, alerta, idestado);

                    updateEntityConductorMulta(unitOfWork, alerta, conductor);

                    unitOfWork.Commit();

                    saveFile(conductor.FileUploadCarnet, alerta.ID_ALERTA, alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.FICHERO_CARNET);
                }

                scope.Complete();
            }
            return true;
        }

        public bool NotifyConductorRenting(RenovarRentingModel model)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var alerta = unitOfWork.RepositoryT_G_ALERTAS.FindOne(x => x.ID_ALERTA.Equals(model.IdAlerta));


                var idestado = unitOfWork.RepositoryT_R_ESTADOS_ACCION.GetByAccion(alerta.ID_TIPO_ALERTA, (int)EnumTipoAccion.ConfirmarRenting).Select(x => x.ID_ESTADO).First();

                modifiedEstadoAlerta(unitOfWork, alerta, idestado);

                updateEntityConductorRenting(unitOfWork, alerta, model.CodConductor.Value);

                unitOfWork.Commit();
            }
            return true;
        }

        public bool RemoveAlerta(int idAlerta)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var alerta = unitOfWork.RepositoryT_G_ALERTAS.FindOne(x => x.ID_ALERTA.Equals(idAlerta));

                alerta.ID_ESTADO = alerta.ID_TIPO_ALERTA == (int)EnumTipoAlerta.Renting ?
                    (int)EnumEstadoAlerta.RentingRechazado : (int)EnumEstadoAlerta.Cancelada;

                alerta.ID_ACCION = null;

                alerta.USUARIO_MODIFICACION = logonUser;

                unitOfWork.RepositoryT_G_ALERTAS.Update(alerta);

                unitOfWork.Commit();
            }
            return true;
        }





        public void NotifyRenovacion(RenovarModel renovacion)
        {
            using (var scope = new TransactionScope())
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var alerta = unitOfWork.RepositoryT_G_ALERTAS.FindOne(x => x.ID_ALERTA.Equals(renovacion.IdAlerta));

                    HttpPostedFileBase file;
                    string filename;
                    switch (alerta.ID_TIPO_ALERTA)
                    {
                        case (int)EnumTipoAlerta.Carnet:
                            var renovacionCarnet = (RenovarCarnetModel)renovacion;

                            file = renovacionCarnet.FileUpload;

                            updateEntityRenovacionCarnet(unitOfWork, renovacionCarnet, alerta);

                            filename = alerta.T_G_ALERTAS_RENOVACION_CARNET.FICHERO_CARNET;

                            //Actualizar la fecha de caducidad del carnet de conducir del conductor.
                            var conductor = unitOfWork.RepositoryECAR_Datos_Conductor.FindOne(x=>x.Cod_Conductor == renovacionCarnet.CodigoConductor);
                            conductor.Fecha_Vencimiento_Carnet = renovacionCarnet.FechaCaducidadCarnet;
                            unitOfWork.RepositoryECAR_Datos_Conductor.Update(conductor);

                            break;

                        case (int)EnumTipoAlerta.ITV:
                            var renovacionITV = (RenovarITVModel)renovacion;

                            file = renovacionITV.FileUpload;

                            updateEntityRenovacionITV(unitOfWork, renovacionITV, alerta);

                            filename = alerta.T_G_ALERTAS_RENOVACION_ITV.FICHERO_ITV;

                            //Crear nuevo registro en ECAR_Datos_ITV, con las nuevas fechas.
                            ECAR_Datos_ITV dato = new ECAR_Datos_ITV
                            {
                                Ultima_ITV = renovacionITV.FechaITV,
                                Vto_ITV = renovacionITV.FechaCaducidadITV,
                                Fichero = renovacionITV.FileUpload == null ? null : FileUtilities.GetFileName(renovacionITV.FileUpload),
                                TipoArchivo = renovacionITV.FileUpload.ContentType,
                                Matricula = renovacionITV.Matricula,
                                Importe = 0,
                                Impuesto_Circulacion = 0,
                                Pr_Conservacion = 0,
                                Tarifa = 0,
                                Tasa = 0,
                                Otros = TK_ECAR_Resource.ResourceManager.GetString("msgDatoActualizadoDesdeAlerta"),
                                ITV_PASADA = true,
                                FechaModificacion = DateTime.Now,
                                UsuarioModificacion = logonUser,
                                Falta = DateTime.Now,
                                UsuarioAlta = logonUser,
                            };

                            unitOfWork.RepositoryECAR_Datos_ITV.Insert(dato);

                            break;
                        default:
                            throw new InvalidOperationException("No se ha podido evaluar el tipo de renovación.");
                    }


                    var idestado = unitOfWork.RepositoryT_R_ESTADOS_ACCION.GetByAccion(alerta.ID_TIPO_ALERTA, (int)EnumTipoAccion.NotificarRenovacion).Select(x => x.ID_ESTADO).First();

                    modifiedEstadoAlerta(unitOfWork, alerta, idestado);

                    unitOfWork.Commit();

                    saveFile(file, alerta.ID_ALERTA, filename);

                    scope.Complete();
                }
            }
        }
        private void updateEntityRenovacionCarnet(IUnitOfWork unitOfWork, RenovarCarnetModel renovacion, T_G_ALERTAS alerta)
        {

            if (alerta.T_G_ALERTAS_RENOVACION_CARNET == null)
                alerta.T_G_ALERTAS_RENOVACION_CARNET = new T_G_ALERTAS_RENOVACION_CARNET();

            alerta.T_G_ALERTAS_RENOVACION_CARNET.FECHA_CADUCIDAD_CARNET = renovacion.FechaCaducidadCarnet.Value;
            alerta.T_G_ALERTAS_RENOVACION_CARNET.DNI = renovacion.DNI;
            alerta.T_G_ALERTAS_RENOVACION_CARNET.COD_CONDUCTOR = renovacion.CodigoConductor;
            alerta.T_G_ALERTAS_RENOVACION_CARNET.CONDUCTOR = renovacion.Conductor;
            alerta.T_G_ALERTAS_RENOVACION_CARNET.FICHERO_CARNET = renovacion.FileUpload == null ? null : FileUtilities.GetFileName(renovacion.FileUpload);

        }

        private void updateEntityRenovacionITV(IUnitOfWork unitOfWork, RenovarITVModel renovacion, T_G_ALERTAS alerta)
        {
            if (alerta.T_G_ALERTAS_RENOVACION_ITV == null)
                alerta.T_G_ALERTAS_RENOVACION_ITV = new T_G_ALERTAS_RENOVACION_ITV();

            alerta.T_G_ALERTAS_RENOVACION_ITV.FECHA_CADUCIDAD_ITV = renovacion.FechaCaducidadITV.Value;
            alerta.T_G_ALERTAS_RENOVACION_ITV.FECHA_ITV = renovacion.FechaITV.Value;
            //alerta.FICHERO = renovacion.FileUpload == null ? null : FileUtilities.GetFileName(renovacion.FileUpload);
            alerta.T_G_ALERTAS_RENOVACION_ITV.FICHERO_ITV = renovacion.FileUpload == null ? null : FileUtilities.GetFileName(renovacion.FileUpload);
        }



        private void modifiedEstadoAlerta(IUnitOfWork unitOfWork, T_G_ALERTAS alerta, int idestado)
        {
            var estadoAccion = unitOfWork.RepositoryT_R_ESTADOS_ACCION.Next(alerta.ID_TIPO_ALERTA, idestado);

            alerta.ID_ESTADO = estadoAccion.ID_ESTADO;

            alerta.ID_ACCION = estadoAccion.ID_ACCION;

            alerta.USUARIO_MODIFICACION = logonUser;

            unitOfWork.RepositoryT_G_ALERTAS.Update(alerta);

        }

        private void updateEntityConductorMulta(IUnitOfWork unitOfWork, T_G_ALERTAS alerta, ConfirmarConductorMultaModel conductor)
        {

            if (alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR == null)
                alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR = new T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR();

            alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.B_AUTORIZACION_PERMISO = conductor.AutorizacionPermiso;
            alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.B_VALIDEZ_PERMISO = conductor.ValidezPermisoESP;
            alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.DOMICILIO = conductor.Domicilio;
            alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.COD_POSTAL = conductor.CodigoPostal;
            alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.NACIONALIDAD_PERMISO = conductor.NacionalidadPermiso;
            alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.NUM_PERMISO_CONDUCIR = conductor.NumPermisoConducir;
            alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.NOMBRE = conductor.Nombre;
            alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.DNI = conductor.DNI;
            alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.POBLACION = conductor.Poblacion;
            alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.PROVINCIA = conductor.Provincia;
            alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.PAIS = conductor.Pais;


            if (string.IsNullOrEmpty(conductor.FileDownloadCarnet) || alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.FICHERO_CARNET != conductor.FileDownloadCarnet)
            {//se trata de un alta o una modificación sino no hacemos nada

                var namefile = conductor.FileUploadCarnet == null ? null : FileUtilities.GetFileName(conductor.FileUploadCarnet);

                namefile = compareFileName(alerta.FICHERO, namefile);

                alerta.T_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR.FICHERO_CARNET = namefile;

            }

        }

        /// <summary>
        /// Compara los nombre si son diferentes devuelve el namecompare sino devuelve el namecompare renombrado
        /// </summary>
        /// <param name="name">Nombre original ya existente.</param>
        /// <param name="namecompare">Nombre que vamos a comparar por si ya existe.</param>
        /// <returns></returns>
        private string compareFileName(string name, string namecompare)
        {
            //tiene el mismo nombre
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(namecompare)
                        && name.ToUpper().Equals(namecompare.ToUpper()))
            {
                return changeFileName(namecompare);


            }
            else
            { return namecompare; }
        }
        private string changeFileName(string name)
        {

            return Path.GetFileNameWithoutExtension(name) + " (1)" + Path.GetExtension(name);
        }

        private void updateEntityConductorRenting(IUnitOfWork unitOfWork, T_G_ALERTAS alerta, int codigoConductor)
        {

            var conductor = unitOfWork.RepositoryECAR_Datos_Conductor.FindOne(x => x.Cod_Conductor.Equals(codigoConductor));


            if (alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR == null)
                alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR = new T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR();

            alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.COD_CONDUCTOR = conductor.Cod_Conductor;
            alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.CP = conductor.Cod_Postal;
            alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.DIRECCION = conductor.Direccion;
            alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.DNI = conductor.DNI;
            alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.FECHA_NACIMIENTO = conductor.Fecha_Nacimiento;
            alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.FECHA_VENCIMIENTO_CARNET = conductor.Fecha_Vencimiento_Carnet;
            alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.NOMBRE = conductor.Nombre + " " + conductor.Apellidos;
            alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.POBLACION = conductor.Poblacion;
            alerta.T_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR.PROVINCIA = conductor.Provincia;
        }


        private void addEntityTypeAlerta(DatosMultaModel modelo, T_G_ALERTAS alerta)
        {
            alerta.T_G_ALERTAS_MULTA = new T_G_ALERTAS_MULTA
            {
                EXPEDIENTE = modelo.Expediente,
                FECHA_DENUNCIA = modelo.FechaDenuncia.Value,
                LUGAR = modelo.Lugar,
                INFRACCION = modelo.Infraccion,
                IMPORTE = modelo.Importe,
                HORA_DENUNCIA = Convert.ToInt16(modelo.HoraDenuncia),
                MINUTOS_DENUNCIA = Convert.ToInt16(modelo.MinutosDenuncia),
            };
        }

        private void addEntityTypeAlerta(DatosSOLREDModel modelo, T_G_ALERTAS alerta)
        {
            alerta.T_G_ALERTAS_SOLICITUD_SOLRED = new T_G_ALERTAS_SOLICITUD_SOLRED
            {
                FECHA_SOLICITUD = modelo.FechaSolicitud.Value,
                ID_TIPO_SOLICITUD_SOLRED = modelo.IdMotivoSolicitud.Value,

            };
        }

        private void addEntityTypeAlerta(DatosRoboModel modelo, T_G_ALERTAS alerta)
        {
            alerta.T_G_ALERTAS_ROBO = new T_G_ALERTAS_ROBO
            {
                FECHA_ROBO = modelo.FechaRobo.Value,

            };
        }

        private void addEntityTypeAlerta(DatosOtraNotificacionModel modelo, T_G_ALERTAS alerta)
        {
            alerta.T_G_ALERTAS_OTRAS_NOTIFICACIONES = new T_G_ALERTAS_OTRAS_NOTIFICACIONES
            {
                ID_TIPO_CLASIFICACION = modelo.IdTipoClasificacion.Value,

            };
        }

        private void addEntityTypeAlerta(DatosCambioConductorModel modelo, T_G_ALERTAS alerta)
        {

            alerta.T_G_ALERTAS_CAMBIO_CONDUCTOR = new T_G_ALERTAS_CAMBIO_CONDUCTOR
            {
                NOMBRE = modelo.Empleado,
                POBLACION = modelo.Poblacion,
                PROVINCIA = modelo.Provincia,
                DOMICILIO = modelo.Domicilio,
                DNI = modelo.Dni,
                CP = modelo.CodigoPostal,
                FECHA_NACIMIENTO = modelo.FechaNacimiento,
                FECHA_VENCIMIENTO_CARNET = modelo.FechaVencimientoCarnet,
                NUM_EMPLEADO = modelo.NumEmpleado.Value,
                MOTIVO = modelo.Motivo,
                FECHA_EFECTO = (DateTime)modelo.FechaEfecto,

            };
        }

        public List<SelectItemAlerta> AllTipoAlertas()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                return (from tipoAlerta in unitOfWork.RepositoryT_M_TIPOS_ALERTAS.Fetch()
                        select new SelectItemAlerta
                        {
                            ID = tipoAlerta.ID_TIPO_ALERTA,
                            DESCRPICION = tipoAlerta.DESCRIPCION
                        }).ToList();

            }
        }
        public List<SelectItemAlerta> AllTipoAlertas(string DescripcionstartsWith)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                T_M_TIPOS_ALERTASSpecification spec = new T_M_TIPOS_ALERTASSpecification
                {
                    DESCRIPCIONStartsWith = DescripcionstartsWith,


                };
                return (from tipoAlerta in unitOfWork.RepositoryT_M_TIPOS_ALERTAS.Where(spec)
                        select new SelectItemAlerta
                        {
                            ID = tipoAlerta.ID_TIPO_ALERTA,
                            DESCRPICION = tipoAlerta.DESCRIPCION
                        }).ToList();
            }
        }

        public List<SelectItemAlerta> AllTipoEstadoAlertas(string DescripcionstartsWith, List<int?> tipos = null)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                ISpecification<T_M_ESTADOS> spec = null;

                spec = new T_M_ESTADOSSpecification
                {
                    DESCRIPCIONStartsWith = DescripcionstartsWith,
                    T_R_ESTADOS_ACCION = new T_R_ESTADOS_ACCIONSpecification
                    {
                        ID_TIPO_ALERTAIN = tipos != null ? tipos : null,

                    },
                };
                T_M_TIPOS_ALERTASSpecification specTipos = new T_M_TIPOS_ALERTASSpecification
                {
                    ID_TIPO_ALERTAIN = tipos != null ? tipos : null,


                };

                var tiposAlertas = unitOfWork.RepositoryT_M_TIPOS_ALERTAS.Where(specTipos).ToList();


                if (tiposAlertas.Any(x => !x.B_AUTOMATICA))
                {
                    var specCancelar = new T_M_ESTADOSSpecification
                    {
                        ID_ESTADO = (int)EnumEstadoAlerta.Cancelada
                    };
                    spec = spec.Or(specCancelar);

                }

                if (tiposAlertas.Any(x => x.ID_TIPO_ALERTA == (int)EnumTipoAlerta.Renting))
                {
                    var specRentingRechazado = new T_M_ESTADOSSpecification
                    {
                        ID_ESTADO = (int)EnumEstadoAlerta.RentingRechazado
                    };
                    spec = spec.Or(specRentingRechazado);

                }

                return (from estado in unitOfWork.RepositoryT_M_ESTADOS.Where(spec)
                        select new SelectItemAlerta
                        {
                            ID = estado.ID_ESTADO,
                            DESCRPICION = estado.DESCRIPCION
                        }).ToList();
            }
        }

        public List<SelectItemAlerta> AllTipoAccionAlertas(string DescripcionstartsWith, List<int?> tipos = null, List<int?> estados = null)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                T_M_ACCIONESSpecification spec = new T_M_ACCIONESSpecification
                {
                    DESCRIPCIONStartsWith = DescripcionstartsWith,
                    T_R_ESTADOS_ACCION = new T_R_ESTADOS_ACCIONSpecification
                    {
                        ID_TIPO_ALERTAIN = tipos != null ? tipos : null,
                        ID_ESTADOIN = estados != null ? estados : null,
                    },
                };
                return (from accion in unitOfWork.RepositoryT_M_ACCIONES.Where(spec)
                        select new SelectItemAlerta
                        {
                            ID = accion.ID_ACCION,
                            DESCRPICION = accion.DESCRIPCION
                        }).ToList();
            }
        }

        public List<SelectItemAlerta> GetEstadosPendienteByTipoAlerta(int idtipo)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                //las alertas seguro no se hace ninguna acción pero pueden no debemos mostrar las atendidas
                //sino sus estados iniciales

                return (from x in unitOfWork.RepositoryT_R_ESTADOS_ACCION.Fetch()
                        where x.ID_TIPO_ALERTA.Equals(idtipo)
                        where idtipo != (int)EnumTipoAlerta.Seguro ? x.ID_ACCION.HasValue : !x.ID_ESTADO_ANTERIOR.HasValue
                        //where idtipo != (int)EnumTipoAlerta.Seguro ? x.ID_ACCION.HasValue : x.ID_ESTADO == (int)EnumEstadoAlerta.VencimientoProximo
                        select new SelectItemAlerta
                        {
                            ID = x.ID_ESTADO,
                            DESCRPICION = x.T_M_ESTADOS.DESCRIPCION
                        }).Distinct().ToList();
            }
        }


        public List<SelectItemAlerta> AllMotivoSolicitudSOLRED()
        {
            using (var unitOfWork = new UnitOfWork())
            {

                return (from x in unitOfWork.RepositoryT_M_TIPOS_SOLICITUD_SOLRED.Fetch()
                        select new SelectItemAlerta
                        {
                            ID = x.ID_TIPO_SOLICITUD_SOLRED,
                            DESCRPICION = x.DESCRIPCION
                        }).ToList();
            }
        }
        public List<SelectItemAlerta> AllTiposClasficacion()
        {
            using (var unitOfWork = new UnitOfWork())
            {

                return (from x in unitOfWork.RepositoryT_M_TIPOS_CLASIFICACION.Fetch()
                        select new SelectItemAlerta
                        {
                            ID = x.ID_TIPO_CLASIFICACION,
                            DESCRPICION = x.DESCRIPCION
                        }).ToList();
            }
        }


        public class SelectItemAlerta
        {
            public int ID { get; set; }
            public string DESCRPICION { get; set; }
        }
    }
}