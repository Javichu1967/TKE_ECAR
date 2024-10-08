using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Application_Services;
using TK_ECAR.Content.resources;
using TK_ECAR.Framework;
using TK_ECAR.Models;

namespace TK_ECAR.Controllers
{
    public class MtoGenericoTiposController : BaseController
    {
        #region index - Sustituyo el método Index, por cada uno de los mantenimientos, para que se remarque en el menú, el que está activo.
        public ActionResult Carburante(int tipoMantenimiento)
        {
            EstableceTextosViewData(tipoMantenimiento);
            MtoGenericoTiposModels mtoGenerico = DevuelveMtoGenerico(tipoMantenimiento, EnumAccionEntity.SinAccion);

            return View("Index");
        }
        public ActionResult Marcas(int tipoMantenimiento)
        {
            EstableceTextosViewData(tipoMantenimiento);
            MtoGenericoTiposModels mtoGenerico = DevuelveMtoGenerico(tipoMantenimiento, EnumAccionEntity.SinAccion);

            return View("Index");
        }
        public ActionResult Modelos(int tipoMantenimiento)
        {
            EstableceTextosViewData(tipoMantenimiento);
            MtoGenericoTiposModels mtoGenerico = DevuelveMtoGenerico(tipoMantenimiento, EnumAccionEntity.SinAccion);

            return View("Index");
        }
        public ActionResult Ruta(int tipoMantenimiento)
        {
            EstableceTextosViewData(tipoMantenimiento);
            MtoGenericoTiposModels mtoGenerico = DevuelveMtoGenerico(tipoMantenimiento, EnumAccionEntity.SinAccion);

            return View("Index");
        }
        public ActionResult Seguro(int tipoMantenimiento)
        {
            EstableceTextosViewData(tipoMantenimiento);
            MtoGenericoTiposModels mtoGenerico = DevuelveMtoGenerico(tipoMantenimiento, EnumAccionEntity.SinAccion);

            return View("Index");
        }
        public ActionResult Ubicaciones(int tipoMantenimiento)
        {
            EstableceTextosViewData(tipoMantenimiento);
            MtoGenericoTiposModels mtoGenerico = DevuelveMtoGenerico(tipoMantenimiento, EnumAccionEntity.SinAccion);

            return View("Index");
        }

        public ActionResult TarjetaCombustible(int tipoMantenimiento)
        {
            EstableceTextosViewData(tipoMantenimiento);
            MtoGenericoTiposModels mtoGenerico = DevuelveMtoGenerico(tipoMantenimiento, EnumAccionEntity.SinAccion);

            return View("Index");
        }
        public ActionResult Vehiculos(int tipoMantenimiento)
        {
            EstableceTextosViewData(tipoMantenimiento);
            MtoGenericoTiposModels mtoGenerico = DevuelveMtoGenerico(tipoMantenimiento, EnumAccionEntity.SinAccion);

            return View("Index");
        }

        public ActionResult TipoLiquidacion(int tipoMantenimiento)
        {
            EstableceTextosViewData(tipoMantenimiento);
            MtoGenericoTiposModels mtoGenerico = DevuelveMtoGenerico(tipoMantenimiento, EnumAccionEntity.SinAccion);

            return View("Index");
        }
        #endregion
        //public ActionResult Index(int tipoMantenimiento)
        //{
        //    EstableceTextosViewData(tipoMantenimiento);
        //    MtoGenericoTiposModels mtoGenerico = DevuelveMtoGenerico(tipoMantenimiento, EnumAccionEntity.SinAccion); 

        //    return View();
        //}

        #region Mantenimiento
        public ActionResult NuevoTipoGenerico(int tipoMantenimiento)
        {

            EstableceTextosViewData(tipoMantenimiento);
            MtoGenericoTiposModels mtoGenerico = DevuelveMtoGenerico(tipoMantenimiento, EnumAccionEntity.Alta);
            mtoGenerico.TipoMtoGenerico = (EnumMtoTiposGenerales)Enum.ToObject(typeof(EnumMtoTiposGenerales), tipoMantenimiento);

            return PartialView("_MtoGenericoTipos", mtoGenerico);
        }

        public ActionResult EditarMtoGenerico(int id, int tipoMantenimiento)
        {
            EstableceTextosViewData(tipoMantenimiento);
            var mtoGenerico = new MtoTiposGeneralesService().GetMtoGeneralTiposDatatable(tipoMantenimiento,true, id).First();
            mtoGenerico.TipoMtoGenerico = (EnumMtoTiposGenerales)Enum.ToObject(typeof(EnumMtoTiposGenerales), tipoMantenimiento);

            return PartialView("_MtoGenericoTipos", mtoGenerico);
        }

