using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using TK_ECAR.Domain;
using System.Transactions;
using TK_ECAR.Infraestructure;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Utils;
using TK_ECAR.Content.resources;
using System.Web;
using TK_ECAR.Framework;
using System.Threading;
using TK_ECAR.Models;
using TK_ECAR.Application_Services;

namespace TK_ECAR.ImportacionFlota.ApplicationServices
{
    public class ImportarFlotaService
    {
        //Definimos evento.
        public delegate void ProcessingImportFlotaEventHandler(int lineProsessing, int TotalLinesToProcess, string msg);
        //Evento de procesado de archivo.
        public event ProcessingImportFlotaEventHandler EventProcesingImportFlota;
        
        public delegate void ErrorImportFlotaEventHandler(int lineaProsessing, string msgError);
        public event ErrorImportFlotaEventHandler EventErrorImportFlota;

        public delegate void IncidenciaImportFlotaEventHandler(int lineaProsessing, string matricula, EnumTipoLineaImportacion tipoIncidencia);
        public event IncidenciaImportFlotaEventHandler EventIncidenciaImportFlota;

        public delegate void FinishedImportFlotaEventHandler(int TotalLinesToProcess, int TotalLinesProcessedOK, int TotalFilasProcesadasIncidencia, int TotalLinesProcessedERROR);
        public event FinishedImportFlotaEventHandler EventFinishedImportFlota;

        const int COL_OBSERVACIONES = 1;
        const int COL_NOMBRE_DELEGACION = 2;
        const int COL_CENTRO_COSTE = 3;
        const int COL_CUENTA = 4; 
        const int COL_DIRECCION = 5;
        const int COL_RESPONSABLE = 6;
        const int COL_MAIL = 7;
        const int COL_TELEFONO = 8;
        const int COL_LUGAR_ENTREGA = 9;
        const int COL_CONCESIONARIO = 10;
        const int COL_DIRECCION_CONCESIONARIO = 11;
        const int COL_PRIORIDAD_ENTREGA = 12;
        const int COL_FECHA_RENOVACION = 13;
        const int COL_FECHA_RECOGIDA = 14;
        const int COL_FECHA_ALTA = 15;
        const int COL_CONTRATO = 16;
        const int COL_CUOTAS = 17;
        const int COL_FECHA_VTO_CONTRATO = 18;
        const int COL_KM_TOTALES = 19;
        const int COL_COSTE_KM_EXC_DEF = 20; //VIENE SEPARADO POR "/"
        const int COL_ID_TIPO_VEHICULO = 21;
        const int COL_TIPO_VEHICULO = 22;
        const int COL_ID_MARCA_VEHICULO = 23;
        const int COL_ID_MODELO = 24;
        const int COL_MODELO = 25;
        const int COL_EXTRAS = 26;
        const int COL_EQUIPAMIENTO = 27;
        const int COL_MATRICULA = 28;

        //const int COL_FECHA_ALTA_NOVALE = 28;
        //const int COL_CONTRATO_NOVALE = 29;
        //const int COL_FECHA_VTO = 30;
        //const int COL_IMPORTE_SEGURO = 31;

        //const int COL_FECHA_MATRICULACION = 32;
        const int COL_BASTIDOR = 29;
        const int COL_FECHA_MATRICULACION = 30;
        const int COL_NUM_EMPLEADO = 31;
        const int COL_NOMBRE_APELLIDOS_CONDUCTOR = 32;
        const int COL_DNI = 33;
        const int COL_MATRICULA_SUSTITUYE = 34;
        const int COL_POLIZA = 35;
        const int COL_IMPORTE_SEGURO = 36;
        const int COL_FECHA_VTO_SEGURO = 37;
        const int COL_ID_TIPO_SEGURO = 38;
        const int COL_TIPO_SEGURO = 39;

        //const int COL_CALLE_POBLACION = 35;
        //const int COL_POBLACION = 36;
        //const int COL_CP = 37;
        //const int COL_FECHA_NACIMIENTO = 38;
        //const int COL_FECHA_EXPEDICION = 39;

        //const int COL_TIPO_SEGURO = 45;
        //const int COL_INCIDENCIA = 46; //ESTA SE AÑADIRÁ PARA PONER UN COMENTARIO SI HAY INCIDENCIA.

