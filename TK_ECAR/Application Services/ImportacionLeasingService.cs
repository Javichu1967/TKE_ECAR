using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TK_ECAR.Application_Services;
using TK_ECAR.Content.resources;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Framework;
using TK_ECAR.Framework.Utils;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;
using TK_ECAR.Models.Portugal;
using TK_ECAR.Utils;
using resourceView = TK_ECAR.Content.resources.TK_ECAR_Resource;

namespace TK_ECAR.ApplicationServices
{
    public class ImportacionLeasingService
    {
        #region Constantes
        private Dictionary<string, string> conversor_codigo_columnaLEASEPLAN = new Dictionary<string, string>();
        private Dictionary<string, int> conversor_codigoClienteLEASEPLAN_Sociedad = new Dictionary<string, int>();
        private Dictionary<string, bool> MatriculasProcesadas = new Dictionary<string, bool>();

        #region Constantes LeasePlan.
        //INICIALMENTE SON LOS VALORES DE ESPAÑA.
        private int PLAN_TXT_INICIO_MATRICULA = 8;
        private int PLAN_TXT_CARACTERES_MATRICULA = 7;
        private int PLAN_TXT_INICIO_CODIGO = 16;
        private int PLAN_TXT_CARACTERES_CODIGO = 3;
        private int PLAN_TXT_INICIO_CODIGO_CLIENTE = 54;
        private int PLAN_TXT_CARACTERES_CODIGO_CLIENTE = 9;
        private int PLAN_TXT_INICIO_FACTURA = 160;
        private int PLAN_TXT_CARACTERES_FACTURA = 8;
        private int PLAN_TXT_INICIO_SIGNO_VALOR_BASE = 142;
        private int PLAN_TXT_CARACTERES_SIGNO_VALOR_BASE = 1;
        private int PLAN_TXT_INICIO_VALOR_BASE = 145;
        private int PLAN_TXT_CARACTERES_VALOR_BASE = 11;
        private int PLAN_TXT_INICIO_IMPUESTO_BASE = 156;
        private int PLAN_TXT_CARACTERES_IMPUESTO_BASE = 4;
        private int PLAN_TXT_INICIO_F_FACTURA = 187;
        private int PLAN_TXT_CARACTERES_F_FACTURA = 8;
        private int PLAN_TXT_INICIO_VALOR_BASE_CON_IMPUESTO = 0;
        private int PLAN_TXT_CARACTERES_VALOR_BASE_CON_IMPUESTO = 0;

        //VALORES PARA PORTUGAL
        private const int PLAN_TXT_PT_INICIO_FACTURA = 1;
        private const int PLAN_TXT_PT_CARACTERES_FACTURA = 17;
        private const int PLAN_TXT_PT_INICIO_F_FACTURA = 18;
        private const int PLAN_TXT_PT_CARACTERES_F_FACTURA = 17;
        private const int PLAN_TXT_PT_INICIO_MATRICULA = 35;
        private const int PLAN_TXT_PT_CARACTERES_MATRICULA = 6;
        private const int PLAN_TXT_PT_INICIO_CODIGO = 81;
        private const int PLAN_TXT_PT_CARACTERES_CODIGO = 3;
        private const int PLAN_TXT_PT_INICIO_VALOR_BASE = 84;
        private const int PLAN_TXT_PT_CARACTERES_VALOR_BASE = 13;
        private const int PLAN_TXT_PT_INICIO_IMPUESTO_BASE = 97;
        private const int PLAN_TXT_PT_CARACTERES_IMPUESTO_BASE = 13;
        private const int PLAN_TXT_PT_INICIO_VALOR_BASE_CON_IMPUESTO = 110;
        private const int PLAN_TXT_PT_CARACTERES_VALOR_BASE_CON_IMPUESTO = 11;
        #endregion Constantes LeasePlan.

        #region Constantes ARVAL.
        private int PLAN_COL_EXCEL_MATRICULA = 2;
        private int PLAN_COL_EXCEL_CODIGO = 3;
        private int PLAN_COL_CODIGO_CLIENTE = 4;
        private int PLAN_COL_EXCEL_FACTURA = 10;
        private int PLAN_COL_EXCEL_VALOR_BASE = 8;
        private int PLAN_COL_EXCEL_IMPUESTO = 9;
        private int PLAN_COL_EXCEL_F_FACTURA = 11;
        private int PLAN_COL_EXCEL_PERIODO_FACTURACION = 13;
        #endregion Constantes ARVAL.

        #region Constantes ALD.
        private int ALD_XLS_MATRICULA = 3;
        private int ALD_XLS_CODIGO = 4;
        private int ALD_XLS_CODIGO_CLIENTE = 6;
        private int ALD_XLS_VALOR_BASE = 12;
        private int ALD_XLS_IMPUESTO = 13;
        private int ALD_XLS_FACTURA = 14;
        private int ALD_XLS_PERIODO_FACTURACION = 15;
        private int ALD_XLS_F_FACTURA = 22;
        private int ALD_XLS_IMPUESTO_BASE = 27;
        //private int ALD_XLS_VALOR_BASE_CON_IMPUESTO = 0; //CALCULADO -> ALD_XLS_VALOR_BASE + ALD_XLS_IMPUESTO_BASE
        #endregion Constantes ALD.

        #region Constantes VW.
        private int VW_XLS_MATRICULA = 11;
        private int VW_XLS_CODIGO_CLIENTE = 4;
        private int VW_XLS_IMP_ALQUILER = 16;
        private int VW_XLS_IMP_SERVICIOS = 17;
        private int VW_XLS_IMP_SEGURO = 18;
        private int VW_XLS_IMPUESTO = 22;
        private int VW_XLS_FACTURA = 6;
        private int VW_XLS_PERIODO_FACTURACION = 8;
        private int VW_XLS_F_FACTURA = 9;
        #endregion Constantes VW.

        #region Constantes comunes.
        private const string PLAN_CODIGO_SEGURO1 = "100";
        private const string PLAN_CODIGO_SEGURO2 = "670";
        private const string PLAN_CODIGO_MANTENIMIENTO1 = "730";
        private const string PLAN_CODIGO_MANTENIMIENTO2 = "740";
        private const string PLAN_CODIGO_MANTENIMIENTO3 = "580";
        private const string PLAN_CODIGO_MANTENIMIENTO4 = "581";
        private const string PLAN_CODIGO_MANTENIMIENTO5 = "600";
        private const string PLAN_CODIGO_MANTENIMIENTO6 = "620";
        private const string PLAN_CODIGO_MANTENIMIENTO7 = "720";
        private const string PLAN_CODIGO_ALQUILER1 = "011";
        private const string PLAN_CODIGO_ALQUILER2 = "999";
        private const string PLAN_CODIGO_ADMINISTRACION1 = "650";
        private const string PLAN_CODIGO_ADMINISTRACION2 = "711";
        private const string PLAN_CODIGO_ADMINISTRACION3 = "000";

