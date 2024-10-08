using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK_ECAR.Application_Services;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Framework;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;

namespace TK_CoreaToECAR.ApplicationServices
{
    static class ImportFromCOREA
    {
        public static bool ImportaDatosVehiculoFromCorea()
        {
            string Paso = string.Empty;

            var datosVehiculo = new List<Datos_Vehiculo>();

            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    List<ECAR_Datos_Vehiculo> EcarVehiculo = new List<ECAR_Datos_Vehiculo>();
                    datosVehiculo = unitOfWork.RepositoryDatos_Vehiculo.Fetch().ToList().OrderBy(x => x.Sociedad).ThenBy(x => x.Matricula).ToList();
                    var cont = 0;
                    foreach (Datos_Vehiculo dato in datosVehiculo)
                    {
                        try
                        {
                            Paso = dato.Matricula;

                            Console.WriteLine($"Matrícula {dato.Matricula}. {cont++} de {(datosVehiculo.Count()-1).ToString("###,##0")}");

                            //ECAR_Datos_VehiculoSpecification spec = new ECAR_Datos_VehiculoSpecification
                            //{
                            //    Sociedad = dato.Sociedad,
                            //    Matricula = dato.Matricula,
                            //};

                            if (unitOfWork.RepositoryECAR_Datos_Vehiculo.FindOne(x=>x.Sociedad == dato.Sociedad && x.Matricula == dato.Matricula) == null)
                            {
                                if (dato.Conductor != null)
                                {
                                    var ElConductor = unitOfWork.RepositoryECAR_Datos_Conductor.FindOne(x => x.Cod_Conductor == dato.Conductor);
                                    if (ElConductor == null)
                                    {
                                        dato.Conductor = null;
                                    }
                                }

                                ECAR_Datos_Vehiculo DV = new ECAR_Datos_Vehiculo();
                                DV.Sociedad = dato.Sociedad;
                                DV.Matricula = dato.Matricula;
                                DV.Marca = GetMarca(dato.Marca);
                                DV.Modelo = GetModelo(dato.Modelo, DV.Marca);
                                DV.Tipo_Vehiculo = dato.Tipo_Vehiculo;
                                DV.Delegacion = GetDelegacion(dato.Delegacion);
                                DV.Directivo = dato.Directivo;
                                DV.EmpresaLeasing = dato.EmpresaLeasing;
                                DV.Conductor = dato.Conductor == null || dato.Conductor == 0 ? null : dato.Conductor;
                                DV.Tipo_Seguro = dato.Tipo_Seguro == null || dato.Tipo_Seguro == 0 ? null : dato.Tipo_Seguro; ;
                                DV.Cia_Seguro = GetCiaSeguros(dato.Cia_Seguro);
                                DV.Poliza_Seguro = dato.Poliza_Seguro;
                                DV.Importe_Seguro = dato.Importe_Seguro;
                                DV.Vto_Seguro = dato.Vto_Seguro;
                                DV.Veh_sustituido = dato.Veh_sustituido;
                                DV.Num_Contrato = dato.Num_Contrato;
                                DV.Fecha_Alta = dato.Fecha_Alta;
                                DV.Baja = dato.Baja;
                                DV.Fecha_Baja = dato.Fecha_Baja;
                                DV.Fecha_Recibidos = dato.Fecha_Recibidos;
                                DV.Fecha_Devolucion = dato.Fecha_Devolucion;
                                DV.Fecha_Incorporacion = dato.Fecha_Incorporacion;
                                DV.Cuotas = dato.Cuotas;
                                DV.Km_Totales = dato.Km_Totales;
                                DV.IDTipoLiquidacion = dato.Tipo_Liquidacion == null || dato.Tipo_Liquidacion == 0 ? (int?)null : dato.Tipo_Liquidacion;
                                DV.Exceso_ajuste = dato.Exceso_ajuste;
                                DV.Coef_exceso = dato.Coef_exceso;
                                DV.Km_Exentos = dato.Km_Exentos;
                                DV.Abono = dato.Abono;
                                DV.Cargo = dato.Cargo;
                                DV.IDCarburante = GetCarburante(dato.Carburante);
                                DV.Orden = dato.Orden;
                                DV.Ubicacion = dato.Ubicacion == null ? (int?)null : Convert.ToInt32(dato.Ubicacion);
                                DV.Observaciones = dato.Observaciones;
                                DV.Falta = dato.Falta;
                                DV.CC = dato.CC;
                                DV.Extras = null;
                                DV.Num_Bastidor = dato.Num_Bastidor;
                                DV.Departamento = null;
                                DV.IDTipoRuta = null;
                                DV.IDTarjetaCombustible = null;
                                DV.LugarEntrega = null;
                                DV.FechaRenovacion = null;
                                DV.Equipamiento = null;
                                DV.FechaModificacion = null;
                                DV.UsuarioAlta = null;
                                DV.UsuarioModificacion = null;
                                DV.Responsable = null;
                                DV.PrioridadEntrega = null;
                                DV.IdentificadorImportacion = null;
                                DV.FechaImportacion = dato.Fecha_Incorporacion;
                                DV.FechaMatriculacion = dato.Fecha_Incorporacion;
                                DV.UsuarioImportacion = null;

                                unitOfWork.RepositoryECAR_Datos_Vehiculo.Insert(DV);

                                unitOfWork.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"<ImportaDatosVehiculoFromCorea> MATRÍCULA {Paso}. {Environment.NewLine} {ex.Message}");
                        }
                    }
                }

            }

