using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Application_Services;
using TK_ECAR.Models;

namespace TK_ECAR.Controllers
{
    public class EmpresasVehiculosController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NuevaEmpresa()
        {
            EmpresasVehiculosModels emp = new EmpresasVehiculosModels();
            emp.Accion = Framework.EnumAccionEntity.Alta;
            return PartialView("_MantenimientoEmpresa", emp);
        }

        public ActionResult EditarEmpresa(int idEmpresa)
        {
            EmpresasVehiculosService serviceEmp = new EmpresasVehiculosService();
            var emp = serviceEmp.GetEmpVehiculoByID(idEmpresa);
            emp.Accion = Framework.EnumAccionEntity.Modificacion;

            return PartialView("_MantenimientoEmpresa", emp);
        }

        public ActionResult BorraEmpresa(int idEmpresa)
        {

            EmpresasVehiculosService serviceEmp = new EmpresasVehiculosService();
            serviceEmp.DeleteEmpVehiculos(idEmpresa);

            return Json("Success", JsonRequestBehavior.AllowGet);
        }


        [ActionName("GuardarEmpresa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarEmpresa(EmpresasVehiculosModels modelo)
        {
            EmpresasVehiculosService serviceEmp = new EmpresasVehiculosService();

            var resOK = true;

            if (modelo.Accion == Framework.EnumAccionEntity.Alta || modelo.Accion == Framework.EnumAccionEntity.Modificacion)
            {
                resOK = serviceEmp.SaveEmpVehiculos(modelo);
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


        #region CargaDatatable
        public ActionResult EmpresasJson()
        {
            var emp = new EmpresasVehiculosService().AllEmpVehiculosDataTable();

            var data = new
            {
                data = emp,
                draw = 1,
                recordsFiltered = emp.Count,
                recordsTotal = emp.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion  
    }
}