        const int AÑOS_ITV_FURGONETA = 2;
        const int AÑOS_ITV_RESTO = 4;

        string columnaEstablecida = string.Empty;

        public void startImport(ImportacionFlotaModels modelo, string user)
        {
            string fileToImport = string.Empty;
            int empresa = 0;
            var paso = string.Empty;
            int FilaProcessing = 0;
            int TotalFilas = 0;
            int TotalFilasProcesadasOK = 0;
            int TotalFilasProcesadasExistentes = 0;
            int TotalFilasProcesadasIncidencia = 0;
            int TotalFilasProcesadasERROR = 0;
            string msg = string.Empty;

            FileStream templateDocumentStream = null;
            ExcelPackage package = null;

            var nombreArchivo = FileUtilities.GetFileName(modelo.FileToImport);
            fileToImport = $"{Global.PathToPathToImportFilesFlota}{nombreArchivo}";
            empresa = Convert.ToInt32(modelo.IDEmpresa);

            try
            {
                //var fileinfo = new FileInfo(HttpContext.Current.Server.MapPath(fileToImport));

                var fileinfo = HttpContext.Current.Server.MapPath(fileToImport);
                
                templateDocumentStream = File.OpenRead(fileinfo);
                templateDocumentStream.Position = 0;
                
                package = new ExcelPackage(templateDocumentStream);

                ExcelWorksheet ws = null;
                ws = package.Workbook.Worksheets[1];
                TotalFilas = ws.Dimension.End.Row - 1; //Quitamos la primera fila que es cabecera.
                for (int r = 2; r <= ws.Dimension.End.Row; r++)
                {
                    var hacerComplete = true;
                    bool incidencia = false;
                    columnaEstablecida = string.Empty;

                    try
                    {
                        using (var scope = new TransactionScope())
                        {
                            using (var unitOfWork = new UnitOfWork())
                            {
                                FilaProcessing = r;
                                msg = TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_EstableciendoDatoVehiculo");
                                paso = $"<ESTABLECIENDO DATO> FILA -> {r}";
                                OnProcessImportFlotaEventHandler(FilaProcessing, TotalFilas, msg);

                                ECAR_Datos_Vehiculo datosVehiculo = new ECAR_Datos_Vehiculo();

                                columnaEstablecida = "EMPRESA";
                                datosVehiculo.Sociedad = empresa;
                                columnaEstablecida = "EMPRESA LEASING";
                                datosVehiculo.EmpresaLeasing = modelo.IDEmpresaLeasing;
                                columnaEstablecida = "MATRICULA";
                                datosVehiculo.Matricula = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_MATRICULA].Value,true,true, (empresa != Constants.CODIGO_EMPRESA_PORTUGAL));
                                if (!string.IsNullOrEmpty(datosVehiculo.Matricula))
                                { 
                                    datosVehiculo.FechaImportacion = DateTime.Now;
                                    columnaEstablecida = "CENTRO_COSTE";
                                    datosVehiculo.CC = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_CENTRO_COSTE].Value);
                                    columnaEstablecida = "DELEGACION_CENTRO_COSTE";
                                    datosVehiculo.Delegacion = DevuelveDelegacion(Global.DevuelveTextoFromExcel(ws.Cells[r, COL_CENTRO_COSTE].Value));
                                    columnaEstablecida = "RESPONSABLE";
                                    datosVehiculo.Responsable = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_RESPONSABLE].Value);
                                    columnaEstablecida = "LUGAR_ENTREGA";
                                    datosVehiculo.LugarEntrega = ""; // Global.DevuelveTextoFromExcel(ws.Cells[r, COL_LUGAR_ENTREGA].Value);
                                    columnaEstablecida = "PRIORIDAD_ENTREGA";
                                    datosVehiculo.PrioridadEntrega = ""; // Global.DevuelveTextoFromExcel(ws.Cells[r, COL_PRIORIDAD_ENTREGA].Value);
                                    columnaEstablecida = "FECHA_RENOVACION";
                                    datosVehiculo.FechaRenovacion = null; // Global.DevuelveFechaFromExcel(ws.Cells[r, COL_FECHA_RENOVACION].Value);
                                    columnaEstablecida = "FECHA_RECOGIDA";
                                    datosVehiculo.Fecha_Incorporacion = Global.DevuelveFechaFromExcel(ws.Cells[r, COL_FECHA_RECOGIDA].Value);
                                    columnaEstablecida = "FECHA_ALTA";
                                    datosVehiculo.Fecha_Alta = Global.DevuelveFechaFromExcel(ws.Cells[r, COL_FECHA_ALTA].Value);
                                    columnaEstablecida = "CONTRATO";
                                    datosVehiculo.Num_Contrato = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_CONTRATO].Value);
                                    columnaEstablecida = "CUOTAS";
                                    datosVehiculo.Cuotas = Global.DevuelveIntFromExcel(ws.Cells[r, COL_CUOTAS].Value);