            catch(Exception ex)
            {
                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"<ImportaDatosVehiculoFromCorea> MATRÍCULA {Paso}. {Environment.NewLine} {ex.Message}");
            }

            return true;
        }


        public static bool ImportaDatosLeasePLanFromCorea()
        {
            string Paso = string.Empty;

            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    List<Datos_LeasePlan> EcarVehiculo = new List<Datos_LeasePlan>();

                    var ejercicios = new List<string>{ "2016-2017", "2017-2018" };

                    var spec = new Datos_LeasePlanSpecification
                    {
                        EjercicioIN=ejercicios,
                    };

                    var datosLeasing = unitOfWork.RepositoryDatos_LeasePlan.Where(spec).OrderBy(x => x.Matricula).ToList();

                    var cont = 0;
                    foreach (Datos_LeasePlan leaseplan in datosLeasing)
                    {
                        Paso = $"{leaseplan.Matricula} - id {leaseplan.Id.ToString("###,###,##0")}";

                        Console.WriteLine($"SOLRED Matrícula {leaseplan.Matricula}. {cont++} de {(datosLeasing.Count() - 1).ToString("###,##0")}");

                        T_G_DATOS_LEASING datos = new T_G_DATOS_LEASING
                        {
                            Administracion = leaseplan.Administracion,
                            Administracion_IVA = leaseplan.Administracion_IVA,
                            Alquiler = leaseplan.Alquiler,
                            Alquiler_IVA = leaseplan.Alquiler_IVA,
                            Asistencia_Carretera = leaseplan.Asistencia_Carretera,
                            Asistencia_Carretera_IVA = leaseplan.Asistencia_Carretera_IVA,
                            Canarias = leaseplan.Canarias,
                            Directivo = leaseplan.Directivo,
                            Ejercicio = leaseplan.Ejercicio,
                            EmpresaLeasing = leaseplan.EmpresaLeasing,
                            FechaAlta = leaseplan.FechaAlta,
                            Fecha_Factura = leaseplan.Fecha_Factura,
                            Fecha_Importacion = leaseplan.Fecha_Importacion,
                            Fecha_Servicio = leaseplan.Fecha_Servicio,
                            Impuesto = leaseplan.Impuesto,
                            Imp_Circulacion = leaseplan.Imp_Circulacion,
                            Imp_Circulacion_IVA = leaseplan.Imp_Circulacion_IVA,
                            Imp_Matriculacion = leaseplan.Imp_Matriculacion,
                            Imp_Matriculacion_IVA = leaseplan.Imp_Matriculacion_IVA,
                            Intereses_Prepagados = leaseplan.Intereses_Prepagados,
                            Intereses_Prepagados_IVA = leaseplan.Intereses_Prepagados_IVA,
                            ITV = leaseplan.ITV,
                            ITV_IVA = leaseplan.ITV_IVA,
                            Mantenimiento = leaseplan.Mantenimiento,
                            Mantenimiento_IVA = leaseplan.Mantenimiento_IVA,
                            Matricula = leaseplan.Matricula,
                            Neumaticos = leaseplan.Neumaticos,
                            Neumaticos_IVA = leaseplan.Neumaticos_IVA,
                            Num_Factura = leaseplan.Num_Factura,
                            Seguro = leaseplan.Seguro,
                            Seguro_IVA = leaseplan.Seguro_IVA,
                            Sociedad = leaseplan.Sociedad == null ? 8100 : Convert.ToInt32(leaseplan.Sociedad),
                            Trimestre = leaseplan.Trimestre,
                            NOMBRE_ARCHIVO_IMPORTACION = "automatico",
                        };


                        unitOfWork.RepositoryT_G_DATOS_LEASING.Insert(datos);
                        unitOfWork.Commit();
                    }

                }

            }

            catch (Exception ex)
            {
                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"<ImportaDatosLeasePLanFromCorea> MATRÍCULA {Paso}. {Environment.NewLine} {ex.Message}");
            }

            return true;
        }


        public static bool ImportaDatosSOLREDFromCorea()
        {
            string Paso = string.Empty;

            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    List<Datos_SolRed> EcarSOLRED = new List<Datos_SolRed>();
                    //VER DESDE QUÉ EJERCICIO SE PASA A E-CAR. HAY MÁS DE 1.000.000 DE REGISTROS.
                    //TAMBIÉN HABRÍA QUE PENSAR EN UN SISTEMA PASARA REGISTROS A UNA TABLA TEMPORAL.

                    var ejercicios = new List<string> { "2016-2017", "2017-2018" };

                    var spec = new Datos_SolRedSpecification
                    {
                        EJERCICIOIN = ejercicios,
                    };

                    var datosSolRed = unitOfWork.RepositoryDatos_SolRed.Where(spec).ToList();

                    var cont = 0;

                    foreach (Datos_SolRed solRED in datosSolRed)
                    {
                        Paso = $"{solRED.MATRICULA} - id {solRED.Id.ToString("###,###,##0")}";

                        Console.WriteLine($"SOLRED Matrícula {solRED.MATRICULA}. {cont++} de {(datosSolRed.Count() - 1).ToString("###,##0")}");

                        T_G_TARJETA_COMBUSTIBLE datos = new T_G_TARJETA_COMBUSTIBLE
                        {
                            BONIF_TOTAL = solRED.BONIF_TOTAL,
                            COD_PRODU = solRED.COD_PRODU,
                            COD_TARJETA = "",
                            DES_PRODU = solRED.DES_PRODU,
                            EJERCICIO = solRED.EJERCICIO,
                            FechaAlta = solRED.FechaAlta,
                            FECHA_FACTURA = solRED.FECHA_FACTURA,
                            FECHA_OPERACION = solRED.FECHA_OPERACION,
                            ID_EMPRESA_TARJETA_COMBUSTIBLE = 1, //SOLRED.
                            IMPORTE = solRED.IMPORTE,
                            IMP_TOTAL = solRED.IMP_TOTAL,
                            IVA = solRED.IVA,
                            KILOMETROS = solRED.KILOMETROS,
                            KmsCiclo = solRED.KmsCiclo,
                            MATRICULA = solRED.MATRICULA,
                            NUM_DOCUMENTO = "",
                            NUM_LITROS = solRED.NUM_LITROS,
                            Sociedad = solRED.Sociedad,
                            TRIMESTRE = solRED.TRIMESTRE,
                            NOMBRE_ARCHIVO_IMPORTACION = "automatico",
                        };


                        unitOfWork.RepositoryT_G_TARJETA_COMBUSTIBLE.Insert(datos);
                        unitOfWork.Commit();
                    }

                }

            }

            catch (Exception ex)
            {
                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"<ImportaDatosSOLREDFromCorea> MATRÍCULA {Paso}. {Environment.NewLine} {ex.Message}");
            }

            return true;
        }


        private static int? GetCiaSeguros(int? cia)
        {
            int? returnValue = null;
            switch (cia)
            {
                case 11:
                    returnValue = 18;
                    break;
                case 10:
                    returnValue = 17;
                    break;
                case 9:
                    returnValue = 16;
                    break;
                case 4:
                    returnValue = 12;
                    break;
                case 8:
                    returnValue = 4;
                    break;
                case 7:
                    returnValue = 15;
                    break;
                case 6:
                    returnValue = 14;
                    break;
                case 5:
                    returnValue = 13;
                    break;
                case 3:
                    returnValue = 11;
                    break;
                case 2:
                    returnValue = 10;
                    break;
                case 1:
                    returnValue = 9;
                    break;
                default:
                    returnValue = null;
                    break;

            }

            return returnValue;
        }

        private static string GetDelegacion(string deleg)
        {
            string returnValue = null;
            int Out = 0;

            if (!string.IsNullOrEmpty(deleg))
            {
                string del = deleg.Trim();
                if (del.Length == 2)
                {
                    returnValue = del;
                }
                else if (del.Length == 6)
                {
                    if (int.TryParse(del, out Out) || int.TryParse(del.Substring(0, 3), out Out))
                    {
                        returnValue = del.Substring(3,2);
                    }
                }
            }

            return returnValue;
        }


        private static int? GetMarca(string generico)
        {
            int? returnValue = null;

            MtoGenericoTiposModels _generico = new MtoTiposGeneralesService().GetMtoGeneralTiposDatatable((int)EnumMtoTiposGenerales.Marcas)
                                                        .Where(x => x.Descripcion.ToUpper() == generico).FirstOrDefault();
            if (_generico != null)
            {
                returnValue = _generico.ID_Tipo;
            }

            return returnValue;
        }

        private static int? GetModelo(string generico, int? marca)
        {
            int? returnValue = null;

            MtoGenericoTiposModels _generico = null;

            if (marca != null)
            {
                _generico = new MtoTiposGeneralesService().GetMtoGeneralTiposDatatable((int)EnumMtoTiposGenerales.Modelos, true, 0, Convert.ToInt32(marca))
                                                 .Where(x => x.Descripcion.ToUpper() == generico).FirstOrDefault();
            }
            if (_generico != null)
            {
                returnValue = _generico.ID_Tipo;
            }

            return returnValue;
        }

        private static int? GetCarburante(string generico)
        {
            int? returnValue = null;

            if (generico != null)
            {
                if (generico.ToUpper() == "G")
                {
                    generico = "GASOLINA";
                }
                else if (generico.ToUpper() == "D")
                {
                    generico = "GASOIL";
                }

                MtoGenericoTiposModels _generico = new MtoTiposGeneralesService().GetMtoGeneralTiposDatatable((int)EnumMtoTiposGenerales.Carburante)
                                                            .Where(x => x.Descripcion.ToUpper() == generico).FirstOrDefault();
                if (_generico != null)
                {
                    returnValue = _generico.ID_Tipo;
                }
            }

            return returnValue;
        }


    }

}
