using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Models;
using TK_ECAR.Application_Services;
using System.Web.Routing;
using TK_ECAR.Utils;
using TK_ECAR.Domain.Specifications;
using TKUtilidades;
using TK_ECAR.Framework;
using System.Web.Script.Serialization;

namespace TK_ECAR.Controllers
{
    public class MiFlotaController : BaseController
    {
        public ActionResult Index()
        {

            ViewData["OcultarNuevoVehiculo"] = "SI";
            UserModel user = (UserModel)Util.GetItemFromMemory("userProfile");

            if (Global.PermitirMantenimientoVehiculo == "SI" && user.IdPerfil == (int)EnumTipoPerfil.SuperUsuario)
            {
                ViewData["OcultarNuevoVehiculo"] = "NO";
            }
            ViewBag.VerSelectEstadoVehiculo = true;
            return View();
        }

        #region ActionLinks Datatable Flota
        public ActionResult VerVehiculo(string matricula)
        //public PartialViewResult VerVehiculo()
        {

            return RedirectToAction("VerVehiculo", new RouteValueDictionary(
                        new { controller = "MiVehiculo", action = "VerVehiculo", matricula = matricula }));
        }

        public PartialViewResult VerConductor(string matricula)
        {
            ViewBag.FichaConductor = true;

            return PartialView("FichaConductor", new FlotaService().GetDatosConductoresVehiculo(matricula));
        }

        public PartialViewResult VerDatosGenerales(string matricula)
        {
            return PartialView("FichaDatosGenerales", new FlotaService().GetDatosGeneralesVehiculo(matricula));
        }
        #endregion

        #region MtoVehiculos
        public ActionResult NuevoVehiculo()
        {
            //BORRAMOS LOS DATOS QUE HAYA EN TEMP DE ITV.
            new VehiculoService().BorraDatosITV_TMP(UserModel.Login ,false, true);

            DatosVehiculoModel modelo = new DatosVehiculoModel();
            modelo.DatosGenerales_Vehiculo = new DatosGeneralesModel();
            modelo.DatosGenerales_Vehiculo.Empresa = UserModel.IdEmpresaUsuario;
            modelo.DatosGenerales_Vehiculo.IDEmpresa = UserModel.IdEmpresaUsuario;
            modelo.DatosGenerales_Vehiculo.IDEmpresaInicial = null;
            modelo.DatosGenerales_Vehiculo.EsVehiculoDeSustitucion = false;
            modelo.DatosGenerales_Vehiculo.Accion = EnumAccionEntity.Alta;
            ViewBag.AccionEntity = (int)EnumAccionEntity.Alta;
            modelo.DatosContrato_Vehiculo = new DatosContratoModel();

            return PartialView("~/Views/Shared/MantenimientoVehiculo/_MtoVehiculo.cshtml", modelo);
        }

        public ActionResult EditarVehiculo(string matricula)
        {
            //BORRAMOS LOS DATOS QUE HAYA EN TEMP DE ITV.
            new VehiculoService().InicializaDatosITV_TMP(UserModel.Login, matricula);

            DatosVehiculoModel modelo = new VehiculoService().GetDatosVehiculoECAR(matricula);
            modelo.DatosGenerales_Vehiculo.Accion = EnumAccionEntity.Modificacion;
            ViewBag.AccionEntity = (int)EnumAccionEntity.Modificacion;

            return PartialView("~/Views/Shared/MantenimientoVehiculo/_MtoVehiculo.cshtml", modelo);
        }