        private const string PLAN_CODIGO_RENTING = "001";
        private const string PLAN_CODIGO_MANTENIMIENTO = "002";
        private const string PLAN_CODIGO_SEGURO = "003";
        private const string PLAN_CODIGO_ADMINISTRACION = "004";
        private const string PLAN_CODIGO_ITV = "005";

        private const double CONST_IMPUESTO_PORTUGAL = 23;
        #endregion Constantes comunes.

        #endregion Constantes

        #region Eventos Importación
        //Definimos evento.
        public delegate void ProcessingImportDatosLeasingEventHandler(int lineProsessing, int TotalLinesToProcess, string msg);
        //Evento de procesado de archivo.
        public event ProcessingImportDatosLeasingEventHandler EventProcessingImportDatosLeasing;

        public delegate void SubProcessingImportDatosLeasingEventHandler(int lineProsessing, int TotalLinesToProcess, string msg);
        public event SubProcessingImportDatosLeasingEventHandler EventSubProcessingImportDatosLeasing;

        public delegate void ErrorImportDatosLeasingEventHandler(int lineaProsessing, string msgError);
        public event ErrorImportDatosLeasingEventHandler EventErrorImportDatosLeasing;

        public delegate void IncidenciaImportDatosLeasingEventHandler(int lineaProsessing, string matricula, EnumTipoLineaImportacion tipoIncidencia);
        public event IncidenciaImportDatosLeasingEventHandler EventIncidenciaImportDatosLeasing;

        public delegate void FinishedImportDatosLeasingEventHandler(int TotalLinesToProcess, int TotalLinesProcessedOK, int TotalFilasProcesadasIncidencia, int TotalLinesProcessedERROR);
        public event FinishedImportDatosLeasingEventHandler EventFinishedImportDatosLeasing;
        #endregion Eventos Importación

        //private const int VEHICULO_DIRECCION = 8;

        private string Paso = string.Empty;
        private int FilaProcessing = 0;
        private int TotalFilas = 0;
        private int TotalFilasProcesadasOK = 0;
        private int TotalFilasProcesadasExistentes = 0;
        private int TotalFilasProcesadasIncidencia = 0;
        private int TotalFilasProcesadasERROR = 0;

        private int sociedadImportacion = 0;
        private int empresaLeasingImportacion = 0;

        private List<ECAR_Datos_Vehiculo> vehiculosImportados = null;

        #region LEASEPLAN
        public bool ImportaDatosLEASING(ImportacionLeasingModels modelo)
        {
            bool valorReturn = true;
            string fileToImport = string.Empty;

            vehiculosImportados = new List<ECAR_Datos_Vehiculo>();

            TotalFilasProcesadasERROR = 0;

            MatriculasProcesadas = new Dictionary<string, bool>();

            OnProcessingImportDatosLeasingEventHandler(0, 0, $"{resourceView.ImportacionComienza} {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");

            var nombreArchivo = FileUtilities.GetFileName(modelo.FileToImport);
            fileToImport = HttpContext.Current.Server.MapPath($"{Global.PathToPathToImportFilesRenting}{nombreArchivo}");
            sociedadImportacion = Convert.ToInt32(modelo.IDEmpresa);
            empresaLeasingImportacion = (int)modelo.IDEmpresaLeasing;

            if (sociedadImportacion == Constants.CODIGO_EMPRESA_PORTUGAL) //PORTUGAL.
            {
                PLAN_TXT_INICIO_FACTURA = PLAN_TXT_PT_INICIO_FACTURA;
                PLAN_TXT_CARACTERES_FACTURA = PLAN_TXT_PT_CARACTERES_FACTURA;
                PLAN_TXT_INICIO_F_FACTURA = PLAN_TXT_PT_INICIO_F_FACTURA;
                PLAN_TXT_CARACTERES_F_FACTURA = PLAN_TXT_PT_CARACTERES_F_FACTURA;
                PLAN_TXT_INICIO_MATRICULA = PLAN_TXT_PT_INICIO_MATRICULA;
                PLAN_TXT_CARACTERES_MATRICULA = PLAN_TXT_PT_CARACTERES_MATRICULA;
                PLAN_TXT_INICIO_CODIGO = PLAN_TXT_PT_INICIO_CODIGO;
                PLAN_TXT_CARACTERES_CODIGO = PLAN_TXT_PT_CARACTERES_CODIGO;
                PLAN_TXT_INICIO_VALOR_BASE = PLAN_TXT_PT_INICIO_VALOR_BASE;
                PLAN_TXT_CARACTERES_VALOR_BASE = PLAN_TXT_PT_CARACTERES_VALOR_BASE;
                PLAN_TXT_INICIO_IMPUESTO_BASE = PLAN_TXT_PT_INICIO_IMPUESTO_BASE;
                PLAN_TXT_CARACTERES_IMPUESTO_BASE = PLAN_TXT_PT_CARACTERES_IMPUESTO_BASE;
                PLAN_TXT_INICIO_VALOR_BASE_CON_IMPUESTO = PLAN_TXT_PT_INICIO_VALOR_BASE_CON_IMPUESTO;
                PLAN_TXT_CARACTERES_VALOR_BASE_CON_IMPUESTO = PLAN_TXT_PT_CARACTERES_VALOR_BASE_CON_IMPUESTO;
            }

            FilaProcessing = 0;

            EstableceConversionDatos();

            try
            {
                Paso = $"ESTABLECIENDO fileinfo {fileToImport}";

                List<Leasing> datosLeasing = new List<Leasing>();

                if (empresaLeasingImportacion == Constants.ID_ARVAL || 
                    empresaLeasingImportacion == Constants.ID_ALD ||
                    empresaLeasingImportacion == Constants.ID_VW)
                {
                    if (empresaLeasingImportacion == Constants.ID_ALD)
                    {
                        PLAN_COL_EXCEL_MATRICULA = ALD_XLS_MATRICULA;
                        PLAN_COL_EXCEL_CODIGO = ALD_XLS_CODIGO;
                        PLAN_COL_CODIGO_CLIENTE = ALD_XLS_CODIGO_CLIENTE;
                        PLAN_COL_EXCEL_FACTURA = ALD_XLS_FACTURA;
                        PLAN_COL_EXCEL_VALOR_BASE = ALD_XLS_VALOR_BASE;
                        PLAN_COL_EXCEL_IMPUESTO = ALD_XLS_IMPUESTO;
                        PLAN_COL_EXCEL_F_FACTURA = ALD_XLS_F_FACTURA;
                        PLAN_COL_EXCEL_PERIODO_FACTURACION = ALD_XLS_PERIODO_FACTURACION;
                        //ALD_XLS_IMPUESTO_BASE = 27;
                    }
                    else if (empresaLeasingImportacion == Constants.ID_VW)
                    {
                        PLAN_COL_EXCEL_MATRICULA = VW_XLS_MATRICULA;
                        PLAN_COL_EXCEL_CODIGO = -1;
                        PLAN_COL_CODIGO_CLIENTE = VW_XLS_CODIGO_CLIENTE;
                        PLAN_COL_EXCEL_FACTURA = VW_XLS_FACTURA;
                        PLAN_COL_EXCEL_VALOR_BASE = -1;
                        PLAN_COL_EXCEL_IMPUESTO = VW_XLS_IMPUESTO;
                        PLAN_COL_EXCEL_F_FACTURA = VW_XLS_F_FACTURA;
                        PLAN_COL_EXCEL_PERIODO_FACTURACION = VW_XLS_PERIODO_FACTURACION;
                        //ALD_XLS_IMPUESTO_BASE = 27;
                    }

                    datosLeasing = CargaLEASINGfromExcel(fileToImport, nombreArchivo);
                }
                else
                {
                    datosLeasing = CargaLEASINGfromTxT(fileToImport, nombreArchivo);
                }

                AñadeLEASEPLAN(datosLeasing, nombreArchivo);
            }
            catch (Exception ex)
            {
                OnErrorImportDatosLeasingEventHandler(FilaProcessing, $"[ERROR] {resourceView.ImportacionProcesandoLínea}{FilaProcessing} - {Paso}. {Environment.NewLine}{Global.GetMessageError(ex)}");
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"<ImportaDatosLEASEPLAN> <FICHERO> {fileToImport}, <PASO> CARGA DATOS, {Global.GetMessageError(ex)}");
                valorReturn = false;
            }

