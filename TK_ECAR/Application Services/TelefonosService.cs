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
    public class TelefonosService
    {
        /// <summary>
        /// Devuelve los teléfonos frecuentes
        /// </summary>
        /// <returns></returns>
        public List<TelefonosFrecuentesModels> GetAllTelefonos(List<int> empresas)
        {
            T_G_TELEFONOS_FRECUENTESSpecification spec = new T_G_TELEFONOS_FRECUENTESSpecification
            {
                ID_EMPRESAIN = empresas.Select(x => (int?)x),
            };

            using (var unitOfWork = new UnitOfWork())
            {
                var listaTelefonos = (from telefono in unitOfWork.RepositoryT_G_TELEFONOS_FRECUENTES.Where(spec)
                                      select new TelefonosFrecuentesModels
                                      {
                                          NUMERO_TELEFONO = telefono.NUMERO_TELEFONO,
                                          DESCRIPCION = telefono.DESCRIPCION,
                                          Accion = EnumAccionEntity.SinAccion,
                                          ID_Empresa = telefono.ID_EMPRESA,
                                          AccionDataTable = ""
                                      }).OrderBy(o => o.DESCRIPCION).ToList();


                foreach (TelefonosFrecuentesModels telefono in listaTelefonos)
                {
                    telefono.DescEmpresa = unitOfWork.RepositorySAPHR_Empresas.Fetch().Where(o => o.CodigoEmpresa == telefono.ID_Empresa).FirstOrDefault().Nombre;
                }

                return listaTelefonos;
            }
        }


        public TelefonosFrecuentesModels GetTelefono(string numTelefono)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                T_G_TELEFONOS_FRECUENTESSpecification spec = new T_G_TELEFONOS_FRECUENTESSpecification();
                spec.NUMERO_TELEFONO = numTelefono;

                var telefono = (from _telefono in unitOfWork.RepositoryT_G_TELEFONOS_FRECUENTES.Where(spec)
                                select new TelefonosFrecuentesModels
                                {
                                    ID_Empresa = _telefono.ID_EMPRESA,
                                    NUMERO_TELEFONO = _telefono.NUMERO_TELEFONO,
                                    DESCRIPCION = _telefono.DESCRIPCION,
                                    Accion = EnumAccionEntity.SinAccion,
                                    AccionDataTable = ""
                                }).FirstOrDefault();

                return telefono;
            }
        }

        public void SaveTelefono(TelefonosFrecuentesModels modelo)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                var telefono = new T_G_TELEFONOS_FRECUENTES
                {
                    ID_EMPRESA = modelo.ID_Empresa,
                    NUMERO_TELEFONO = modelo.NUMERO_TELEFONO,
                    DESCRIPCION = modelo.DESCRIPCION
                };

                if (modelo.Accion == EnumAccionEntity.Modificacion)
                {
                    unitOfWork.RepositoryT_G_TELEFONOS_FRECUENTES.Update(telefono);
                }
                else
                {
                    unitOfWork.RepositoryT_G_TELEFONOS_FRECUENTES.Insert(telefono);
                }

                unitOfWork.Commit();
            }
        }

        public void DeleteTelefono(string numTelefono)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                unitOfWork.RepositoryT_G_TELEFONOS_FRECUENTES.Delete(numTelefono);

                unitOfWork.Commit();
            }
        }
    }
}