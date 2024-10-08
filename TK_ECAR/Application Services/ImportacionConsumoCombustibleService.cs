using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml;
using TK_ECAR.Application_Services;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Framework;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;
using TK_ECAR.Models.Portugal;
using TK_ECAR.Utils;
using resourceView = TK_ECAR.Content.resources.TK_ECAR_Resource;

namespace TK_ECAR.PortugalImportacion.ApplicationServices
{
    public class ImportacionConsumoCombustibleService
    {
        #region Constantes
        private int COL_TRIMESTRE2 = 16;
        private int COL_FECHA_FACTURA = 11;
        private int CO_FECHA_OPERACION1 = 16; //COL_FECHA_OPERACION1 + COL_FECHA_OPERACION2 (DIA + HORA)
        private int COL_FECHA_OPERACION2 = 17;
        private int COL_MATRICULA = 13;
        private int COL_DES_PRODU = 22;
        private int COL_COD_PRODU = 29;
        private int COL_KILOMETROS = 21;
        private int COL_NUM_LITROS = 23;
        private int COL_IMPORTE = 25;
        private int COL_BONIF_TOTAL = 36;
        private int COL_IVA = 28;
        private int COL_IMP_TOTAL1 = 31;
        private int COL_IMP_TOTAL2 = 37;
        private int COL_KmsCiclo = -1;
        private int COL_DOCUMENTO = 15;
        private int COL_TARJETA = 12;

        private int COL_GALP_TRIMESTRE2 = 28;
        private int COL_GALP_FECHA_FACTURA = 26;
        private int CO_GALP_FECHA_OPERACION1 = 30; //COL_FECHA_OPERACION1 + COL_FECHA_OPERACION2
        private int COL_GALP_FECHA_OPERACION2 = 31;
        private int COL_GALP_MATRICULA = 21;
        private int COL_GALP_DES_PRODU = 42;
        private int COL_GALP_COD_PRODU = 41;
        private int COL_GALP_KILOMETROS = 37;
        private int COL_GALP_NUM_LITROS = 43;
        private int COL_GALP_IMPORTE = 50;
        private int COL_GALP_BONIF_TOTAL = 48;
        private int COL_GALP_IVA = 51;
        private int COL_GALP_IMP_TOTAL1 = 50;
        private int COL_GALP_IMP_TOTAL2 = 48;
        private int COL_GALP_KmsCiclo = 38;
        private int COL_GALP_DOCUMENTO = 40;
        private int COL_GALP_TARJETA = 4;

        #endregion

        #region Eventos Importación
        //Definimos evento.
        public delegate void ProcessingImportConsumoCombustibleEventHandler(int lineProsessing, int TotalLinesToProcess, string msg);
        //Evento de procesado de archivo.
        public event ProcessingImportConsumoCombustibleEventHandler EventProcessingImportConsumoCombustible;

        public delegate void SubProcessingImportConsumoCombustibleEventHandler(int lineProsessing, int TotalLinesToProcess, string msg);
        public event SubProcessingImportConsumoCombustibleEventHandler EventSubProcessingImportConsumoCombustible;

        public delegate void ErrorImportConsumoCombustibleEventHandler(int lineaProsessing, string msgError);
        public event ErrorImportConsumoCombustibleEventHandler EventErrorImportConsumoCombustible;

        public delegate void IncidenciaImportConsumoCombustibleEventHandler(int lineaProsessing, string matricula, EnumTipoLineaImportacion tipoIncidencia);
        public event IncidenciaImportConsumoCombustibleEventHandler EventIncidenciaImportConsumoCombustible;

        public delegate void FinishedImportConsumoCombustibleEventHandler(int TotalLinesToProcess, int TotalLinesProcessedOK, int TotalFilasProcesadasIncidencia, int TotalLinesProcessedERROR);
        public event FinishedImportConsumoCombustibleEventHandler EventFinishedImportConsumoCombustible;
        #endregion Eventos Importación

        //private const int VEHICULO_DIRECCION = 8;

        private string Paso = string.Empty;
        private int FilaProcessing = 0;
        private int TotalFilas = 0;
        private int TotalFilasProcesadasOK = 0;
        private int TotalFilasProcesadasExistentes = 0;
        private int TotalFilasProcesadasIncidencia = 0;
        private int TotalFilasProcesadasERROR = 0;