        public ActionResult BorraMtoGenerico(int id, int tipoMantenimiento)
        {
            var mtoTiposGenService = new MtoTiposGeneralesService();

            var mtoGenerico = mtoTiposGenService.GetMtoGeneralTiposDatatable(tipoMantenimiento, true, id).First();
            mtoGenerico.TipoMtoGenerico = (EnumMtoTiposGenerales)Enum.ToObject(typeof(EnumMtoTiposGenerales), tipoMantenimiento);
            mtoGenerico.Baja = true;
            mtoGenerico.Accion = EnumAccionEntity.Baja;
            mtoTiposGenService.SaveMtoTipoGeneral(mtoGenerico);

            return Json("Success", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarMtoGenerico(MtoGenericoTiposModels modelo)
        {
            var mtoTiposGenService = new MtoTiposGeneralesService();

            mtoTiposGenService.SaveMtoTipoGeneral(modelo);

            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CargaDatatable
        public ActionResult MtoGenericoJson(int tipoMantenimiento)
        {
            var mtoGenericoList = new MtoTiposGeneralesService().GetMtoGeneralTiposDatatable(tipoMantenimiento);

            var data = new
            {
                data = mtoGenericoList,
                draw = 1,
                recordsFiltered = mtoGenericoList.Count,
                recordsTotal = mtoGenericoList.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region métodos privados
        private MtoGenericoTiposModels DevuelveMtoGenerico(int tipoMantenimiento, EnumAccionEntity accion, int id = 0)
        {
            MtoGenericoTiposModels valorReturn = new MtoGenericoTiposModels();

            valorReturn.Accion = accion;

            if (tipoMantenimiento != (int)EnumMtoTiposGenerales.Modelos)
            {
                valorReturn.ID_Tipo_FOREIGN = 0;
            }

            valorReturn.TipoMtoGenerico = (EnumMtoTiposGenerales)Enum.ToObject(typeof(EnumMtoTiposGenerales), tipoMantenimiento);

            return valorReturn;
        }

        private void EstableceTextosViewData(int tipoMantenimiento)
        {
            var _MtoGenerico = string.Empty;
            var _MtoGenericoInf = string.Empty;
            var _CabeceraMtoGenerico = string.Empty;
            var _CabeceraMtoGenericoForeign = string.Empty;
            var _BorrarMtoGenerico = string.Empty; 

            switch (tipoMantenimiento)
            {
                case (int)EnumMtoTiposGenerales.Carburante:
                    _MtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("TitleCarburante");
                    _MtoGenericoInf = @TK_ECAR_Resource.ResourceManager.GetString("TitleCarburanteInf");
                    _CabeceraMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("CabCarburante");
                    _BorrarMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("LBorrarCarburante"); 
                    break;
                case (int)EnumMtoTiposGenerales.Marcas:
                    _MtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("TitleMarcaVehiculo");
                    _MtoGenericoInf = @TK_ECAR_Resource.ResourceManager.GetString("TitleMarcaVehiculoInf");
                    _CabeceraMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("CabMarcaVehiculo");
                    _BorrarMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("LBorrarMarca");
                    break;
                case (int)EnumMtoTiposGenerales.Modelos:
                    _MtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("TitleModeloVehiculo");
                    _MtoGenericoInf = @TK_ECAR_Resource.ResourceManager.GetString("TitleModeloVehiculoInf");
                    _CabeceraMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("CabModeloVehiculo");
                    _BorrarMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("LBorrarModelo");
                    _CabeceraMtoGenericoForeign = @TK_ECAR_Resource.ResourceManager.GetString("CabMarcaVehiculo");
                    break;
                case (int)EnumMtoTiposGenerales.Ruta:
                    _MtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("TitleTipoRuta");
                    _MtoGenericoInf = @TK_ECAR_Resource.ResourceManager.GetString("TitleTipoRutaInf");
                    _CabeceraMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("CabTipoRuta");
                    _BorrarMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("LBorrarTipoRuta");
                    break;
                case (int)EnumMtoTiposGenerales.Seguro:
                    _MtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("TitleTipoSeguro");
                    _MtoGenericoInf = @TK_ECAR_Resource.ResourceManager.GetString("TitleTipoSeguroInf");
                    _CabeceraMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("CabTipoSeguro");
                    _BorrarMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("LBorrarTipoSeguro");
                    break;
                case (int)EnumMtoTiposGenerales.TarjetaCombustible:
                    _MtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("TitleTipoTarjetaCombustible");
                    _MtoGenericoInf = @TK_ECAR_Resource.ResourceManager.GetString("TitleTipoTarjetaCombustibleInf");
                    _CabeceraMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("CabTipoTarjetaCombustible");
                    _BorrarMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("LBorrarTarjetaCombustible");
                    break;
                case (int)EnumMtoTiposGenerales.Ubicaciones:
                    _MtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("TitleUbicacion");
                    _MtoGenericoInf = @TK_ECAR_Resource.ResourceManager.GetString("TitleUbicacionInf");
                    _CabeceraMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("CabUbicacion");
                    _BorrarMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("LBorrarUbicacion");
                    break;
                case (int)EnumMtoTiposGenerales.Vehiculos:
                    _MtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("TitleTipoVehiculo");
                    _MtoGenericoInf = @TK_ECAR_Resource.ResourceManager.GetString("TitleTipoVehiculoInf");
                    _CabeceraMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("CabTipoVehiculo");
                    _BorrarMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("LBorrarTipoVehiculo");
                    break;
                case (int)EnumMtoTiposGenerales.TipoLiquidacionSeguro:
                    _MtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("TitleTipoLiquidacionSeguro");
                    _MtoGenericoInf = @TK_ECAR_Resource.ResourceManager.GetString("TitleTipoLiquidacionSeguroInf");
                    _CabeceraMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("CabTipoLiquidacionSeguro");
                    _BorrarMtoGenerico = @TK_ECAR_Resource.ResourceManager.GetString("LBorrarTipoLiquidacionSeguro");
                    break;
            }

            ViewData["MtoGenerico"] = _MtoGenerico;
            ViewData["MtoGenericoInf"] = _MtoGenericoInf;
            ViewData["CabeceraMtoGenerico"] = _CabeceraMtoGenerico;
            ViewData["BorrarMtoGenerico"] = _BorrarMtoGenerico;
            ViewData["TipoMtoGenerico"] = tipoMantenimiento;
            ViewData["CabeceraMtoGenericoForeign"] = _CabeceraMtoGenericoForeign; 

        }
        #endregion
    }
}