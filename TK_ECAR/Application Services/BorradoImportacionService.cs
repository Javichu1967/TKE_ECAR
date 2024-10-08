using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Framework;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;
using TK_ECAR.Utils;
using static TK_ECAR.Utils.Global;

namespace TK_ECAR.Application_Services
{
    public class BorradoImportacionService
    {
        public List<SelectChosen> GetArchivosBorradoChosen(EnumTipoBorradoImportacion tipoBorrado)
        {
            List<SelectChosen> archivos = new List<SelectChosen>();

            IEnumerable<string> lista = Enumerable.Empty<string>();

            using (var unitOfWork = new UnitOfWork())
            {
                switch (tipoBorrado)
                {
                    case EnumTipoBorradoImportacion.BorrarImportacionFlota:
                        lista = unitOfWork.RepositoryECAR_Datos_Vehiculo.Fetch()
                                            .Where(x=> x.NOMBRE_ARCHIVO_IMPORTACION != null)
                                            .Select(x => x.NOMBRE_ARCHIVO_IMPORTACION).Distinct();
                        break;
                    case EnumTipoBorradoImportacion.BorrarImportacionFacuracion:
                        lista = unitOfWork.RepositoryT_G_DATOS_LEASING.Fetch()
                                            .Where(x => x.NOMBRE_ARCHIVO_IMPORTACION != null)
                                            .Select(x => x.NOMBRE_ARCHIVO_IMPORTACION).Distinct();
                        break;
                    case EnumTipoBorradoImportacion.BorrarImportacionViaVerde:
                        lista = unitOfWork.RepositoryT_G_VIA_VERDE_EXTRACTOS.Fetch()
                                            .Where(x => x.NOMBRE_ARCHIVO_IMPORTACION != null)
                                            .Select(x => x.NOMBRE_ARCHIVO_IMPORTACION).Distinct();
                        break;
                    case EnumTipoBorradoImportacion.BorrarImportacionCombustible:
                        lista = unitOfWork.RepositoryT_G_TARJETA_COMBUSTIBLE.Fetch()
                                            .Where(x => x.NOMBRE_ARCHIVO_IMPORTACION != null)
                                            .Select(x => x.NOMBRE_ARCHIVO_IMPORTACION).Distinct();
                        break;
                }

                foreach(string nombre in lista)
                {
                    archivos.Add(new SelectChosen
                    {
                        PonerValuePorDelanteDeTexto = false,
                        text = nombre,
                        value = nombre,
                    });
                }
            }

            return archivos.OrderBy(x => x.text).ToList();

        }

        #region Borrado de datos de importación
        public bool BorraDatosImportacion(EnumTipoBorradoImportacion tipoBorrado, string nombreArchivo)
        {
            bool valorReturn = true;
            switch (tipoBorrado)
            {
                case EnumTipoBorradoImportacion.BorrarImportacionFlota:
                    valorReturn = BorrarDatosFlota(nombreArchivo);
                    break;
                case EnumTipoBorradoImportacion.BorrarImportacionFacuracion:
                    valorReturn = BorrarDatosFacturacion(nombreArchivo);
                    break;
                case EnumTipoBorradoImportacion.BorrarImportacionViaVerde:
                    valorReturn = BorrarDatosViaVerde(nombreArchivo);
                    break;
                case EnumTipoBorradoImportacion.BorrarImportacionCombustible:
                    valorReturn = BorrarDatosCombustible(nombreArchivo);
                    break;
                default:
                    break;
            }

            return valorReturn;
        }

        private bool BorrarDatosFlota(string nombreArchivo)
        {
            bool valorReturn = true;

            try
            {
                ECAR_Datos_VehiculoSpecification spec = new ECAR_Datos_VehiculoSpecification
                {
                    NOMBRE_ARCHIVO_IMPORTACION = nombreArchivo,
                };

                using (var scope = new TransactionScope())
                {
                    using (var unitOfWork = new UnitOfWork())
                    {
                        foreach (ECAR_Datos_Vehiculo vehiculo in unitOfWork.RepositoryECAR_Datos_Vehiculo.Where(spec))
                        {
                            ECAR_Datos_ITVSpecification specITV = new ECAR_Datos_ITVSpecification
                            {
                                Matricula = vehiculo.Matricula,
                            };
                            unitOfWork.RepositoryECAR_Datos_ITV.RemoveRange(
                                        unitOfWork.RepositoryECAR_Datos_ITV.Where(specITV).ToList());
                            unitOfWork.Commit();

                            T_G_DATOS_LEASINGSpecification specLeasing = new T_G_DATOS_LEASINGSpecification
                            {
                                Matricula = vehiculo.Matricula,
                            };
                            unitOfWork.RepositoryT_G_DATOS_LEASING.RemoveRange(
                                        unitOfWork.RepositoryT_G_DATOS_LEASING.Where(specLeasing).ToList());
                            unitOfWork.Commit();
                        }
                        unitOfWork.RepositoryECAR_Datos_Vehiculo.RemoveRange(
                                    unitOfWork.RepositoryECAR_Datos_Vehiculo.Where(spec).ToList());
                        unitOfWork.Commit();

                        //unitOfWork.RepositoryECAR_Datos_Vehiculo.RemoveRange(
                        //    unitOfWork.RepositoryECAR_Datos_Vehiculo.Include(
                        //        x => x.ECAR_Datos_ITV)
                        //        .Where(x => x.NOMBRE_ARCHIVO_IMPORTACION == nombreArchivo).ToList());
                        //unitOfWork.Commit();
                    }
                    scope.Complete();
                }
            }

            catch (Exception ex)
            {
                valorReturn = false;
                Global.EscribeLogApp(TipoDeLog.ERROR, $"<BorrarDatosFlota>. {ex.Message}");
            }

            return valorReturn;
        }

