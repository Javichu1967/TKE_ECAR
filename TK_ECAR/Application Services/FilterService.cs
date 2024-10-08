using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Controllers;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Framework;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;
using TK_ECAR.Utils;

namespace TK_ECAR.Application_Services
{
    public class FilterService
    {

        #region selección chosen
        /// <summary>
        /// Devuelve las empresas que cumplan que el nombre o el código de empresa contenga term. 
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public List<SelectChosen> GetEmpresasChosen(string term)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                SAPHR_EmpresasSpecification spec = new SAPHR_EmpresasSpecification
                {
                    Activo = true
                };
                var empresas = (from empresa in unitOfWork.RepositorySAPHR_Empresas.Where(spec)
                                where empresa.Nombre.Contains(term) || SqlFunctions.StringConvert((double)empresa.CodigoEmpresa).Trim().Contains(term)
                                select new SelectChosen
                                {
                                    text = empresa.Nombre,
                                    value = SqlFunctions.StringConvert((double)empresa.CodigoEmpresa).Trim()
                                }).OrderBy(o => o.value).ThenBy(x=>x.text).ToList();

                return empresas;
            }

        }

        public List<SelectChosen> GetDireccionesTerritorialesChosen(string term, string empresasSel)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                ISpecification<SAPHR_DireccionesTerritoriales> specGeneral = null;

                ISpecification<SAPHR_DireccionesTerritoriales> spec = new SAPHR_DireccionesTerritorialesSpecification
                {
                    Baja = false,
                };

                var specNombre = new SAPHR_DireccionesTerritorialesSpecification
                {
                    NombreContains = term
                };
                specGeneral = specNombre;

                var specCodigo = new SAPHR_DireccionesTerritorialesSpecification
                {
                    IdDTContains = term
                };
                specGeneral = specGeneral.Or(specCodigo);

                if (!string.IsNullOrEmpty(empresasSel))
                {
                    var specEmpresa = new SAPHR_DireccionesTerritorialesSpecification();
                    specEmpresa.EmpresaIN = Global.GetSplitListNullableInt(empresasSel, '-');
                    spec = spec.And(specEmpresa);
                }

                spec = spec.And(specGeneral);

                var direccionesTerritoriales = (from DT in unitOfWork.RepositorySAPHR_DireccionesTerritoriales.Where(spec)
                                                select new SelectChosen
                                                {
                                                    text = DT.Nombre,
                                                    value = DT.IdDT
                                                }).OrderBy(o => o.value).ThenBy(x => x.text).ToList();

                return direccionesTerritoriales;
            }
        }

        public List<SelectChosen> GetDelegacionesChosen(string term, string empresasSel, string dirTerritorialSel)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                ISpecification<SAPHR_Delegaciones> specGeneral = null;

                ISpecification<SAPHR_Delegaciones> spec = new SAPHR_DelegacionesSpecification
                {
                    Baja = false
                };

                var specNombre = new SAPHR_DelegacionesSpecification
                {
                    NombreContains = term
                };
                specGeneral = specNombre;

                var specCodigo = new SAPHR_DelegacionesSpecification
                {
                    IdDelegacionContains = term
                };
                specGeneral = specGeneral.Or(specCodigo);

                if (!string.IsNullOrEmpty(empresasSel))
                {
                    var specEmpresa = new SAPHR_DelegacionesSpecification();
                    specEmpresa.EmpresaIN = Global.GetSplitListNullableInt(empresasSel, '-');
                    spec = spec.And(specEmpresa);
                }

                if (!string.IsNullOrEmpty(dirTerritorialSel))
                {
                    var specDT = new SAPHR_DelegacionesSpecification();
                    specDT.IdDTIN = dirTerritorialSel.Split('-');
                    spec = spec.And(specDT);
                }

                spec = spec.And(specGeneral);

                var delegaciones = (from Dele in unitOfWork.RepositorySAPHR_Delegaciones.Where(spec)
                                    select new SelectChosen
                                    {
                                        text = Dele.Nombre,
                                        value = Dele.IdDelegacion
                                    }).OrderBy(o => o.value).ThenBy(x => x.text).ToList();

                return delegaciones;
            }
        }

        public List<SelectChosen> GetDelegacionesChosen(SAPHR_DelegacionesSpecification spec)
        {
            using (var unitOfWork = new UnitOfWork())
            {


                var delegaciones = (from Dele in unitOfWork.RepositorySAPHR_Delegaciones.Where(spec)
                                    select new SelectChosen
                                    {
                                        DevolverValueFormateado = false,
                                        text = Dele.Nombre,
                                        value = Dele.IdDelegacion
                                    }).OrderBy(o => o.value).ThenBy(x => x.text).ToList();

                return delegaciones;
            }
        }

        public List<SelectChosen> GetCentrosCosteChosen(string term, string empresasSel, string dirTerritorialSel, string delegacionSel)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                ISpecification<SAPHR_CentrosCoste> specGeneral = null;

                ISpecification<SAPHR_CentrosCoste> spec = new SAPHR_CentrosCosteSpecification
                {
                    Baja = false
                };

                var specNombre = new SAPHR_CentrosCosteSpecification
                {
                    NombreContains = term
                };
                specGeneral = specNombre;

                var specCodigo = new SAPHR_CentrosCosteSpecification
                {
                    IdCecoContains = term
                };
                specGeneral = specGeneral.Or(specCodigo);

                if (!string.IsNullOrEmpty(empresasSel))
                {
                    var specEmpresa = new SAPHR_CentrosCosteSpecification();
                    specEmpresa.EmpresaIN = Global.GetSplitListNullableInt(empresasSel, '-');
                    spec = spec.And(specEmpresa);
                }

                if (!string.IsNullOrEmpty(delegacionSel))
                {
                    var specDeleg = new SAPHR_CentrosCosteSpecification();
                    specDeleg.IdDelegacionIN = delegacionSel.Split('-');
                    spec = spec.And(specDeleg);
                }
                else if (!string.IsNullOrEmpty(dirTerritorialSel))
                {
                    var specDelegDT = new SAPHR_CentrosCosteSpecification();
                    specDelegDT.IdDelegacionIN = GeDelegacionesPorDT(dirTerritorialSel.Split('-').ToList());
                    spec = spec.And(specDelegDT);
                }

                spec = spec.And(specGeneral);

                var centrosCoste = (from CeCo in unitOfWork.RepositorySAPHR_CentrosCoste.Where(spec)
                                        select new SelectChosen
                                        {
                                            text = CeCo.Nombre,
                                            value =  CeCo.IdCeco 
                                        }).OrderBy(o => o.value).ThenBy(x => x.text).ToList();

                return centrosCoste;

            }
        }

        /// <summary>
        /// Devuelve todas las agrupaciones de filtrado
        /// </summary>
        /// <returns></returns>
        public List<SelectChosen> GetAgrupacionChosen()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                BaseController usuario = new BaseController();

                var cecosUsuario = new List<string>();
                cecosUsuario = usuario.CecosUser.Distinct().ToList();

                ISpecification<SAPHR_FiltroSeleccionCentrosCoste>  spec = null;

                foreach (var ceco in cecosUsuario)
                {
                    var filtrospec = new SAPHR_FiltroSeleccionCentrosCosteSpecification();
                    filtrospec.FiltroCentroCosteStartsWith = ceco.Substring(0, 3);
                    if (ceco.Substring(0, 3) != "105")
                    {
                        filtrospec.FiltroCentroCosteEndsWith = ceco.Substring(ceco.Length-1, 1);
                    }
                    else
                    {
                        filtrospec.FiltroCentroCosteEndsWith = "X";
                    }
                    if (spec != null)
                    {
                        spec = spec.Or(filtrospec);
                    }
                    else
                    {
                        spec = filtrospec;
                    }
                }

                if (spec == null)
                {
                    spec = new SAPHR_FiltroSeleccionCentrosCosteSpecification();
                }

                var centrosCoste = (from agrupacion in unitOfWork.RepositorySAPHR_FiltroSeleccionCentrosCoste.Where(spec)
                                    select new SelectChosen
                                    {
                                        PonerValuePorDelanteDeTexto = false,
                                        text = agrupacion.Agrupacion1 + (agrupacion.Agrupacion2 != null ? " - " + agrupacion.Agrupacion2 : ""),
                                        value = agrupacion.FiltroCentroCoste
                                    }).OrderBy(o => o.text).ThenBy(x => x.value).ToList();

                string agrAnt = string.Empty;
                var centrosCosteAgr = new List<SelectChosen>();
                foreach (var agr in centrosCoste)
                {
                    if (agr.text.ToUpper() != agrAnt.ToUpper())
                    {
                        centrosCosteAgr.Add(new SelectChosen { PonerValuePorDelanteDeTexto = false, text = agr.text, value = agr.value });
                    }
                    else
                    {
                        centrosCosteAgr.First(x => x.text == agr.text).value = centrosCosteAgr.First(x => x.text == agr.text).value + "," + agr.value;
                    }
                    agrAnt = agr.text;
                }

                return centrosCosteAgr;
            }
        }


        ///// <summary>
        ///// Devuelve las agrupaciones de filtrado por empresa
        ///// </summary>
        ///// <param name="lEmpresas"></param>
        ///// <returns></returns>
        //public List<SelectChosen> GetAgrupacionPorEmpresaChosen(List<int?> lEmpresas)
        //{
        //    using (var unitOfWork = new UnitOfWork())
        //    {

        //        var lCeco = DevuelveDistintosCentrosCosteParaCompararFiltro(lEmpresas, new List<string>(), new List<string>());

        //        return DevuelveFiltroAgrupacionesCentrosCoste(lCeco);
        //    }
        //}


        ///// <summary>
        ///// Devuelve las agrupaciones de filtrado por DT
        ///// </summary>
        ///// <param name="lDT"></param>
        ///// <returns></returns>
        //public List<SelectChosen> GetAgrupacionPorDTChosen(List<string> lDT)
        //{
        //    using (var unitOfWork = new UnitOfWork())
        //    {

        //        var lCeco = DevuelveDistintosCentrosCosteParaCompararFiltro(new List<int?>(), lDT, new List<string>());

        //        return DevuelveFiltroAgrupacionesCentrosCoste(lCeco);
        //    }
        //} 


        ///// <summary>
        ///// Devuelve las agrupaciones de filtrado por Delegación
        ///// </summary>
        ///// <param name="lDeleg"></param>
        ///// <returns></returns>
        //public List<SelectChosen> GetAgrupacionPorDelegacionChosen(List<string> lDeleg)
        //{
        //    using (var unitOfWork = new UnitOfWork())
        //    {

        //        var lCeco = DevuelveDistintosCentrosCosteParaCompararFiltro(new List<int?>(), new List<string>(), lDeleg);

        //        return DevuelveFiltroAgrupacionesCentrosCoste(lCeco);
        //    }
        //}

        //#region métodos privados de chosen
        //public List<string> DevuelveDistintosCentrosCosteParaCompararFiltro(List<int?> lEmpresas, List<string> lDT, List<string> lDeleg)
        //{

        //    using (var unitOfWork = new UnitOfWork())
        //    {
        //        SAPHR_CentrosCosteSpecification spec = new SAPHR_CentrosCosteSpecification
        //        {
        //            Baja = false
        //        };
        //        if (lEmpresas.Count > 0)
        //        {
        //            spec.EmpresaIN = lEmpresas;
        //        }
        //        if (lDT.Count > 0)
        //        {
        //            SAPHR_DelegacionesSpecification specDel = new SAPHR_DelegacionesSpecification
        //            {
        //                Baja = false,
        //                IdDTIN = lDT
        //            };
        //            List<string> ldelegacionesDT = new List<string>();

        //            ldelegacionesDT = (from dele in unitOfWork.RepositorySAPHR_Delegaciones.Where(specDel)
        //                                select (dele.IdDelegacion)).ToList();

        //            spec.IdDelegacionIN = ldelegacionesDT;
        //        }
        //        if (lDeleg.Count > 0)
        //        {
        //            spec.IdDelegacionIN = lDeleg;
        //        }

        //        //Int32 valor = 0;
        //        List<string> lReturn = new List<string>();
        //        lReturn = (from agrupacion in unitOfWork.RepositorySAPHR_CentrosCoste.Where(spec)
        //                   select (agrupacion.IdCeco.Substring(0, 7))).Distinct().Select(idCeco => Int32.TryParse(idCeco, out valor) ? valor.ToString() : idCeco).ToList();


        //        return lReturn;
        //    }

        //}


        //private List<SelectChosen> DevuelveFiltroAgrupacionesCentrosCoste(List<string> lCeco)
        //{
        //    using (var unitOfWork = new UnitOfWork())
        //    {
        //        var centrosCosteFiltro = (from agrupacion in unitOfWork.RepositorySAPHR_FiltroSeleccionCentrosCoste.Fetch().
        //                        Where(filtro => lCeco.Any(ceco => filtro.FiltroCentroCoste.Substring(0, 3).ToUpper() == ceco.ToUpper()))
        //                            select new SelectChosen
        //                            {
        //                                text = agrupacion.Agrupacion1 + (agrupacion.Agrupacion2 != null ? " - " + agrupacion.Agrupacion2 : ""),
        //                                value = agrupacion.Agrupacion1 + (agrupacion.Agrupacion2 != null ? "-" + agrupacion.Agrupacion2 : "")
        //                            }).Distinct().OrderBy(o => o.text).ToList();
        //        return centrosCosteFiltro;
        //    }
        //}
        //#endregion

        #endregion

        /// <summary>
        /// Devuelve el campo FiltroCentroCoste según las agrupaciones que se pasen como parámetro. Esto era cuando se pasaban las agrupaciones por el texto. 
        /// Ya no procede.
        /// </summary>
        /// <param name="lAgrupaciones"></param>
        /// <returns></returns>
        public List<string> GetFiltroCentroCosteAgrupados(List<string> lAgrupaciones)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var filtroCentrosCoste = (from agrupacion in unitOfWork.RepositorySAPHR_FiltroSeleccionCentrosCoste.Fetch().
                                          Where(filtro => lAgrupaciones.Any(agr => filtro.Agrupacion1 + (filtro.Agrupacion2 != null ? "-" + filtro.Agrupacion2 : "") == agr.ToUpper()))
                                          select (agrupacion.FiltroCentroCoste)).Distinct().ToList();


                return filtroCentrosCoste;

            }
        }


        /// <summary>
        /// Devuelve una lista de delegaciones que estén dentro de la lista de Direcciones Territoriales que se pasan por parámetro
        /// </summary>
        /// <param name="lDT"></param>
        /// <returns></returns>
        public List<string> GeDelegacionesPorDT(List<string> lDT)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                SAPHR_DelegacionesSpecification spec = new SAPHR_DelegacionesSpecification
                {
                    Baja = false,
                    IdDTIN = lDT
                };

                var delegaciones = (from deleg in unitOfWork.RepositorySAPHR_Delegaciones.Where(spec)
                                          select (deleg.IdDelegacion)).Distinct().ToList();


                return delegaciones;

            }
        }

    }
}