                                    columnaEstablecida = "COL_FECHA_VTO_CONTRATO";
                                    datosVehiculo.Fecha_Vto_Contrato = Global.DevuelveFechaFromExcel(ws.Cells[r, COL_FECHA_VTO_CONTRATO].Value);

                                    columnaEstablecida = "VTO_SEGURO";
                                    datosVehiculo.Vto_Seguro = Global.DevuelveFechaFromExcel(ws.Cells[r, COL_FECHA_VTO_SEGURO].Value); //datosVehiculo.Fecha_Alta.Value.AddMonths(12);

                                    columnaEstablecida = "KM_TOTALES";
                                    datosVehiculo.Km_Totales = Global.DevuelveIntFromExcel(ws.Cells[r, COL_KM_TOTALES].Value);
                                    columnaEstablecida = "COEF_EXCESO_COSTE_KM_TOTALES";
                                    datosVehiculo.Coef_exceso = Global.DevuelveIntFromExcel(ws.Cells[r, COL_KM_TOTALES].Value);
                                    columnaEstablecida = "KM_EXC_DEF";
                                    datosVehiculo.Abono = Global.DevuelveDoubleFromExcel(ws.Cells[r, COL_COSTE_KM_EXC_DEF].Value);
                                    columnaEstablecida = "KM_EXC_DEF";
                                    datosVehiculo.Cargo = Global.DevuelveDoubleFromExcel(ws.Cells[r, COL_COSTE_KM_EXC_DEF].Value);
                                    columnaEstablecida = "TIPO_VEHICULO";
                                    datosVehiculo.Tipo_Vehiculo = Global.DevuelveIntFromExcel(ws.Cells[r, COL_ID_TIPO_VEHICULO].Value);
                                    columnaEstablecida = "MARCA_VEHICULO";
                                    datosVehiculo.Marca = Global.DevuelveIntFromExcel(ws.Cells[r, COL_ID_MARCA_VEHICULO].Value);
                                    columnaEstablecida = "MODELO";
                                    datosVehiculo.Modelo = Global.DevuelveIntFromExcel(ws.Cells[r, COL_ID_MODELO].Value);
                                    columnaEstablecida = "EXTRAS";
                                    datosVehiculo.Extras = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_EXTRAS].Value);
                                    columnaEstablecida = "EQUIPAMIENTO";
                                    datosVehiculo.Equipamiento = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_EQUIPAMIENTO].Value);
                                    columnaEstablecida = "BASTIDOR";
                                    datosVehiculo.Num_Bastidor = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_BASTIDOR].Value);
                                    columnaEstablecida = "FECHA_MATRICULACION";
                                    datosVehiculo.FechaMatriculacion = Global.DevuelveFechaFromExcel(ws.Cells[r, COL_FECHA_MATRICULACION].Value);
                                    columnaEstablecida = "MATRICULA_SUSTITUYE";
                                    datosVehiculo.Veh_sustituido = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_MATRICULA_SUSTITUYE].Value);
                                    columnaEstablecida = "POLIZA";
                                    datosVehiculo.Poliza_Seguro = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_POLIZA].Value);
                                    columnaEstablecida = "IMPORTE_SEGURO";
                                    datosVehiculo.Importe_Seguro = Global.DevuelveDoubleFromExcel(ws.Cells[r, COL_IMPORTE_SEGURO].Value);
                                    columnaEstablecida = "TIPO_SEGURO";
                                    datosVehiculo.Tipo_Seguro = Global.DevuelveIntFromExcel(ws.Cells[r, COL_ID_TIPO_SEGURO].Value);
                                    columnaEstablecida = "";
                                    datosVehiculo.IdentificadorImportacion = modelo.IdentificadorImportacion;
                                    datosVehiculo.UsuarioImportacion = user;
                                    datosVehiculo.UsuarioAlta = user;
                                    datosVehiculo.Baja = false;
                                    datosVehiculo.Falta = DateTime.Now;
                                    datosVehiculo.NOMBRE_ARCHIVO_IMPORTACION = nombreArchivo;

                                    datosVehiculo.Conductor = null;

                                    if (!ExisteVehiculo(unitOfWork, datosVehiculo))
                                    {
                                        var numEmpleado = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_NUM_EMPLEADO].Value, true);
                                        if (!string.IsNullOrEmpty(numEmpleado))
                                        {
                                            paso = $"<ESTABLECIENDO CONDUCTOR> FILA -> {r}";
                                            var conductor = GetConductorByNumEmpleado(numEmpleado);
                                            if (conductor == null)
                                            {
                                                var _nif = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_DNI].Value);
                                                if (!string.IsNullOrEmpty(_nif))
                                                {
                                                    conductor = GetConductorByNIF(_nif);
                                                    if (conductor != null)
                                                    {
                                                        datosVehiculo.Conductor = conductor.Cod_Conductor;
                                                    }
                                                    else
                                                    {

                                                        //paso = $"<GENERANDO CONDUCTOR POR Nº EMPLEADO ERRÓNEO> FILA -> {r}";
                                                        //datosVehiculo.Conductor = GeneraConductor(unitOfWork, ws, r);
                                                        incidencia = true;
                                                        msg = TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_IncidenciaConductorNoEncontrado");
                                                        OnEventIncidenciaImportFlota(FilaProcessing, datosVehiculo.Matricula, EnumTipoLineaImportacion.ConductorNoEncontrado);
                                                    }
                                                }
                                                else
                                                {
                                                    //paso = $"<GENERANDO CONDUCTOR> FILA -> {r}";
                                                    //datosVehiculo.Conductor = GeneraConductor(unitOfWork, ws, r);
                                                    incidencia = true;
                                                    msg = TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_IncidenciaSinConductor");
                                                    OnEventIncidenciaImportFlota(FilaProcessing, datosVehiculo.Matricula, EnumTipoLineaImportacion.NoHayConductor);
                                                }
                                            }
                                            else
                                            {
                                                datosVehiculo.Conductor = conductor.Cod_Conductor;
                                            }
                                        }
                                        else
                                        {
                                            //paso = $"<GENERANDO CONDUCTOR> FILA -> {r}";
                                            //datosVehiculo.Conductor = GeneraConductor(unitOfWork, ws, r);
                                            incidencia = true;
                                            msg = TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_IncidenciaSinConductor");
                                            OnEventIncidenciaImportFlota(FilaProcessing, datosVehiculo.Matricula, EnumTipoLineaImportacion.NoHayConductor);
                                        }

                                        //GeneraVehiculo
                                        paso = $"<GENERANDO VEHÍCULO> FILA -> {r}";
                                        unitOfWork.RepositoryECAR_Datos_Vehiculo.Insert(datosVehiculo);
                                        unitOfWork.Commit();

                                        paso = $"<GENERANDO ITV> FILA -> {r}";
                                        //Calcular la fecha de la primera ITV.
                                        if (datosVehiculo.FechaMatriculacion != null && datosVehiculo.Tipo_Vehiculo != null && datosVehiculo.Tipo_Vehiculo != 0)
                                        {
                                            GeneraITV(unitOfWork, datosVehiculo);
                                        }
                                        else
                                        {
                                            if (datosVehiculo.FechaMatriculacion == null)
                                            {
                                                incidencia = true;
                                                OnEventIncidenciaImportFlota(FilaProcessing, datosVehiculo.Matricula, EnumTipoLineaImportacion.SinFechaMatriculacion);
                                            }
                                            else
                                            {
                                                incidencia = true;
                                                OnEventIncidenciaImportFlota(FilaProcessing, datosVehiculo.Matricula, EnumTipoLineaImportacion.SinTipoDeVehiculo);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        incidencia = true;
                                        msg = TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_IncidenciaVehiculoExistente");
                                        hacerComplete = false;
                                        OnEventIncidenciaImportFlota(FilaProcessing, datosVehiculo.Matricula, EnumTipoLineaImportacion.VehiculoExistente);
                                    }
                                }
                                else
                                {
                                    incidencia = true;
                                    msg = TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_IncidenciaMatriculaVacia");
                                    hacerComplete = false;
                                    OnEventIncidenciaImportFlota(FilaProcessing, datosVehiculo.Matricula, EnumTipoLineaImportacion.MatriculaVacia);
                                }
                                //OnProcessImportFlotaEventHandler(FilaProcessing, TotalFilas, msg);
                            }

                            if (hacerComplete)
                            {
                                paso = $"<TRANSACTION> FILA -> {r}";
                                scope.Complete();

                                if (!incidencia)
                                {
                                    TotalFilasProcesadasOK++;
                                }

                                msg = TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_VehiculoImportadoOK");
                            }

                            TotalFilasProcesadasExistentes++;

                            if (incidencia)
                            {
                                TotalFilasProcesadasIncidencia++;
                            }
                            OnProcessImportFlotaEventHandler(FilaProcessing, TotalFilas, msg);
                       }
                    }
                    catch (Exception ex)
                    {
                        var msgError = Global.GetMessageError(ex);

                        TotalFilasProcesadasExistentes++;
                        TotalFilasProcesadasERROR++;

                        Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"<startImport> <FICHERO> {fileToImport}, FILA {FilaProcessing}, PASO {paso}. {Environment.NewLine}{msgError}");
                        msg = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_ErrorVehiculo")} {FilaProcessing}"; 
                        msg = msg + $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_Error")} " + columnaEstablecida != "" ? $" COLUMNA [{columnaEstablecida}] " : "";
                        OnErrorImportFlotaEventHandler(FilaProcessing, $"[ERROR] {msg}. {Environment.NewLine}{msgError}");
                    }
                }
            }

            catch (Exception ex)
            {
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"<startImport> <FICHERO> {fileToImport}, INICIO. {Environment.NewLine}{ex.Message}");
                msg = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_ErrorGeneral")}";
                OnErrorImportFlotaEventHandler(FilaProcessing, $"[ERROR] {msg}. {Environment.NewLine}{Global.GetMessageError(ex)}");
            }

            finally
            {
                if (package != null)
                {
                    package.Dispose();
                }
                if (templateDocumentStream != null)
                {
                    templateDocumentStream.Close();
                    templateDocumentStream.Dispose();
                }
                
                OnFinishedImportFlotaEventHandler(TotalFilas, TotalFilasProcesadasOK, TotalFilasProcesadasIncidencia, TotalFilasProcesadasERROR);
            }
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

        private static ECAR_Datos_Conductor GetConductorByNumEmpleado(string numEmpleado)
        {
            ECAR_Datos_Conductor valorRreturn = new ECAR_Datos_Conductor();
            using (var unitOfWork = new UnitOfWork())
            {
                valorRreturn = unitOfWork.RepositoryECAR_Datos_Conductor.Fetch().Where(x => x.Num_Empleado == numEmpleado).FirstOrDefault();
            }

            return valorRreturn;
        }

        private static ECAR_Datos_Conductor GetConductorByNIF(string NIF)
        {
            ECAR_Datos_Conductor valorRreturn = new ECAR_Datos_Conductor();
            using (var unitOfWork = new UnitOfWork())
            {
                valorRreturn = unitOfWork.RepositoryECAR_Datos_Conductor.Fetch()
                    .Where(x => x.Num_Empleado.Replace(" ", "")
                                                .Replace(".", "")
                                                .Replace("_", "")
                                                .Replace("-", "") == NIF).FirstOrDefault();
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

        private static bool ExisteVehiculo(UnitOfWork unitOfWork, ECAR_Datos_Vehiculo datosVehiculo)
        {
            bool valorReturn = true;

            ECAR_Datos_VehiculoSpecification spec = new ECAR_Datos_VehiculoSpecification
            {
                Matricula = datosVehiculo.Matricula,
            };

            var vehiculo = unitOfWork.RepositoryECAR_Datos_Vehiculo.Where(spec).FirstOrDefault();

            if (vehiculo == null)
            {
                valorReturn = false;
            }

            return valorReturn;
        }



        private static void GeneraITV(UnitOfWork unitOfWork, ECAR_Datos_Vehiculo datosVehiculo)
        {
            if (datosVehiculo.FechaMatriculacion != null && datosVehiculo.Tipo_Vehiculo != null && !string.IsNullOrEmpty(datosVehiculo.Matricula))
            {
                var años_ITV = AÑOS_ITV_RESTO;
                if (datosVehiculo.Tipo_Vehiculo == 2)
                {
                    años_ITV = AÑOS_ITV_FURGONETA;
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

        private int GeneraConductor(UnitOfWork unitOfWork, ExcelWorksheet ws, int r)
        {
            ECAR_Datos_Conductor conductor = new ECAR_Datos_Conductor();
            //columnaEstablecida = "NOMBRE_CONDUCTOR";
            //conductor.Nombre = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_NOMBRE_CONDUCTOR].Value);
            //columnaEstablecida = "APELLIDOS_CONDUCTOR";
            //conductor.Apellidos = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_APELLIDOS_CONDUCTOR].Value);
            //columnaEstablecida = "CALLE_POBLACION";
            //conductor.Direccion = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_CALLE_POBLACION].Value);
            //columnaEstablecida = "TELEFONO_CONDUCTOR";
            //conductor.Tlf = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_TELEFONO_CONDUCTOR].Value);
            //columnaEstablecida = "DNI";
            //conductor.DNI = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_DNI].Value);
            //columnaEstablecida = "POBLACION";
            //conductor.Poblacion = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_POBLACION].Value);
            //columnaEstablecida = "CP";
            //conductor.Cod_Postal = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_CP].Value);
            //columnaEstablecida = "FECHA_NACIMIENTO";
            //conductor.Fecha_Nacimiento = Global.DevuelveFechaFromExcel(ws.Cells[r, COL_FECHA_NACIMIENTO].Value);
            //columnaEstablecida = "FECHA_EXPEDICION";
            //conductor.Fecha_Carnet = Global.DevuelveFechaFromExcel(ws.Cells[r, COL_FECHA_EXPEDICION].Value);
            //columnaEstablecida = "";
            //conductor.PendienteDefinir = true;

            //var matricula = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_MATRICULA].Value);

            //if (!string.IsNullOrEmpty(conductor.Nombre))
            //{
            //    unitOfWork.RepositoryECAR_Datos_Conductor.Insert(conductor);
            //    unitOfWork.Commit();

            //    OnEventIncidenciaImportFlota(r, matricula, EnumTipoLineaImportacion.ConductorNuevo);
            //}
            //else
            //{
            //    conductor.Cod_Conductor = -1;
            //    OnEventIncidenciaImportFlota(r, matricula, EnumTipoLineaImportacion.NoHayConductor);
            //}

            return conductor.Cod_Conductor;
        }

        #region eventos
        private void OnProcessImportFlotaEventHandler(int lineProsessing, int TotalLinesToProcess, string msg)
        {
            EventProcesingImportFlota?.Invoke(lineProsessing, TotalLinesToProcess, msg);
        }

        private void OnErrorImportFlotaEventHandler(int lineProsessing, string msgError)
        {
            EventErrorImportFlota?.Invoke(lineProsessing, msgError);
        }

        private void OnFinishedImportFlotaEventHandler(int TotalLinesToProcess, int TotalLinesProcessedOK, int TotalFilasProcesadasIncidencia, int TotalLinesProcessedERROR)
        {
            EventFinishedImportFlota?.Invoke(TotalLinesToProcess, TotalLinesProcessedOK, TotalFilasProcesadasIncidencia, TotalLinesProcessedERROR);
        }

        private void OnEventIncidenciaImportFlota(int lineProsessing, string matricula, EnumTipoLineaImportacion tipoIncidencia)
        {
            EventIncidenciaImportFlota?.Invoke(lineProsessing, matricula, tipoIncidencia);
        }
        #endregion eventos

        public bool ArchivoImportadoConAnterioridad(string nombreArchivo)
        {
            bool valorReturn = false;

            ECAR_Datos_VehiculoSpecification spec = new ECAR_Datos_VehiculoSpecification
            {
                NOMBRE_ARCHIVO_IMPORTACION = nombreArchivo,
            };

            using (var unitOfWork = new UnitOfWork())
            {
                valorReturn = unitOfWork.RepositoryECAR_Datos_Vehiculo.Where(spec).Count() > 0;
            }

            return valorReturn;
        }

    }
}