        private bool BorrarDatosCombustible(string nombreArchivo)
        {
            bool valorReturn = true;

            try
            {
                T_G_TARJETA_COMBUSTIBLESpecification spec = new T_G_TARJETA_COMBUSTIBLESpecification
                {
                    NOMBRE_ARCHIVO_IMPORTACION = nombreArchivo,
                };

                using (var unitOfWork = new UnitOfWork())
                {
                    unitOfWork.RepositoryT_G_TARJETA_COMBUSTIBLE.RemoveRange(unitOfWork.RepositoryT_G_TARJETA_COMBUSTIBLE.Where(spec).ToList());
                    unitOfWork.Commit();
                }
            }

            catch(Exception ex)
            {
                valorReturn = false;
                Global.EscribeLogApp(TipoDeLog.ERROR, $"<BorrarDatosCombustible>. {ex.Message}");
            }

            return valorReturn;
        }


        private bool BorrarDatosFacturacion(string nombreArchivo)
        {
            bool valorReturn = true;

            try
            {
                T_G_DATOS_LEASINGSpecification spec = new T_G_DATOS_LEASINGSpecification
                {
                    //Sociedad = empresa,
                    NOMBRE_ARCHIVO_IMPORTACION = nombreArchivo,
                };

                using (var unitOfWork = new UnitOfWork())
                {
                    unitOfWork.RepositoryT_G_DATOS_LEASING.RemoveRange(unitOfWork.RepositoryT_G_DATOS_LEASING.Where(spec).ToList());
                    unitOfWork.Commit();
                }
            }

            catch (Exception ex)
            {
                valorReturn = false;
                Global.EscribeLogApp(TipoDeLog.ERROR, $"<BorrarDatosLeasing>. {ex.Message}");
            }

            return valorReturn;
        }

        private bool BorrarDatosViaVerde(string nombreArchivo)
        {
            bool valorReturn = true;

            try
            {
                T_G_VIA_VERDE_EXTRACTOSSpecification spec = new T_G_VIA_VERDE_EXTRACTOSSpecification
                {
                    NOMBRE_ARCHIVO_IMPORTACION = nombreArchivo,
                };

                using (var scope = new TransactionScope())
                {
                    using (var unitOfWork = new UnitOfWork())
                    {
                        foreach (T_G_VIA_VERDE_EXTRACTOS extracto in unitOfWork.RepositoryT_G_VIA_VERDE_EXTRACTOS.Where(spec))
                        {
                            var idExtracto = extracto.ID_EXTRACTO;
                            T_G_VIA_VERDE_IDENTIFICADORESSpecification specIdentificador = new T_G_VIA_VERDE_IDENTIFICADORESSpecification
                            {
                                ID_EXTRACTO = idExtracto,
                            };

                            foreach (T_G_VIA_VERDE_IDENTIFICADORES identificador in unitOfWork.RepositoryT_G_VIA_VERDE_IDENTIFICADORES.Where(specIdentificador))
                            {
                                T_G_VIA_VERDE_TRANSACCIONESSpecification specTransaccion = new T_G_VIA_VERDE_TRANSACCIONESSpecification
                                {
                                    ID_IDENTIFICADOR = identificador.ID_IDENTIFICADOR,
                                };
                                unitOfWork.RepositoryT_G_VIA_VERDE_TRANSACCIONES.RemoveRange(unitOfWork.RepositoryT_G_VIA_VERDE_TRANSACCIONES.Where(specTransaccion).ToList());
                                unitOfWork.Commit();
                            }
                            unitOfWork.RepositoryT_G_VIA_VERDE_IDENTIFICADORES.RemoveRange(unitOfWork.RepositoryT_G_VIA_VERDE_IDENTIFICADORES.Where(specIdentificador).ToList());
                            unitOfWork.Commit();

                            unitOfWork.RepositoryT_G_VIA_VERDE_EXTRACTOS.Delete(extracto);
                            unitOfWork.Commit();
                        }
                    }
                    scope.Complete();
                }
            }

            catch (Exception ex)
            {
                valorReturn = false;
                Global.EscribeLogApp(TipoDeLog.ERROR, $"<BorrarDatosViaVerde>. {ex.Message}");
            }

            return valorReturn;
        }

        #endregion Borrado de datos de importación
    }
}