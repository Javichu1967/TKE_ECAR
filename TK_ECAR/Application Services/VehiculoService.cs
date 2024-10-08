using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Application_Services.DTOs;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Framework;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;
using TK_ECAR.Utils;
using resourceView = TK_ECAR.Content.resources.TK_ECAR_Resource;

namespace TK_ECAR.Application_Services
{
    public class VehiculoService
    {
        #region Carga de Datos
        public DatosVehiculoModel GetDatosVehiculoECAR(string matricula)
        {

            var datosGeneralesVehiculo = GetDatosGeneralesVehiculoECAR(matricula);
            var datosITVVehiculo = GetListDatosITV_ECAR(matricula);
            var datosContratoVehiculo = GetDatosContratoVehiculoECAR(matricula);

            DatosVehiculoModel datosVehiculo = new DatosVehiculoModel
            {
                DatosGenerales_Vehiculo = datosGeneralesVehiculo != null ? datosGeneralesVehiculo : new DatosGeneralesModel(),
                //DatosITV_Vehiculo = datosITVVehiculo != null ? datosITVVehiculo : new DatosITVModel(),
                DatosContrato_Vehiculo = datosContratoVehiculo != null ? datosContratoVehiculo : new DatosContratoModel(),
            };

            return datosVehiculo;
        }



        public DatosGeneralesModel GetDatosGeneralesVehiculoECAR(string matricula)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                var datosVehiculo = (from vehiculo in unitOfWork.RepositoryECAR_Datos_Vehiculo.Fetch().Where(o => o.Matricula == matricula)
                                     //join
                                     //tvehiculo in unitOfWork.RepositoryTipo_Vehiculo.Fetch() on vehiculo.Tipo_Vehiculo equals tvehiculo.Cod_Tipo into tiposVehiculos
                                     //from tiposVehiculosfinal in tiposVehiculos.DefaultIfEmpty()
                                     //join
                                     //seguros in unitOfWork.RepositoryCompañias_Seguro.Fetch() on vehiculo.Cia_Seguro equals seguros.Cod_Cia into compañias
                                     //from compañiasSeguro in compañias.DefaultIfEmpty()
                                     //join
                                     //tipos in unitOfWork.RepositoryTipo_Seguro.Fetch() on vehiculo.Tipo_Seguro equals tipos.Cod_Tipo into tiposseg
                                     //from tiposSeguro in tiposseg.DefaultIfEmpty()
                                     select new DatosGeneralesModel
                                     {
                                         Matricula = vehiculo.Matricula,
                                         MatriculaInicial = vehiculo.Matricula,
                                         MatriculaSustituida = vehiculo.Veh_sustituido,
                                         EsVehiculoDeSustitucion = vehiculo.Veh_sustituido == null || vehiculo.Veh_sustituido == "" ? false : true,
                                         IDMarca = vehiculo.T_M_MARCA_VEHICULOS.ID_MARCA,
                                         MarcaVehiculo = vehiculo.Marca,
                                         Marca = vehiculo.T_M_MARCA_VEHICULOS.DESCRIPCION,
                                         IDModelo = vehiculo.T_M_MODELOS_VEHICULO.ID_MODELO,
                                         Modelo = vehiculo.T_M_MODELOS_VEHICULO.DESCRIPCION,
                                         IDTipoVehiculo = vehiculo.T_M_TIPOS_VEHICULO.ID_TIPO_VEHICULO,
                                         TipoVehiculo = vehiculo.T_M_TIPOS_VEHICULO.DESCRIPCION,
                                         Bastidor = vehiculo.Num_Bastidor,
                                         EmpresaLeasing = vehiculo.EmpresaLeasing,
                                         IDEmpresaLeasing = vehiculo.EmpresaLeasing,
                                         Cia_Seguro = vehiculo.Cia_Seguro,
                                         IDCia_Seguro = vehiculo.Cia_Seguro,
                                         Seguro_Compañia = vehiculo.T_M_EMPRESAS_VEHICULOS.NOMBRE,
                                         IDTipoSeguro = vehiculo.T_M_TIPO_SEGURO_VEHICULO.ID_SEGURO_VEHICULO,
                                         Seguro_Tipo = vehiculo.T_M_TIPO_SEGURO_VEHICULO.DESCRIPCION,
                                         Seguro_Poliza = vehiculo.Poliza_Seguro,
                                         Seguro_Importe = vehiculo.Importe_Seguro,
                                         Seguro_FechaVencimiento = vehiculo.Vto_Seguro,
                                         Observaciones = vehiculo.Observaciones,
                                         Empresa = vehiculo.Sociedad,
                                         IDEmpresa = vehiculo.Sociedad,
                                         IDEmpresaInicial = vehiculo.Sociedad,
                                         CentroCoste = vehiculo.CC,
                                         IDCentroCoste = vehiculo.CC,
                                         IDCentroCosteInicial = vehiculo.CC,
                                         Carburante = vehiculo.T_M_TIPOS_CARBURANTE.ID_CARBURANTE,
                                         Conductor = vehiculo.ECAR_Datos_Conductor.Nombre + " " + vehiculo.ECAR_Datos_Conductor.Apellidos,
                                         IDConductor = vehiculo.ECAR_Datos_Conductor.Cod_Conductor,
                                         IDConductorInicial = vehiculo.ECAR_Datos_Conductor.Cod_Conductor,
                                         Directivo = vehiculo.Directivo,
                                         Equipamiento = vehiculo.Equipamiento,
                                         Extras = vehiculo.Extras,
                                         FechaAltaRegistro = vehiculo.Falta,
                                         FechaRenovacion = vehiculo.FechaRenovacion,
                                         IDCarburante = vehiculo.IDCarburante,
                                         IDDelegacion = vehiculo.Delegacion,
                                         Delegacion = vehiculo.Delegacion,
                                         Departamento = vehiculo.Departamento,
                                         IDDepartamento = vehiculo.Departamento,
                                         IDTarjetaCombustible = vehiculo.IDTarjetaCombustible,
                                         PIN_Tarjeta = vehiculo.IDTarjetaCombustible == null ? "" : vehiculo.T_M_TARJETAS_CONBUSTIBLE.PIN,
                                         IDTarjetaCombustibleInicial = vehiculo.IDTarjetaCombustible,
                                         TarjetaCombustible = vehiculo.IDTarjetaCombustible,
                                         IDTipoRuta = vehiculo.IDTipoRuta,
                                         TipoRuta = vehiculo.IDTipoRuta,
                                         IDUbicacion = vehiculo.Ubicacion,
                                         Ubicacion = vehiculo.Ubicacion,
                                         Veh_sustituido = vehiculo.Veh_sustituido,
                                         IdentificadorImportacion = vehiculo.IdentificadorImportacion,
                                     }).FirstOrDefault();

