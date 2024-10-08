using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TK_ECAR.Application_Services;
using TK_ECAR.Models;
using TK_ECAR.Utils;

namespace TK_ECAR.Controllers
{
    public class ConductoresController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NuevoConductor()
        {
            ConductorECARModel conductor = new ConductorECARModel();
            conductor.PersonalInterno = true;
            conductor.IdTipoDocumentoIdentificacion = 1;
            conductor.TipoDocumentoIdentificacion = 1;
            conductor.Accion = Framework.EnumAccionEntity.Alta;
            return PartialView("_MantenimientoConductor", conductor);
        }

        public ActionResult EditarConductor(int idConductor)
        {
            ConductoresECARService serviceConductor = new ConductoresECARService();
            var conductor = serviceConductor.GetConductorByID_ECAR(idConductor);
            conductor.Accion = Framework.EnumAccionEntity.Modificacion;

            return PartialView("_MantenimientoConductor", conductor);
        }

        public ActionResult ConsultarConductor(int idConductor)
        {
            ConductoresECARService serviceConductor = new ConductoresECARService();
            var conductor = serviceConductor.GetConductorByID_ECAR(idConductor);
            conductor.Accion = Framework.EnumAccionEntity.Consulta;

            return PartialView("_MantenimientoConductor", conductor);
        }

        public ActionResult BorraConductor(int idConductor)
        {

            ConductoresECARService serviceConductor = new ConductoresECARService();
            serviceConductor.DeleteConductorECAR(idConductor);

            return Json("Success", JsonRequestBehavior.AllowGet);
        }


        [ActionName("GuardarConductor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarConductor(ConductorECARModel modelo)
        {
            ConductoresECARService serviceConductor = new ConductoresECARService();

            var resOK = true;

            if (modelo.Accion == Framework.EnumAccionEntity.Alta || modelo.Accion == Framework.EnumAccionEntity.Modificacion)
            {
                resOK = serviceConductor.SaveConductorECAR(modelo);
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


        public JsonResult GetConductorSAPByID(string ID)
        {
            var seleccion = new ConductoresECARService().GetConductorSAPByID(Convert.ToInt32(ID));

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUsuarioSAPByID_Chosen(string ID)
        {
            var seleccion = new ConductoresECARService().GetConductorSAPByID(Convert.ToInt32(ID));

            var usuarioChosen = new List<SelectChosen>();
            if (seleccion != null)
            {
                usuarioChosen.Add(new SelectChosen
                {
                    text = seleccion.NombreCompleto,
                    value = seleccion.NumEmpleado.ToString(),
                    DevolverValueFormateado = false,
                    PonerValuePorDelanteDeTexto = false,
                });
            }


            return Json(usuarioChosen, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUsuarioSAPByCecosUser(string term)
        {
            
            var listaSeleccion = new ConductoresECARService().GetConductorSAPByCecoChosen(term, CecosUserFormatted);

            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExisteConductor(string ID)
        {

            var listaSeleccion = new ConductoresECARService().GetConductorByNumEmpleado_ECAR(Convert.ToInt32(ID));

            var existe = (listaSeleccion != null ? "SI" : "NO");

            return Json(existe, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTiposDocumentoIdentificacion(string term)
        {

            var listaSeleccion = GetEnumeradores<EnumDocumentoIdentificacion>(term);

            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetTiposDocumentoIdentificacionByVal(int idDoc)
        {

            var listaSeleccion = GetEnumeradorByVal<EnumDocumentoIdentificacion>(idDoc);

            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);
        }


        //public JsonResult GetCentrosCosteID(string ID)
        //{
        //    var seleccion = UserModel.CecosModelList.Where(x=>x.IdCeco == ID).FirstOrDefault();

        //    var cecoChosen = new List<SelectChosen>();
        //    if (seleccion != null)
        //    {
        //        cecoChosen.Add(new SelectChosen
        //        {
        //            text = seleccion.Ceco,
        //            value = seleccion.IdCeco,
        //            DevolverValueFormateado = false,
        //            PonerValuePorDelanteDeTexto = false,
        //        });
        //    }

        //    return Json(cecoChosen, JsonRequestBehavior.AllowGet);
        //}


        #region CargaDatatable
        //Carga de DataTable Server-Side
        //public ActionResult ConductoresJson(string _search, int _start, int _length, string _orderColumnName, string _orderDirection, int _draw)
        //{

        //    var conductores = new List<ConductoresOnlyDataTableModel>();

        //    if (!string.IsNullOrEmpty(_orderColumnName))
        //    {
        //        var param = _orderColumnName;
        //        var propertyInfo = typeof(ConductoresOnlyDataTableModel).GetProperty(param);

        //        if (_orderDirection == "asc")
        //        {
        //            conductores = new ConductoresECARService().AllConductoresOnlyDataTable_ECAR(_search).OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
        //        }
        //        else
        //        {
        //            conductores = new ConductoresECARService().AllConductoresOnlyDataTable_ECAR(_search).OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();

        //        }
        //    }
        //    else
        //    {
        //        conductores = new ConductoresECARService().AllConductoresOnlyDataTable_ECAR(_search);
        //    }

        //    if (_start+_length > conductores.Count)
        //    {
        //        _length = (conductores.Count) - _start;
        //    }
        //    var datos = conductores.GetRange(_start, _length);

        //    var data = new
        //    {
        //        data = datos,
        //        draw = _draw,
        //        recordsFiltered = conductores.Count,
        //        recordsTotal = conductores.Count,
        //    };

        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult ConductoresJson()
        {
            var conductores = new ConductoresECARService().AllConductoresDataTable_ECAR(); //.AllConductoresOnlyDataTable_ECAR();

            var data = new
            {
                data = conductores,
                draw = 1,
                recordsFiltered = conductores.Count,
                recordsTotal = conductores.Count
            };

            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(data),
                ContentType = "application/json"
            };


            return result; // Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion  
    }
}