        #region ConsumoCombustible
        public bool ImportaConsumoCombustible(ImportacionDatosModels modelo)
        {
            //int COL_Sociedad = 8150;
            //VAR int COL_EJERCICIO = ;
            //int COL_TRIMESTRE1 = 27;
            var miFlotaService = new FlotaService();
            Dictionary<string, bool> MatriculasProcesadas = new Dictionary<string, bool>();

            if (modelo.IDEmpresa == Constants.CODIGO_EMPRESA_PORTUGAL)
            {
                COL_TRIMESTRE2 = COL_GALP_TRIMESTRE2;
                COL_FECHA_FACTURA = COL_GALP_FECHA_FACTURA;
                CO_FECHA_OPERACION1 = CO_GALP_FECHA_OPERACION1;
                COL_FECHA_OPERACION2 = COL_GALP_FECHA_OPERACION2;
                COL_MATRICULA = COL_GALP_MATRICULA;
                COL_DES_PRODU = COL_GALP_DES_PRODU;
                COL_COD_PRODU = COL_GALP_COD_PRODU;
                COL_KILOMETROS = COL_GALP_KILOMETROS;
                COL_NUM_LITROS = COL_GALP_NUM_LITROS;
                COL_IMPORTE = COL_GALP_IMPORTE;
                COL_BONIF_TOTAL = COL_GALP_BONIF_TOTAL;
                COL_IVA = COL_GALP_IVA;
                COL_IMP_TOTAL1 = COL_GALP_IMP_TOTAL1;
                COL_IMP_TOTAL2 = COL_GALP_IMP_TOTAL2;
                COL_KmsCiclo = COL_GALP_KmsCiclo;
                COL_DOCUMENTO = COL_GALP_DOCUMENTO;
                COL_TARJETA = COL_GALP_TARJETA;
            }

            var nombreFichero = FileUtilities.GetFileName(modelo.FileToImport);

            var fileToImport = HttpContext.Current.Server.MapPath($"{Global.PathToPathToImportFilesConsumoCombustible}{nombreFichero}");

            //var fileinfo = new FileInfo(fileToImport);

            OnProcessingImportConsumoCombustibleEventHandler(0, 0, $"{resourceView.ImportacionComienza} {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");

            if(Archivo_ConsumoCombustible_ImportadoConAnterioridad(nombreFichero))
            {
                OnProcessingImportConsumoCombustibleEventHandler(0, 0, $"{resourceView.ImportacionBorrandoDatosAnteriores} {nombreFichero}");
                BorrarDatosImportadosConAnterioridad(nombreFichero);
            }

            FileStream templateDocumentStream = null;
            ExcelPackage package = null;

            FilaProcessing = 0;

            try
            {
                Paso = $"ABRIENDO ARCHIVO {fileToImport}";

                templateDocumentStream = File.OpenRead(fileToImport);
                templateDocumentStream.Position = 0;

                package = new ExcelPackage(templateDocumentStream);

                ExcelWorksheet ws = null;
                ws = package.Workbook.Worksheets[1];
                TotalFilas = ws.Dimension.End.Row - 1; //Quitamos la primera fila que es cabecera.

                for (int r = 2; r <= ws.Dimension.End.Row; r++)
                {
                    try
                    {
                        Paso = "ESTABLECIENDO DATO";
                        FilaProcessing = r;

                        OnProcessingImportConsumoCombustibleEventHandler(FilaProcessing, TotalFilas, $"{resourceView.ImportacionProcesandoLínea}");

                        DatosCombustibleModels datosCombustible = new DatosCombustibleModels
                        {
                            BONIF_TOTAL = Convert.ToDecimal(ws.Cells[r, COL_BONIF_TOTAL].Value),
                            COD_PRODU = (string)ws.Cells[r, COL_COD_PRODU].Value,
                            DES_PRODU = (string)ws.Cells[r, COL_DES_PRODU].Value,
                            IMPORTE = Convert.ToDecimal(ws.Cells[r, COL_IMPORTE].Value),
                            IMP_TOTAL = Convert.ToDecimal(ws.Cells[r, COL_IMP_TOTAL1].Value) + Convert.ToDecimal(ws.Cells[r, COL_IMP_TOTAL2].Value),
                            IVA = Convert.ToDecimal(ws.Cells[r, COL_IVA].Value),
                            KILOMETROS = Convert.ToDecimal(ws.Cells[r, COL_KILOMETROS].Value),
                            MATRICULA = Global.DevuelveTextoFromExcel(ws.Cells[r, COL_MATRICULA].Value, true, true, (modelo.IDEmpresa != Constants.CODIGO_EMPRESA_PORTUGAL)) ,
                            NUM_LITROS = Convert.ToDecimal(ws.Cells[r, COL_NUM_LITROS].Value),
                            NUM_DOCUMENTO = (string)ws.Cells[r, COL_DOCUMENTO].Value,
                            COD_TARJETA = (string)ws.Cells[r, COL_TARJETA].Value,
                            NOMBRE_ARCHIVO_IMPORTACION = nombreFichero,
                            Sociedad = modelo.IDEmpresa,
                            IDEmpresaEmisoraTarjeta = modelo.IDEmpresaEmisora,
                        };

                        if (modelo.IDEmpresa != Constants.CODIGO_EMPRESA_PORTUGAL)
                        {
                            datosCombustible.KmsCiclo = 0;
                            datosCombustible.FECHA_OPERACION = Global.ConcatHoursAndMinutesToDate(Global.GetFormatedFechaFromString(ws.Cells[r, CO_FECHA_OPERACION1].Text), Convert.ToInt16(ws.Cells[r, COL_FECHA_OPERACION2].Text.Substring(0,2)), Convert.ToInt16(ws.Cells[r, COL_FECHA_OPERACION2].Text.Substring(2)));
                            datosCombustible.FECHA_FACTURA = Global.GetFormatedFechaFromString(ws.Cells[r, COL_FECHA_FACTURA].Text);
                        }
                        else
                        {
                            datosCombustible.FECHA_FACTURA = Convert.ToDateTime(ws.Cells[r, COL_FECHA_FACTURA].Text);
                            datosCombustible.FECHA_OPERACION = Convert.ToDateTime(ws.Cells[r, CO_FECHA_OPERACION1].Text + " " + ws.Cells[r, COL_FECHA_OPERACION2].Text);
                            datosCombustible.KmsCiclo = Convert.ToDecimal(ws.Cells[r, COL_KmsCiclo].Value);
                        }
                        

                        Paso = $"ESTABLECIENDO EJERCICIO TARJETA {datosCombustible.COD_TARJETA}";

                        datosCombustible.EJERCICIO = (datosCombustible.FECHA_OPERACION.Value.Month >= 10 ?
                                                (datosCombustible.FECHA_OPERACION.Value.Year.ToString() + "-" + (datosCombustible.FECHA_OPERACION.Value.Year + 1).ToString()) :
                                                ((datosCombustible.FECHA_OPERACION.Value.Year - 1).ToString() + "-" + datosCombustible.FECHA_OPERACION.Value.Year.ToString()));

                        var mes = datosCombustible.FECHA_FACTURA.Value.Month;
                        if (mes <= 3)
                        {
                            datosCombustible.TRIMESTRE = "1º";
                        }
                        else if (mes >= 10)
                        {
                            datosCombustible.TRIMESTRE = "4º";
                        }
                        else if (mes > 3 && mes <= 6)
                        {
                            datosCombustible.TRIMESTRE = "2º";
                        }
                        else
                        {
                            datosCombustible.TRIMESTRE = "3º";
                        }

                        Paso = $"COMPROBANDO MATRÍCULA {datosCombustible.MATRICULA} - {datosCombustible.COD_TARJETA}";
                        //Comprobamos que la matrícula exista.
                        if (!MatriculasProcesadas.ContainsKey(datosCombustible.MATRICULA))
                        {
                            if (!miFlotaService.VehiculoExistente(datosCombustible.MATRICULA))
                            {
                                MatriculasProcesadas.Add(datosCombustible.MATRICULA, false);
                                TotalFilasProcesadasIncidencia++;
                                OnIncidenciaImportConsumoCombustible(0, datosCombustible.MATRICULA, EnumTipoLineaImportacion.MatriculaInexistente);
                            }
                            else
                            {
                                MatriculasProcesadas.Add(datosCombustible.MATRICULA, true);
                            }
                        }

                        Paso = $"ACTUALIZANDO BBDD {datosCombustible.COD_TARJETA}";
                        ActualizaTarjetaCombustibleECAR(datosCombustible);

                        TotalFilasProcesadasOK++;
                    }
                    catch (Exception ex)
                    {
                        TotalFilasProcesadasERROR++;
                        OnErrorImportConsumoCombustibleEventHandler(FilaProcessing, $"[ERROR] {resourceView.ImportacionProcesandoLínea}{FilaProcessing} - {Paso}. {Environment.NewLine}{Global.GetMessageError(ex)}");
                        Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"<ImportaDatosTarjetaCombustible> <FICHERO> {fileToImport}, FILA {FilaProcessing}, PASO {Paso}, {Global.GetMessageError(ex)}");
                    }
                }
            }