                return datosVehiculo;
            }
        }

        public DatosContratoModel GetDatosContratoVehiculoECAR(string matricula)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var datosITV_Vehiculo = (from vehiculo in unitOfWork.RepositoryECAR_Datos_Vehiculo.Fetch().Where(o => o.Matricula == matricula)
                                         //join
                                         //liquidacion in unitOfWork.RepositoryTipo_Liquidacion.Fetch() on vehiculo.Tipo_Liquidacion equals liquidacion.Cod_tipo into liquidaciones
                                         //from tiposliquidaciones in liquidaciones.DefaultIfEmpty()

                                         select new DatosContratoModel
                                         {
                                             Matricula = vehiculo.Matricula,
                                             NumContrato = vehiculo.Num_Contrato,
                                             FechaAlta = vehiculo.Fecha_Alta,
                                             FechaRecogida = vehiculo.Fecha_Incorporacion,
                                             Baja = vehiculo.Baja == null ? false : vehiculo.Baja,
                                             Renovacion = vehiculo.Veh_sustituido != null ? true : false,
                                             Cuotas = vehiculo.Cuotas,
                                             FechaFinalizacion = vehiculo.Fecha_Vto_Contrato,
                                             FechaDevolucion = vehiculo.Fecha_Devolucion,
                                             FechaBaja = vehiculo.Fecha_Baja,
                                             KMTotales = vehiculo.Km_Totales,
                                             IDTipoLiquidacion = vehiculo.IDTipoLiquidacion,
                                             TipoLiquidacion = vehiculo.ECAR_Tipo_Liquidacion.Descripcion,
                                             ExcesoAjuste = vehiculo.Exceso_ajuste,
                                             CoefExceso = vehiculo.Coef_exceso,
                                             KMExentos = vehiculo.Km_Exentos,
                                             Abono = vehiculo.Abono,
                                             Cargo = vehiculo.Cargo,
                                             FechaMatriculacion = vehiculo.FechaMatriculacion,
                                             FechaDevolución = vehiculo.Fecha_Devolucion,
                                             FechaRecibido = vehiculo.Fecha_Recibidos,
                                             LugarEntrega = vehiculo.LugarEntrega,
                                             Responsable = vehiculo.Responsable,
                                             FechaImportacion = vehiculo.FechaImportacion,
                                             PrioridadEntrega = vehiculo.PrioridadEntrega,
                                             FechaPrevistaEntrega = vehiculo.FechaPrevistaEntrega,
                                         }).FirstOrDefault();

                if (datosITV_Vehiculo != null)
                {
                    if (datosITV_Vehiculo.FechaFinalizacion == null)
                    {
                        datosITV_Vehiculo.FechaFinalizacion = datosITV_Vehiculo.FechaAlta == null ? null : datosITV_Vehiculo.Cuotas == null ? datosITV_Vehiculo.FechaAlta : datosITV_Vehiculo.FechaAlta.Value.AddMonths((int)datosITV_Vehiculo.Cuotas);
                    }
                }

                return datosITV_Vehiculo;
            }
        }

        public List<DatosITVModel> GetListDatosITV_ECAR(string matricula)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var datosITV = (from datoITV in unitOfWork.RepositoryECAR_Datos_ITV.Fetch().Where(x => x.Matricula == matricula)
                                select new DatosITVModel
                                {
                                    ID = datoITV.Id,
                                    Matricula = datoITV.Matricula,
                                    FechaUltimaITV = datoITV.Ultima_ITV,
                                    FechaVtoITV = datoITV.Vto_ITV,
                                    TarifaITV = datoITV.Tarifa,
                                    TasaITV = datoITV.Tasa,
                                    ImporteITV = datoITV.Importe,
                                    PrimaConservacionITV = datoITV.Pr_Conservacion,
                                    ImporteCirculacionITV = datoITV.Impuesto_Circulacion,
                                    Observaciones = datoITV.Otros,
                                    Documento = datoITV.Fichero,
                                    TipoArchivo = datoITV.TipoArchivo,
                                    ITV_Pasada = datoITV.ITV_PASADA == null ? false : datoITV.ITV_PASADA,
                                }).OrderByDescending(o => o.FechaVtoITV).ToList();
                return datosITV;
            }
        }

        #endregion Mantenimiento

        #region Vehículo
        public EnumResultadoEntity SaveVehiculoECAR(DatosVehiculoModel modelo, string login)
        {
            try
            {
                modelo.DatosGenerales_Vehiculo.Directivo = (modelo.DatosGenerales_Vehiculo.IDTipoVehiculo == null ? 0 :
                               (Global.ID_TURISMO_DIRECCION() == (int)modelo.DatosGenerales_Vehiculo.IDTipoVehiculo ? 1 : 0));

                using (var scope = new TransactionScope())
                {
                    using (var unitOfWork = new UnitOfWork())
                    {
                        //Datos Generales y Contrato
                        var vehiculo = new ECAR_Datos_Vehiculo
                        {
                            //Generales
                            Sociedad = (int)modelo.DatosGenerales_Vehiculo.IDEmpresa,
                            Matricula = modelo.DatosGenerales_Vehiculo.Matricula,
                            Marca = modelo.DatosGenerales_Vehiculo.IDMarca,
                            Modelo = modelo.DatosGenerales_Vehiculo.IDModelo,
                            Tipo_Vehiculo = modelo.DatosGenerales_Vehiculo.IDTipoVehiculo,
                            Num_Bastidor = modelo.DatosGenerales_Vehiculo.Bastidor,
                            EmpresaLeasing = modelo.DatosGenerales_Vehiculo.IDEmpresaLeasing,
                            Cia_Seguro = modelo.DatosGenerales_Vehiculo.IDCia_Seguro,
                            Tipo_Seguro = modelo.DatosGenerales_Vehiculo.IDTipoSeguro,
                            Poliza_Seguro = modelo.DatosGenerales_Vehiculo.Seguro_Poliza,
                            Importe_Seguro = modelo.DatosGenerales_Vehiculo.Seguro_Importe,
                            Vto_Seguro = modelo.DatosGenerales_Vehiculo.Seguro_FechaVencimiento,
                            Observaciones = modelo.DatosGenerales_Vehiculo.Observaciones,
                            CC = modelo.DatosGenerales_Vehiculo.IDCentroCoste,
                            IDCarburante = modelo.DatosGenerales_Vehiculo.IDCarburante,
                            Conductor = modelo.DatosGenerales_Vehiculo.IDConductor,
                            Directivo = modelo.DatosGenerales_Vehiculo.Directivo,
                            Extras = modelo.DatosGenerales_Vehiculo.Extras,
                            FechaRenovacion = modelo.DatosGenerales_Vehiculo.FechaRenovacion,
                            Delegacion = modelo.DatosGenerales_Vehiculo.IDDelegacion,
                            Departamento = modelo.DatosGenerales_Vehiculo.IDDepartamento,
                            IDTarjetaCombustible = modelo.DatosGenerales_Vehiculo.IDTarjetaCombustible,
                            IDTipoRuta = modelo.DatosGenerales_Vehiculo.IDTipoRuta,
                            Ubicacion = modelo.DatosGenerales_Vehiculo.IDUbicacion,
                            Veh_sustituido = string.IsNullOrEmpty(modelo.DatosGenerales_Vehiculo.Veh_sustituido) ? null : modelo.DatosGenerales_Vehiculo.Veh_sustituido,
                            IdentificadorImportacion = modelo.DatosGenerales_Vehiculo.IdentificadorImportacion,
                            Equipamiento = modelo.DatosGenerales_Vehiculo.Equipamiento,
                            UsuarioImportacion = modelo.DatosGenerales_Vehiculo.UsuarioImportacion,

                            //Contrato
                            Num_Contrato = modelo.DatosContrato_Vehiculo.NumContrato,
                            Fecha_Alta = modelo.DatosContrato_Vehiculo.FechaAlta,
                            
                            Fecha_Incorporacion = modelo.DatosContrato_Vehiculo.FechaRecogida,
                            Baja = modelo.DatosContrato_Vehiculo.Baja == null ? false : modelo.DatosContrato_Vehiculo.Baja,
                            Cuotas = modelo.DatosContrato_Vehiculo.Cuotas,
                            Fecha_Devolucion = modelo.DatosContrato_Vehiculo.FechaDevolucion,
                            Fecha_Baja = modelo.DatosContrato_Vehiculo.FechaBaja,
                            Km_Totales = modelo.DatosContrato_Vehiculo.KMTotales,
                            IDTipoLiquidacion = modelo.DatosContrato_Vehiculo.IDTipoLiquidacion,
                            Exceso_ajuste = modelo.DatosContrato_Vehiculo.ExcesoAjuste,
                            Coef_exceso = modelo.DatosContrato_Vehiculo.CoefExceso,
                            Km_Exentos = modelo.DatosContrato_Vehiculo.KMExentos,
                            Abono = modelo.DatosContrato_Vehiculo.Abono,
                            Cargo = modelo.DatosContrato_Vehiculo.Cargo,
                            FechaMatriculacion = modelo.DatosContrato_Vehiculo.FechaMatriculacion,
                            Fecha_Recibidos = modelo.DatosContrato_Vehiculo.FechaRecibido,
                            LugarEntrega =  modelo.DatosContrato_Vehiculo.LugarEntrega,
                            Responsable =  modelo.DatosContrato_Vehiculo.Responsable,
                            PrioridadEntrega = modelo.DatosContrato_Vehiculo.PrioridadEntrega,
                            FechaImportacion = modelo.DatosContrato_Vehiculo.FechaImportacion,
                            FechaPrevistaEntrega = modelo.DatosContrato_Vehiculo.FechaPrevistaEntrega,
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = login,

                        };

                        if (modelo.DatosGenerales_Vehiculo.Accion == EnumAccionEntity.Alta)
                        {
                            vehiculo.Falta = DateTime.Now;
                            vehiculo.UsuarioAlta = login;

                            unitOfWork.RepositoryECAR_Datos_Vehiculo.Insert(vehiculo);
                        }
                        else
                        {
                            BorraDatosITV(unitOfWork, modelo.DatosGenerales_Vehiculo.Matricula); //Borramos los datos de ITV, por si luego no hay datos.
                            unitOfWork.Commit();

                            unitOfWork.RepositoryECAR_Datos_Vehiculo.Update(vehiculo);
                        }

                        unitOfWork.Commit();

                        //Datos ITV
                        List<DatosITV_TMPModel> datosITV_TMP = GetListDatosITV_TMP(login, modelo.DatosGenerales_Vehiculo.Accion == EnumAccionEntity.Alta ? "" : modelo.DatosGenerales_Vehiculo.Matricula);

                        datosITV_TMP.ForEach(x => x.Matricula = modelo.DatosGenerales_Vehiculo.Matricula);

                        if (datosITV_TMP.Count > 0)
                        {
                            SaveDatosITV(unitOfWork, datosITV_TMP, login);
                            unitOfWork.Commit();
                        }

                        if (modelo.DatosGenerales_Vehiculo.IDEmpresa != modelo.DatosGenerales_Vehiculo.IDEmpresaInicial && modelo.DatosGenerales_Vehiculo.IDEmpresaInicial != null)
                        {
                            SaveHistoricoCambiosEmpresa(unitOfWork, modelo.DatosGenerales_Vehiculo.IDEmpresaInicial,
                                                            modelo.DatosGenerales_Vehiculo.IDEmpresa,
                                                            modelo.DatosGenerales_Vehiculo.Matricula, login);
                            unitOfWork.Commit();
                        }


                        if (modelo.DatosGenerales_Vehiculo.IDConductor != modelo.DatosGenerales_Vehiculo.IDConductorInicial)
                        {
                            SaveHistoricoCambiosConductor(unitOfWork, modelo.DatosGenerales_Vehiculo.IDConductorInicial,
                                                            modelo.DatosGenerales_Vehiculo.IDConductor, 
                                                            modelo.DatosGenerales_Vehiculo.Matricula, login);
                            unitOfWork.Commit();
                        }

                        if (modelo.DatosGenerales_Vehiculo.IDTarjetaCombustible != modelo.DatosGenerales_Vehiculo.IDTarjetaCombustibleInicial)
                        {
                            SaveHistoricoCambiosTarjeta(unitOfWork, modelo.DatosGenerales_Vehiculo.IDTarjetaCombustibleInicial,
                                                            modelo.DatosGenerales_Vehiculo.IDTarjetaCombustible,
                                                            modelo.DatosGenerales_Vehiculo.Matricula, login);
                            unitOfWork.Commit();
                        }

                        if (modelo.DatosGenerales_Vehiculo.IDCentroCoste != modelo.DatosGenerales_Vehiculo.IDCentroCosteInicial)
                        {
                            SaveHistoricoCambiosCentroCoste(unitOfWork, modelo.DatosGenerales_Vehiculo.IDCentroCosteInicial,
                                                            modelo.DatosGenerales_Vehiculo.IDCentroCoste,
                                                            modelo.DatosGenerales_Vehiculo.Matricula, login);
                            unitOfWork.Commit();
                        }

                    }

                    scope.Complete();
                }
            }
            catch(Exception ex)
            {
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"(<SaveVehiculoECAR ({modelo.DatosGenerales_Vehiculo.Matricula})> {ex.Message})");
                return EnumResultadoEntity.Error_en_Proceso;
            }

            return EnumResultadoEntity.GrabacionCorrecta;
        }

        public ECAR_Datos_Vehiculo GetVehiculoByIdentificadorImportacion(string identificador)
        {
            ECAR_Datos_VehiculoSpecification spec = new ECAR_Datos_VehiculoSpecification
            {
                IdentificadorImportacion = identificador
            };

            using (var unitOfWork = new UnitOfWork())
            {
                var vehiculo = unitOfWork.RepositoryECAR_Datos_Vehiculo.Where(spec).FirstOrDefault();

                return vehiculo;
            }

        }


        #endregion Vehículo

        #region Conductor
        public bool SaveHistoricoCambiosConductor(UnitOfWork unitOfWork, int? conductorAnt, int? conductorNew, string matricula, string login)
        {
            var Paso = "";
            try
            {
                T_G_HIST_CAMBIOS_CONDUCTOR hist = new T_G_HIST_CAMBIOS_CONDUCTOR
                {
                    FECHA_ALTA = DateTime.Now,
                    ID_CONDUCTOR_ANT = conductorAnt,
                    ID_CONDUCTOR_NUEVO = conductorNew,
                    ID_ESTADO = 6, //Atendido
                    MATRICULA = matricula,
                    USUARIO_CREACION = login,
                };
                unitOfWork.RepositoryT_G_HIST_CAMBIOS_CONDUCTOR.Insert(hist);
            }

            catch (Exception ex)
            {
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"(<SaveHistoricoCambiosConductor ({login})> {Paso}. {ex.Message})");
                throw ex;
            }
            return true;
        }
        #endregion Conductor

        #region Empresa
        public bool SaveHistoricoCambiosEmpresa(UnitOfWork unitOfWork, int? empresaAnt, int? empresaNew, string matricula, string login)
        {
            var Paso = "";
            try
            {
                T_G_HIST_CAMBIOS_EMPRESA hist = new T_G_HIST_CAMBIOS_EMPRESA
                {
                    FECHA_ALTA = DateTime.Now,
                    ID_EMPRESA_ANT = empresaAnt,
                    ID_EMPRESA_NUEVA = empresaNew,
                    ID_ESTADO = 6, //Atendido
                    MATRICULA = matricula,
                    USUARIO_CREACION = login,
                };
                unitOfWork.RepositoryT_G_HIST_CAMBIOS_EMPRESA.Insert(hist);
            }

            catch (Exception ex)
            {
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"(<SaveHistoricoCambiosConductor ({login})> {Paso}. {ex.Message})");
                throw ex;
            }
            return true;
        }
        #endregion empresa

        #region Tarjeta Combustible
        public bool SaveHistoricoCambiosTarjeta(UnitOfWork unitOfWork, int? tarjetaAnt, int? tarjetaNew, string matricula, string login)
        {
            var Paso = "";
            try
            {
                T_G_HIST_CAMBIOS_TARJETA hist = new T_G_HIST_CAMBIOS_TARJETA
                {
                    FECHA_ALTA = DateTime.Now,
                    ID_TARJETA_ANT = tarjetaAnt,
                    ID_TARJETA_NUEVA = tarjetaNew,
                    MATRICULA = matricula,
                    USUARIO_CREACION = login,
                };
                unitOfWork.RepositoryT_G_HIST_CAMBIOS_TARJETA.Insert(hist);
            }

            catch (Exception ex)
            {
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"(<SaveHistoricoCambiosTarjeta ({login})> {Paso}. {ex.Message})");
                throw ex;
            }
            return true;
        }
        #endregion Tarjeta Combustible

        #region Centro de Coste
        public bool SaveHistoricoCambiosCentroCoste(UnitOfWork unitOfWork, string cecoAnt, string cecoNew, string matricula, string login)
        {
            var Paso = "";
            try
            {
                T_G_HIST_CAMBIOS_CENTRO_COSTE hist = new T_G_HIST_CAMBIOS_CENTRO_COSTE
                {
                    FECHA_ALTA = DateTime.Now,
                    ID_CENTRO_COSTE_ANT = cecoAnt,
                    ID_CENTRO_COSTE_NUEVO = cecoNew,
                    MATRICULA = matricula,
                    USUARIO_CREACION = login,
                };
                unitOfWork.RepositoryT_G_HIST_CAMBIOS_CENTRO_COSTE.Insert(hist);
            }

            catch (Exception ex)
            {
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"(<SaveHistoricoCambiosCentroCoste ({login})> {Paso}. {ex.Message})");
                throw ex;
            }
            return true;
        }
        #endregion Centro de Coste

        #region ITV
        public bool BorraDatosITV(UnitOfWork unitOfWork, string matricula)
        {
            ECAR_Datos_ITVSpecification spec = new ECAR_Datos_ITVSpecification
            {
                Matricula = matricula,
            };
            var datosITV = (from datoITV in unitOfWork.RepositoryECAR_Datos_ITV.Where(spec)
                            select new DatosITVModel
                            {
                                Matricula = datoITV.Matricula,
                                FechaUltimaITV = datoITV.Ultima_ITV,
                                FechaVtoITV = datoITV.Vto_ITV,
                                Documento = datoITV.Fichero,
                                ID = datoITV.Id,
                                ImporteITV = datoITV.Importe,
                                ImporteCirculacionITV = datoITV.Impuesto_Circulacion,
                                PrimaConservacionITV = datoITV.Pr_Conservacion,
                                TarifaITV = datoITV.Tarifa,
                                TasaITV = datoITV.Tasa,
                            }).ToList();

            foreach (DatosITVModel dato in datosITV)
            {
                ECAR_Datos_ITV datoBorrar = new ECAR_Datos_ITV
                {
                    Matricula = dato.Matricula,
                    Ultima_ITV = dato.FechaUltimaITV,
                    Vto_ITV = dato.FechaVtoITV,
                    Fichero = dato.Documento,
                    Id = dato.ID,
                    Importe = dato.ImporteITV,
                    Impuesto_Circulacion = dato.ImporteCirculacionITV,
                    Pr_Conservacion = dato.PrimaConservacionITV,
                    Tarifa = dato.TarifaITV,
                    Tasa = dato.TasaITV,
                };

                unitOfWork.RepositoryECAR_Datos_ITV.Delete(datoBorrar);

                if (!string.IsNullOrEmpty(datoBorrar.Fichero))
                {
                    //Renombramos el archivo por si hay otro y hay que borrar este.
                    FileUtilities.RenameFile($"{Global.PathToUploadDocumentITV}{matricula}/", datoBorrar.Fichero, "TMP_" + datoBorrar.Fichero);
                }

                unitOfWork.Commit();
            }

            return true;
        }

        public bool SaveDatosITV(UnitOfWork unitOfWork, List<DatosITV_TMPModel> modelo, string login)
        {
            var Paso = "";
            try
            {
                Paso = "RECORRIENDO DATOS TMP";
                foreach (DatosITV_TMPModel datoITV in modelo.OrderBy(x=>x.FechaUltimaITV).ToList())
                {
                    ECAR_Datos_ITV dato = new ECAR_Datos_ITV();
                    ECAR_Datos_ITV datoExistente = new ECAR_Datos_ITV(); 

                    if (datoITV.ID == -1) //Es una línea nueva.
                    {
                        Paso = "RECORRIENDO DATOS TMP LINEA NUEVA";
                        dato = DevuelveECAR_DatoITVRelleno(datoITV, login, EnumAccionEntity.Alta);
                        unitOfWork.RepositoryECAR_Datos_ITV.Insert(dato);
                    }
                    else
                    {
                        Paso = $"RECORRIENDO DATOS TMP LINEA EXISTENTE ID ({datoITV.ID.ToString()})";
                        datoExistente = unitOfWork.RepositoryECAR_Datos_ITV.Fetch()
                                        .Where(x => x.Id == datoITV.ID &&
                                                x.Matricula == datoITV.Matricula).FirstOrDefault();
                        dato = DevuelveECAR_DatoITVRelleno(datoITV, login, EnumAccionEntity.Modificacion);
                        unitOfWork.RepositoryECAR_Datos_ITV.Update(dato);
                    }

                    //Comprobar archivos
                    if (!string.IsNullOrEmpty(datoITV.FileUpload_download))
                    {
                        Paso = $"RECORRIENDO DATOS TMP MOVER ARCHIVO ({datoITV.FileUpload_download})";
                        //Mover el archivo del directorio TMP al final.
                        var pathTMP = $"{Global.PathToUploadDocumentITV_TMP}{login}/";
                        var pathFinal = $"{Global.PathToUploadDocumentITV}{dato.Matricula}/";
                        if (FileUtilities.ExisteDocumentoToUploadEnDisco(pathTMP + datoITV.FileUpload_download))
                        {
                            FileUtilities.MoveFile(pathTMP, datoITV.FileUpload_download, pathFinal, datoITV.FileUpload_download);
                        }
                        else if (FileUtilities.ExisteDocumentoToUploadEnDisco(pathFinal + "TMP_" + datoITV.FileUpload_download))
                        {
                            FileUtilities.RenameFile(pathFinal, "TMP_" + datoITV.FileUpload_download, datoITV.FileUpload_download);
                        }
                        //Borrar el documento antiguo si existe. Comprobar antes que no coincidan los nombres.
                        if (!string.IsNullOrEmpty(datoExistente.Fichero))
                        {
                            if (datoExistente.Fichero.ToUpper() != datoITV.FileUpload_download.ToUpper())
                            {
                                FileUtilities.BorraDocumentoFisico(pathFinal + datoExistente.Fichero);
                            }
                        }
                    }
                    else if (datoITV.ID != -1 && string.IsNullOrEmpty(datoExistente.Fichero))
                    {
                        Paso = $"RECORRIENDO DATOS TMP BORRAR ARCHIVO EXISTENTE ({datoITV.FileUpload_download})";
                        //Borrar el archivo que estaba subido. Si lo hubiere (o hubiese). 
                        FileUtilities.BorraDocumentoFisico($"{Global.PathToUploadDocumentITV}{datoExistente.Matricula}/{datoITV.FileUpload_download}");
                    }
                }
            }

            catch (Exception ex)
            {
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"(<SaveDatosITV ({login})> {Paso}. {ex.Message})");
                throw ex;
            }
            return true;
        }

        private ECAR_Datos_ITV DevuelveECAR_DatoITVRelleno(DatosITV_TMPModel datoITV, string login, EnumAccionEntity accion)
        {
            ECAR_Datos_ITV varorReturn = new ECAR_Datos_ITV
            {
                Ultima_ITV = datoITV.FechaUltimaITV,
                Vto_ITV = datoITV.FechaVtoITV,
                Fichero = datoITV.Documento,
                TipoArchivo = datoITV.TipoArchivo,
                Matricula = datoITV.Matricula,
                Importe = datoITV.ImporteITV,
                Impuesto_Circulacion = datoITV.ImporteCirculacionITV,
                Pr_Conservacion = datoITV.PrimaConservacionITV,
                Tarifa = datoITV.TarifaITV,
                Tasa = datoITV.TasaITV,
                Otros = datoITV.Observaciones,
                ITV_PASADA = datoITV.ITV_Pasada == null ? false : datoITV.ITV_Pasada,
                FechaModificacion = DateTime.Now,
                UsuarioModificacion = login,
            };

            if (accion == EnumAccionEntity.Alta)
            {
                varorReturn.Falta = DateTime.Now;
                varorReturn.UsuarioAlta = login;
            }
            else
            {
                varorReturn.Id = datoITV.ID;
            }

            return varorReturn;
        }

        #region ITV_TMP
        /// <summary>
        /// Devuelve todos los datos de las ITV. Tanto los que ya estaban, como los 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="matricula"></param>
        /// <returns></returns>
        public List<DatosITV_TMPModel> GetListDatosITV_TMP(string login, string matricula = "")
        {
            BorraDatosITV_TMP(login, true);

            TMP_RENOVACION_ITVSpecification spec = new TMP_RENOVACION_ITVSpecification
            {
                LOGIN = login,
                LINEA_NUEVA = true,
            };

            //if (!string.IsNullOrEmpty(matricula))
            //{
            //    spec.MATRICULA = matricula;
            //}

            using (var unitOfWork = new UnitOfWork())
            {
                var datosITV = (from datoITV in unitOfWork.RepositoryECAR_Datos_ITV.Fetch().Where(x => x.Matricula == matricula)

                                select new DatosITV_TMPModel
                                {
                                    ID = datoITV.Id,
                                    Matricula = datoITV.Matricula,
                                    FechaUltimaITV = datoITV.Ultima_ITV,
                                    FechaVtoITV = datoITV.Vto_ITV,
                                    Login = login,
                                    Linea = datoITV.Id,
                                    LineaNueva = false,
                                    Documento = datoITV.Fichero,
                                    FileUpload_download = datoITV.Fichero,
                                    TipoArchivo = datoITV.TipoArchivo,
                                    AccionDataTable = "",
                                    ImporteCirculacionITV = datoITV.Impuesto_Circulacion,
                                    ImporteITV = datoITV.Importe,
                                    PrimaConservacionITV = datoITV.Pr_Conservacion,
                                    TarifaITV = datoITV.Tarifa,
                                    TasaITV = datoITV.Tasa,
                                    Observaciones = datoITV.Otros,
                                    ITV_Pasada = datoITV.ITV_PASADA == null ? false : datoITV.ITV_PASADA,
                                })
                                .Union
                                (from datoITV in unitOfWork.RepositoryTMP_RENOVACION_ITV.Where(spec)
                                select new DatosITV_TMPModel
                                {
                                    ID = datoITV.ID,
                                    Matricula = datoITV.MATRICULA,
                                    FechaUltimaITV = datoITV.FECHA_ITV,
                                    FechaVtoITV = datoITV.FECHA_CADUCIDAD_ITV,
                                    Login = datoITV.LOGIN,
                                    Linea = datoITV.NUM_LINEA,
                                    LineaNueva = datoITV.LINEA_NUEVA,
                                    Documento = datoITV.FICHERO_ITV,
                                    FileUpload_download = datoITV.FICHERO_ITV,
                                    TipoArchivo = datoITV.TIPO_ARCHIVO,
                                    AccionDataTable = "",
                                    ImporteCirculacionITV = datoITV.IMPUESTO_CIRCULACIÓN,
                                    ImporteITV = datoITV.IMPORTE,
                                    PrimaConservacionITV = datoITV.PR_CONSERVACION,
                                    TarifaITV = datoITV.TARIFA,
                                    TasaITV = datoITV.TASA,
                                    Observaciones = datoITV.OBSERVACIONES,
                                    ITV_Pasada = datoITV.ITV_PASADA == null ? false : datoITV.ITV_PASADA,
                                }).ToList();

                //int numLinea = 0;
                //foreach(DatosITV_TMPModel dato in datosITV.OrderByDescending(o => o.FechaVtoITV))
                //{
                //    numLinea++;
                //    dato.Linea = numLinea;
                //}

                return datosITV.OrderByDescending(o => o.FechaUltimaITV).ToList();
            }
        }

        public DatosITV_TMPModel GetLineaITV_TMP(int linea, string login, string matricula = "")
        {
            TMP_RENOVACION_ITVSpecification spec = new TMP_RENOVACION_ITVSpecification
            {
                LOGIN = login,
                NUM_LINEA = linea,
            };

            if (!string.IsNullOrEmpty(matricula))
            {
                spec.MATRICULA = matricula;
            }

            using (var unitOfWork = new UnitOfWork())
            {
                var lineaITV = (from datoITV in unitOfWork.RepositoryTMP_RENOVACION_ITV.Where(spec)
                                select new DatosITV_TMPModel
                                {
                                    ID = datoITV.ID,
                                    Matricula = datoITV.MATRICULA,
                                    FechaUltimaITV = datoITV.FECHA_ITV,
                                    FechaVtoITV = datoITV.FECHA_CADUCIDAD_ITV,
                                    Login = datoITV.LOGIN,
                                    Linea = datoITV.NUM_LINEA,
                                    LineaNueva = datoITV.LINEA_NUEVA,
                                    Documento = datoITV.FICHERO_ITV,
                                    FileUpload_download = datoITV.FICHERO_ITV,
                                    TipoArchivo = datoITV.TIPO_ARCHIVO,
                                    AccionDataTable = "",
                                    ImporteCirculacionITV = datoITV.IMPUESTO_CIRCULACIÓN,
                                    ImporteITV = datoITV.IMPORTE,
                                    PrimaConservacionITV = datoITV.PR_CONSERVACION,
                                    TarifaITV = datoITV.TARIFA,
                                    TasaITV = datoITV.TASA,
                                    Observaciones = datoITV.OBSERVACIONES,
                                    ITV_Pasada = datoITV.ITV_PASADA == null ? false : datoITV.ITV_PASADA,
                                }).FirstOrDefault();

                return lineaITV;
            }

        }


        public bool BorraDatosITV_TMP(string login, bool DejarLineasNuevas = false, bool borrarArchivosTMP = false)
        {
            TMP_RENOVACION_ITVSpecification spec = new TMP_RENOVACION_ITVSpecification
            {
                LOGIN = login,
            };

            if (DejarLineasNuevas)
            {
                spec.LINEA_NUEVA = false;
            }

            using (var unitOfWork = new UnitOfWork())
            {
                var datosITV = (from datoITV in unitOfWork.RepositoryTMP_RENOVACION_ITV.Where(spec)
                                select new DatosITV_TMPModel
                                {
                                    ID = datoITV.ID,
                                    Matricula = datoITV.MATRICULA,
                                    FechaUltimaITV = datoITV.FECHA_ITV,
                                    FechaVtoITV = datoITV.FECHA_CADUCIDAD_ITV,
                                    Login = datoITV.LOGIN,
                                    Linea = datoITV.NUM_LINEA,
                                    LineaNueva = datoITV.LINEA_NUEVA,
                                    Documento = datoITV.FICHERO_ITV,
                                    TipoArchivo = datoITV.TIPO_ARCHIVO,
                                    ITV_Pasada = datoITV.ITV_PASADA == null ? false : datoITV.ITV_PASADA,
                                }).ToList();

                foreach (DatosITV_TMPModel dato in datosITV)
                {
                    TMP_RENOVACION_ITV datoBorrar = new TMP_RENOVACION_ITV
                    {
                        ID = dato.ID,
                        MATRICULA = dato.Matricula,
                        FECHA_ITV = dato.FechaUltimaITV,
                        FECHA_CADUCIDAD_ITV = dato.FechaVtoITV,
                        LOGIN = dato.Login,
                        NUM_LINEA = dato.Linea,
                        LINEA_NUEVA = dato.LineaNueva,
                        FICHERO_ITV = dato.Documento,
                        TIPO_ARCHIVO = dato.TipoArchivo,
                        ITV_PASADA = dato.ITV_Pasada == null ? false : dato.ITV_Pasada,
                    };

                    unitOfWork.RepositoryTMP_RENOVACION_ITV.Delete(datoBorrar);
                }

                unitOfWork.Commit();
            }

            if (borrarArchivosTMP)
            {
                var path = $"{Global.PathToUploadDocumentITV_TMP}{login}/";
                FileUtilities.DeleteFilesFromDirectory(path);
            }
            return true;
        }


        public bool InicializaDatosITV_TMP(string login, string matricula)
        {
            BorraDatosITV_TMP(login);

            TMP_RENOVACION_ITVSpecification spec = new TMP_RENOVACION_ITVSpecification
            {
                LOGIN = login,
            };

            List<DatosITV_TMPModel> datosITV = new List<DatosITV_TMPModel>();

            using (var unitOfWork = new UnitOfWork())
            {
                datosITV = (from datoITV in unitOfWork.RepositoryECAR_Datos_ITV.Fetch().Where(x => x.Matricula == matricula)

                                select new DatosITV_TMPModel
                                {
                                    ID = datoITV.Id,
                                    Matricula = datoITV.Matricula,
                                    FechaUltimaITV = datoITV.Ultima_ITV,
                                    FechaVtoITV = datoITV.Vto_ITV,
                                    Login = login,
                                    Linea = datoITV.Id,
                                    LineaNueva = false,
                                    Documento = datoITV.Fichero,
                                    FileUpload_download = datoITV.Fichero,
                                    TipoArchivo = datoITV.TipoArchivo,
                                    AccionDataTable = "",
                                    ImporteCirculacionITV = datoITV.Impuesto_Circulacion,
                                    ImporteITV = datoITV.Importe,
                                    PrimaConservacionITV = datoITV.Pr_Conservacion,
                                    TarifaITV = datoITV.Tarifa,
                                    TasaITV = datoITV.Tasa,
                                    Observaciones = datoITV.Otros,
                                    ITV_Pasada = datoITV.ITV_PASADA == null ? false : datoITV.ITV_PASADA,
                                }).ToList();

                var linea = 0;
                foreach(DatosITV_TMPModel dato in datosITV.OrderByDescending(x=>x.FechaUltimaITV))
                {
                    dato.Linea= linea++;
                    var datoTmpITV = DevuelveDatoITV_TMP_Relleno(null, dato, login, matricula, linea, EnumAccionEntity.Alta);
                    unitOfWork.RepositoryTMP_RENOVACION_ITV.Insert(datoTmpITV);
                    unitOfWork.Commit();
                }
            }

            return true;
        }


        public bool BorraLineaITV_TMP(int linea, string login, string matricula = "")
        {
            TMP_RENOVACION_ITVSpecification spec = new TMP_RENOVACION_ITVSpecification
            {
                LOGIN = login,
                NUM_LINEA = linea,
            };

            if (!string.IsNullOrEmpty(matricula))
            {
                spec.MATRICULA = matricula;
            }

            using (var unitOfWork = new UnitOfWork())
            {
                var lineaITV = (from datoITV in unitOfWork.RepositoryTMP_RENOVACION_ITV.Where(spec)
                                select new DatosITV_TMPModel
                                {
                                    ID = datoITV.ID,
                                    Matricula = datoITV.MATRICULA,
                                    FechaUltimaITV = datoITV.FECHA_ITV,
                                    FechaVtoITV = datoITV.FECHA_CADUCIDAD_ITV,
                                    Login = datoITV.LOGIN,
                                    Linea = datoITV.NUM_LINEA,
                                    LineaNueva = datoITV.LINEA_NUEVA,
                                    Documento = datoITV.FICHERO_ITV,
                                    TipoArchivo = datoITV.TIPO_ARCHIVO,
                                    ITV_Pasada = datoITV.ITV_PASADA == null ? false : datoITV.ITV_PASADA,
                                }).FirstOrDefault();

                var fichero = "";
                if (lineaITV != null)
                {
                    fichero = lineaITV.Documento;
                    TMP_RENOVACION_ITV datoBorrar = new TMP_RENOVACION_ITV
                    {
                        ID= lineaITV.ID,
                        MATRICULA = lineaITV.Matricula,
                        FECHA_ITV = lineaITV.FechaUltimaITV,
                        FECHA_CADUCIDAD_ITV = lineaITV.FechaVtoITV,
                        LOGIN = lineaITV.Login,
                        NUM_LINEA = lineaITV.Linea,
                        LINEA_NUEVA = lineaITV.LineaNueva,
                        FICHERO_ITV = lineaITV.Documento,
                        TIPO_ARCHIVO = lineaITV.TipoArchivo,
                        ITV_PASADA = lineaITV.ITV_Pasada == null ? false : lineaITV.ITV_Pasada,
                    };

                    unitOfWork.RepositoryTMP_RENOVACION_ITV.Delete(datoBorrar);

                    if (!string.IsNullOrEmpty(fichero))
                    {
                        var path = $"{Global.PathToUploadDocumentITV_TMP}{login}/";
                        FileUtilities.BorraDocumentoFisico(path + fichero);
                    }
                    unitOfWork.Commit();
                }
            }

            return true;
        }


        public bool SaveDatosITV_TMP(DatosITV_TMPModel modelo, string login, string matricula = "")
        {
            int numLinea = 1;
            var nombreArchivo = "";

            try
            {
                using (var scope = new TransactionScope())
                {
                    using (var unitOfWork = new UnitOfWork())
                    {
                        //if (modelo.Accion == EnumAccionEntity.Alta)
                        //{
                        var lineaTMP = unitOfWork.RepositoryTMP_RENOVACION_ITV.Fetch()
                                       .Where(x => x.LOGIN.ToUpper() == login.ToUpper() &&
                                                x.NUM_LINEA == modelo.Linea).FirstOrDefault();

                        if (lineaTMP == null) //Alta.
                        { 
                            var lineasTMP = unitOfWork.RepositoryTMP_RENOVACION_ITV.Fetch()
                                           .Where(x => x.LOGIN.ToUpper() == login.ToUpper())
                                           .OrderByDescending(x => x.NUM_LINEA).ToList();
                            if (lineasTMP != null)
                            {
                                if (lineasTMP.Count > 0)
                                {
                                    numLinea = lineasTMP[0].NUM_LINEA + 1;
                                }
                            }

                            var datoTmpITV = DevuelveDatoITV_TMP_Relleno(lineaTMP, modelo, login, matricula, numLinea, EnumAccionEntity.Alta);

                            unitOfWork.RepositoryTMP_RENOVACION_ITV.Insert(datoTmpITV);
                        }
                        else //Modificación.
                        {
                            nombreArchivo = lineaTMP.FICHERO_ITV;
                            numLinea = lineaTMP.NUM_LINEA;
                            var datoTmpITV = DevuelveDatoITV_TMP_Relleno(lineaTMP, modelo, login, matricula, numLinea, EnumAccionEntity.Modificacion);

                            unitOfWork.RepositoryTMP_RENOVACION_ITV.Update(datoTmpITV);
                        }
                        unitOfWork.Commit();
                    }

                    if (modelo.FileUpload != null)
                    {
                        var filename = modelo.FileUpload == null ? null : FileUtilities.GetFileName(modelo.FileUpload);
                        saveFileTMP_ITV(modelo.FileUpload, login, filename);
                    }
                    else if (string.IsNullOrEmpty(modelo.FileUpload_download) && !string.IsNullOrEmpty(nombreArchivo))
                    {
                        var ficheroTMP = $"{Global.PathToUploadDocumentITV_TMP}{login}/{nombreArchivo}";
                        FileUtilities.BorraDocumentoFisico(ficheroTMP);
                    }
                    scope.Complete();
                }
            }

            catch(Exception ex)
            {
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"(<SaveDatosITV_TMP ({login})> {ex.Message})");
                return false;
            }
            return true;
        }

        private TMP_RENOVACION_ITV DevuelveDatoITV_TMP_Relleno(TMP_RENOVACION_ITV lineaTMP, DatosITV_TMPModel modelo, string login, string matricula, int numLinea, EnumAccionEntity accion)
        {
            TMP_RENOVACION_ITV valorReturn = null;
            if (lineaTMP == null)
            {
                valorReturn = new TMP_RENOVACION_ITV();
            }
            else
            {
                valorReturn = lineaTMP;
            }
            
            valorReturn.LOGIN = login;
            valorReturn.MATRICULA = matricula;
            valorReturn.FECHA_CADUCIDAD_ITV = modelo.FechaVtoITV;
            valorReturn.FECHA_ITV = modelo.FechaUltimaITV;
            valorReturn.LINEA_NUEVA = true;
            valorReturn.NUM_LINEA = numLinea;
            valorReturn.IMPORTE = modelo.ImporteITV;
            valorReturn.IMPUESTO_CIRCULACIÓN = modelo.ImporteCirculacionITV;
            valorReturn.PR_CONSERVACION = modelo.PrimaConservacionITV;
            valorReturn.TARIFA = modelo.TarifaITV;
            valorReturn.TASA = modelo.TasaITV;
            valorReturn.OBSERVACIONES = modelo.Observaciones;
            valorReturn.ITV_PASADA = modelo.ITV_Pasada;

            if (modelo.FileUpload != null)
            {
                valorReturn.FICHERO_ITV = modelo.FileUpload.FileName;
                valorReturn.TIPO_ARCHIVO = modelo.FileUpload.ContentType;
            }
            else
            {
                valorReturn.FICHERO_ITV = modelo.FileUpload_download;
                if (!string.IsNullOrEmpty(modelo.FileUpload_download))
                {
                    valorReturn.TIPO_ARCHIVO = modelo.TipoArchivo;
                }
                else
                {
                    valorReturn.TIPO_ARCHIVO = "";
                }
            }

            if (accion == EnumAccionEntity.Alta)
            {
                valorReturn.ID = -1;
            }
            else
            {
                valorReturn.ID = modelo.ID;
            }

            return valorReturn;
        }


        private void saveFileTMP_ITV(HttpPostedFileBase file, string login, string namefile)
        {
            if (file != null)
            {
                var path = $"{Global.PathToUploadDocumentITV_TMP}{login}/";

                FileUtilities.UploadFile(file, path, namefile);
            }
        }
        #endregion ITV_TMP

        #endregion ITV

        #region Selección Cosen
        public List<SelectChosen> GetAllDepartamentosChosen(List<int> empresas, string term = "")
        {
            List<SelectChosen> departamentos = new List<SelectChosen>();

            SAPHR_UnidadesOrganizativasSpecification spec = new SAPHR_UnidadesOrganizativasSpecification
            {
                Baja = false,
                EmpresaIN = empresas.Select(x => (int?)x).ToList(),
            };

            departamentos = GetDepartamentosChosen(spec, term);

            return departamentos;
        }

        public List<SelectChosen> GetDepartamentosByIDChosen(string departamento)
        {
            List<SelectChosen> departamentos = new List<SelectChosen>();

            SAPHR_UnidadesOrganizativasSpecification spec = new SAPHR_UnidadesOrganizativasSpecification
            {
                Baja = true,
                IdUniOrganizativa = departamento,
            };

            departamentos = GetDepartamentosChosen(spec);

            return departamentos;
        }

        private List<SelectChosen> GetDepartamentosChosen(SAPHR_UnidadesOrganizativasSpecification spec, string term = "")
        {
            List<SelectChosen> departamentos = new List<SelectChosen>();

            using (var unitOfWork = new UnitOfWork())
            {
                departamentos = (from unidad in unitOfWork.RepositorySAPHR_UnidadesOrganizativas.Where(spec)
                                 where (term != "" ? unidad.Nombre.ToUpper().Contains(term.ToUpper()) || 
                                        unidad.IdUniOrganizativa.Contains(term) : true)
                                 select new SelectChosen
                                 {
                                     text = unidad.Nombre,
                                     value = unidad.IdUniOrganizativa,
                                 }).OrderBy(x => x.text).ToList();
            };

            return departamentos;
        }
        #endregion

    }
}