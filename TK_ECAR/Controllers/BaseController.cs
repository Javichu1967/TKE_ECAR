using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Application_Services;
using TK_ECAR.Framework;
using TK_ECAR.Models;
using TK_ECAR.Filters;
using TKUtilidades;
using TK_ECAR.Utils;

namespace TK_ECAR.Controllers
{
    [AuthorizeUser]
    public class BaseController : Controller
    {
        public UserModel UserModel
        {
            get
            {
                return (UserModel)Util.GetItemFromMemory("userProfile");
            }
        }

        public List<string> CecosUser
        {
            get
            {
                return  UserModel.CecosModelList
                    .Select(x => x.IdCeco)
                    .Distinct()
                    .ToList();
            }
        }

        public List<string> CecosUserFormatted
        {
            get
            {
                return UserModel.CecosModelList
                    .Select(x => x.IdCecoFormatted)
                    .Distinct()
                    .ToList();
            }
        }
        public List<string> CecosUserByFilter(FilterModel modelo)
        {
            var empresas = new List<int>();
            var dts = new List<string>();
            var delegaciones = new List<string>();
            var centrosCoste = new List<string>();


            if (modelo.AgrupacionesFiltroSeleccionadas != null)
            {
                var agrupaciones = modelo.AgrupacionesFiltroSeleccionadas.Split(',').ToList();

                centrosCoste = cecosUserInAgrupaciones(agrupaciones);

            }
            else
            {
                if (modelo.EmpresasSeleccionadas != null)
                    empresas = modelo.EmpresasSeleccionadas.Split(',').Select(x => int.Parse(x)).ToList();

                if (modelo.DireccionesTerritorialesSeleccionadas != null)
                    dts = modelo.DireccionesTerritorialesSeleccionadas.Split(',').ToList();

                if (modelo.DelegacionesSeleccionadas != null)
                    delegaciones = modelo.DelegacionesSeleccionadas.Split(',').ToList();

                if (modelo.CentrosCosteSeleccionados != null)
                    centrosCoste = modelo.CentrosCosteSeleccionados.Split(',').ToList();
            }
             
            return (from x in UserModel.CecosModelList
                    where (!empresas.Any() || empresas.Contains(x.CodigoEmpresa))
                    where (!dts.Any() || dts.Contains(x.IdDT))
                    where (!delegaciones.Any() || delegaciones.Contains(x.IdDelegacion))
                    where ((!centrosCoste.Any() || centrosCoste.Contains(x.IdCeco)) && x.Baja == false) //Añado que mire la baja porque hay vehiculos cuyo CECO ha cambiado de empresa, 
                    select x.IdCeco).ToList();                                                          // dándose de baja el CECO en esa empresa. Pero salen en la empresa que no es también.
        }

        private List<string> cecosUserInAgrupaciones(List<string> codigosAgrupaciones)
        {
            IEnumerable<string> auxEnumerable =  null;
            foreach(var codigoAgrupacion in codigosAgrupaciones)
            {
                if (auxEnumerable == null)
                {
                    auxEnumerable = cecosInAgrupacion(codigoAgrupacion);
                }
                else
                {
                    auxEnumerable = auxEnumerable.Union(cecosInAgrupacion(codigoAgrupacion));
                }
            }
             return auxEnumerable               
                    .Distinct()
                    .ToList();
             
        }

        private IEnumerable<string> cecosInAgrupacion(string agrupacion)
        {
            var cecos = UserModel.CecosModelList.Where(x => x.IdCeco.StartsWith(agrupacion.Substring(0, 3)));
                                
            if (!agrupacion.EndsWith(Constants.AGRUPACION_COSTE_ENDWITH))
            {                
                cecos = cecos
                        .Where(x => x.IdCeco.EndsWith(agrupacion.Substring(agrupacion.Length - 1, 1)));
            }           
            return cecos.Select(x => x.IdCeco);
        }

        /// <summary>
        /// Devuelve si la opción de menú es accesible para el usuario que la está usando.
        /// </summary>
        /// <param name="opcion"></param>
        /// <returns></returns>
        public bool OptionMenuVisible(EnumOpcionesMenu opcion)
        {
            bool valorReturn = false;

            valorReturn = UserModel.OpcionesMenu.Any(o => o.idMenu == (int)opcion);
            if (valorReturn && opcion == EnumOpcionesMenu.MiVehiculo)
            {
                string Matricula = new MiVehiculoService().GetMatriculaVehiculoActivo(UserModel.Dni);

                valorReturn = !string.IsNullOrEmpty(Matricula);
            }

            return valorReturn;
        }

        public IEnumerable<MenuModel> GetUserMenu()
        {
            return UserModel.OpcionesMenu;
        }


        public JsonResult GetCentrosCosteID(string ID)
        {
            var seleccion = UserModel.CecosModelList.Where(x => x.IdCeco == ID || x.IdCecoFormatted == ID).FirstOrDefault();

            var cecoChosen = new List<SelectChosen>();
            if (seleccion != null)
            {
                cecoChosen.Add(new SelectChosen
                {
                    text = seleccion.Ceco,
                    value = seleccion.IdCeco,
                    DevolverValueFormateado = false,
                    PonerValuePorDelanteDeTexto = false,
                });
            }

            return Json(cecoChosen, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Devuelve una lista (chosen) con los id y descripciones del enumerado
        /// </summary>
        /// <param name="idEnum"></param>
        /// <returns></returns>
        public IEnumerable<SelectChosen> GetEnumeradorByVal<T>(int idEnum) where T : struct, IConvertible
        {

            var listaSeleccion = EnumUtilities.GetEnumByVal<T>(idEnum);

            var EnumFiltrados = new List<SelectChosen>();

            EnumFiltrados = listaSeleccion.Where(o => o.value == idEnum.ToString()).ToList();


            return EnumFiltrados;
        }

        public IEnumerable<SelectChosen> GetEnumeradores<T>(string term) where T : struct, IConvertible
        {

            var listaSeleccion = EnumUtilities.GetAllEnums<T>();


            var EnumFiltrados = new List<SelectChosen>();

            if (term != string.Empty)
            {
                EnumFiltrados = listaSeleccion.Where(o => o.text.Contains(term)).ToList();
                return EnumFiltrados;
            }

            EnumFiltrados = listaSeleccion.ToList();
            return EnumFiltrados;
        }


    }


}
