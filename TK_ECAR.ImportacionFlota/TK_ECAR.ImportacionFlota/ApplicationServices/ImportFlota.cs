using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK_ECAR.Domain;
using System.Transactions;
using TK_ECAR.ImportacionFlota.Global;
using TK_ECAR.Infraestructure;
using TK_ECAR.Domain.Specifications;

namespace TK_ECAR.ImportacionFlota.ApplicationServices
{
    public class ImportFlota
    {
        //Definimos evento para pasar el CPID y la Fecha de inicio y así poder crear esas carpetas.
        public delegate void ProcessingImportFlotaEventHandler(int lineProsessing, int TotalLinesToProcess);
        //Evento de procesado de archivo.
        public event ProcessingImportFlotaEventHandler EventProcesingImportFlota;
        
        public delegate void ErrorImportFlotaEventHandler(int lineaProsessing, string msgError);
        public event ErrorImportFlotaEventHandler EventErrorImportFlota;

        public delegate void FinishedImportFlotaEventHandler(int TotalLinesToProcess, int TotalLinesProcessedOK, int TotalLinesProcessedERROR);
        public event FinishedImportFlotaEventHandler EventFinishedImportFlota;

        public void startImport(string fileToImport, int empresa)
        {
            var paso = string.Empty;
            int FilaProcessing = 0;
            int TotalFilas = 0;
            int TotalFilasProcesadasOK = 0;
            int TotalFilasProcesadasExistentes = 0;
            int TotalFilasProcesadasERROR = 0;
            try
            {
                var fileinfo = new FileInfo(fileToImport);

                using (var scope = new TransactionScope())
                {
                    using (var unitOfWork = new UnitOfWork())
                    {
                        using (ExcelPackage package = new ExcelPackage(fileinfo))
                        {
                            ExcelWorksheet ws = null;
                            ws = package.Workbook.Worksheets[1];
                            TotalFilas = ws.Dimension.End.Row;
                            for (int r = 2; r <= ws.Dimension.End.Row; r++)
                            {
                                FilaProcessing = r;
                                try
                                {
                                    paso = $"<ESTABLECIENDO DATO> FILA -> {r}";
                                    ECAR_Datos_Vehiculo datosVehiculo = new ECAR_Datos_Vehiculo
                                    {
                                        Sociedad = empresa,
                                        Matricula = DevuelveTexto(ws.Cells[r, GlobalApp.COL_MATRICULA].Value),
                                        FechaImportacion = DateTime.Now,
                                        CC = DevuelveTexto(ws.Cells[r, GlobalApp.COL_CENTRO_COSTE].Value),
                                        Delegacion = DevuelveDelegacion(DevuelveTexto(ws.Cells[r, GlobalApp.COL_CENTRO_COSTE].Value)),
                                        Responsable = DevuelveTexto(ws.Cells[r, GlobalApp.COL_RESPONSABLE].Value),
                                        LugarEntrega = DevuelveTexto(ws.Cells[r, GlobalApp.COL_LUGAR_ENTREGA].Value),
                                        PrioridadEntrega = DevuelveTexto(ws.Cells[r, GlobalApp.COL_PRIORIDAD_ENTREGA].Value),
                                        FechaRenovacion = DevuelveFecha(ws.Cells[r, GlobalApp.COL_FECHA_RENOVACION].Value),
                                        Fecha_Incorporacion = DevuelveFecha(ws.Cells[r, GlobalApp.COL_FECHA_RECOGIDA].Value),
                                        Fecha_Alta = DevuelveFecha(ws.Cells[r, GlobalApp.COL_FECHA_ALTA].Value),
                                        Num_Contrato = DevuelveTexto(ws.Cells[r, GlobalApp.COL_CONTRATO].Value),
                                        Cuotas = DevuelveInt(ws.Cells[r, GlobalApp.COL_CUOTAS].Value),
                                        Km_Totales = DevuelveInt(ws.Cells[r, GlobalApp.COL_KM_TOTALES].Value),
                                        Coef_exceso = DevuelveInt(ws.Cells[r, GlobalApp.COL_KM_TOTALES].Value),
                                        Abono = DevuelveDouble(ws.Cells[r, GlobalApp.COL_COSTE_KM_EXC_DEF].Value),
                                        Cargo = DevuelveDouble(ws.Cells[r, GlobalApp.COL_COSTE_KM_EXC_DEF].Value),
                                        Tipo_Vehiculo = DevuelveInt(ws.Cells[r, GlobalApp.COL_ID_TIPO_VEHICULO].Value),
                                        Marca = DevuelveInt(ws.Cells[r, GlobalApp.COL_ID_MARCA_VEHICULO].Value),
                                        Modelo = DevuelveInt(ws.Cells[r, GlobalApp.COL_ID_MODELO].Value),
                                        Extras = DevuelveTexto(ws.Cells[r, GlobalApp.COL_EXTRAS].Value),
                                        Equipamiento = DevuelveTexto(ws.Cells[r, GlobalApp.COL_EQUIPAMIENTO].Value),
                                        Num_Bastidor = DevuelveTexto(ws.Cells[r, GlobalApp.COL_BASTIDOR].Value),
                                        FechaMatriculacion = DevuelveFecha(ws.Cells[r, GlobalApp.COL_FECHA_MATRICULACION].Value),
                                        Veh_sustituido = DevuelveTexto(ws.Cells[r, GlobalApp.COL_MATRICULA_SUSTITUYE].Value),
                                        Poliza_Seguro = DevuelveTexto(ws.Cells[r, GlobalApp.COL_POLIZA].Value),
                                        Importe_Seguro = DevuelveDouble(ws.Cells[r, GlobalApp.COL_IMPORTE_SEGURO].Value),
                                        Vto_Seguro = DevuelveFecha(ws.Cells[r, GlobalApp.COL_FECHA_MATRICULACION].Value),
                                        Tipo_Seguro = DevuelveInt(ws.Cells[r, GlobalApp.COL_ID_TIPO_SEGURO].Value),
                                    };

                                    var numEmpleado = DevuelveTexto(ws.Cells[r, GlobalApp.COL_NUM_EMPLEADO].Value);
                                    if (!string.IsNullOrEmpty(numEmpleado))
                                    {
                                        paso = $"<ESTABLECIENDO CONDUCTOR> FILA -> {r}";
                                        var conductor = GetConductorByNumEmpleado(numEmpleado);
                                        if (conductor != null)
                                        {
                                            datosVehiculo.Conductor = conductor.Cod_Conductor;
                                        }
                                        else
                                        {
                                            paso = $"<GENERANDO CONDUCTOR POR Nº EMPLEADO ERRÓNEO> FILA -> {r}";
                                            datosVehiculo.Conductor = GeneraConductor(unitOfWork, ws, r);
                                        }
                                    }
                                    else
                                    {
                                        paso = $"<GENERANDO CONDUCTOR> FILA -> {r}";
                                        datosVehiculo.Conductor = GeneraConductor(unitOfWork, ws, r);
                                    }
                                    //if (datosVehiculo.Conductor == null && !string.IsNullOrEmpty(datosVehiculo.Veh_sustituido))
                                    //{
                                    //    datosVehiculo.Conductor = GetCodigoConductorByMatricula(datosVehiculo.Veh_sustituido);
                                    //}

                                    paso = $"<GENERANDO VEHÍCULO> FILA -> {r}";
                                    if (GeneraVehiculo(unitOfWork, datosVehiculo))
                                    {
                                        paso = $"<GENERANDO ITV> FILA -> {r}";
                                        //Calcular la fecha de la primera ITV.
                                        GeneraITV(unitOfWork, datosVehiculo);

                                        paso = $"<COMMIT> FILA -> {r}";
                                        unitOfWork.Commit();

                                        paso = $"<TRANSACTION> FILA -> {r}";
                                        scope.Complete();

                                        TotalFilasProcesadasOK++;
                                    }
                                    else
                                    {
                                        paso = $"<GENERANDO VEHÍCULO> VEHÍCULO YA EXISTENTE. FILA -> {r}";
                                        TotalFilasProcesadasExistentes++;
                                    }

                                    OnProcessImportFlotaEventHandler(FilaProcessing, TotalFilas);
                                }
                                catch (Exception ex)
                                {
                                    TotalFilasProcesadasERROR++;
                                    GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"<startImport> <FICHERO> {fileToImport}, FILA {FilaProcessing}, PASO {paso}. {Environment.NewLine}{ex.Message}");
                                    OnErrorImportFlotaEventHandler(FilaProcessing, $"<startImport> <FICHERO> {fileToImport}, FILA {FilaProcessing}, PASO {paso}. {Environment.NewLine}{ex.Message}");
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"<startImport> <FICHERO> {fileToImport}, INICIO. {Environment.NewLine}{ex.Message}");
                OnErrorImportFlotaEventHandler(FilaProcessing, $"<startImport> <FICHERO> {fileToImport}, INICIO. {Environment.NewLine}{ex.Message}");
            }

            finally
            {
                OnFinishedImportFlotaEventHandler(TotalFilas, TotalFilasProcesadasOK, TotalFilasProcesadasERROR);
            }
        }

        private static string DevuelveTexto(object valorCelda)
        {
            string valorReturn = null;

            if (valorCelda != null)
            {
                valorReturn = Convert.ToString(valorCelda);
            }

            return valorReturn;
        }

        private static string DevuelveDelegacion(string ceco)
        {
            string valorReturn = null;

            if (!string.IsNullOrEmpty(ceco))
            {
                valorReturn = ceco.Substring(3, 2);
            }

            return valorReturn;
        }

        private static DateTime? DevuelveFecha(object fecha)
        {
            DateTime? valorReturn = null;

            if (fecha != null)
            {
                valorReturn = Convert.ToDateTime(fecha);
            }

            return valorReturn;
        }

        private static int? DevuelveInt(object valorCelda)
        {
            int? valorReturn = null;

            if (valorCelda != null)
            {
                valorReturn = Convert.ToInt32(valorCelda);
            }

            return valorReturn;
        }

        private static double? DevuelveDouble(object valorCelda)
        {
            double? valorReturn = null;

            if (valorCelda != null)
            {
                valorReturn = Convert.ToDouble(valorCelda);
            }

            return valorReturn;
        }

        private static ECAR_Datos_Conductor GetConductorByNumEmpleado(string numEmpleado)
        {
            ECAR_Datos_Conductor valorRreturn = new ECAR_Datos_Conductor();
            using (var unitOfWork = new UnitOfWork())
            {
                valorRreturn = unitOfWork.RepositoryECAR_Datos_Conductor.Fetch().Where(x => x.Num_Empleado == numEmpleado).FirstOrDefault();
            }

            return valorRreturn;
        }

        private static int? GetCodigoConductorByMatricula(string matricula)
        {
            int? valorRreturn = null;
            using (var unitOfWork = new UnitOfWork())
            {
                var conductor = unitOfWork.RepositoryECAR_Datos_Vehiculo.Fetch().Where(x => x.Matricula == matricula).FirstOrDefault();
                if (conductor != null)
                {
                    if (conductor.Conductor != 0)
                    {
                        valorRreturn = conductor.Conductor;
                    }
                }
            }

            return valorRreturn;
        }


        private static bool GeneraVehiculo(UnitOfWork unitOfWork, ECAR_Datos_Vehiculo datosVehiculo)
        {
            bool valorReturn = true;

            ECAR_Datos_VehiculoSpecification spec = new ECAR_Datos_VehiculoSpecification
            {
                Sociedad = datosVehiculo.Sociedad,
                Matricula = datosVehiculo.Matricula,
            };


            var vehiculo = unitOfWork.RepositoryECAR_Datos_Vehiculo.Where(spec).FirstOrDefault();

            if (vehiculo == null)
            {
                unitOfWork.RepositoryECAR_Datos_Vehiculo.Insert(datosVehiculo);
                unitOfWork.Commit();
            }
            else
            {
                valorReturn = false;
            }

            return valorReturn;
        }

        private static void GeneraITV(UnitOfWork unitOfWork, ECAR_Datos_Vehiculo datosVehiculo)
        {
            if (datosVehiculo.FechaMatriculacion != null && datosVehiculo.Tipo_Vehiculo != null && !string.IsNullOrEmpty(datosVehiculo.Matricula))
            {
                var años_ITV = GlobalApp.AÑOS_ITV_RESTO;
                if (datosVehiculo.Tipo_Vehiculo == 2)
                {
                    años_ITV = GlobalApp.AÑOS_ITV_FURGONETA;
                }
                ECAR_Datos_ITV vehiculo_ITV = new ECAR_Datos_ITV
                {
                    Falta = DateTime.Now,
                    Matricula = datosVehiculo.Matricula,
                    Vto_ITV = datosVehiculo.FechaMatriculacion.Value.AddYears(años_ITV),
                    Otros = "AUTOMÁTICA",
                };

                unitOfWork.RepositoryECAR_Datos_ITV.Insert(vehiculo_ITV);

                unitOfWork.Commit();
            }
        }

        private static int GeneraConductor(UnitOfWork unitOfWork, ExcelWorksheet ws, int r)
        {
            ECAR_Datos_Conductor conductor = new ECAR_Datos_Conductor
            {
                Nombre = DevuelveTexto(ws.Cells[r, GlobalApp.COL_NOMBRE_CONDUCTOR].Value),
                Apellidos = DevuelveTexto(ws.Cells[r, GlobalApp.COL_APELLIDOS_CONDUCTOR].Value),
                Direccion = DevuelveTexto(ws.Cells[r, GlobalApp.COL_CALLE_POBLACION].Value),
                Tlf = DevuelveTexto(ws.Cells[r, GlobalApp.COL_TELEFONO_CONDUCTOR].Value),
                DNI = DevuelveTexto(ws.Cells[r, GlobalApp.COL_DNI].Value),
                Poblacion = DevuelveTexto(ws.Cells[r, GlobalApp.COL_POBLACION].Value),
                Cod_Postal = DevuelveTexto(ws.Cells[r, GlobalApp.COL_CP].Value),
                Fecha_Nacimiento = DevuelveFecha(ws.Cells[r, GlobalApp.COL_FECHA_NACIMIENTO].Value),
                Fecha_Carnet = DevuelveFecha(ws.Cells[r, GlobalApp.COL_FECHA_EXPEDICION].Value),
                PendienteDefinir = true,
            };

            unitOfWork.RepositoryECAR_Datos_Conductor.Insert(conductor);

            unitOfWork.Commit();

            return conductor.Cod_Conductor;
        }

        #region eventos
        private void OnProcessImportFlotaEventHandler(int lineProsessing, int TotalLinesToProcess)
        {
            EventProcesingImportFlota?.Invoke(lineProsessing, TotalLinesToProcess);
        }

        private void OnErrorImportFlotaEventHandler(int lineProsessing, string msgError)
        {
            EventErrorImportFlota?.Invoke(lineProsessing, msgError);
        }

        private void OnFinishedImportFlotaEventHandler(int TotalLinesToProcess, int TotalLinesProcessedOK, int TotalLinesProcessedERROR)
        {
            EventFinishedImportFlota?.Invoke(TotalLinesToProcess, TotalLinesProcessedOK, TotalLinesProcessedERROR);
        }
        #endregion eventos


    }
}
