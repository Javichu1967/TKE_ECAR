using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Application_Services;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Framework;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;
using TK_ECAR.Utils;

namespace TK_ECAR.Controllers
{
    public class FilterController : BaseController
    {
        #region Empresa
        public JsonResult GetEmpresas(string term)
        {
            var listaSeleccion = castToSelectChosen((from empresa in UserModel.CecosModelList
                                                     where empresa.Empresa.ToUpper().Contains(term.ToUpper()) ||
                                                     empresa.CodigoEmpresa.ToString().Contains(term)
                                                     select new
                                                     {
                                                         text = empresa.Empresa,
                                                         value = empresa.CodigoEmpresa.ToString()
                                                     }));

            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmpresaByID_Chosen(int? id)
        {
            var listaSeleccion = castToSelectChosen((from empresa in UserModel.CecosModelList
                                                     where empresa.CodigoEmpresa == id
                                                     select new
                                                     {
                                                         text = empresa.Empresa,
                                                         value = empresa.CodigoEmpresa.ToString()
                                                     }));

            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmpresaByDelegacion_Chosen(string idDelegacion)
        {
            var listaSeleccion = castToSelectChosen((from dele in UserModel.CecosModelList
                                                     where dele.IdDelegacion == idDelegacion
                                                     select new
                                                     {
                                                         text = dele.Empresa,
                                                         value = dele.CodigoEmpresa.ToString()
                                                     }));

            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmpresaByCentroCoste_Chosen(string idCeco)
        {
            var listaSeleccion = castToSelectChosen((from ceco in UserModel.CecosModelList
                                                     where ceco.IdCeco == idCeco || ceco.IdCecoFormatted == idCeco
                                                     select new
                                                     {
                                                         text = ceco.Empresa,
                                                         value = ceco.CodigoEmpresa.ToString()
                                                     }));

            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Dirección Territorial
        public JsonResult GetDireccionesTerritoriales(string term, string empresasSel)
        {
            var lEmpresasSel = new List<int?>();

            if (!string.IsNullOrEmpty(empresasSel))
                lEmpresasSel = Global.GetSplitListNullableInt(empresasSel, '-');

            var listaSeleccion = castToSelectChosen((from dirT in UserModel.CecosModelList
                                                     where (!lEmpresasSel.Any() || lEmpresasSel.Contains(dirT.CodigoEmpresa))
                                                     where ((dirT.IdDT != null ? dirT.DireccionTerritorial.ToUpper().Contains(term.ToUpper()) || dirT.IdDT.ToString().Contains(term) : false))
                                                     select new
                                                     {
                                                         text = dirT.DireccionTerritorial,
                                                         value = dirT.IdDT
                                                     }));

            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Delegación
        public JsonResult GetDelegaciones(string term, string empresasSel, string dirTerritorialSel)
        {

            var lEmpresasSel = new List<int?>();
            var lDtSel = new List<string>();

            if (!string.IsNullOrEmpty(empresasSel))
            {
                lEmpresasSel = Global.GetSplitListNullableInt(empresasSel, '-');
            }
            else
            {

            }


            if (!string.IsNullOrEmpty(dirTerritorialSel))
            {
                lDtSel = dirTerritorialSel.Split('-').ToList();
                if (!lEmpresasSel.Any())
                {
                    var lEmpresasSelAux = castToSelectChosen((from dt in UserModel.CecosModelList
                                                              where (lDtSel.Contains(dt.IdDT))
                                                              select new
                                                              {
                                                                  text = dt.Empresa,
                                                                  value = dt.CodigoEmpresa.ToString()
                                                              })).Select(x => Convert.ToInt32(x.value));
                    lEmpresasSel = lEmpresasSelAux.Select(x => (int?)x).ToList();
                }
            }


            //ATENCIÓN.
            // En el Where de where ((!lDtSel.Any() || lDtSel.Contains(deleg.IdDT)) || string.IsNullOrEmpty(deleg.IdDT))
            // se pone la condición   || string.IsNullOrEmpty(deleg.IdDT)), porque puede haber delegaciones que no tengan Dirección Territorial (delegaciones de Central)

            var listaSeleccion = castToSelectChosen((from deleg in UserModel.CecosModelList
                                                     where (!lEmpresasSel.Any() || lEmpresasSel.Contains(deleg.CodigoEmpresa))
                                                     where ((!lDtSel.Any() || lDtSel.Contains(deleg.IdDT)) || string.IsNullOrEmpty(deleg.IdDT))
                                                     where ((deleg.IdDelegacion != null ? deleg.Delegacion.ToUpper().Contains(term.ToUpper()) || deleg.IdDelegacion.ToString().Contains(term) : false))
                                                     select new
                                                     {
                                                         text = deleg.Delegacion,
                                                         value = deleg.IdDelegacion
                                                     }), false);

            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetDelegacionByID_Chosen(string idDelegacion, int? idEmpresa)
        {

            var listaSeleccion = castToSelectChosen((from deleg in UserModel.CecosModelList
                                                     where deleg.CodigoEmpresa == idEmpresa
                                                     where deleg.IdDelegacion == idDelegacion
                                                     select new
                                                     {
                                                         text = deleg.Delegacion,
                                                         value = deleg.IdDelegacion
                                                     }), false);

            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetDelegacionesByEmpresa_Chosen(string term, int idEmpresa)
        {

            var listaSeleccion = castToSelectChosen((from deleg in UserModel.CecosModelList
                                                     where deleg.CodigoEmpresa == idEmpresa &&
                                                     (deleg.Delegacion != null ?
                                                     (deleg.Delegacion.ToUpper().Contains(term.ToUpper()) || 
                                                     deleg.IdDelegacion.ToString().Contains(term)) : false)
                                                     select new
                                                     {
                                                         text = deleg.Delegacion,
                                                         value = deleg.IdDelegacion
                                                     }), false);

            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDelegacionByCecos_Chosen(string idCeco)
        {

            var listaSeleccion = castToSelectChosen((from deleg in UserModel.CecosModelList
                                                     where deleg.IdCeco == idCeco || deleg.IdCecoFormatted == idCeco
                                                     select new
                                                     {
                                                         text = deleg.Delegacion,
                                                         value = deleg.IdDelegacion
                                                     }), false);

            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Centro de Coste
        public JsonResult GetCentrosCoste(string term, string empresasSel, string dirTerritorialSel, string delegacionSel, bool MirarBaja = true)
        {

            var lEmpresasSel = new List<int?>();
            var lDtSel = new List<string>();
            var lDelegSel = new List<string>();

            if (!string.IsNullOrEmpty(empresasSel))
                lEmpresasSel = Global.GetSplitListNullableInt(empresasSel, '-');

            if (!string.IsNullOrEmpty(dirTerritorialSel))
                lDtSel = dirTerritorialSel.Split('-').ToList();

            if (!string.IsNullOrEmpty(delegacionSel))
                lDelegSel = delegacionSel.Split('-').ToList();

            var listaSeleccion = castToSelectChosen((from CECO in UserModel.CecosModelList
                                                     where (!lEmpresasSel.Any() || lEmpresasSel.Contains(CECO.CodigoEmpresa))
                                                     where (!lDtSel.Any() || lDtSel.Contains(CECO.IdDT))
                                                     where (!lDelegSel.Any() || lDelegSel.Contains(CECO.IdDelegacion) || string.IsNullOrEmpty(CECO.IdDelegacion))
                                                     where ((CECO.Ceco.ToUpper().Contains(term.ToUpper()) || CECO.IdCeco.ToString().Contains(term)) 
                                                            && (MirarBaja ? CECO.Baja == false : true))
                                                     
                                                     select new
                                                     {
                                                         text = CECO.Ceco,
                                                         value = CECO.IdCeco
                                                     }));
            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);
        }
        #endregion


        private List<SelectChosen> castToSelectChosen(IEnumerable<dynamic> lista, bool valueSinCerosIzquierda = true)
        {

            return lista.Distinct().Select(x => new SelectChosen
            {
                DevolverValueFormateado = valueSinCerosIzquierda,
                text = x.text,
                value = x.value,

            }).ToList();

        }
        //public JsonResult GetTipoAlertas(string term,string estadosSel, string accionesSel)
        //{

        //    var service = new AlertasService();
        //    List<int?> estadosSeleccionados = null;
        //    List<int?> accionesSeleccionados = null;
        //    if (!string.IsNullOrEmpty(estadosSel))
        //        estadosSeleccionados = Global.GetSplitListNullableInt(estadosSel, '-');

        //    if (!string.IsNullOrEmpty(accionesSel))
        //        accionesSeleccionados = Global.GetSplitListNullableInt(accionesSel, '-');

        //    var listaSeleccion =service.AllTipoAlertas(term, estadosSeleccionados, accionesSeleccionados)
        //                        .Select(x=> new
        //                        {
        //                            value = x.ID,
        //                            text = x.DESCRPICION,
        //                        });


        //    return Json(listaSeleccion, JsonRequestBehavior.AllowGet);

        //}
        public JsonResult GetTipoAlertas(string term)
        {

            var service = new AlertasService();
            //List<int?> estadosSeleccionados = null;
            //List<int?> accionesSeleccionados = null;
            //if (!string.IsNullOrEmpty(estadosSel))
            //    estadosSeleccionados = Global.GetSplitListNullableInt(estadosSel, '-');

            //if (!string.IsNullOrEmpty(accionesSel))
            //    accionesSeleccionados = Global.GetSplitListNullableInt(accionesSel, '-');

            var listaSeleccion = service.AllTipoAlertas(term)
                                .Select(x => new
                                {
                                    value = x.ID,
                                    text = x.DESCRPICION,
                                });


            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetTiposAccionAlertas(string term, string tiposSel, string estadosSel)
        {

            var service = new AlertasService();

            List<int?> tiposSeleccionados = null;
            List<int?> estadosSeleccionados = null;
            if (!string.IsNullOrEmpty(tiposSel))
                tiposSeleccionados = Global.GetSplitListNullableInt(tiposSel, '-');

            if (!string.IsNullOrEmpty(estadosSel))
                estadosSeleccionados = Global.GetSplitListNullableInt(estadosSel, '-');

            var listaSeleccion = service.AllTipoAccionAlertas(term, tiposSeleccionados, estadosSeleccionados)
                              .Select(x => new
                              {
                                  value = x.ID,
                                  text = x.DESCRPICION,
                              });


            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetTipoEstadoAlertas(string term, string tiposSel)
        {

            var service = new AlertasService();

            List<int?> tiposSeleccionados = null;
            //List<int?> accionesSeleccionados = null;
            if (!string.IsNullOrEmpty(tiposSel))
                tiposSeleccionados = Global.GetSplitListNullableInt(tiposSel, '-');

            //if (!string.IsNullOrEmpty(accionesSel))
            //    accionesSeleccionados = Global.GetSplitListNullableInt(accionesSel, '-');

            var listaSeleccion = service.AllTipoEstadoAlertas(term, tiposSeleccionados)
                              .Select(x => new
                              {
                                  value = x.ID,
                                  text = x.DESCRPICION,
                              });


            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetTiposAlertasByVal(int? valor)
        {
            var service = new AlertasService();


            var listaSeleccion = (from element in service.AllTipoAlertas()
                                  where element.ID.Equals(valor ?? 0)
                                  select new
                                  {
                                      text = element.DESCRPICION,
                                      value = element.ID
                                  });
            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTiposEstadosPendienteByTipo(int? valor)
        {
            var service = new AlertasService();


            var listaSeleccion = (from element in service.GetEstadosPendienteByTipoAlerta(valor ?? 0)

                                  select new
                                  {
                                      text = element.DESCRPICION,
                                      value = element.ID
                                  });
            return Json(listaSeleccion, JsonRequestBehavior.AllowGet);
        }

    }


}