        [ActionName("GuardarVehiculo")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarVehiculo(FormCollection collection)
        {
            var matriculaInicial = collection["MatriculaInicial"];
            var tarjetaInicial = Convierte_a_Int(collection["IDTarjetaCombustibleInicial"]);

            DatosVehiculoModel modelo = RellenaModeloDatosVehiculo(collection);

            if (new VehiculoService().SaveVehiculoECAR(modelo, UserModel.Login) != EnumResultadoEntity.GrabacionCorrecta)
            {
                return Json("ERROR", JsonRequestBehavior.AllowGet);
            }

            return Json("OK", JsonRequestBehavior.AllowGet);
        }


        public ActionResult NuevaITV()
        {
            DatosITV_TMPModel modelo = new DatosITV_TMPModel();
            modelo.Linea = -1;
            modelo.LineaNueva = true;
            modelo.Accion = EnumAccionEntity.Alta;
             
            return PartialView("~/Views/Shared/MantenimientoVehiculo/_MtoITV.cshtml", modelo);
        }


        [ActionName("GuardarITV")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarITV(DatosITV_TMPModel modelo)
        {
            var result = "OK";

            if (!new VehiculoService().SaveDatosITV_TMP(modelo, UserModel.Login))
            {
                result = "ERROR";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditarLineaTMP_ITV(int linea, string login, int accion)
        {
            DatosITV_TMPModel modelo = new DatosITV_TMPModel();

            modelo = new VehiculoService().GetLineaITV_TMP(linea, login);
            modelo.Accion = (EnumAccionEntity)accion;

            return PartialView("~/Views/Shared/MantenimientoVehiculo/_MtoITV.cshtml", modelo);
        }


        public ActionResult BorraLineaTMP_ITV(int linea, string login)
        {
            var result = "OK";

            if (!new VehiculoService().BorraLineaITV_TMP(linea, login))
            {
                result = "ERROR";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult VerificaMatricula(string matricula)
        {
            var result = "Inexistente";

            var vehiculo = new VehiculoService().GetDatosGeneralesVehiculoECAR(matricula);

            if (vehiculo != null)
            {
                result = "Existente";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Chosen Datos Generales
        public JsonResult GetTarjetaCombustibleByID_Chosen(int? ID)
        {
            var seleccion = new List<SelectChosen>();

            var valorSeleccion = new TarjetasCombustibleService().GetTarjetaByID(Convert.ToInt32(ID));

            if (valorSeleccion != null)
            {
                seleccion.Add(new SelectChosen
                {
                    DevolverValueFormateado = true,
                    PonerValuePorDelanteDeTexto = false,
                    text = valorSeleccion.CodTarjeta,
                    value = valorSeleccion.IDTarjeta.ToString(),
                });
            }
            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTarjetasCombustible_Chosen(string term, int ?idEmpresa)
        {
            var seleccion = (from tarjeta in new TarjetasCombustibleService().AlltarjetasDataTable(new List<int?>(new int?[] { idEmpresa }))
                             where tarjeta.CodTarjeta.ToUpper().Contains(term)
                             select (new SelectChosen
                             {
                                 DevolverValueFormateado = true,
                                 PonerValuePorDelanteDeTexto = false,
                                 text = tarjeta.CodTarjeta,
                                 value = tarjeta.IDTarjeta.ToString(),
                             })).ToList();

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmpresaVehiculoByID_Chosen(int? ID)
        {
            var seleccion = new EmpresasVehiculosService().GetEmpVehiculoByID_Chosen(Convert.ToInt32(ID));

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetEmpresaVehiculo_chosen(string term, int? TipoEmpresa)
        {
            var seleccion = new EmpresasVehiculosService().GetEmpVehiculo_Chosen(term, Convert.ToInt32(TipoEmpresa));

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetConductorByID_Chosen(int? ID)
        {
            List<SelectChosen> seleccion = new List<SelectChosen>();

            var conductor = new ConductoresService().GetConductorByID(Convert.ToInt32(ID));

            if (conductor != null)
            {
                seleccion.Add(new SelectChosen
                {
                    DevolverValueFormateado = false,
                    PonerValuePorDelanteDeTexto = false,
                    text = conductor.Nombre,
                    value = conductor.Cod_Conductor.ToString(),
                });
            }

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetConductores_Chosen(string term)
        {
            var seleccion = new ConductoresService().GetConductoresChosen(term);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartamentoByID_Chosen(string ID)
        {
            var seleccion = new VehiculoService().GetDepartamentosByIDChosen(ID);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartamentoByEmpresa(string term, int idEmpresa)
        {
            var seleccion = new VehiculoService().GetAllDepartamentosChosen(new List<int>(new int[] { idEmpresa }), term);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetTipoRutaByID_Chosen(int? ID)
        {

            var seleccion = GetMtoTiposGeneralesByID_Chosen(ID, EnumMtoTiposGenerales.Ruta);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTiposDeRuta_Chosen(string term)
        {
            var seleccion = GetMtoTiposGenerales_Chosen(EnumMtoTiposGenerales.Ruta, 0, term);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMarcaVehiculoByID_Chosen(int? ID)
        {
            var seleccion = GetMtoTiposGeneralesByID_Chosen(ID, EnumMtoTiposGenerales.Marcas);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMarcaVehiculoByModelo_Chosen(int? IDModelo)
        {
            var marca = new MtoTiposGeneralesService().GetMarcaByModelo((int)IDModelo);
            var seleccion = new List<SelectChosen>();
            seleccion.Add(new SelectChosen
                        {
                            DevolverValueFormateado = true,
                            text = marca.Descripcion,
                            value = marca.ID_Tipo.ToString(),
                        });

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMarcaVehiculo_Chosen(string term)
        {
            var seleccion = GetMtoTiposGenerales_Chosen(EnumMtoTiposGenerales.Marcas, 0, term);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModeloVehiculoByMarca_Chosen(int? IDMarca)
        {

            var seleccion = (from tipo in new MtoTiposGeneralesService().GetMtoGeneralTiposDatatable((int)EnumMtoTiposGenerales.Modelos)
                             where tipo.ID_Tipo_FOREIGN == IDMarca
                             select (new SelectChosen
                             {
                                 DevolverValueFormateado = true,
                                 text = tipo.Descripcion,
                                 value = tipo.ID_Tipo.ToString(),
                             })).OrderBy(x => x.text).ToList();

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModelosVehiculoByMarcas_Chosen(string marcasSeleccionadas)
        {
            List<int> marcasVehiculo = new List<int>();

            if (!string.IsNullOrEmpty(marcasSeleccionadas))
            {
                marcasVehiculo = marcasSeleccionadas.Split('-').Select(x=>Convert.ToInt32(x)).ToList();
            }

            var seleccion = (from tipo in new MtoTiposGeneralesService().GetMtoGeneralTiposDatatable((int)EnumMtoTiposGenerales.Modelos)
                             where marcasVehiculo.Contains((int)tipo.ID_Tipo_FOREIGN) || !marcasVehiculo.Any()
                             select (new SelectChosen
                             {
                                 DevolverValueFormateado = false,
                                 PonerValuePorDelanteDeTexto = false,
                                 text = tipo.Descripcion,
                                 value = tipo.ID_Tipo.ToString(),
                             })).OrderBy(x => x.text).ToList();

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModeloVehiculoByID_Chosen(int? ID)
        {
            var seleccion = GetMtoTiposGeneralesByID_Chosen(ID, EnumMtoTiposGenerales.Modelos);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModeloVehiculo_Chosen(string term, int IDMarca)
        {
            var seleccion = GetMtoTiposGenerales_Chosen(EnumMtoTiposGenerales.Modelos, IDMarca, term);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTipoVehiculoByID_Chosen(int? ID)
        {
            var seleccion = GetMtoTiposGeneralesByID_Chosen(ID, EnumMtoTiposGenerales.Vehiculos);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTipoVehiculo_Chosen(string term)
        {
            var seleccion = GetMtoTiposGenerales_Chosen(EnumMtoTiposGenerales.Vehiculos, 0, term);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCarburanteByID_Chosen(int? ID)
        {
            var seleccion = GetMtoTiposGeneralesByID_Chosen(ID, EnumMtoTiposGenerales.Carburante);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCarburante_Chosen(string term)
        {
            var seleccion = GetMtoTiposGenerales_Chosen(EnumMtoTiposGenerales.Carburante, 0, term);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUbicacionByID_Chosen(int? ID)
        {
            var seleccion = GetMtoTiposGeneralesByID_Chosen(ID, EnumMtoTiposGenerales.Ubicaciones);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUbicacion_Chosen(string term)
        {
            var seleccion = GetMtoTiposGenerales_Chosen(EnumMtoTiposGenerales.Ubicaciones, 0, term);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        #endregion  Chosen Datos Generales

        #region Chosen Datos Seguro
        public JsonResult GetTipoSeguroByID_Chosen(int? ID)
        {
            var seleccion = GetMtoTiposGeneralesByID_Chosen(ID, EnumMtoTiposGenerales.Seguro);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTiposSeguro_Chosen(string term)
        {
            var seleccion = GetMtoTiposGenerales_Chosen(EnumMtoTiposGenerales.Seguro, 0, term);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        #endregion Chosen Datos Seguro


        #region Chosen Datos Contrato
        public JsonResult GetTipoLiquidacionByID_Chosen(int? ID)
        {
            var seleccion = GetMtoTiposGeneralesByID_Chosen(ID, EnumMtoTiposGenerales.TipoLiquidacionSeguro);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTipoLiquidacion_Chosen(string term)
        {
            var seleccion = GetMtoTiposGenerales_Chosen(EnumMtoTiposGenerales.TipoLiquidacionSeguro, 0, term);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        #endregion Chosen Datos Contrato


        #region selección tablas generales

        private List<SelectChosen> GetMtoTiposGeneralesByID_Chosen(int? ID, EnumMtoTiposGenerales tipo)
        {
            var seleccion = new List<SelectChosen>();

            var valorSeleccion = new MtoTiposGeneralesService().GetMtoGeneralTiposDatatable((int)tipo, true, Convert.ToInt32(ID)).FirstOrDefault();

            if (valorSeleccion != null)
            {
                seleccion.Add(new SelectChosen
                {
                    DevolverValueFormateado = true,
                    PonerValuePorDelanteDeTexto = false,
                    text = valorSeleccion.Descripcion,
                    value = valorSeleccion.ID_Tipo.ToString(),
                });
            }

            return seleccion;
        }


        private List<SelectChosen> GetMtoTiposGenerales_Chosen(EnumMtoTiposGenerales MtoTipo, int foreignKey = 0, string term = "")
        {
            var seleccion = (from tipo in new MtoTiposGeneralesService().GetMtoGeneralTiposDatatable((int)MtoTipo, true, 0, foreignKey, term)
                             select (new SelectChosen
                             {
                                 DevolverValueFormateado = true,
                                 PonerValuePorDelanteDeTexto = false,
                                 text = tipo.Descripcion,
                                 value = tipo.ID_Tipo.ToString(),
                             })).OrderBy(x => x.text).ToList();
            return seleccion;
        }
        #endregion tablas generales


        #region validaciones
        public JsonResult GetMatriculaAsociadaTarjeta(int idTarjeta)
        {
            var matricula = "";

            var tarjeta = new TarjetasCombustibleService().GetTarjetaByID(idTarjeta);

            if (tarjeta != null)
            {
                matricula = tarjeta.MatriculaAsociadaTarjeta;
            }

            return Json(matricula, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetDatosVehiculoMatriculaSustituida(string MatriculaSustituida)
        {
            VehiculoSustituido datos = new VehiculoSustituido();

            var vehiculo = new VehiculoService().GetDatosGeneralesVehiculoECAR(MatriculaSustituida);

            if (vehiculo != null)
            {
                datos.Ceco = vehiculo.IDCentroCoste;
                datos.conductor = vehiculo.IDConductor;
                datos.Delegacion = vehiculo.IDDelegacion;
                datos.Departamento = vehiculo.IDDepartamento;
                datos.TarjetaCombustible = vehiculo.IDTarjetaCombustible;
                datos.Ubicacion = vehiculo.Ubicacion;
            }

            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPIN_Tarjeta(int idTarjeta)
        {
            var PinTarjeta = "";

            var tarjeta = new TarjetasCombustibleService().GetTarjetaByID(idTarjeta);

            if (tarjeta != null)
            {
                PinTarjeta = tarjeta.PIN;
            }

            return Json(PinTarjeta, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region CargaDatatable
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult BuscaMiFlota(FilterModel filtro)
        {

            List<MiFlotaDatatableModel> miflota = new List<MiFlotaDatatableModel>();

            if (filtro.EmpresasSeleccionadas != "-1")
            {

                var lCeCo = CecosUserByFilter(filtro);

                miflota = new FlotaService().AllMiFlotaDataTable(lCeCo, filtro.EstadoVehiculo);
            }
            var data = new
            {
                data = miflota,
                draw = 1,
                recordsFiltered = miflota.Count,
                recordsTotal = miflota.Count
            };


            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(data),
                ContentType = "application/json"
            };



            return result; //Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DatosITV_TMPJson()
        {
            var datosITV_TMP = new List<DatosITV_TMPModel>();

            datosITV_TMP = new VehiculoService().GetListDatosITV_TMP(UserModel.Login);

            var data = new
            {
                data = datosITV_TMP,
                draw = 1,
                recordsFiltered = datosITV_TMP.Count,
                recordsTotal = datosITV_TMP.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Métodos Privados
        private int? Convierte_a_Int(string valor)
        {
            int? valorReturn = null;

            try
            {
                if (!string.IsNullOrEmpty(valor))
                {
                    valorReturn = Convert.ToInt32(valor);
                }
            }
            catch (Exception ex)
            {
                return valorReturn;
            }

            return valorReturn;
        }

        private double? Convierte_a_Double(string valor)
        {
            double? valorReturn = null;

            try
            {
                if (!string.IsNullOrEmpty(valor))
                {
                    valorReturn = Convert.ToDouble(valor);
                }
            }
            catch (Exception ex)
            {
                return valorReturn;
            }

            return valorReturn;
        }

        private DateTime? Convierte_a_Date(string valor)
        {
            DateTime? valorReturn = null;

            try
            {
                if (!string.IsNullOrEmpty(valor))
                {
                    valorReturn = Convert.ToDateTime(valor);
                }
            }
            catch (Exception ex)
            {
                return valorReturn;
            }

            return valorReturn;
        }


        private DatosVehiculoModel RellenaModeloDatosVehiculo(FormCollection collection)
        {
            DatosVehiculoModel modelo = new DatosVehiculoModel();

            modelo.DatosGenerales_Vehiculo = new DatosGeneralesModel();
            //DATOS GENERALES
            modelo.DatosGenerales_Vehiculo.Accion = Global.GetValueFromEnumAccionEntity(collection["Accion"]);
            modelo.DatosGenerales_Vehiculo.IDEmpresa = Convierte_a_Int(collection["IDEmpresa"]);
            modelo.DatosGenerales_Vehiculo.IDEmpresaInicial = Convierte_a_Int(collection["IDEmpresaInicial"]);
            modelo.DatosGenerales_Vehiculo.IDDelegacion = collection["IDDelegacion"] == null ? null : collection["IDDelegacion"].Replace(",", "");
            modelo.DatosGenerales_Vehiculo.IDCarburante = Convierte_a_Int(collection["IDCarburante"]);
            modelo.DatosGenerales_Vehiculo.IDModelo = Convierte_a_Int(collection["IDModelo"]);
            modelo.DatosGenerales_Vehiculo.IDMarca = Convierte_a_Int(collection["IDMarca"]);
            modelo.DatosGenerales_Vehiculo.IDTipoVehiculo = Convierte_a_Int(collection["IDTipoVehiculo"]);
            modelo.DatosGenerales_Vehiculo.Ubicacion = Convierte_a_Int(collection["IDUbicacion"]);
            modelo.DatosGenerales_Vehiculo.IDUbicacion = Convierte_a_Int(collection["IDUbicacion"]);
            modelo.DatosGenerales_Vehiculo.IDTarjetaCombustible = Convierte_a_Int(collection["IDTarjetaCombustible"]);
            modelo.DatosGenerales_Vehiculo.IDTarjetaCombustibleInicial = Convierte_a_Int(collection["IDTarjetaCombustibleInicial"]);
            modelo.DatosGenerales_Vehiculo.IDTipoRuta = Convierte_a_Int(collection["IDTipoRuta"]);
            modelo.DatosGenerales_Vehiculo.IDConductor = Convierte_a_Int(collection["IDConductor"]);
            modelo.DatosGenerales_Vehiculo.IDConductorInicial = Convierte_a_Int(collection["IDConductorInicial"]);
            modelo.DatosGenerales_Vehiculo.Matricula = collection["MatriculaInicial"].ToUpper(); //Al deshabilitarse en la vista, el elemento Matricula, no baja. Por lo que cojo el valor de MatriculaInicial.
            modelo.DatosGenerales_Vehiculo.Veh_sustituido = collection["MatriculaSustituida"] == null ? null : collection["MatriculaSustituida"].ToUpper();
            modelo.DatosGenerales_Vehiculo.Extras = collection["Extras"];
            modelo.DatosGenerales_Vehiculo.Bastidor = collection["Bastidor"] == null ? null : collection["Bastidor"].ToUpper();
            modelo.DatosGenerales_Vehiculo.CentroCoste = collection["IDCentroCoste"];
            modelo.DatosGenerales_Vehiculo.IDCentroCoste = collection["IDCentroCoste"];
            modelo.DatosGenerales_Vehiculo.IDCentroCosteInicial = collection["IDCentroCosteInicial"]; 
            modelo.DatosGenerales_Vehiculo.Departamento = collection["IDDepartamento"];
            modelo.DatosGenerales_Vehiculo.IDDepartamento = collection["IDDepartamento"];
            modelo.DatosGenerales_Vehiculo.EmpresaLeasing = Convierte_a_Int(collection["IDEmpresaLeasing"]);
            modelo.DatosGenerales_Vehiculo.IDEmpresaLeasing = Convierte_a_Int(collection["IDEmpresaLeasing"]);
            modelo.DatosGenerales_Vehiculo.IdentificadorImportacion = collection["IdentificadorImportacion"];
            modelo.DatosGenerales_Vehiculo.UsuarioImportacion = collection["UsuarioImportacion"];
            modelo.DatosGenerales_Vehiculo.Equipamiento = collection["Equipamiento"]; 

            //DATOS SEGURO
            modelo.DatosGenerales_Vehiculo.Cia_Seguro = Convierte_a_Int(collection["IDCia_Seguro"]);
            modelo.DatosGenerales_Vehiculo.IDCia_Seguro = Convierte_a_Int(collection["IDCia_Seguro"]);
            modelo.DatosGenerales_Vehiculo.IDTipoSeguro = Convierte_a_Int(collection["IDTipoSeguro"]);
            modelo.DatosGenerales_Vehiculo.Seguro_Poliza = collection["Seguro_Poliza"];
            modelo.DatosGenerales_Vehiculo.Seguro_Importe = Convierte_a_Double(collection["Seguro_Importe"]);
            modelo.DatosGenerales_Vehiculo.Seguro_FechaVencimiento = Convierte_a_Date(collection["Seguro_FechaVencimiento"]);
            modelo.DatosGenerales_Vehiculo.Observaciones = collection["Observaciones"];

            modelo.DatosContrato_Vehiculo = new DatosContratoModel();
            //DATOS CONTRATO
            modelo.DatosContrato_Vehiculo.Baja = collection["Baja"] == null ? false : (collection["Baja"] == "true" ? true : false);
            modelo.DatosContrato_Vehiculo.IDTipoLiquidacion = Convierte_a_Int(collection["IDTipoLiquidacion"]);
            modelo.DatosContrato_Vehiculo.NumContrato = collection["NumContrato"];
            modelo.DatosContrato_Vehiculo.FechaAlta = Convierte_a_Date(collection["FechaAlta"]);
            modelo.DatosContrato_Vehiculo.FechaFinalizacion = Convierte_a_Date(collection["FechaFinalizacion"]);
            modelo.DatosContrato_Vehiculo.FechaBaja = Convierte_a_Date(collection["FechaBaja"]);
            modelo.DatosContrato_Vehiculo.FechaDevolucion = Convierte_a_Date(collection["FechaDevolucion"]);
            modelo.DatosContrato_Vehiculo.FechaRecogida = Convierte_a_Date(collection["FechaRecogida"]);
            modelo.DatosContrato_Vehiculo.Cuotas = Convierte_a_Int(collection["Cuotas"]);
            modelo.DatosContrato_Vehiculo.KMTotales = Convierte_a_Int(collection["KMTotales"]);
            modelo.DatosContrato_Vehiculo.KMExentos = Convierte_a_Int(collection["KMExentos"]);
            modelo.DatosContrato_Vehiculo.ExcesoAjuste = Convierte_a_Int(collection["ExcesoAjuste"]);
            modelo.DatosContrato_Vehiculo.CoefExceso = Convierte_a_Int(collection["CoefExceso"]);
            modelo.DatosContrato_Vehiculo.Abono = Convierte_a_Double(collection["Abono"]);
            modelo.DatosContrato_Vehiculo.Cargo = Convierte_a_Double(collection["Cargo"]);
            modelo.DatosContrato_Vehiculo.FechaMatriculacion = Convierte_a_Date(collection["FechaMatriculacion"]);
            modelo.DatosContrato_Vehiculo.FechaRecibido = Convierte_a_Date(collection["FechaRecibido"]);
            modelo.DatosContrato_Vehiculo.LugarEntrega = collection["LugarEntrega"];
            modelo.DatosContrato_Vehiculo.Responsable = collection["Responsable"];
            modelo.DatosContrato_Vehiculo.PrioridadEntrega = collection["PrioridadEntrega"];
            modelo.DatosContrato_Vehiculo.FechaImportacion = Convierte_a_Date(collection["FechaImportacion"]);
            modelo.DatosContrato_Vehiculo.FechaPrevistaEntrega = Convierte_a_Date(collection["FechaPrevistaEntrega"]); 

            return modelo;
        }
        #endregion Métodos Privados
    }


    class VehiculoSustituido
    {
        public string Delegacion { get; set; }
        public string Ceco { get; set; }
        public string Departamento { get; set; }
        public int? Ubicacion { get; set; }
        public int? conductor { get; set; }
        public int? TarjetaCombustible { get; set; }
    }


}