            finally
            {
                OnFinishedImportDatosLeasingEventHandler(TotalFilas, TotalFilasProcesadasOK, TotalFilasProcesadasIncidencia, TotalFilasProcesadasERROR);
            }

            return valorReturn;
        }


        #region carga desde Txt.
        private List<Leasing> CargaLEASINGfromTxT(string fileToImport, string archivoLeasing)
        {
            string line;
            string lineToProcess = "";
            int linea = 0;

            var fileinfo = new FileInfo(fileToImport);

            List<Leasing> datosLeasing = new List<Leasing>();

            System.IO.StreamReader file = null;

            Paso = $"LEYENDO ARCHIVO {archivoLeasing}";

            file = new System.IO.StreamReader(fileToImport);

            Paso = $"CARGANDO DATOS DEL ARCHIVO {archivoLeasing}";

            OnProcessingImportDatosLeasingEventHandler(linea, 0, $"{resourceView.ImportacionLeyendoArchivo}");

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    lineToProcess = line;
                    linea++;

                    Paso = $"PROCESANDO LÍNEA {linea}";

                    Leasing dato = new Leasing();
                    dato = GetLeasingFromLineTxT(line);

                    OnSubProcessingImportDatosLeasingEventHandler(linea, 0, $"{resourceView.ImportacionProcesandoLínea} MATRICULA ({dato.MATRICULA}) - FACTURA ({dato.FACTURA})");

                    var itemExistente = datosLeasing.Where(o => o.FACTURA == dato.FACTURA &&
                                                            o.MATRICULA == dato.MATRICULA).FirstOrDefault();

                    if (itemExistente == null)
                    {
                        datosLeasing.Add(dato);
                    }
                    else
                    {
                        //ActualizaImporteElemento(datosLeasing.Where(o => o.FACTURA == dato.FACTURA &&
                        //                                    o.FECHA_FACTURA == dato.FECHA_FACTURA &&
                        //                                    o.MATRICULA == dato.MATRICULA).FirstOrDefault(), dato);
                        ActualizaImporteElemento(itemExistente, dato);
                    }
                }

