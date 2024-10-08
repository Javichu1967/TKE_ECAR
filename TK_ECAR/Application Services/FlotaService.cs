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
    public class FlotaService
    {


        public bool VehiculoExistente(string matricula)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                return unitOfWork.RepositoryECAR_Datos_Vehiculo.Fetch().Where(o => o.Matricula == matricula).FirstOrDefault() != null;
            }
        }


        /// <summary>
        /// Devuelve todos los registros de Datos_Vehiculo tengan los dentros de CECO pasados por parámetro y según sea baja
        /// </summary>
        /// <param name="CentrosCoste"></param>
        /// /// <returns></returns>
        public List<MiFlotaDatatableModel> AllMiFlotaDataTable(List<string> lCentrosCoste, EnumAltasBajasAmbas estadoVehiculo)
        //Por cada agrupación seleccionada, obtenemos los datos de mi vehículo.
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var datosITV = unitOfWork.RepositoryECAR_Datos_ITV.Fetch().OrderByDescending(o => o.Vto_ITV);

                var miFlotaModel = (from vehiculo in unitOfWork.RepositoryECAR_Datos_Vehiculo.Fetch()
                                    where ((lCentrosCoste.Contains(vehiculo.CC) || vehiculo.CC == null || vehiculo.CC == "") &&
                                    (estadoVehiculo == EnumAltasBajasAmbas.Bajas ? vehiculo.Baja == true :
                                        estadoVehiculo == EnumAltasBajasAmbas.Altas ? vehiculo.Baja == false : true))
                                    join
                                    conductor in unitOfWork.RepositoryECAR_Datos_Conductor.Fetch() on vehiculo.Conductor equals conductor.Cod_Conductor into conductores
                                    from conductoresfinal in conductores.DefaultIfEmpty()
                                    join
                                    leasing in unitOfWork.RepositoryT_M_EMPRESAS_VEHICULOS.Fetch() on vehiculo.EmpresaLeasing equals leasing.ID_EMPRESA into empleasing
                                    from empLeasing in empleasing.DefaultIfEmpty()
                                    let datoITV = datosITV.Where(o => o.Matricula == vehiculo.Matricula).FirstOrDefault()
                                    select new MiFlotaDatatableModel
                                    {
                                        Sociedad = vehiculo.Sociedad,
                                        Matricula = vehiculo.Matricula,
                                        Conductor = (conductoresfinal.Nombre ?? "") + " " + (conductoresfinal.Apellidos ?? ""),
                                        CECO = vehiculo.CC,
                                        Modelo = vehiculo.T_M_MODELOS_VEHICULO.DESCRIPCION,
                                        FechaRecogida = vehiculo.Fecha_Incorporacion,
                                        FechaAlta = vehiculo.Fecha_Alta,
                                        Cuotas = vehiculo.Cuotas,
                                        CiaRenting = empLeasing.NOMBRE,
                                        FechaProximaITV = (datoITV == null ? null : datoITV.Vto_ITV),
                                        NumContrato = vehiculo.Num_Contrato,
                                        FechaVtoSeguro = vehiculo.Vto_Seguro,
                                        Baja = vehiculo.Baja,
                                        EstadoDelVehiculo = (vehiculo.Baja == true ? "Baja" : "Alta"),
                                        Accion = ""
                                    }).ToList();

                miFlotaModel.ForEach(x =>
                {
                    x.FechaVtoRenting = x.FechaAlta == null ? null : x.Cuotas == null ? x.FechaAlta : x.FechaAlta.Value.AddMonths((int)x.Cuotas);
                    x.FechaVtoSeguro = (x.FechaVtoSeguro == null ? (x.FechaAlta != null ? x.FechaAlta.Value.AddMonths(12) : (DateTime?)null) : x.FechaVtoSeguro);
                });

                return miFlotaModel;
            }
        }

        /// <summary>
        /// Devuelve los datos del Vehiculo (Datos Generales, Datos ITV, Datos Contrato, Datos Conductor, ...)
        /// </summary>
        /// <param name="matricula"></param>
        /// <returns></returns>
        public DatosVehiculoModel GetDatosVehiculo(string matricula)
        {

            var datosGeneralesVehiculo = GetDatosGeneralesVehiculo(matricula);
            var datosITVVehiculo = GetDatosITVVehiculo(matricula);
            var datosContratoVehiculo = GetDatosContratoVehiculo(matricula);
            var datosConductoresVehiculo = GetDatosConductoresVehiculo(matricula);
            var datosMultasVehiculo = GetDatosVehiculoMultas(matricula);
            var datosSOLREDVehiculo = GetDatosVehiculoConsumoCombustible(matricula);
            var datosLeasePlanVehiculo = GetDatosVehiculoLeasePlan(matricula);
            var datosDocumentacionVehiculo = GetDatosVehiculoDocumentacion(matricula);
            var DatosViaVerdeVehiculo = GetViaVerdeDataTableByMatricula(matricula);

            DatosVehiculoModel datosVehiculo = new DatosVehiculoModel
            {
                DatosGenerales_Vehiculo = datosGeneralesVehiculo != null ? datosGeneralesVehiculo : new DatosGeneralesModel(),
                DatosITV_Vehiculo = datosITVVehiculo != null ? datosITVVehiculo : new DatosITVModel(),
                DatosContrato_Vehiculo = datosContratoVehiculo != null ? datosContratoVehiculo : new DatosContratoModel(),
                DatosConductor_Vehiculo = datosConductoresVehiculo != null ? datosConductoresVehiculo : new ConductoresVehiculoModel(),
                DatosMultas_Vehiculo = datosMultasVehiculo != null ? datosMultasVehiculo : new List<DatosVehiculoMultaModel>(),
                DatosSOLRED_Vehiculo = datosSOLREDVehiculo != null ? datosSOLREDVehiculo : new List<DatosVehiculoSOLREDModel>(),
                DatosLeasePlan_Vehiculo = datosLeasePlanVehiculo != null ? datosLeasePlanVehiculo : new List<DatosVehiculoLeasePlanModel>(),
                DatosDocumento_Vehiculo = datosDocumentacionVehiculo != null ? datosDocumentacionVehiculo : new List<DatosVehiculoDocumentacionModel>(),
                DatosVia_VerdeDataTable_Vehiculo = DatosViaVerdeVehiculo != null ? DatosViaVerdeVehiculo : new ViaVerdeDatatable(),
            };

            return datosVehiculo;
        }


        public DatosGeneralesModel GetDatosGeneralesVehiculo(string matricula)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var fechaAltaVehiculo = unitOfWork.RepositoryECAR_Datos_Vehiculo.Fetch().Where(o => o.Matricula == matricula).FirstOrDefault().Fecha_Alta;

                var datosVehiculo = (from vehiculo in unitOfWork.RepositoryECAR_Datos_Vehiculo.Fetch().Where(o => o.Matricula == matricula)
                                     join
                                     seguros in unitOfWork.RepositoryT_M_EMPRESAS_VEHICULOS.Fetch() on vehiculo.Cia_Seguro equals seguros.ID_EMPRESA into compañias
                                     from compañiasSeguro in compañias.DefaultIfEmpty()
                                     join
                                     tipos in unitOfWork.RepositoryT_M_TIPO_SEGURO_VEHICULO.Fetch() on vehiculo.Tipo_Seguro equals tipos.ID_SEGURO_VEHICULO into tiposseg
                                     from tiposSeguro in tiposseg.DefaultIfEmpty()
                                     select new DatosGeneralesModel
                                     {
                                         Matricula = vehiculo.Matricula,
                                         Marca = vehiculo.T_M_MARCA_VEHICULOS.DESCRIPCION,
                                         Modelo = vehiculo.T_M_MODELOS_VEHICULO.DESCRIPCION,
                                         TipoVehiculo = vehiculo.T_M_TIPOS_VEHICULO.DESCRIPCION,
                                         Bastidor = vehiculo.Num_Bastidor,
                                         Seguro_Compañia = compañiasSeguro.NOMBRE,
                                         Seguro_Tipo = tiposSeguro.DESCRIPCION,
                                         Seguro_Poliza = vehiculo.Poliza_Seguro,
                                         Seguro_Importe = vehiculo.Importe_Seguro,
                                         Seguro_FechaVencimiento = vehiculo.Vto_Seguro,
                                         Observaciones = vehiculo.Observaciones,
                                         Empresa = vehiculo.Sociedad,
                                         IDEmpresa = vehiculo.Sociedad,

                                         //IDCentroCoste = vehiculo.CC,
                                         //CentroCoste = vehiculo.CC,
                                         //IDDelegacion = vehiculo.Delegacion,
                                     }).FirstOrDefault();

                datosVehiculo.Seguro_FechaVencimiento = (datosVehiculo.Seguro_FechaVencimiento == null ? (fechaAltaVehiculo != null ? fechaAltaVehiculo.Value.AddMonths(12) : (DateTime?)null) : datosVehiculo.Seguro_FechaVencimiento);

                return datosVehiculo;
            }
        }

        public DatosITVModel GetDatosITVVehiculo(string matricula)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                DatosITVModel datosITV_Vehiculo = new DatosITVModel();

                var datoITV = unitOfWork.RepositoryECAR_Datos_ITV.Fetch().Where(x => x.Matricula == matricula).OrderByDescending(o => o.Vto_ITV).FirstOrDefault();

                if (datoITV != null)
                {
                    datosITV_Vehiculo = new DatosITVModel
                    {
                        Matricula = datoITV.Matricula,
                        FechaUltimaITV = datoITV.Ultima_ITV,
                        FechaVtoITV = datoITV.Vto_ITV,
                        TarifaITV = datoITV.Tarifa,
                        TasaITV = datoITV.Tasa,
                        ImporteITV = datoITV.Importe,
                        PrimaConservacionITV = datoITV.Pr_Conservacion,
                        ImporteCirculacionITV = datoITV.Impuesto_Circulacion,
                        Observaciones = datoITV.Otros
                    };
                }

                return datosITV_Vehiculo;
            }
        }

        public DatosContratoModel GetDatosContratoVehiculo(string matricula)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var datosContrato_Vehiculo = (from vehiculo in unitOfWork.RepositoryECAR_Datos_Vehiculo.Fetch().Where(o => o.Matricula == matricula)
                                              join
                                              liquidacion in unitOfWork.RepositoryECAR_Tipo_Liquidacion.Fetch() on vehiculo.IDTipoLiquidacion equals liquidacion.Cod_tipo into liquidaciones
                                              from tiposliquidaciones in liquidaciones.DefaultIfEmpty()

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
                                                  TipoLiquidacion = tiposliquidaciones.Descripcion,
                                                  ExcesoAjuste = vehiculo.Exceso_ajuste,
                                                  CoefExceso = vehiculo.Coef_exceso,
                                                  KMExentos = vehiculo.Km_Exentos,
                                                  Abono = vehiculo.Abono,
                                                  Cargo = vehiculo.Cargo
                                              }).FirstOrDefault();

                if (datosContrato_Vehiculo != null)
                {
                    if (datosContrato_Vehiculo.FechaFinalizacion == null)
                    {
                        datosContrato_Vehiculo.FechaFinalizacion = datosContrato_Vehiculo.FechaAlta == null ? null : datosContrato_Vehiculo.Cuotas == null ? datosContrato_Vehiculo.FechaAlta : datosContrato_Vehiculo.FechaAlta.Value.AddMonths((int)datosContrato_Vehiculo.Cuotas);
                    }
                }

                return datosContrato_Vehiculo;
            }
        }


        public ConductoresVehiculoModel GetDatosConductoresVehiculo(string matricula)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                ConductoresVehiculoModel DatosConductores = new ConductoresVehiculoModel();

                var datosConductor = GetDatosConductor(matricula);

                if (datosConductor != null)
                {
                    T_G_HIST_CAMBIOS_CONDUCTORSpecification specHis = new T_G_HIST_CAMBIOS_CONDUCTORSpecification
                    {
                        MATRICULA = matricula
                    };

                    var Conductores = unitOfWork.RepositoryECAR_Datos_Conductor.Fetch().ToList();

                    List<DatoHistoricoConductores> ListaConductoresH = new List<DatoHistoricoConductores>();

                    if (Conductores != null)
                    {
                        var DatosHistorico = unitOfWork.RepositoryT_G_HIST_CAMBIOS_CONDUCTOR.Where(specHis).OrderBy(o => o.FECHA_ALTA).ThenBy(o => o.ID_HIST_CAMBIOS_CONDUCTOR).ToList();

                        //Procesamos la lista de Historico de Conductores ordenada ascendente por fecha y por id.
                        //Solo cogemos el valor de Cod_ConductorAnterior si es el primer registro y no es null.
                        //Después cogemos los valores de Cod_ConductorNuevo si tiene valor.
                        bool esPrimerRegistro = true;
                        foreach (T_G_HIST_CAMBIOS_CONDUCTOR dato in DatosHistorico)
                        {
                            DatoHistoricoConductores datoHistoricoAnt = new DatoHistoricoConductores();

                            if (esPrimerRegistro)
                            {
                                if (dato.ID_CONDUCTOR_ANT != null)
                                {
                                    ECAR_Datos_Conductor conductor = Conductores.FirstOrDefault(o => o.Cod_Conductor == dato.ID_CONDUCTOR_ANT);

                                    datoHistoricoAnt.Conductor = conductor.Nombre + " " + conductor.Apellidos;
                                    datoHistoricoAnt.Fecha = dato.FECHA_ALTA;

                                    ListaConductoresH.Add(datoHistoricoAnt);
                                }
                                esPrimerRegistro = false;
                            }

                            if (dato.ID_CONDUCTOR_NUEVO.HasValue)
                            {
                                ECAR_Datos_Conductor conductor = Conductores.FirstOrDefault(o => o.Cod_Conductor == dato.ID_CONDUCTOR_NUEVO);

                                DatoHistoricoConductores datoHistoricoAct = new DatoHistoricoConductores();

                                datoHistoricoAct.Conductor = conductor.Nombre + " " + conductor.Apellidos;
                                datoHistoricoAct.Fecha = dato.FECHA_ALTA;

                                ListaConductoresH.Add(datoHistoricoAct);
                            }
                        }
                    }

                    //Buscar el número de empleado en UsuariosSAP, si este vienen vacío de COREA.
                    var _numeroempleado = string.Empty;
                    if (string.IsNullOrEmpty(datosConductor.NumEmpleado) && !string.IsNullOrEmpty(datosConductor.DNI))
                    {
                        var _empleado = unitOfWork.RepositorySAPHR_UsuariosSAP.FindOne(o => o.Dni == datosConductor.DNI.Replace("-", "").Replace(".", "").Replace(" ", ""));
                        if (_empleado != null)
                        {
                            _numeroempleado = _empleado.NumeroEmpleado.ToString();
                        }
                    }
                    else
                    {
                        _numeroempleado = datosConductor.NumEmpleado;
                    }
                    DatosConductores = new ConductoresVehiculoModel
                    {
                        Matricula = datosConductor.Matricula,
                        Nombre = datosConductor.Nombre,
                        Apellidos = datosConductor.Apellidos,
                        CodPostal = datosConductor.CodPostal,
                        CECO = datosConductor.CECO,
                        Direccion = datosConductor.Direccion,
                        FechaNacimiento = datosConductor.FechaNacimiento,
                        FechaVtoCarnet = datosConductor.FechaVtoCarnet,
                        Movil = datosConductor.Movil,
                        Poblacion = datosConductor.Poblacion,
                        Provincia = datosConductor.Provincia,
                        Telefono = datosConductor.Telefono,
                        CarnetConducir = datosConductor.CarnetConducir,
                        idAlertaCarnet = datosConductor.idAlertaCarnet,
                        DNI = datosConductor.DNI,
                        NumEmpleado = _numeroempleado,
                        ListaConductores = ListaConductoresH.OrderByDescending(o => o.Fecha).ToList()
                    };
                }


                return DatosConductores;
            }
        }

        public DatosConductorModel GetDatosConductor(string matricula)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                ECAR_Datos_VehiculoSpecification spec = new ECAR_Datos_VehiculoSpecification
                {
                    Matricula = matricula,
                };

                var datosConductorAux = (from vehiculo in unitOfWork.RepositoryECAR_Datos_Vehiculo.Where(spec)
                                         join
                                         conductor in unitOfWork.RepositoryECAR_Datos_Conductor.Fetch() on vehiculo.Conductor equals conductor.Cod_Conductor into DatosConductor
                                         from DatosConductorFinal in DatosConductor.DefaultIfEmpty()
                                             //let carnetConductor = 
                                             //          unitOfWork.RepositoryT_G_ALERTAS_RENOVACION_CARNET.Fetch()
                                             //          .Where(o=>o.COD_CONDUCTOR == DatosConductorFinal.Cod_Conductor)
                                             //          .OrderByDescending(c=>c.ID_ALERTA).FirstOrDefault()
                                         select new
                                         {
                                             Matricula = vehiculo.Matricula,
                                             Nombre = (DatosConductorFinal.Nombre ?? ""),
                                             Apellidos = (DatosConductorFinal.Apellidos ?? ""),
                                             CodPostal = DatosConductorFinal.Cod_Postal,
                                             CECO = DatosConductorFinal.CodCeco,
                                             Direccion = DatosConductorFinal.Direccion,
                                             FechaNacimiento = DatosConductorFinal.Fecha_Nacimiento,
                                             FechaVtoCarnet = DatosConductorFinal.Fecha_Vencimiento_Carnet,
                                             Movil = DatosConductorFinal.Movil,
                                             Poblacion = DatosConductorFinal.Poblacion,
                                             Provincia = DatosConductorFinal.Provincia,
                                             Telefono = DatosConductorFinal.Tlf,
                                             codConductor = vehiculo.Conductor,
                                             Dni = DatosConductorFinal.DNI,
                                             NumEmpleado = DatosConductorFinal.Num_Empleado
                                             //CarnetConducir = carnetConductor.FICHERO_CARNET,
                                             //idAlertaCarnet = carnetConductor.ID_ALERTA
                                         }).FirstOrDefault();

                var datosConductor = new DatosConductorModel();

                if (datosConductorAux != null)
                {
                    datosConductor = new DatosConductorModel
                    {
                        Matricula = datosConductorAux.Matricula,
                        Nombre = datosConductorAux.Nombre,
                        Apellidos = datosConductorAux.Apellidos,
                        CodPostal = datosConductorAux.CodPostal,
                        CECO = datosConductorAux.CECO,
                        Direccion = datosConductorAux.Direccion,
                        FechaNacimiento = datosConductorAux.FechaNacimiento,
                        FechaVtoCarnet = datosConductorAux.FechaVtoCarnet,
                        Movil = datosConductorAux.Movil,
                        Poblacion = datosConductorAux.Poblacion,
                        Provincia = datosConductorAux.Provincia,
                        Telefono = datosConductorAux.Telefono,
                        DNI = datosConductorAux.Dni,
                        NumEmpleado = datosConductorAux.NumEmpleado
                    };

                    var carnetConductor = unitOfWork.RepositoryT_G_ALERTAS_RENOVACION_CARNET.Fetch()
                                        .Where(o => o.COD_CONDUCTOR == datosConductorAux.codConductor)
                                        .OrderByDescending(c => c.ID_ALERTA).FirstOrDefault();
                    if (carnetConductor != null)
                    {
                        if (!string.IsNullOrEmpty(carnetConductor.FICHERO_CARNET))
                        {
                            datosConductor.idAlertaCarnet = carnetConductor.ID_ALERTA;
                            datosConductor.CarnetConducir = carnetConductor.FICHERO_CARNET;
                        }
                    }
                }

                return datosConductor;
            }
        }

        public DatosConductorModel GetDatosConductor(int codConductor)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                ECAR_Datos_ConductorSpecification spec = new ECAR_Datos_ConductorSpecification
                {
                    Cod_Conductor = codConductor
                };

                var datosConductor = (from conductor in unitOfWork.RepositoryECAR_Datos_Conductor.Where(spec)
                                      select new DatosConductorModel
                                      {
                                          Matricula = "",
                                          Nombre = (conductor.Nombre ?? ""),
                                          Apellidos = (conductor.Apellidos ?? ""),
                                          CodPostal = conductor.Cod_Postal,
                                          CECO = conductor.CodCeco,
                                          Direccion = conductor.Direccion,
                                          FechaNacimiento = conductor.Fecha_Nacimiento,
                                          FechaVtoCarnet = conductor.Fecha_Vencimiento_Carnet,
                                          Movil = conductor.Movil,
                                          Poblacion = conductor.Poblacion,
                                          Provincia = conductor.Provincia,
                                          Telefono = conductor.Tlf,
                                          CarnetConducir = ""
                                      }).FirstOrDefault();

                return datosConductor;
            }
        }



        public IEnumerable<DatosVehiculoMultaModel> GetDatosVehiculoMultas(string matricula)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var datosMulta_Vehiculo = (from vehiculo in unitOfWork.RepositoryECAR_Datos_Multas.Fetch().Where(o => o.Matricula == matricula)
                                           select new DatosVehiculoMultaModel
                                           {
                                               Matricula = vehiculo.Matricula,
                                               Fecha = vehiculo.Fecha,
                                               Importe = vehiculo.Importe,
                                               Motivo = vehiculo.Motivo
                                           })
                                           .Union //Segundo obtenemos los documentos del vehículo asociados a alguna alerta.
                                           (from multaTipoAlerta in unitOfWork.RepositoryT_G_ALERTAS.Fetch()
                                                                .Where(o => o.MATRICULA == matricula && o.ID_TIPO_ALERTA == (int)EnumTipoAlerta.Multa)
                                            join 
                                            multa in unitOfWork.RepositoryT_G_ALERTAS_MULTA.Fetch() on multaTipoAlerta.ID_ALERTA equals multa.ID_ALERTA
                                            select new DatosVehiculoMultaModel
                                            {
                                                Matricula = multaTipoAlerta.MATRICULA,
                                                Fecha = multa.FECHA_DENUNCIA,
                                                Importe = (double)multa.IMPORTE,
                                                Motivo = multa.INFRACCION
                                            }).OrderByDescending(o => o.Fecha).ToList();

                return new List<DatosVehiculoMultaModel>(datosMulta_Vehiculo);
            }
        }

        public IEnumerable<DatosVehiculoSOLREDModel> GetDatosVehiculoConsumoCombustible(string matricula) //Puede que haya que modificarlo para recibir el parámetro Sociedad.
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var empresa = unitOfWork.RepositoryECAR_Datos_Vehiculo.Fetch().Where(o => o.Matricula == matricula).FirstOrDefault().Sociedad;

                List<DatosVehiculoSOLREDModel> datosSOLRED_Vehiculo;

                //if (empresa == Constants.CODIGO_EMPRESA_PORTUGAL)
                //{
                    datosSOLRED_Vehiculo = (from vehiculo in unitOfWork.RepositoryT_G_TARJETA_COMBUSTIBLE.Fetch().Where(o => o.MATRICULA == matricula)
                                            select new DatosVehiculoSOLREDModel
                                            {
                                                Matricula = vehiculo.MATRICULA,
                                                Ejercicio = vehiculo.EJERCICIO,
                                                Sociedad = vehiculo.Sociedad,
                                                FechaFactura = vehiculo.FECHA_FACTURA,
                                                FechaOperacion = vehiculo.FECHA_OPERACION,
                                                DescProducto = vehiculo.DES_PRODU,
                                                Kilometros = vehiculo.KILOMETROS,
                                                Litros = vehiculo.NUM_LITROS,
                                                Importe = vehiculo.IMPORTE,
                                                CodTarjeta = vehiculo.COD_TARJETA,
                                                NumDocumento = vehiculo.NUM_DOCUMENTO,
                                            }).OrderByDescending(o => o.FechaFactura).ToList();
                //}
                //else
                //{
                //    datosSOLRED_Vehiculo = (from vehiculo in unitOfWork.RepositoryECAR_Datos_SolRed.Fetch().Where(o => o.MATRICULA == matricula)
                //                            select new DatosVehiculoSOLREDModel
                //                            {
                //                                Matricula = vehiculo.MATRICULA,
                //                                Ejercicio = vehiculo.EJERCICIO,
                //                                Sociedad = vehiculo.Sociedad,
                //                                FechaFactura = vehiculo.FECHA_FACTURA,
                //                                FechaOperacion = vehiculo.FECHA_OPERACION,
                //                                DescProducto = vehiculo.DES_PRODU,
                //                                Kilometros = vehiculo.KILOMETROS,
                //                                Litros = vehiculo.NUM_LITROS,
                //                                Importe = vehiculo.IMPORTE,
                //                                CodTarjeta = "",
                //                                NumDocumento = "",
                //                            }).OrderByDescending(o => o.FechaFactura).ToList();

                //}

                return datosSOLRED_Vehiculo;
            }
        }

        public IEnumerable<DatosVehiculoLeasePlanModel> GetDatosVehiculoLeasePlan(string matricula) //Puede que haya que modificarlo para recibir el parámetro Sociedad.
        {
            using (var unitOfWork = new UnitOfWork())
            {
                IEnumerable<DatosVehiculoLeasePlanModel> datosLeasePlan_Vehiculo;
                var empresa = unitOfWork.RepositoryECAR_Datos_Vehiculo.Fetch().Where(o => o.Matricula == matricula).FirstOrDefault().Sociedad;

                T_G_DATOS_LEASINGSpecification spec = new T_G_DATOS_LEASINGSpecification
                {
                    Matricula = matricula,
                    Sociedad = empresa,
                };

                    datosLeasePlan_Vehiculo = (from vehiculo in unitOfWork.RepositoryT_G_DATOS_LEASING.Where(spec)
                                               select new DatosVehiculoLeasePlanModel
                                               {
                                                   Matricula = vehiculo.Matricula,
                                                   Ejercicio = vehiculo.Ejercicio,
                                                   Sociedad = vehiculo.Sociedad,
                                                   FechaFactura = vehiculo.Fecha_Factura,
                                                   NumFactura = vehiculo.Num_Factura,
                                                   Trimestre = vehiculo.Trimestre,
                                                   ImpRenting = vehiculo.Alquiler,
                                                   ImpMantenimiento = vehiculo.Mantenimiento,
                                                   ImpAdministracion = vehiculo.Administracion,
                                                   ImpSeguro = vehiculo.Seguro,
                                                   ImpITV = vehiculo.ITV_IVA
                                               }).OrderByDescending(o => o.FechaFactura);

                return new List<DatosVehiculoLeasePlanModel>(datosLeasePlan_Vehiculo.ToList());
            }
        }

        public IEnumerable<DatosVehiculoDocumentacionModel> GetDatosVehiculoDocumentacion(string matricula) //Puede que haya que modificarlo para recibir el parámetro Sociedad.
        {
            using (var unitOfWork = new UnitOfWork())
            {                                   //Primero obtenemos documentos asociados al vehículo
                var datosDocumentacion_Vehiculo = (from documento in unitOfWork.RepositoryT_G_DOCUMENTACION_VEHICULO.Fetch().Where(o => o.MATRICULA == matricula)
                                                   join
                                                   tipoAlerta in unitOfWork.RepositoryT_M_TIPOS_ALERTAS.Fetch() on documento.ID_TIPO_ALERTA equals tipoAlerta.ID_TIPO_ALERTA
                                                   select new DatosVehiculoDocumentacionModel
                                                   {
                                                       Matricula = documento.MATRICULA,
                                                       DescDocumento = documento.DESCRIPCION,
                                                       idDocumento = documento.ID_DOCUMENTO,
                                                       Documento = documento.FICHERO,
                                                       DescCategoria = tipoAlerta.DESCRIPCION,
                                                       FechaAlta = documento.FECHA_ALTA,
                                                       IdAlerta = 0,
                                                       idCategoria = 0,
                                                       Accion = ""
                                                   })
                                               .Union //Segundo obtenemos los documentos del vehículo asociados a alguna alerta.
                                               (from documentoAlerta in unitOfWork.RepositoryT_G_ALERTAS.Fetch().Where(o => o.MATRICULA == matricula && o.FICHERO != null)
                                                join
                                                tipoAlerta in unitOfWork.RepositoryT_M_TIPOS_ALERTAS.Fetch() on documentoAlerta.ID_TIPO_ALERTA equals tipoAlerta.ID_TIPO_ALERTA
                                                select new DatosVehiculoDocumentacionModel
                                                {
                                                    Matricula = documentoAlerta.MATRICULA,
                                                    DescDocumento = resourceView.DescDocumentoAsociado + tipoAlerta.DESCRIPCION,
                                                    idDocumento = 0,
                                                    Documento = (documentoAlerta.FICHERO != null ? documentoAlerta.FICHERO : ""),
                                                    DescCategoria = tipoAlerta.DESCRIPCION,
                                                    FechaAlta = new DateTime().Date,
                                                    IdAlerta = documentoAlerta.ID_ALERTA,
                                                    idCategoria = 0,
                                                    Accion = ""
                                                })
                                               .Union //Tercero obtenemos los documentos del vehículo asociados a la ITV.
                                               (from documentoITV in unitOfWork.RepositoryECAR_Datos_ITV.Fetch().Where(o => o.Matricula == matricula && o.Fichero != null)
                                                select new DatosVehiculoDocumentacionModel
                                                {
                                                    Matricula = documentoITV.Matricula,
                                                    DescDocumento = resourceView.DescDocumentoITV,
                                                    idDocumento = documentoITV.Id,
                                                    Documento = (documentoITV.Fichero != null ? documentoITV.Fichero : ""),
                                                    DescCategoria = "ITV",
                                                    FechaAlta = new DateTime().Date,
                                                    IdAlerta = 0,
                                                    idCategoria = (int)EnumTipoAlerta.ITV, //Para documentos de ITV que se ponene en el mantenimiento del vehículo.
                                                    Accion = ""
                                                });


                //Sacar los distintos documentos que pudiera tener una alerta y no están en la tabla de Alertas

                List<DatosVehiculoDocumentacionModel> returnValue = new List<DatosVehiculoDocumentacionModel>(); //datosDocumentacion_Vehiculo.OrderByDescending(o => o.DescDocumento).ToList();

                //List<DatosVehiculoDocumentacionModel> qq = datosDocumentacion_Vehiculo.Where(o => o.IdAlerta != 0).ToList();

                foreach (DatosVehiculoDocumentacionModel dato in datosDocumentacion_Vehiculo.Where(o => o.IdAlerta != 0))
                {
                    var datosAlerta = new AlertasService().GetAlerta(dato.IdAlerta);
                    if (datosAlerta != null)
                    {
                        if (datosAlerta.DatosOtraNotificacion != null)
                        {
                            if (!string.IsNullOrEmpty(datosAlerta.DatosOtraNotificacion.Fichero))
                            {
                                returnValue.Add(addDocumentoAlerta(dato, datosAlerta.DatosOtraNotificacion.Fichero, datosAlerta.IdTipoAlerta, matricula, dato.IdAlerta));
                            }
                        }

                        if (datosAlerta.DatosRenovacionITV != null)
                        {
                            if (!string.IsNullOrEmpty(datosAlerta.DatosRenovacionITV.FicheroITV))
                            {
                                returnValue.Add(addDocumentoAlerta(dato, datosAlerta.DatosRenovacionITV.FicheroITV, datosAlerta.IdTipoAlerta, matricula, dato.IdAlerta));
                            }
                        }

                        if (datosAlerta.DatosRenovacionCarnet != null)
                        {
                            if (!string.IsNullOrEmpty(datosAlerta.DatosRenovacionCarnet.FicheroCarnet))
                            {
                                returnValue.Add(addDocumentoAlerta(dato, datosAlerta.DatosRenovacionCarnet.FicheroCarnet, datosAlerta.IdTipoAlerta, matricula, dato.IdAlerta));
                            }
                        }

                        if (datosAlerta.ConductorConfirmadoMulta != null)
                        {
                            if (!string.IsNullOrEmpty(datosAlerta.ConductorConfirmadoMulta.FicheroCarnet))
                            {
                                returnValue.Add(addDocumentoAlerta(dato, datosAlerta.ConductorConfirmadoMulta.FicheroCarnet, datosAlerta.IdTipoAlerta, matricula, dato.IdAlerta));
                            }
                        }

                        if (datosAlerta.DatosCambioConductor != null)
                        {
                            if (!string.IsNullOrEmpty(datosAlerta.DatosCambioConductor.FicheroEmiteRenting))
                            {
                                returnValue.Add(addDocumentoAlerta(dato, datosAlerta.DatosCambioConductor.FicheroEmiteRenting, datosAlerta.IdTipoAlerta, matricula, dato.IdAlerta));
                            }
                        }
                    }
                }

                foreach (DatosVehiculoDocumentacionModel dato in datosDocumentacion_Vehiculo.Where(o => !string.IsNullOrEmpty(o.Documento)))
                {
                    returnValue.Add(dato);
                }

                //return new List<DatosVehiculoDocumentacionModel>(datosDocumentacion_Vehiculo.OrderByDescending(o => o.DescDocumento).ToList());
                return returnValue.OrderByDescending(o => o.DescDocumento);
            }
        }

        private DatosVehiculoDocumentacionModel addDocumentoAlerta(DatosVehiculoDocumentacionModel dato, string documento, int idTipoAlerta, string matricula, int idAlerta)
        {
            var type = typeof(EnumTipoAlerta);

            EnumTipoAlerta tipoAlerta = (EnumTipoAlerta)idTipoAlerta;

            var memInfo = type.GetMember(tipoAlerta.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            var descriptionTipoAlerta = ((DescriptionAttribute)attributes[0]).Description;

            DatosVehiculoDocumentacionModel returnValue = new DatosVehiculoDocumentacionModel
            {
                Matricula = matricula,
                DescDocumento = resourceView.DescDocumentoAsociado + descriptionTipoAlerta,
                idDocumento = 0,
                Documento = documento,
                DescCategoria = descriptionTipoAlerta,
                FechaAlta = new DateTime().Date,
                IdAlerta = idAlerta,
                Accion = ""
            };

            return returnValue;

        }



        public List<string> GetMatriculas(string term, List<string> cecos, bool matriculasDeBaja = false)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                return unitOfWork.RepositoryECAR_Datos_Vehiculo.GetMatriculas(term, cecos, matriculasDeBaja);


            }
        }

        public DatosVehiculoDocumentacionModel GetDatosDocumentacionVehiculo(int idDocumento, string matricula)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var datosDocumentacion_Vehiculo = (from documento in unitOfWork.RepositoryT_G_DOCUMENTACION_VEHICULO.Fetch().Where(o => o.ID_DOCUMENTO == idDocumento && o.MATRICULA == matricula)
                                                   join
                                                   tipoAlerta in unitOfWork.RepositoryT_M_TIPOS_ALERTAS.Fetch() on documento.ID_TIPO_ALERTA equals tipoAlerta.ID_TIPO_ALERTA
                                                   select new DatosVehiculoDocumentacionModel
                                                   {
                                                       Matricula = documento.MATRICULA,
                                                       DescDocumento = documento.DESCRIPCION,
                                                       idDocumento = documento.ID_DOCUMENTO,
                                                       Documento = documento.FICHERO,
                                                       DescCategoria = tipoAlerta.DESCRIPCION,
                                                       FechaAlta = documento.FECHA_ALTA,
                                                       Accion = ""
                                                   }).FirstOrDefault();

                return datosDocumentacion_Vehiculo;
            }
        }
        #region Mantenimiento

        #region Documentacion
        public void SaveDocumentacionVehiculo(DatosVehiculoDocumentacionModel modelo)
        {
            string doc = string.Empty;
            using (var unitOfWork = new UnitOfWork())
            {

                var documento = new T_G_DOCUMENTACION_VEHICULO
                {
                    ID_TIPO_ALERTA = modelo.idCategoria,
                    DESCRIPCION = modelo.DescDocumento,
                    MATRICULA = modelo.Matricula,
                    FECHA_ALTA = DateTime.Now
                };
                if (modelo.FileUploadDocumentacion != null)
                {
                    if (modelo.FileUploadDocumentacion.FileName != "")
                    {
                        documento.FICHERO = FileUtilities.GetFileName(modelo.FileUploadDocumentacion); //modelo.FileUploadDocumentacion.FileName
                        doc = Global.GetPathToUploadDocumentMiVehiculo(modelo.Matricula, "-", "_") + documento.FICHERO;
                    }
                }

                unitOfWork.RepositoryT_G_DOCUMENTACION_VEHICULO.Insert(documento);

                unitOfWork.Commit();
            }

            //FileUtilities utilityFile = new FileUtilities();

            if (modelo.FileUploadDocumentacion != null)
            {
                FileUtilities.BorraDocumentoFisico(doc);
                FileUtilities.UploadFile(modelo.FileUploadDocumentacion, Global.GetPathToUploadDocumentMiVehiculo(modelo.Matricula, "-", "_"));
            }

        }

        public void DeleteDocumentacion(int idDocumento, string matricula)
        {
            string nombreArchivo = string.Empty;

            using (var unitOfWork = new UnitOfWork())
            {
                T_G_DOCUMENTACION_VEHICULOSpecification spec = new T_G_DOCUMENTACION_VEHICULOSpecification
                {
                    ID_DOCUMENTO = idDocumento,
                    MATRICULA = matricula
                };
                T_G_DOCUMENTACION_VEHICULO DocumentacionV = new UnitOfWork().RepositoryT_G_DOCUMENTACION_VEHICULO.Where(spec).FirstOrDefault();

                T_G_DOCUMENTACION_VEHICULO DocVehiculo = new T_G_DOCUMENTACION_VEHICULO();
                DocVehiculo.ID_DOCUMENTO = DocumentacionV.ID_DOCUMENTO;
                DocVehiculo.MATRICULA = DocumentacionV.MATRICULA;

                nombreArchivo = Global.GetPathToUploadDocumentMiVehiculo(matricula, "-", "_") + DocumentacionV.FICHERO;

                unitOfWork.RepositoryT_G_DOCUMENTACION_VEHICULO.Delete(DocVehiculo);

                unitOfWork.Commit();

            }

            FileUtilities.BorraDocumentoFisico(nombreArchivo);

        }
        #endregion Documentacion
        #endregion Mantenimiento

        #region ViaVerde

        public List<Via_Verde_Extractos> GetViaVerdeExtractosByMatricula(string Matricula)
        {
            List<Via_Verde_Extractos> extractos = new List<Via_Verde_Extractos>();

            using (var unitOfWork = new UnitOfWork())
            {
                T_G_VIA_VERDE_IDENTIFICADORESSpecification spec = new T_G_VIA_VERDE_IDENTIFICADORESSpecification
                {
                    MATRICULA = Matricula
                };

                var identificadores = unitOfWork.RepositoryT_G_VIA_VERDE_IDENTIFICADORES
                                .Where(spec).ToList().OrderBy(x => x.ID_EXTRACTO);

                if (identificadores != null)
                {
                    string extractoAct = "";
                    foreach (T_G_VIA_VERDE_IDENTIFICADORES identificador in identificadores)
                    {
                        if (extractoAct != identificador.ID_EXTRACTO)
                        {
                            extractoAct = identificador.ID_EXTRACTO;
                            var extracto = GetViaVerdeExtracto(identificador.ID_EXTRACTO);
                            if (extracto != null)
                            {
                                extractos.Add(extracto);
                            }
                        }

                        extractos[extractos.Count() - 1].IDENTIFICADORES = new List<Via_Verde_Identificadores>();

                        Via_Verde_Identificadores ident = new Via_Verde_Identificadores
                        {
                            IDENTIFICADOR = identificador.IDENTIFICADOR,
                            ID_EXTRACTO = identificador.ID_EXTRACTO,
                            ID_IDENTIFICADOR = identificador.ID_IDENTIFICADOR,
                            MATRICULA = identificador.MATRICULA,
                            REF_PAGO = identificador.REF_PAGO,
                            TOTAL = identificador.TOTAL,
                            TRANSACCIONES = new List<Via_Verde_Transacciones>(GetViaVerdeTransacciones(identificador.ID_IDENTIFICADOR)),
                        };


                        extractos[extractos.Count() - 1].IDENTIFICADORES.Add(ident);
                    }
                }
            }
            return extractos;
        }

        public Via_Verde_Extractos GetViaVerdeExtracto(string idExtracto)
        {
            Via_Verde_Extractos valorReturn = new Via_Verde_Extractos();

            using (var unitOfWork = new UnitOfWork())
            {

                var extracto = unitOfWork.RepositoryT_G_VIA_VERDE_EXTRACTOS.Fetch()
                          .FirstOrDefault(x => x.ID_EXTRACTO == idExtracto);
                if (extracto != null)
                {
                    valorReturn.ID_EXTRACTO = extracto.ID_EXTRACTO;
                    valorReturn.ID_CLIENTE = extracto.ID_CLIENTE;
                    valorReturn.AÑO_EMISION = extracto.AÑO_EMISION;
                    valorReturn.MES_EMISION = extracto.MES_EMISION;
                    valorReturn.TOTAL = extracto.TOTAL;
                    valorReturn.TOTAL_IVA = extracto.TOTAL_IVA;

                    if (extracto.T_M_CLIENTES != null)
                    {
                        valorReturn.CLIENTE = new ClientesModels
                        {
                            ID_CLIENTE = extracto.T_M_CLIENTES.ID_CLIENTE,
                            CODIGO_POSTAL = extracto.T_M_CLIENTES.CODIGO_POSTAL,
                            DIRECCION = extracto.T_M_CLIENTES.DIRECCION,
                            LOCALIDAD = extracto.T_M_CLIENTES.LOCALIDAD,
                            NIF = extracto.T_M_CLIENTES.NIF,
                            NOMBRE = extracto.T_M_CLIENTES.NOMBRE,
                        };
                    }

                }
                else
                {
                    return null;
                }
            }

            return valorReturn;
        }

        public List<Via_Verde_Transacciones> GetViaVerdeTransacciones(int idIdentificador)
        {
            List<Via_Verde_Transacciones> valorReturn = new List<Via_Verde_Transacciones>();

            using (var unitOfWork = new UnitOfWork())
            {
                T_G_VIA_VERDE_TRANSACCIONESSpecification spec = new T_G_VIA_VERDE_TRANSACCIONESSpecification
                {
                    ID_IDENTIFICADOR = idIdentificador,
                };

                var transacciones = (from transaccion in unitOfWork.RepositoryT_G_VIA_VERDE_TRANSACCIONES.Where(spec)
                                     select new Via_Verde_Transacciones
                                     {
                                         DECUENTO = transaccion.DECUENTO,
                                         FECHA_ENTRADA = transaccion.FECHA_ENTRADA,
                                         FECHA_SALIDA = transaccion.FECHA_SALIDA,
                                         FECHA_TARJETA = transaccion.FECHA_TARJETA,
                                         ID_IDENTIFICADOR = transaccion.ID_IDENTIFICADOR,
                                         ID_TRANSACCION = transaccion.ID_TRANSACCION,
                                         IMPORTE = transaccion.IMPORTE,
                                         LUGAR_ENTRADA = transaccion.LUGAR_ENTRADA,
                                         LUGAR_SALIDA = transaccion.LUGAR_SALIDA,
                                         NUM_TARJETA = transaccion.NUM_TARJETA,
                                         OPERADOR = transaccion.OPERADOR,
                                         PORCENTAJE_IMPUESTO = transaccion.PORCENTAJE_IMPUESTO,
                                         TIPO = transaccion.TIPO,
                                     }).OrderBy(x => x.FECHA_ENTRADA).ToList();

                if (transacciones != null)
                {
                    return transacciones;
                }
            }

            return valorReturn;
        }


        public ViaVerdeDatatable GetViaVerdeDataTableByMatricula(string Matricula)
        {
            ViaVerdeDatatable DatosViaVerdeDataTable = new ViaVerdeDatatable();
            DatosViaVerdeDataTable.LineasDataTable = new List<ViaVerdeLineaDatatable>();
            DatosViaVerdeDataTable.MATRICULA = Matricula;

            string nombreCliente = "";

            using (var unitOfWork = new UnitOfWork())
            {
                T_G_VIA_VERDE_IDENTIFICADORESSpecification spec = new T_G_VIA_VERDE_IDENTIFICADORESSpecification
                {
                    MATRICULA = Matricula
                };

                var identificadores = unitOfWork.RepositoryT_G_VIA_VERDE_IDENTIFICADORES
                                .Where(spec).ToList().OrderBy(x => x.ID_EXTRACTO);

                if (identificadores != null)
                {
                    string extractoAct = "";
                    foreach (T_G_VIA_VERDE_IDENTIFICADORES identificador in identificadores)
                    {
                        if (extractoAct != identificador.ID_EXTRACTO)
                        {
                            extractoAct = identificador.ID_EXTRACTO;
                            var extracto = GetViaVerdeExtracto(identificador.ID_EXTRACTO);
                            if (extracto != null)
                            {
                                nombreCliente = extracto.CLIENTE.NOMBRE;
                            }
                        }

                        RellenaViaVerdeTransaccionesDataTable(identificador.ID_IDENTIFICADOR, DatosViaVerdeDataTable, nombreCliente, identificador.REF_PAGO);
                    }
                }
            }
            return DatosViaVerdeDataTable;
        }

        private void RellenaViaVerdeTransaccionesDataTable(int idIdentificador, ViaVerdeDatatable DatosViaVerdeDataTable, string nombreCliente, string refPago)
        {
            //DatosViaVerdeDataTable.LineasDataTable = new List<ViaVerdeLineaDatatable>();

            using (var unitOfWork = new UnitOfWork())
            {
                T_G_VIA_VERDE_TRANSACCIONESSpecification spec = new T_G_VIA_VERDE_TRANSACCIONESSpecification
                {
                    ID_IDENTIFICADOR = idIdentificador,
                };

                foreach (T_G_VIA_VERDE_TRANSACCIONES transaccion in unitOfWork.RepositoryT_G_VIA_VERDE_TRANSACCIONES.Where(spec).OrderBy(x => x.FECHA_ENTRADA).ToList())
                {
                    DatosViaVerdeDataTable.LineasDataTable.Add(new ViaVerdeLineaDatatable
                    {
                        REF_PAGO = refPago,
                        DECUENTO = transaccion.DECUENTO,
                        FECHA_ENTRADA = transaccion.FECHA_ENTRADA,
                        FECHA_SALIDA = transaccion.FECHA_SALIDA,
                        OPERADOR = transaccion.OPERADOR,
                        IMPORTE = transaccion.IMPORTE,
                        LUGAR_ENTRADA = transaccion.LUGAR_ENTRADA,
                        LUGAR_SALIDA = transaccion.LUGAR_SALIDA,
                    });
                    DatosViaVerdeDataTable.TOTAL_IMPORTE = (DatosViaVerdeDataTable.TOTAL_IMPORTE == null ? 0 : DatosViaVerdeDataTable.TOTAL_IMPORTE) + transaccion.IMPORTE;
                    DatosViaVerdeDataTable.TOTAL_DESCUENTO = (DatosViaVerdeDataTable.TOTAL_DESCUENTO == null ? 0 : DatosViaVerdeDataTable.TOTAL_DESCUENTO) + transaccion.DECUENTO;
                }
            }
        }

        #endregion
    }
}