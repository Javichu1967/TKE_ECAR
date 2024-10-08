using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Framework;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;

namespace TK_ECAR.Application_Services
{
    public class PreavisosAlertasService
    {
        private List<PreavisosAlertasModel> GetTiposAlertas(T_M_TIPOS_ALERTASSpecification spec)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var listaTipoAlertas = (from tipoAlerta in unitOfWork.RepositoryT_M_TIPOS_ALERTAS.Where(spec)
                                         select new PreavisosAlertasModel
                                         {
                                             idTipoAlerta = tipoAlerta.ID_TIPO_ALERTA,
                                             DescTipoAlerta = tipoAlerta.DESCRIPCION,
                                             Prioridad = tipoAlerta.PRIORIDAD,
                                             DiasPreaviso = tipoAlerta.DIAS_PREAVISO,
                                             Automatica = tipoAlerta.B_AUTOMATICA,
                                             AccionDatatable = ""
                                         }).OrderBy(o => o.DescTipoAlerta).ToList();

                return listaTipoAlertas;
            }
        }

        /// <summary>
        /// Devuelve los Tipos de Alertas automáticas
        /// </summary>
        /// <param name="Categorias"></param>
        /// <returns></returns>
        public List<PreavisosAlertasModel> GetTiposAlertasAutomaticasDatatable()
        {
            T_M_TIPOS_ALERTASSpecification spec = new T_M_TIPOS_ALERTASSpecification
            {
                B_AUTOMATICA = true
            };

            var listaTipoAlertas = GetTiposAlertas(spec);

            return listaTipoAlertas;
        }

        /// <summary>
        /// Devuelve los Tipos de Alertas que no son automáticas
        /// </summary>
        /// <param name="Categorias"></param>
        /// <returns></returns>
        public List<PreavisosAlertasModel> GetTiposAlertasNOAutomaticasDatatable()
        {
            T_M_TIPOS_ALERTASSpecification spec = new T_M_TIPOS_ALERTASSpecification
            {
                B_AUTOMATICA = false
            };

            var listaTipoAlertas = GetTiposAlertas(spec);

            return listaTipoAlertas;
        }

        /// <summary>
        /// Devuelve todos los Tipos de Alertas
        /// </summary>
        /// <param name="Categorias"></param>
        /// <returns></returns>
        public List<PreavisosAlertasModel> GetTiposAlertasDatatable()
        {
            T_M_TIPOS_ALERTASSpecification spec = new T_M_TIPOS_ALERTASSpecification
            {
            };

            var listaTipoAlertas = GetTiposAlertas(spec);

            return listaTipoAlertas;
        }

        public PreavisosAlertasModel GetTipoAlertas(int idtTipoAlerta)
        {
            T_M_TIPOS_ALERTASSpecification spec = new T_M_TIPOS_ALERTASSpecification
            {
                ID_TIPO_ALERTA = idtTipoAlerta
            };

            using (var unitOfWork = new UnitOfWork())
            {
                var listaTipoAlertas = (from tipoAlerta in unitOfWork.RepositoryT_M_TIPOS_ALERTAS.Where(spec)
                                        select new PreavisosAlertasModel
                                        {
                                            idTipoAlerta = tipoAlerta.ID_TIPO_ALERTA,
                                            DescTipoAlerta = tipoAlerta.DESCRIPCION,
                                            Prioridad = tipoAlerta.PRIORIDAD,
                                            DiasPreaviso = tipoAlerta.DIAS_PREAVISO,
                                            Automatica = tipoAlerta.B_AUTOMATICA,
                                            AccionDatatable = ""
                                        }).FirstOrDefault();

                return listaTipoAlertas;
            }
        }


        public void SaveTipoAlerta(PreavisosAlertasModel modelo)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                var tipoAlerta = new T_M_TIPOS_ALERTAS
                {
                    ID_TIPO_ALERTA = modelo.idTipoAlerta,
                    DESCRIPCION = modelo.DescTipoAlerta,
                    B_AUTOMATICA = modelo.Automatica,
                    PRIORIDAD = modelo.Prioridad,
                    DIAS_PREAVISO = modelo.DiasPreaviso == 0 ? null : modelo.DiasPreaviso
                };

                unitOfWork.RepositoryT_M_TIPOS_ALERTAS.Update(tipoAlerta);

                unitOfWork.Commit();
            }
        }
    }
}