                catch (Exception ex)
                {
                    OnErrorImportDatosLeasingEventHandler(FilaProcessing, $"[ERROR] {resourceView.ImportacionProcesandoLínea}{linea} - {Paso}. {Environment.NewLine}{Global.GetMessageError(ex)}");

                    Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"<CargaLeasePlanPortugal> <FICHERO> {archivoLeasing}, <Nº LINEA> {linea}, <TEXTO> {lineToProcess}, {Global.GetMessageError(ex)}");
                    //return false;
                }
            }

            file.Close();

            OnSubProcessingImportDatosLeasingEventHandler(0, 0, "");

            return datosLeasing;
        }


        private Leasing GetLeasingFromLineTxT(string line)
        {
            Leasing leasing = new Leasing();

            if (sociedadImportacion == Constants.CODIGO_EMPRESA_PORTUGAL) //PORTUGAL.
            {
                leasing.SOCIEDAD = sociedadImportacion;
            }
            else
            {
                leasing.SOCIEDAD = conversor_codigoClienteLEASEPLAN_Sociedad[GetDatoFromLineaTxtLeasing(line, PLAN_TXT_INICIO_CODIGO_CLIENTE, PLAN_TXT_CARACTERES_CODIGO_CLIENTE)];
            }
            leasing.EMPRESA_LEASING = empresaLeasingImportacion;
            leasing.FACTURA = GetDatoFromLineaTxtLeasing(line, PLAN_TXT_INICIO_FACTURA, PLAN_TXT_CARACTERES_FACTURA);
            leasing.FECHA_FACTURA = GetFechaFromLineaTxtLeasing(line, PLAN_TXT_INICIO_F_FACTURA, PLAN_TXT_CARACTERES_F_FACTURA);
            leasing.MATRICULA = GetDatoFromLineaTxtLeasing(line, PLAN_TXT_INICIO_MATRICULA, PLAN_TXT_CARACTERES_MATRICULA,' ',true, (leasing.SOCIEDAD != Constants.CODIGO_EMPRESA_PORTUGAL));
            leasing.EJERCICIO = DevuelveEjercicioDeFecha(leasing.FECHA_FACTURA);
            leasing.TRIMESTRE = DevuelveTrimestreDeFecha(leasing.FECHA_FACTURA);
            leasing.DIRECTIVO = EsCocheDirectivo(leasing.MATRICULA);
            RellenaImporteFromTxt(line, leasing);

            return leasing;
        }

        private string GetDatoFromLineaTxtLeasing(string line, int inicio, int numCaracteres, char quitarPorLaIzquierda = '?', bool esMatricula = false, bool esMatriculaEspañola = false)
        {
            string valorReturn = string.Empty;

            valorReturn = line.Substring(inicio - 1, numCaracteres).Trim().TrimStart(quitarPorLaIzquierda);

            if (esMatricula)
            {
                if (!esMatriculaEspañola)
                {
                    if (valorReturn.IndexOf('-') == -1)
                    {
                        valorReturn = $"{valorReturn.Substring(0, 2)}-{valorReturn.Substring(2, 2)}-{valorReturn.Substring(4)}";
                    }
                }
                else
                {
                    if (valorReturn.IndexOf('-') == -1)
                    {
                        valorReturn = $"{valorReturn.Substring(0, 4)}-{valorReturn.Substring(4)}";
                    }
                }
            }


            return valorReturn;
        }

        private DateTime GetFechaFromLineaTxtLeasing(string line, int inicio, int numCaracteres)
        {
            string valor = line.Substring(inicio - 1, numCaracteres).Trim();
            return Global.GetFormatedFechaFromString(valor);
        }

        private void RellenaImporteFromTxt(string line, Leasing lease)
        {
            //lease.IMP_CIRCULACION = 0;
            //lease.IMP_CIRCULACION_IVA = 0;
            //lease.IMP_MATRICULACION = 0;
            //lease.IMP_MATRICULACION_IVA = 0;
            //lease.IMP_NEUMATICOS = 0;
            //lease.IMP_NEUMATICOS_IVA = 0;
            //lease.IMP_ASISTEN_CARRETERA = 0;
            //lease.IMP_ASISTEN_CARRETERA_IVA = 0;
            //lease.IMP_ITV = 0;
            //lease.IMP_ITV_IVA = 0;

            string codigo = GetDatoFromLineaTxtLeasing(line, PLAN_TXT_INICIO_CODIGO, PLAN_TXT_CARACTERES_CODIGO);
            lease.CONCEPTO = codigo;
            if (conversor_codigo_columnaLEASEPLAN.ContainsKey(codigo))
            {
                string valorBase = GetDatoFromLineaTxtLeasing(line, PLAN_TXT_INICIO_VALOR_BASE, PLAN_TXT_CARACTERES_VALOR_BASE, '0');
                string valorBaseConImpuesto = "0";
                lease.IMPUESTO = 0;
                if (lease.SOCIEDAD == Constants.CODIGO_EMPRESA_PORTUGAL)
                {
                    valorBaseConImpuesto = GetDatoFromLineaTxtLeasing(line, PLAN_TXT_INICIO_VALOR_BASE_CON_IMPUESTO, PLAN_TXT_CARACTERES_VALOR_BASE_CON_IMPUESTO);
                    valorBase = $"{valorBase.Substring(0, PLAN_TXT_CARACTERES_VALOR_BASE - 2)},{valorBase.Substring(PLAN_TXT_CARACTERES_VALOR_BASE - 2)}";
                    valorBaseConImpuesto = $"{valorBaseConImpuesto.Substring(0, PLAN_TXT_CARACTERES_VALOR_BASE_CON_IMPUESTO - 2)},{valorBaseConImpuesto.Substring(PLAN_TXT_CARACTERES_VALOR_BASE_CON_IMPUESTO - 2)}";
                    if (valorBase != valorBaseConImpuesto)
                    {
                        lease.IMPUESTO = Convert.ToDecimal(CONST_IMPUESTO_PORTUGAL);
                    }
                }
                else
                {
                    string valorImpuesto = GetDatoFromLineaTxtLeasing(line, PLAN_TXT_INICIO_IMPUESTO_BASE, PLAN_TXT_CARACTERES_IMPUESTO_BASE, '0');
                    valorBase = (Convert.ToDouble(valorBase) / 100).ToString();
                    if (!string.IsNullOrEmpty(valorImpuesto))
                    {
                        lease.IMPUESTO = Convert.ToDecimal(Convert.ToDouble(valorImpuesto) / 10000);
                        valorBaseConImpuesto = ((Convert.ToDouble(valorBase)) * (1 + (Convert.ToDouble(valorImpuesto) / 10000))).ToString();
                    }
                }
                RellenaImporte(codigo, valorBase, valorBaseConImpuesto, lease);
            }
            else
            {
                OnIncidenciaImportDatosLeasing(FilaProcessing, lease.MATRICULA, EnumTipoLineaImportacion.ConceptoLeasinNoContemplado);
            }
        }
        #endregion carga desde Txt.


        #region Carga desde Excel
        private List<Leasing> CargaLEASINGfromExcel(string fileToImport, string archivoLeasing)
        {
            int linea = 0;
            int totalFilas = 0;
            FileStream templateDocumentStream = null;
            ExcelPackage package = null;

            List<Leasing> datosLeasing = new List<Leasing>();
            var columnaEstablecida = string.Empty;

            try
            {
                Paso = $"ABRIENDO ARCHIVO {archivoLeasing}";

                OnProcessingImportDatosLeasingEventHandler(0, 0, $"{resourceView.ImportacionLeyendoArchivo}");

                templateDocumentStream = File.OpenRead(fileToImport);
                templateDocumentStream.Position = 0;

                package = new ExcelPackage(templateDocumentStream);

                ExcelWorksheet ws = null;
                ws = package.Workbook.Worksheets[1];
                //TotalFilas = ws.Dimension.End.Row - 1; //Quitamos la primera fila que es cabecera.

                Paso = $"PROCESANDO ARCHIVO {archivoLeasing}";
                totalFilas = ws.Dimension.End.Row;
                for (int r = 2; r <= ws.Dimension.End.Row; r++)
                {
                    TotalFilasProcesadasExistentes++;
                    Leasing dato = new Leasing();
                    Paso = $"PROCESANDO FILA {r}";
                    FilaProcessing = r;
                    try
                    {
                        dato.SOCIEDAD = conversor_codigoClienteLEASEPLAN_Sociedad[Global.DevuelveTextoFromExcel(ws.Cells[r, PLAN_COL_CODIGO_CLIENTE].Value,true)]; //sociedadImportacion;
                        dato.EMPRESA_LEASING = empresaLeasingImportacion;
                        columnaEstablecida = "MATRICULA";
                        dato.MATRICULA = Global.DevuelveTextoFromExcel(ws.Cells[r, PLAN_COL_EXCEL_MATRICULA].Value, true, true, (dato.SOCIEDAD != Constants.CODIGO_EMPRESA_PORTUGAL));
                        columnaEstablecida = "FACTURA";
                        dato.FACTURA = Global.DevuelveTextoFromExcel(ws.Cells[r, PLAN_COL_EXCEL_FACTURA].Value);
                        columnaEstablecida = "FECHA_FACTURA";
                        if (empresaLeasingImportacion == Constants.ID_VW)
                        {
                            dato.FECHA_FACTURA = (DateTime)Global.DevuelveFechaFromExcel(ws.Cells[r, PLAN_COL_EXCEL_F_FACTURA].Value); 
                        }
                        else
                        {
                            dato.FECHA_FACTURA = Global.GetFormatedFechaFromString(ws.Cells[r, PLAN_COL_EXCEL_F_FACTURA].Text);
                        }
                        
                        columnaEstablecida = "EJERCICIO";
                        var fechaPeriodo = new DateTime();

                        //PLAN_COL_EXCEL_MATRICULA = VW_XLS_MATRICULA;
                        //PLAN_COL_EXCEL_CODIGO = -1;
                        //PLAN_COL_CODIGO_CLIENTE = VW_XLS_CODIGO_CLIENTE;
                        //PLAN_COL_EXCEL_FACTURA = VW_XLS_FACTURA;
                        //PLAN_COL_EXCEL_VALOR_BASE = -1;
                        //PLAN_COL_EXCEL_IMPUESTO = VW_XLS_IMPUESTO;
                        //PLAN_COL_EXCEL_F_FACTURA = VW_XLS_F_FACTURA;
                        //PLAN_COL_EXCEL_PERIODO_FACTURACION = VW_XLS_PERIODO_FACTURACION;

                        if (empresaLeasingImportacion == Constants.ID_ALD)
                        {
                            fechaPeriodo = Global.GetFormatedFechaFromString(ws.Cells[r, PLAN_COL_EXCEL_PERIODO_FACTURACION].Text + "01");
                        }
                        else if (empresaLeasingImportacion == Constants.ID_VW)
                        {
                            fechaPeriodo = (DateTime)Global.DevuelveFechaFromExcel(ws.Cells[r, PLAN_COL_EXCEL_PERIODO_FACTURACION].Value);
                        }
                        else
                        {
                            fechaPeriodo = Global.GetFormatedFechaFromString(ws.Cells[r, PLAN_COL_EXCEL_PERIODO_FACTURACION].Text);
                        }
                        dato.EJERCICIO = DevuelveEjercicioDeFecha(fechaPeriodo);
                        columnaEstablecida = "TRIMESTRE";
                        dato.TRIMESTRE = DevuelveTrimestreDeFecha(fechaPeriodo);
                        columnaEstablecida = "DIRECTIVO";
                        dato.DIRECTIVO = EsCocheDirectivo(dato.MATRICULA);
                        columnaEstablecida = "impuestoBase";
                        var impuestoBase = 0.0;
                        if (empresaLeasingImportacion == Constants.ID_ALD || empresaLeasingImportacion == Constants.ID_VW)
                        {
                            impuestoBase = Convert.ToDouble(String.Format("{0:0.00}", Global.DevuelveDoubleFromExcel(ws.Cells[r, PLAN_COL_EXCEL_IMPUESTO].Value) / 100));
                        }
                        else
                        {
                            impuestoBase = Convert.ToDouble(String.Format("{0:0.00}", Global.DevuelveDoubleFromExcel(ws.Cells[r, PLAN_COL_EXCEL_IMPUESTO].Value)));
                        }

                        if (empresaLeasingImportacion != Constants.ID_VW)
                        {
                            columnaEstablecida = "codigo";
                            var codigo = "";
                            if (empresaLeasingImportacion == Constants.ID_ALD)
                            {
                                codigo = "00" + Global.DevuelveTextoFromExcel(ws.Cells[r, PLAN_COL_EXCEL_CODIGO].Value);
                            }
                            else
                            {
                                codigo = Global.DevuelveTextoFromExcel(ws.Cells[r, PLAN_COL_EXCEL_CODIGO].Value).Substring(0, 3);
                            }
                            columnaEstablecida = "valorBase";
                            var valorBase = Convert.ToDouble(String.Format("{0:0.00}", Global.DevuelveDoubleFromExcel(ws.Cells[r, PLAN_COL_EXCEL_VALOR_BASE].Value)));

                            columnaEstablecida = "valorImpuestoBaseExcel";
                            var valorImpuestoBaseExcel = 0.0;
                            if (empresaLeasingImportacion == Constants.ID_ALD)
                            {
                                valorImpuestoBaseExcel = Convert.ToDouble(String.Format("{0:0.00}", Global.DevuelveDoubleFromExcel(ws.Cells[r, ALD_XLS_IMPUESTO_BASE].Value)));
                            }

                            columnaEstablecida = "RellenaImporteFromExcel";
                            RellenaImporteFromExcel(dato, codigo, valorBase, impuestoBase, valorImpuestoBaseExcel);
                        }
                        else
                        {
                            dato.IMP_SEGURO = (double)Global.DevuelveDoubleFromExcel(ws.Cells[r, VW_XLS_IMP_SEGURO].Value);
                            dato.IMP_SEGURO_IVA = Convert.ToDouble(((dato.IMP_SEGURO) * (1 + (impuestoBase))).ToString("###,###,##0.00"));

                            dato.IMP_MANTENIMIENTO = (double)Global.DevuelveDoubleFromExcel(ws.Cells[r, VW_XLS_IMP_SERVICIOS].Value);
                            dato.IMP_MANTENIMIENTO_IVA = Convert.ToDouble(((dato.IMP_MANTENIMIENTO) * (1 + (impuestoBase))).ToString("###,###,##0.00"));

                            dato.IMP_ALQUILER = (double)Global.DevuelveDoubleFromExcel(ws.Cells[r, VW_XLS_IMP_ALQUILER].Value);
                            dato.IMP_ALQUILER_IVA = Convert.ToDouble(((dato.IMP_ALQUILER) * (1 + (impuestoBase))).ToString("###,###,##0.00"));

                            dato.IMP_ADMINISTRACION = 0;
                            dato.IMP_ADMINISTRACION_IVA = 0;

                            dato.IMP_ITV = 0;
                            dato.IMP_ITV_IVA = 0;
                        }
                        OnSubProcessingImportDatosLeasingEventHandler(FilaProcessing, totalFilas, $"{resourceView.ImportacionProcesandoLínea} MATRICULA ({dato.MATRICULA}) - FACTURA ({dato.FACTURA})");

                        var itemExistente = datosLeasing.Where(o => o.FACTURA == dato.FACTURA &&
                                                                o.MATRICULA == dato.MATRICULA).FirstOrDefault();

                        if (itemExistente == null)
                        {
                            datosLeasing.Add(dato);
                        }
                        else
                        {
                            ActualizaImporteElemento(itemExistente, dato);
                        }
                        TotalFilasProcesadasOK++;
                    }
                    catch (Exception ex)
                    {
                        var msgError = Global.GetMessageError(ex);

                        TotalFilasProcesadasERROR++;

                        Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"<CargaLEASINGfromExcel> <FICHERO> {fileToImport}, FILA {FilaProcessing}. {Environment.NewLine}{msgError}");
                        var msg = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_Error")} " + columnaEstablecida != "" ? $" COLUMNA [{columnaEstablecida}] " : "";
                        OnErrorImportDatosLeasingEventHandler(FilaProcessing, $"[ERROR] {msg}. {Environment.NewLine}{msgError}");
                    }

                }
            }
            catch (Exception ex)
            {
                var msgError = Global.GetMessageError(ex);

                TotalFilasProcesadasERROR++;

                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"<CargaLEASINGfromExcel> <FICHERO> {fileToImport}, PASO {Paso}. {Environment.NewLine}{msgError}");
                var msg = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_Error")} " + $" PASO [{Paso}] ";
                OnErrorImportDatosLeasingEventHandler(FilaProcessing, $"[ERROR] {msg}. {Environment.NewLine}{msgError}");
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

                OnSubProcessingImportDatosLeasingEventHandler(0, 0, "");
            }

            return datosLeasing;
        }


        private void RellenaImporteFromExcel(Leasing dato, string codigo, double valorBase, double impuestoBase, double valorImpuestoBaseExcel)
        {

            dato.CONCEPTO = codigo;
            if (conversor_codigo_columnaLEASEPLAN.ContainsKey(codigo))
            {
                string valorBaseConImpuesto = "0";
                dato.IMPUESTO = Convert.ToDecimal(impuestoBase);
                if (empresaLeasingImportacion == Constants.ID_ALD)
                {
                    valorBaseConImpuesto = (valorBase + valorImpuestoBaseExcel).ToString("###,###,##0.00");
                }
                else
                {
                    valorBaseConImpuesto = ((valorBase) * (1 + (impuestoBase))).ToString("###,###,##0.00");
                }

                RellenaImporte(codigo, valorBase.ToString(), valorBaseConImpuesto, dato);
            }
        }

        #endregion Carga desde Excel

        #region métodos comunes
        private string DevuelveEjercicioDeFecha(DateTime fecha)
        {
            return (fecha.Month >= 10 ?
                                    (fecha.Year.ToString() + "-" + (fecha.Year + 1).ToString()) :
                                    ((fecha.Year - 1).ToString() + "-" + fecha.Year.ToString()));
        }


        private string DevuelveTrimestreDeFecha(DateTime fecha)
        {
            var mes = fecha.Month;
            if (mes >= 10)
            {
                return "1º";
            }
            else if (mes >= 7)
            {
                return "4º";
            }
            else if (mes > 3 && mes <= 6)
            {
                return "3º";
            }
            else
            {
                return "2º";
            }

        }

        private void RellenaImporte(string codigo, string valorBase, string valorBaseConImpuesto, Leasing lease)
        { 
            switch (conversor_codigo_columnaLEASEPLAN[codigo])
            {
                case "SEGURO":
                    lease.IMP_SEGURO = Convert.ToDouble(valorBase);
                    lease.IMP_SEGURO_IVA = Convert.ToDouble(valorBaseConImpuesto);
                    break;
                case "MANTENIMIENTO":
                    lease.IMP_MANTENIMIENTO = Convert.ToDouble(valorBase);
                    lease.IMP_MANTENIMIENTO_IVA = Convert.ToDouble(valorBaseConImpuesto);
                    break;
                case "ALQUILER":
                    lease.IMP_ALQUILER = Convert.ToDouble(valorBase);
                    lease.IMP_ALQUILER_IVA = Convert.ToDouble(valorBaseConImpuesto);
                    break;
                case "ADMINISTRACION":
                    lease.IMP_ADMINISTRACION = Convert.ToDouble(valorBase);
                    lease.IMP_ADMINISTRACION_IVA = Convert.ToDouble(valorBaseConImpuesto);
                    break;
                case "ITV":
                    lease.IMP_ITV = Convert.ToDouble(valorBase);
                    lease.IMP_ITV_IVA = Convert.ToDouble(valorBaseConImpuesto);
                    break;
            }
        }

        private void ActualizaImporteElemento(Leasing leaseExistente, Leasing lease)
        {
            leaseExistente.IMP_SEGURO = Math.Round(leaseExistente.IMP_SEGURO + lease.IMP_SEGURO, 2);
            leaseExistente.IMP_SEGURO_IVA = Math.Round(leaseExistente.IMP_SEGURO_IVA + lease.IMP_SEGURO_IVA, 2);
            leaseExistente.IMP_MANTENIMIENTO = Math.Round(leaseExistente.IMP_MANTENIMIENTO + lease.IMP_MANTENIMIENTO,2);
            leaseExistente.IMP_MANTENIMIENTO_IVA = Math.Round(leaseExistente.IMP_MANTENIMIENTO_IVA + lease.IMP_MANTENIMIENTO_IVA, 2);
            leaseExistente.IMP_ALQUILER = Math.Round(leaseExistente.IMP_ALQUILER + lease.IMP_ALQUILER, 2);
            leaseExistente.IMP_ALQUILER_IVA = Math.Round(leaseExistente.IMP_ALQUILER_IVA + lease.IMP_ALQUILER_IVA, 2);
            leaseExistente.IMP_ADMINISTRACION = Math.Round(leaseExistente.IMP_ADMINISTRACION + lease.IMP_ADMINISTRACION, 2);
            leaseExistente.IMP_ADMINISTRACION_IVA = Math.Round(leaseExistente.IMP_ADMINISTRACION_IVA + lease.IMP_ADMINISTRACION_IVA, 2);
            leaseExistente.IMP_ITV = Math.Round(leaseExistente.IMP_ITV + lease.IMP_ITV, 2);
            leaseExistente.IMP_ITV_IVA = Math.Round(leaseExistente.IMP_ITV_IVA + lease.IMP_ITV_IVA, 2);

            if (leaseExistente.IMPUESTO != lease.IMPUESTO && leaseExistente.IMPUESTO == 0)
            {
                leaseExistente.IMPUESTO = lease.IMPUESTO;
            }
        }

        private bool AñadeLEASEPLAN(List<Leasing> Lease, string nombreArchivo)
        {
            Paso = $"ACTUALIZANDO DATOS EN BBDD";
            FilaProcessing = 0;
            TotalFilasProcesadasExistentes = 0;
            TotalFilasProcesadasOK = 0 - TotalFilasProcesadasIncidencia;
            TotalFilas = 0;

            try
            {
                if (Archivo_LEASING_ImportadoConAnterioridad(nombreArchivo, sociedadImportacion))
                {
                    Paso = $"BORRANDO DATOS IMPORTADOS CON ANTERIORIDAD EN BBDD";
                    OnProcessingImportDatosLeasingEventHandler(0, 0, $"{resourceView.ImportacionBorrandoDatosAnteriores} {nombreArchivo}");
                    //BorrarDatosImportadosConAnterioridad(nombreArchivo, sociedadImportacion);
                    BorrarDatosImportadosConAnterioridad(nombreArchivo);
                }

                var miFlotaService = new FlotaService();

                using (var unitOfWork = new UnitOfWork())
                {

                    OnProcessingImportDatosLeasingEventHandler(0, 0, $"{resourceView.ImportacionComienzaActualizacionBBDD}");

                    var cont = 0;
                    var matricula = "";
                    TotalFilas = Lease.Count;
                    bool procesarMatricula = true;
                    foreach (Leasing leaseplan in Lease)
                    {
                        TotalFilasProcesadasExistentes++;
                        cont++;
                        FilaProcessing = cont;
                        Paso = $"ACTUALIZANDO DATOS EN BBDD. MATRICULA ({leaseplan.MATRICULA}) - FACTURA ({leaseplan.FACTURA})";
                        OnProcessingImportDatosLeasingEventHandler(cont, TotalFilas, $"{resourceView.ImportacionProcesandoLínea} MATRICULA ({leaseplan.MATRICULA}) - FACTURA ({leaseplan.FACTURA})");
                        try
                        {
                            matricula = leaseplan.MATRICULA;

                            //Comprobamos que la matrícula exista.
                            if (!MatriculasProcesadas.ContainsKey(matricula))
                            {
                                if (!miFlotaService.VehiculoExistente(matricula))
                                {
                                    MatriculasProcesadas.Add(matricula, false);
                                    TotalFilasProcesadasIncidencia++;
                                    OnIncidenciaImportDatosLeasing(0, matricula, EnumTipoLineaImportacion.MatriculaInexistente);
                                    procesarMatricula = false;
                                }
                                else
                                {
                                    MatriculasProcesadas.Add(matricula, true);
                                    procesarMatricula = true;
                                }
                            }
                            else
                            {
                                procesarMatricula = MatriculasProcesadas[matricula];
                            }

                            if (procesarMatricula)
                            {
                                var EsCanarias = false;
                                if (leaseplan.SOCIEDAD == 8100)
                                {
                                    EsCanarias = (Math.Abs((Convert.ToDouble(leaseplan.IMPUESTO) * 100) - Convert.ToInt32(Global.PorcentajeImpuestoPeninsula)) < 0.0000001 ? false : true);
                                }
                                T_G_DATOS_LEASING datos = new T_G_DATOS_LEASING
                                {
                                    Administracion = leaseplan.IMP_ADMINISTRACION,
                                    Administracion_IVA = leaseplan.IMP_ADMINISTRACION_IVA,
                                    Alquiler = leaseplan.IMP_ALQUILER,
                                    Alquiler_IVA = leaseplan.IMP_ALQUILER_IVA,
                                    Asistencia_Carretera = leaseplan.IMP_ASISTEN_CARRETERA,
                                    Asistencia_Carretera_IVA = leaseplan.IMP_ASISTEN_CARRETERA_IVA,
                                    Canarias = EsCanarias,   //leaseplan.CANARIAS,
                                    Directivo = leaseplan.DIRECTIVO,
                                    Ejercicio = leaseplan.EJERCICIO,
                                    EmpresaLeasing = leaseplan.EMPRESA_LEASING,
                                    FechaAlta = leaseplan.FECHA_ALTA,
                                    Fecha_Factura = leaseplan.FECHA_FACTURA,
                                    Fecha_Importacion = leaseplan.FECHA_IMPORTACION,
                                    Fecha_Servicio = leaseplan.FECHA_SERVICIO,
                                    Impuesto = leaseplan.IMPUESTO,
                                    Imp_Circulacion = leaseplan.IMP_CIRCULACION,
                                    Imp_Circulacion_IVA = leaseplan.IMP_CIRCULACION_IVA,
                                    Imp_Matriculacion = leaseplan.IMP_MATRICULACION,
                                    Imp_Matriculacion_IVA = leaseplan.IMP_MATRICULACION_IVA,
                                    Intereses_Prepagados = leaseplan.IMP_INTERESES_PREPAGADOS,
                                    Intereses_Prepagados_IVA = leaseplan.IMP_INTERESES_PREPAGADOS_IVA,
                                    ITV = leaseplan.IMP_ITV,
                                    ITV_IVA = leaseplan.IMP_ITV_IVA,
                                    Mantenimiento = leaseplan.IMP_MANTENIMIENTO,
                                    Mantenimiento_IVA = leaseplan.IMP_MANTENIMIENTO_IVA,
                                    Matricula = leaseplan.MATRICULA,
                                    Neumaticos = leaseplan.IMP_NEUMATICOS,
                                    Neumaticos_IVA = leaseplan.IMP_NEUMATICOS_IVA,
                                    Num_Factura = leaseplan.FACTURA,
                                    Seguro = leaseplan.IMP_SEGURO,
                                    Seguro_IVA = leaseplan.IMP_SEGURO_IVA,
                                    Sociedad = leaseplan.SOCIEDAD,
                                    Trimestre = leaseplan.TRIMESTRE,
                                    NOMBRE_ARCHIVO_IMPORTACION = nombreArchivo,
                                };

                                unitOfWork.RepositoryT_G_DATOS_LEASING.Insert(datos);
                                unitOfWork.Commit();

                                TotalFilasProcesadasOK++;
                            }
                            else
                            {
                                TotalFilasProcesadasIncidencia++;
                                OnIncidenciaImportDatosLeasing(0, matricula, EnumTipoLineaImportacion.MatriculaInexistente);
                            }
                        }
                        catch (Exception ex)
                        {
                            TotalFilasProcesadasERROR++;
                            OnErrorImportDatosLeasingEventHandler(FilaProcessing, $"[ERROR] {resourceView.ImportacionProcesandoLínea}{cont} - {Paso}. {Environment.NewLine}{Global.GetMessageError(ex)}");

                            Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"<AñadeLEASEPLAN> <ELEMENTO> {cont}, <MATRICULA> {matricula}, {Global.GetMessageError(ex)}");
                            //return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnErrorImportDatosLeasingEventHandler(FilaProcessing, $"[ERROR] {resourceView.ImportacionProcesandoLínea}{FilaProcessing} - {Paso}. {Environment.NewLine}{Global.GetMessageError(ex)}");

                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"<AñadeLEASEPLAN> {Paso}, {Global.GetMessageError(ex)}");
                //return false;
            }

            if (TotalFilasProcesadasOK < 0)
            {
                TotalFilasProcesadasOK = 0;
            }

            return true;
        }

        private Boolean EsCocheDirectivo(string matricula)
        {
            bool valorReturn = false;

            var tipoVehiculo = vehiculosImportados.FirstOrDefault(x => x.Matricula == matricula);

            if (tipoVehiculo == null)
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    tipoVehiculo = (from v in unitOfWork.RepositoryECAR_Datos_Vehiculo.Fetch().Where(o => o.Matricula == matricula)
                                    select v).FirstOrDefault();

                    if (tipoVehiculo != null)
                    {
                        valorReturn = (tipoVehiculo.Directivo == null ? (tipoVehiculo.Tipo_Vehiculo == null ? 0 : (tipoVehiculo.Tipo_Vehiculo == GlobalCostes.VEHICULO_DIRECCION ? 1 : 0)) : tipoVehiculo.Directivo) == 1;
                        tipoVehiculo.Directivo = valorReturn ? 1 : 0;
                        vehiculosImportados.Add(tipoVehiculo);
                        if (!MatriculasProcesadas.ContainsKey(matricula))
                        {
                            MatriculasProcesadas.Add(matricula, true);
                        }
                    }
                    else
                    {
                        if (!MatriculasProcesadas.ContainsKey(matricula))
                        {
                            MatriculasProcesadas.Add(matricula, false);
                            TotalFilasProcesadasIncidencia++;
                            OnIncidenciaImportDatosLeasing(FilaProcessing, matricula, EnumTipoLineaImportacion.MatriculaInexistente);
                        }
                    }
                }
            }
            else
            {
                valorReturn = (tipoVehiculo.Directivo == null ? 0 : tipoVehiculo.Directivo) == 1;
            }

            return valorReturn;
        }

        private void EstableceConversionDatos()
        {
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_SEGURO1, "SEGURO");
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_SEGURO2, "SEGURO");
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_MANTENIMIENTO1, "MANTENIMIENTO");
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_MANTENIMIENTO2, "MANTENIMIENTO");
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_MANTENIMIENTO3, "MANTENIMIENTO");
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_MANTENIMIENTO4, "MANTENIMIENTO");
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_MANTENIMIENTO5, "MANTENIMIENTO");
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_MANTENIMIENTO6, "MANTENIMIENTO");
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_MANTENIMIENTO7, "MANTENIMIENTO");
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_ALQUILER1, "ALQUILER");
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_ALQUILER2, "ALQUILER");
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_ADMINISTRACION1, "ADMINISTRACION");
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_ADMINISTRACION2, "ADMINISTRACION");
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_ADMINISTRACION3, "ADMINISTRACION");

            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_RENTING, "ALQUILER");
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_MANTENIMIENTO, "MANTENIMIENTO");
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_SEGURO, "SEGURO");
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_ADMINISTRACION, "ADMINISTRACION");
            conversor_codigo_columnaLEASEPLAN.Add(PLAN_CODIGO_ITV, "ITV");

            using (var unitOfWork = new UnitOfWork())
            {
                foreach (T_M_CORRESPONDENCIA_COD_CLI_EMPRESA_TKE item in unitOfWork.RepositoryT_M_CORRESPONDENCIA_COD_CLI_EMPRESA_TKE.Fetch())
                {
                    conversor_codigoClienteLEASEPLAN_Sociedad.Add(item.CODIGO_CLIENTE, item.CODIGO_EMPRESA_TKE);
                }
            }

        }


        #endregion métodos comunes

        #endregion


        #region Métodos publicos
        public bool Archivo_LEASING_ImportadoConAnterioridad(string nombreArchivo, int empresa)
        {
            bool valorReturn = false;

            T_G_DATOS_LEASINGSpecification spec = new T_G_DATOS_LEASINGSpecification
            {
                Sociedad = empresa,
                NOMBRE_ARCHIVO_IMPORTACION = nombreArchivo,
            };

            using (var unitOfWork = new UnitOfWork())
            {
                valorReturn = unitOfWork.RepositoryT_G_DATOS_LEASING.Where(spec).Count() > 0;
            }

            return valorReturn;
        }

        public bool BorrarDatosImportadosConAnterioridad(string nombreArchivo)
        {
            bool valorReturn = true;

            valorReturn = new BorradoImportacionService().BorraDatosImportacion(EnumTipoBorradoImportacion.BorrarImportacionCombustible, nombreArchivo);

            return valorReturn;
        }
        #endregion Métodos publicos

        #region eventos
        private void OnProcessingImportDatosLeasingEventHandler(int lineProsessing, int TotalLinesToProcess, string msg)
        {
            EventProcessingImportDatosLeasing?.Invoke(lineProsessing, TotalLinesToProcess, msg);
        }

        private void OnSubProcessingImportDatosLeasingEventHandler(int lineProsessing, int TotalLinesToProcess, string msg)
        {
            EventSubProcessingImportDatosLeasing?.Invoke(lineProsessing, TotalLinesToProcess, msg);
        }

        private void OnErrorImportDatosLeasingEventHandler(int lineProsessing, string msgError)
        {
            EventErrorImportDatosLeasing?.Invoke(lineProsessing, msgError);
        }

        private void OnFinishedImportDatosLeasingEventHandler(int TotalLinesToProcess, int TotalLinesProcessedOK, int TotalFilasProcesadasIncidencia, int TotalLinesProcessedERROR)
        {
            EventFinishedImportDatosLeasing?.Invoke(TotalLinesToProcess, TotalLinesProcessedOK, TotalFilasProcesadasIncidencia, TotalLinesProcessedERROR);
        }

        private void OnIncidenciaImportDatosLeasing(int lineProsessing, string matricula, EnumTipoLineaImportacion tipoIncidencia)
        {
            EventIncidenciaImportDatosLeasing?.Invoke(lineProsessing, matricula, tipoIncidencia);
        }
    }

    #endregion eventos

}
