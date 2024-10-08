using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Framework;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;
using TK_ECAR.Utils;
using TKUtilidades;

namespace TK_ECAR.Application_Services
{
    public class TarjetasCombustibleService
    {
        /// <summary>
        /// Devuelve las tarjetas de combustible
        /// </summary>
        /// <param name="mirarBaja"></param>
        /// <returns></returns>
        public List<TarjetasCombustibleDataTableModel> AlltarjetasDataTable(List<int?> idEmpresas, bool mirarBaja = true, List<int?> empresasEmisoras = null)
        {
            V_TARJETAS_COMBUSTIBLE_EMPRESASSpecification espec = new V_TARJETAS_COMBUSTIBLE_EMPRESASSpecification();

            if (idEmpresas != null)
            {
                espec.ID_EMPRESAIN = idEmpresas.Select(x => (int?)x).ToList();
            }

            if (empresasEmisoras != null)
            {
                espec.ID_EMPRESA_EMISORAIN = empresasEmisoras;
            }

            if (mirarBaja)
            {
                espec.BAJA = false;
            }

            return GetTarjetasCombustible(espec);
        }


        /// <summary>
        /// Devuelve una empresa según su ID
        /// </summary>
        /// <param name="idConductor"></param>
        /// <returns></returns>
        public TarjetasCombustibleModels GetTarjetaByID(int idtarjeta)
        {

            V_TARJETAS_COMBUSTIBLE_EMPRESASSpecification espec = new V_TARJETAS_COMBUSTIBLE_EMPRESASSpecification
            {
                ID_TARJETA = idtarjeta,
            };

            var lista = GetTarjetasCombustible(espec).FirstOrDefault();

            var tarjeta = new TarjetasCombustibleModels();

            if (lista != null)
            {
                tarjeta.Accion = EnumAccionEntity.SinAccion;
                tarjeta.Baja = lista.Baja;
                tarjeta.CodTarjeta = lista.CodTarjeta;
                tarjeta.FechaCaducidad = lista.FechaCaducidad;
                tarjeta.IDEmpresa = lista.IDEmpresa;
                tarjeta.IDEmpresaEmisora = lista.IDEmpresaEmisora;
                tarjeta.IDTarjeta = lista.IDTarjeta;
                tarjeta.PIN = lista.PIN;
                tarjeta.MatriculaAsociadaTarjeta = lista.MatriculaAsociadaTarjeta;
            }

            return tarjeta;

        }


        public List<TarjetasCombustibleDataTableModel> AlltarjetasDataTable(bool mirarBaja = true)
        {
            V_TARJETAS_COMBUSTIBLE_EMPRESASSpecification espec = new V_TARJETAS_COMBUSTIBLE_EMPRESASSpecification();

            if (mirarBaja)
            {
                espec.BAJA = false;
            }

            return GetTarjetasCombustible(espec);
        }


        /// <summary>
        ///Devuelve las empresas de renting y aseguradoras, según la especificación
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        private List<TarjetasCombustibleDataTableModel> GetTarjetasCombustible(V_TARJETAS_COMBUSTIBLE_EMPRESASSpecification spec)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var lista = (from tarjeta in unitOfWork.RepositoryV_TARJETAS_COMBUSTIBLE_EMPRESAS.Where(spec)
                             join
                             tipo in unitOfWork.RepositoryT_M_TIPO_TARJETA_COMBUSTIBLE.Fetch() on tarjeta.ID_EMPRESA_EMISORA equals tipo.ID_TARJETA_COMBUSTIBLE
                             where tipo.BAJA == false
                             join
                             vehiculo in unitOfWork.RepositoryECAR_Datos_Vehiculo.Fetch() on tarjeta.ID_TARJETA equals vehiculo.IDTarjetaCombustible into tarjetasVehiculos
                             from tarjetasVehiculosfinal in tarjetasVehiculos.DefaultIfEmpty()
                             select new TarjetasCombustibleDataTableModel
                                        {
                                            IDEmpresa = tarjeta.ID_EMPRESA,
                                            CodTarjeta = tarjeta.COD_TARJETA,
                                            IDEmpresaEmisora = tarjeta.ID_EMPRESA_EMISORA,
                                            FechaCaducidad = tarjeta.FECHA_CADUCIDAD,
                                            IDTarjeta = tarjeta.ID_TARJETA,
                                            NombreEmpresaEmisora = tarjeta.NOMBRE_EMPRESA_EMISORA,
                                            NombreEmpresaTarjeta = tarjeta.NOMBRE_EMPRESA,
                                            PIN = tarjeta.PIN,
                                            Baja = tarjeta.BAJA,
                                            Accion = EnumAccionEntity.SinAccion,
                                            AccionDatatable = "",
                                            MatriculaAsociadaTarjeta = tarjetasVehiculosfinal.Matricula ?? "",
                             }).OrderBy(x => x.CodTarjeta).ToList();