            catch (Exception ex)
            {
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"<ImportaDatosTarjetaCombustible> <FICHERO> {fileToImport}, PASO {Paso}, {Global.GetMessageError(ex)}");
                OnErrorImportConsumoCombustibleEventHandler(FilaProcessing, $"[ERROR] {resourceView.ImportacionProcesandoLínea}{FilaProcessing} - {Paso}. {Environment.NewLine}{Global.GetMessageError(ex)}");
                return false;
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

                OnFinishedImportConsumoCombustibleEventHandler(TotalFilas, TotalFilasProcesadasOK, TotalFilasProcesadasIncidencia, TotalFilasProcesadasERROR);
            }


            return true;
        }

        private bool ActualizaTarjetaCombustibleECAR(DatosCombustibleModels datosCombustible)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                T_G_TARJETA_COMBUSTIBLE datos = new T_G_TARJETA_COMBUSTIBLE
                {
                    BONIF_TOTAL = (double)datosCombustible.BONIF_TOTAL,
                    COD_PRODU = datosCombustible.COD_PRODU,
                    DES_PRODU = datosCombustible.DES_PRODU,
                    EJERCICIO = datosCombustible.EJERCICIO,
                    FechaAlta = DateTime.Now,
                    FECHA_FACTURA = datosCombustible.FECHA_FACTURA,
                    FECHA_OPERACION = datosCombustible.FECHA_OPERACION,
                    IMPORTE = (double)datosCombustible.IMPORTE,
                    IMP_TOTAL = (double)datosCombustible.IMP_TOTAL,
                    IVA = (double)datosCombustible.IVA,
                    KILOMETROS = (double)datosCombustible.KILOMETROS,
                    KmsCiclo = (double)datosCombustible.KmsCiclo,
                    NUM_LITROS = (double)datosCombustible.NUM_LITROS,
                    MATRICULA = datosCombustible.MATRICULA,
                    Sociedad = datosCombustible.Sociedad,
                    ID_EMPRESA_TARJETA_COMBUSTIBLE = datosCombustible.IDEmpresaEmisoraTarjeta,
                    TRIMESTRE = datosCombustible.TRIMESTRE,
                    COD_TARJETA = datosCombustible.COD_TARJETA,
                    NUM_DOCUMENTO =  datosCombustible.NUM_DOCUMENTO,
                    NOMBRE_ARCHIVO_IMPORTACION = datosCombustible.NOMBRE_ARCHIVO_IMPORTACION,
                };

                unitOfWork.RepositoryT_G_TARJETA_COMBUSTIBLE.Insert(datos);
                unitOfWork.Commit();
            }


            return true;
        }

        public bool Archivo_ConsumoCombustible_ImportadoConAnterioridad(string nombreArchivo)
        {
            bool valorReturn = false;

            T_G_TARJETA_COMBUSTIBLESpecification spec = new T_G_TARJETA_COMBUSTIBLESpecification
            {
                NOMBRE_ARCHIVO_IMPORTACION = nombreArchivo,
            };

            using (var unitOfWork = new UnitOfWork())
            {
                valorReturn = unitOfWork.RepositoryT_G_TARJETA_COMBUSTIBLE.Where(spec).Count() > 0;
            }

            return valorReturn;
        }

        public bool BorrarDatosImportadosConAnterioridad(string nombreArchivo)
        {
            bool valorReturn = true;

            valorReturn = new BorradoImportacionService().BorraDatosImportacion(EnumTipoBorradoImportacion.BorrarImportacionCombustible, nombreArchivo);

            return valorReturn;
        }
        #endregion ConsumoCombustible

        #region eventos
        private void OnProcessingImportConsumoCombustibleEventHandler(int lineProsessing, int TotalLinesToProcess, string msg)
        {
            EventProcessingImportConsumoCombustible?.Invoke(lineProsessing, TotalLinesToProcess, msg);
        }

        private void OnSubProcessingImportConsumoCombustibleEventHandler(int lineProsessing, int TotalLinesToProcess, string msg)
        {
            EventSubProcessingImportConsumoCombustible?.Invoke(lineProsessing, TotalLinesToProcess, msg);
        }

        private void OnErrorImportConsumoCombustibleEventHandler(int lineProsessing, string msgError)
        {
            EventErrorImportConsumoCombustible?.Invoke(lineProsessing, msgError);
        }

        private void OnFinishedImportConsumoCombustibleEventHandler(int TotalLinesToProcess, int TotalLinesProcessedOK, int TotalFilasProcesadasIncidencia, int TotalLinesProcessedERROR)
        {
            EventFinishedImportConsumoCombustible?.Invoke(TotalLinesToProcess, TotalLinesProcessedOK, TotalFilasProcesadasIncidencia, TotalLinesProcessedERROR);
        }

        private void OnIncidenciaImportConsumoCombustible(int lineProsessing, string matricula, EnumTipoLineaImportacion tipoIncidencia)
        {
            EventIncidenciaImportConsumoCombustible?.Invoke(lineProsessing, matricula, tipoIncidencia);
        }
    }

    #endregion eventos

}
