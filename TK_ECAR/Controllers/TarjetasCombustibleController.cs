using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Application_Services;
using TK_ECAR.Models;

namespace TK_ECAR.Controllers
{
    public class TarjetasCombustibleController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        #region mantenimiento
        public ActionResult Nuevatarjeta()
        {
            TarjetasCombustibleModels emp = new TarjetasCombustibleModels();
            emp.Accion = Framework.EnumAccionEntity.Alta;
            return PartialView("_MantenimientoTarjeta", emp);
        }

        public ActionResult NuevaTarjetaAlerta(int idAlerta, int idEstado, string matricula)
        {
            TarjetasCombustibleModels emp = new TarjetasCombustibleModels();
            emp.Accion = Framework.EnumAccionEntity.Alta;
            emp.IdAlerta = idAlerta;
            emp.IdEstado = idEstado;
            emp.MatriculaAsociadaTarjeta = matricula;
            return PartialView("_MantenimientoTarjeta", emp);
        }

        public ActionResult Editartarjeta(int idTarjeta)
        {
            TarjetasCombustibleService service = new TarjetasCombustibleService();
            var emp = service.GetTarjetaByID(idTarjeta);
            emp.Accion = Framework.EnumAccionEntity.Modificacion;

            return PartialView("_MantenimientoTarjeta", emp);
        }

        public ActionResult Borratarjeta(int idTarjeta)
        {

            TarjetasCombustibleService service = new TarjetasCombustibleService();
            service.DeleteTarjeta(idTarjeta);

            return Json("Success", JsonRequestBehavior.AllowGet);
        }


        [ActionName("GuardarTarjeta")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarTarjeta(TarjetasCombustibleModels modelo)
        {
            TarjetasCombustibleService service = new TarjetasCombustibleService();

            var resOK = true;

            if (modelo.Accion == Framework.EnumAccionEntity.Alta || modelo.Accion == Framework.EnumAccionEntity.Modificacion)
            {
                resOK = service.SaveTarjeta(modelo);
                if (modelo.IdAlerta > 0 && resOK)
                {
                    resOK = new AlertasService().RunAccion(modelo.IdAlerta, modelo.IdEstado);
                }
            }

            if (resOK)
            {
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TarjetaYaExistente(int IdAlerta, int IdEstado)
        {
            var resOK = true;

            resOK = new AlertasService().RunAccion(IdAlerta, IdEstado);

            if (resOK)
            {
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion


        #region chosen
        public JsonResult GetEmpresasEmisorasTarjetaCombustible(string term)
        {
            var seleccion = new MtoTiposGeneralesService().GetTiposTarjetaCombustibleChosen(term);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmpresaEmisoraTarjetaByID_Chosen(int ID)
        {
            var seleccion = new MtoTiposGeneralesService().GetTiposTarjetaCombustibleByIDChosen(ID);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }
        #endregion



        public ActionResult GetTarjetasByEmpresaEmisoraJson(int empresa)
        {
            List<int?> empresas = new List<int?> { empresa };

            var tarjetas = new TarjetasCombustibleService().AlltarjetasDataTable(null, true, empresas);

            var data = new
            {
                data = tarjetas,
                draw = 1,
                recordsFiltered = tarjetas.Count,
                recordsTotal = tarjetas.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        


        #region CargaDatatable
        public ActionResult TarjetasJson()
        {
            var tarjetas = new TarjetasCombustibleService().AlltarjetasDataTable(UserModel.Empresas.Select(i => (int?)i).ToList());

            var data = new
            {
                data = tarjetas,
                draw = 1,
                recordsFiltered = tarjetas.Count,
                recordsTotal = tarjetas.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion  
    }
}