                return lista;
            }

        }


        public bool SaveTarjeta(TarjetasCombustibleModels modelo)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    using (var unitOfWork = new UnitOfWork())
                    {
                        var tarjeta = new T_M_TARJETAS_CONBUSTIBLE
                        {
                            COD_TARJETA = modelo.CodTarjeta,
                            FECHA_CADUCIDAD = (DateTime)modelo.FechaCaducidad,
                            ID_EMPRESA = (int)modelo.IDEmpresa,
                            ID_EMPRESA_EMISORA = (int)modelo.IDEmpresaEmisora,
                            PIN = modelo.PIN,
                            BAJA = modelo.Baja,
                        };

                        if (modelo.Accion == EnumAccionEntity.Alta)
                        {
                            unitOfWork.RepositoryT_M_TARJETAS_CONBUSTIBLE.Insert(tarjeta);
                        }
                        else
                        {
                            tarjeta.ID_TARJETA = modelo.IDTarjeta;
                            unitOfWork.RepositoryT_M_TARJETAS_CONBUSTIBLE.Update(tarjeta);
                        }

                        unitOfWork.Commit();

                        //Si el modelo trae matrícula, sustituir por la que tiene y generar registro en histótico.
                        if (!string.IsNullOrEmpty(modelo.MatriculaAsociadaTarjeta))
                        {
                            var vehiculo = unitOfWork.RepositoryECAR_Datos_Vehiculo.FindOne(x => x.Matricula == modelo.MatriculaAsociadaTarjeta);
                            var numtarjetaAnt = vehiculo.IDTarjetaCombustible;
                            vehiculo.IDTarjetaCombustible = tarjeta.ID_TARJETA;
                            var hist = new T_G_HIST_CAMBIOS_TARJETA
                            {
                                FECHA_ALTA = DateTime.Now,
                                ID_TARJETA_ANT = numtarjetaAnt,
                                ID_TARJETA_NUEVA = tarjeta.ID_TARJETA,
                                MATRICULA = modelo.MatriculaAsociadaTarjeta,
                                USUARIO_CREACION = ((UserModel)Util.GetItemFromMemory("userProfile")).Login,
                            };
                            unitOfWork.RepositoryT_G_HIST_CAMBIOS_TARJETA.Insert(hist);
                            unitOfWork.Commit();
                        }
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, ex.Message);
                return false;
            }

            return true;
        }

        public bool DeleteTarjeta(int idTarjeta)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var tarjeta = unitOfWork.RepositoryT_M_TARJETAS_CONBUSTIBLE.Fetch().Where(x => x.ID_TARJETA == idTarjeta).FirstOrDefault();

                    tarjeta.BAJA = true;

                    unitOfWork.RepositoryT_M_TARJETAS_CONBUSTIBLE.Update(tarjeta);

                    unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, ex.Message);
                return false;
            }

            return true;
        }


        public void DeleteTarjetasByIdEmpresaEmisora(int idEmpresaEmisora, UnitOfWork unitOfWork)
        {
            T_M_TARJETAS_CONBUSTIBLESpecification spec = new T_M_TARJETAS_CONBUSTIBLESpecification
            {
                ID_EMPRESA_EMISORA = idEmpresaEmisora,
            };

            var tarjetas = unitOfWork.RepositoryT_M_TARJETAS_CONBUSTIBLE.Where(spec).ToList();

            foreach(T_M_TARJETAS_CONBUSTIBLE tarjeta in tarjetas)
            {
                tarjeta.BAJA = true;

                unitOfWork.RepositoryT_M_TARJETAS_CONBUSTIBLE.Update(tarjeta);

                unitOfWork.Commit();
            }
        }

    }
}