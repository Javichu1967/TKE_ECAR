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
using TK_ECAR.Framework.Utils;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;
using TK_ECAR.Models.Portugal;
using TK_ECAR.Utils;
using resourceView = TK_ECAR.Content.resources.TK_ECAR_Resource;

namespace TK_ECAR.PortugalImportacion.ApplicationServices
{
    public class ImportacionPortugalService
    {
        #region Constantes
        private Dictionary<string, string> conversor_codigo_columnaLEASEPLAN = new Dictionary<string, string>();
        private Dictionary<string, bool> MatriculasProcesadas = new Dictionary<string, bool>();

        private const int PLAN_INICIO_FACTURA = 1;
        private const int PLAN_CARACTERES_FACTURA = 17;
        private const int PLAN_INICIO_F_FACTURA = 18;
        private const int PLAN_CARACTERES_F_FACTURA = 17;
        private const int PLAN_INICIO_MATRICULA = 35;
        private const int PLAN_CARACTERES_MATRICULA = 6;
        private const int PLAN_INICIO_CODIGO = 81;
        private const int PLAN_CARACTERES_CODIGO = 3;
        private const int PLAN_INICIO_VALOR_BASE = 84;
        private const int PLAN_CARACTERES_VALOR_BASE = 13;
        private const int PLAN_INICIO_IVA_BASE = 97;
        private const int PLAN_CARACTERES_IVA_BASE = 13;
        private const int PLAN_INICIO_VALOR_BASE_CON_IVA = 110;
        private const int PLAN_CARACTERES_VALOR_BASE_CON_IVA = 11;

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

        #endregion

        #region Eventos Importación
        //Definimos evento.
        public delegate void ProcessingImportDatosPortugalEventHandler(int lineProsessing, int TotalLinesToProcess, string msg);
        //Evento de procesado de archivo.
        public event ProcessingImportDatosPortugalEventHandler EventProcessingImportDatosPortugal;

        public delegate void SubProcessingImportDatosPortugalEventHandler(int lineProsessing, int TotalLinesToProcess, string msg);
        public event SubProcessingImportDatosPortugalEventHandler EventSubProcessingImportDatosPortugal;

        public delegate void ErrorImportDatosPortugalEventHandler(int lineaProsessing, string msgError);
        public event ErrorImportDatosPortugalEventHandler EventErrorImportDatosPortugal;

        public delegate void IncidenciaImportDatosPortugalEventHandler(int lineaProsessing, string matricula, EnumTipoLineaImportacion tipoIncidencia);
        public event IncidenciaImportDatosPortugalEventHandler EventIncidenciaImportDatosPortugal;

        public delegate void FinishedImportDatosPortugalEventHandler(int TotalLinesToProcess, int TotalLinesProcessedOK, int TotalFilasProcesadasIncidencia, int TotalLinesProcessedERROR);
        public event FinishedImportDatosPortugalEventHandler EventFinishedImportDatosPortugal;
        #endregion Eventos Importación

        //private const int VEHICULO_DIRECCION = 8;

        private string Paso = string.Empty;
        private int FilaProcessing = 0;
        private int TotalFilas = 0;
        private int TotalFilasProcesadasOK = 0;
        private int TotalFilasProcesadasExistentes = 0;
        private int TotalFilasProcesadasIncidencia = 0;
        private int TotalFilasProcesadasERROR = 0;

        #region ViaVerde XML

        public void ProcessViaVerde(ImportacionDatosModels modelo)
        {
            string fileToImport = string.Empty;
            int empresa = 0;
            string msg = string.Empty;

            Paso = string.Empty;
            FilaProcessing = 0;

            //FileStream templateDocumentStream = null;

            var nombreArchivo = FileUtilities.GetFileName(modelo.FileToImport);
            fileToImport = HttpContext.Current.Server.MapPath($"{Global.PathToPathToImportFilesViaVerde}{nombreArchivo}");
            empresa = Convert.ToInt32(modelo.IDEmpresa);

            try
            {
                OnProcessingImportDatosPortugalEventHandler(0, 0, $"{resourceView.ImportacionComienza} {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
                OnProcessingImportDatosPortugalEventHandler(0, 0, $"{resourceView.ImportacionComienzaSerialize} {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
                Paso = $"Llama a SerializeViaVerde {fileToImport}";
                EXTRACTO DatosImportar = SerializeViaVerde(fileToImport);
                DatosImportar.NOMBRE_ARCHIVO_IMPORTACION = nombreArchivo;
                OnProcessingImportDatosPortugalEventHandler(0, 0, $"{resourceView.ImportacionFinalizaSerialize} {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
                Paso = $"Llama a Actualiza_E_CAR";
                Actualiza_E_CAR(DatosImportar);
            }
            catch (Exception ex)
            {
                OnErrorImportDatosPortugalEventHandler(FilaProcessing, $"[ERROR] {resourceView.ImportacionProcesandoLínea}{FilaProcessing} - {Paso}. {Environment.NewLine}{Global.GetMessageError(ex)}");
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"<ProcessViaVerde...> {Paso}, {Global.GetMessageError(ex)}");
            }

            finally
            {
                OnFinishedImportDatosPortugalEventHandler(TotalFilas, TotalFilasProcesadasOK, TotalFilasProcesadasIncidencia, TotalFilasProcesadasERROR);

                Global.EscribeLogApp(Global.TipoDeLog.INFO, $"<Finalizado el proceso de importación Via Verde... {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
                Global.EscribeLogApp(Global.TipoDeLog.INFO, $"********************************************************************************************");
            }
        }

        private EXTRACTO SerializeViaVerde(string fileToImport)
        {
            Paso = $"Entra en SerializeViaVerde {fileToImport}";
            OnProcessingImportDatosPortugalEventHandler(0, 0, resourceView.ImportacionAbriendoArchivo);
            Paso = $"ProcessViaVerde abriendo archivo {fileToImport}";

            EXTRACTO DatosImportar = new EXTRACTO();
            try
            {

                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-PT");
                List<string> mesesPortugues = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.ToList();
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                //List<string> mesesEspañol = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.ToList();

                Paso = $" <Cargando fichero {fileToImport}> ";
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(fileToImport);
                XmlNode nodeExtracto = xDoc.SelectNodes("EXTRACTO").Item(0);
                DatosImportar.IDENTIFICADOR = new List<Identificador>();
                DatosImportar.id = nodeExtracto.Attributes[0].Value; //id
                TotalFilas = nodeExtracto.ChildNodes.Count;
                foreach (XmlNode node in nodeExtracto.ChildNodes)
                {
                    FilaProcessing++;
                    TotalFilasProcesadasExistentes++;
                    OnProcessingImportDatosPortugalEventHandler(FilaProcessing, nodeExtracto.ChildNodes.Count, $"{resourceView.ImportacionProcesandoLínea}");
                    Paso = $" <foreach (XmlNode node in nodeExtracto.ChildNodes)> <NODO {node.LocalName.ToUpper()}> ";
                    switch (node.LocalName.ToUpper())
                    {
                        case "MES_EMISSAO":
                            //DatosImportar.MES_EMISSAO = mesesEspañol.ElementAt(mesesPortugues.FindIndex(x=> x.StartsWith(node.InnerText.ToLower().Substring(0,3))))
                            //                            + node.InnerText.ToLower().Substring(3);
                            //var mes = mesesPortugues.Where(x => x.StartsWith(node.InnerText.ToLower().Substring(0, 3))).FirstOrDefault();
                            //mes.First().ToString().ToUpper() + mes.Substring(1); //Convertir la primera letra en mayúscula.
                            DatosImportar.MES_EMISSAO = mesesPortugues.FindIndex(x => x.StartsWith(node.InnerText.ToLower().Substring(0, 3))) + 1;
                            DatosImportar.AÑO_EMISSAO = Convert.ToInt16(node.InnerText.ToLower().Substring(3).Replace("-", ""));
                            break;
                        case "CLIENTE":
                            RellenaCliente(DatosImportar, node);
                            break;
                        case "IDENTIFICADOR":
                            DatosImportar.IDENTIFICADOR.Add(DevuelveIdentificador(DatosImportar, node));
                            break;
                        case "TOTAL":
                            DatosImportar.TOTAL = Convert.ToDecimal(node.InnerText);
                            break;
                        case "TOTAL_IVA":
                            DatosImportar.TOTAL_IVA = Convert.ToDecimal(node.InnerText);
                            break;
                    }
                    TotalFilasProcesadasOK++;
                }
            }
            catch (Exception ex)
            {
                OnErrorImportDatosPortugalEventHandler(FilaProcessing, $"[ERROR] {resourceView.ImportacionProcesandoLínea}{FilaProcessing} - {Paso}. {Environment.NewLine}{Global.GetMessageError(ex)}");
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"<SerializeViaVerde> {Paso}, {Global.GetMessageError(ex)}");
                TotalFilasProcesadasERROR++;
            }

            return DatosImportar;
        }

        private void RellenaCliente(EXTRACTO DatosImportar, XmlNode node)
        {
            DatosImportar.Cliente = new CLIENTE();
            DatosImportar.Cliente.id = node.Attributes[0].Value; //id
            foreach (XmlNode nodeCliente in node.ChildNodes)
            {
                switch (nodeCliente.LocalName.ToUpper())
                {
                    case "NIF":
                        DatosImportar.Cliente.NIF = nodeCliente.InnerText;
                        break;
                    case "NOME":
                        DatosImportar.Cliente.NOME = nodeCliente.InnerText;
                        break;
                    case "MORADA":
                        DatosImportar.Cliente.MORADA = nodeCliente.InnerText;
                        break;
                    case "LOCALIDADE":
                        DatosImportar.Cliente.LOCALIDADE = nodeCliente.InnerText;
                        break;
                    case "CODIGO_POSTAL":
                        DatosImportar.Cliente.CODIGO_POSTAL = nodeCliente.InnerText;
                        break;
                }

            }
        }

        private Identificador DevuelveIdentificador(EXTRACTO DatosImportar, XmlNode node)
        {
            Identificador myIdentificador = new Identificador();
            myIdentificador.id = node.Attributes[0].Value; //id
            myIdentificador.TRANSACCAO = new List<TRANSACCIONES>();
            foreach (XmlNode nodeIdentificador in node.ChildNodes)
            {
                switch (nodeIdentificador.LocalName.ToUpper())
                {
                    case "MATRICULA":
                        myIdentificador.MATRICULA = nodeIdentificador.InnerText;
                        break;
                    case "REF_PAGAMENTO":
                        myIdentificador.REF_PAGAMENTO = nodeIdentificador.InnerText;
                        break;
                    case "TRANSACCAO":
                        myIdentificador.TRANSACCAO.Add(DevuelveTransaccion(nodeIdentificador));
                        break;
                    case "TOTAL":
                        myIdentificador.TOTAL = Convert.ToDecimal(nodeIdentificador.InnerText);
                        break;
                }

            }
            return myIdentificador;
        }


        private TRANSACCIONES DevuelveTransaccion(XmlNode node)
        {
            var Paso = "";

            TRANSACCIONES Transaccion = new TRANSACCIONES();

            try
            {
                foreach (XmlNode nodeTransaccion in node.ChildNodes)
                {
                    Paso = $" <foreach (XmlNode nodeTransaccion in node.ChildNodes)> <NODO {nodeTransaccion.LocalName.ToUpper()}> ";
                    switch (nodeTransaccion.LocalName.ToUpper())
                    {
                        case "DATA_ENTRADA":
                            Transaccion.DATA_ENTRADA = DevuelveFechaDeString(nodeTransaccion.InnerText);
                            break;
                        case "HORA_ENTRADA":
                            Transaccion.HORA_ENTRADA = nodeTransaccion.InnerText;
                            break;
                        case "ENTRADA":
                            Transaccion.ENTRADA = nodeTransaccion.InnerText;
                            break;
                        case "DATA_SAIDA":
                            Transaccion.DATA_SAIDA = DevuelveFechaDeString(nodeTransaccion.InnerText);
                            break;
                        case "HORA_SAIDA":
                            Transaccion.HORA_SAIDA = nodeTransaccion.InnerText;
                            break;
                        case "SAIDA":
                            Transaccion.SAIDA = nodeTransaccion.InnerText;
                            break;
                        case "IMPORTANCIA":
                            Transaccion.IMPORTANCIA = Convert.ToDecimal(nodeTransaccion.InnerText);
                            break;
                        case "VALOR_DESCONTO":
                            Transaccion.VALOR_DESCONTO = Convert.ToDecimal(nodeTransaccion.InnerText);
                            break;
                        case "TAXA_IVA":
                            Transaccion.TAXA_IVA = Convert.ToInt16(nodeTransaccion.InnerText);
                            break;
                        case "OPERADOR":
                            Transaccion.OPERADOR = nodeTransaccion.InnerText;
                            break;
                        case "TIPO":
                            Transaccion.TIPO = nodeTransaccion.InnerText;
                            break;
                        case "DATA_DEBITO":
                            Transaccion.DATA_DEBITO = DevuelveFechaDeString(nodeTransaccion.InnerText);
                            break;
                        case "CARTAO":
                            Transaccion.CARTAO = nodeTransaccion.InnerText;
                            break;
                    }

                }
            }
            catch(Exception ex)
            {
                OnErrorImportDatosPortugalEventHandler(FilaProcessing, $"[ERROR] {resourceView.ImportacionProcesandoLínea}{FilaProcessing} - {Paso}. {Environment.NewLine}{Global.GetMessageError(ex)}");
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"<DevuelveTransaccion> {Paso}, {Global.GetMessageError(ex)}");
                throw ex;
            }

            return Transaccion;
        }


        private DateTime? DevuelveFechaDeString(string fecha)
        {
            DateTime? returnValue = null;

            if (!string.IsNullOrEmpty(fecha) && fecha.ToLower() != "null")
            {
                returnValue = Convert.ToDateTime(fecha);
            }

            return returnValue;
        }

        #region Actualizar tablas E-CAR
        private bool Actualiza_E_CAR(EXTRACTO DatosImportar)
        {
            TotalFilas = DatosImportar.IDENTIFICADOR.Count;
            TotalFilasProcesadasERROR = 0;
            TotalFilasProcesadasExistentes = 0;
            TotalFilasProcesadasOK = 0;
            OnProcessingImportDatosPortugalEventHandler(0, DatosImportar.IDENTIFICADOR.Count, $"{resourceView.ImportacionComienzaActualizacionBBDD} {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
            var miFlotaService = new FlotaService();
            var Paso = "using (var unitOfWork = new UnitOfWork())";
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    Paso = "Tratando cliente";
                    var cliente = unitOfWork.RepositoryT_M_CLIENTES.Fetch().Where(x => x.ID_CLIENTE == DatosImportar.Cliente.id).FirstOrDefault();
                    if (cliente == null)
                    {
                        T_M_CLIENTES clienteAlta = new T_M_CLIENTES
                        {
                            ID_CLIENTE = DatosImportar.Cliente.id,
                            NIF = DatosImportar.Cliente.NIF,
                            NOMBRE = DatosImportar.Cliente.NOME,
                            DIRECCION = DatosImportar.Cliente.MORADA,
                            LOCALIDAD = DatosImportar.Cliente.LOCALIDADE,
                            CODIGO_POSTAL = DatosImportar.Cliente.CODIGO_POSTAL,
                        };
                        unitOfWork.RepositoryT_M_CLIENTES.Insert(clienteAlta);
                        unitOfWork.Commit();
                    }

                    Paso = "Tratando extracto";

                    var extracto = new T_G_VIA_VERDE_EXTRACTOS();
                    extracto = unitOfWork.RepositoryT_G_VIA_VERDE_EXTRACTOS.Fetch().Where(x => x.ID_EXTRACTO == DatosImportar.id).FirstOrDefault();
                    if (extracto == null)
                    {
                        extracto = new T_G_VIA_VERDE_EXTRACTOS
                        {
                            ID_EXTRACTO = DatosImportar.id,
                            AÑO_EMISION = DatosImportar.AÑO_EMISSAO,
                            ID_CLIENTE = DatosImportar.Cliente.id,
                            MES_EMISION = DatosImportar.MES_EMISSAO,
                            TOTAL = DatosImportar.TOTAL,
                            TOTAL_IVA = DatosImportar.TOTAL_IVA,
                            NOMBRE_ARCHIVO_IMPORTACION = DatosImportar.NOMBRE_ARCHIVO_IMPORTACION,
                        };
                        unitOfWork.RepositoryT_G_VIA_VERDE_EXTRACTOS.Insert(extracto);
                    }
                    else
                    {
                        extracto.AÑO_EMISION = DatosImportar.AÑO_EMISSAO;
                        extracto.ID_CLIENTE = DatosImportar.Cliente.id;
                        extracto.MES_EMISION = DatosImportar.MES_EMISSAO;
                        extracto.TOTAL = DatosImportar.TOTAL;
                        extracto.TOTAL_IVA = DatosImportar.TOTAL_IVA;
                        extracto.NOMBRE_ARCHIVO_IMPORTACION = DatosImportar.NOMBRE_ARCHIVO_IMPORTACION;
                        unitOfWork.RepositoryT_G_VIA_VERDE_EXTRACTOS.Update(extracto);
                    }
                    unitOfWork.Commit();

                    Global.EscribeLogApp(Global.TipoDeLog.INFO, $"<EXTRACTO> <ID_EXTRACTO> {extracto.ID_EXTRACTO}, <TOTAL> {extracto.TOTAL} {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");

                    var contIden = 0;
                    foreach (Identificador identificador in DatosImportar.IDENTIFICADOR)
                    {
                        contIden++;
                        TotalFilasProcesadasExistentes++;
                        OnProcessingImportDatosPortugalEventHandler(contIden, DatosImportar.IDENTIFICADOR.Count, $"{resourceView.ImportacionProcesandoLínea} MATRICULA ({identificador.MATRICULA}) REF.PAGAMENTO ({identificador.REF_PAGAMENTO})");
                        Paso = "Tratando IDENTIFICADOR";
                        try
                        {
                            Global.EscribeLogApp(Global.TipoDeLog.INFO, $"<IDENTIFICADOR> {contIden} de {DatosImportar.IDENTIFICADOR.Count()} <MATRICULA> {identificador.MATRICULA} , <REF_PAGAMENTO> {identificador.REF_PAGAMENTO} <FECHA IMPORTACIÓN> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
                            var indetificadorProcess = new T_G_VIA_VERDE_IDENTIFICADORES();

                            indetificadorProcess = unitOfWork.RepositoryT_G_VIA_VERDE_IDENTIFICADORES.Fetch()
                                                    .Where(x => x.ID_EXTRACTO == DatosImportar.id && x.IDENTIFICADOR == identificador.id)
                                                    .FirstOrDefault();
                            if (indetificadorProcess == null)
                            {
                                //Comprobamos que la matrícula exista.
                                if (!MatriculasProcesadas.ContainsKey(identificador.MATRICULA))
                                {
                                    if (!miFlotaService.VehiculoExistente(identificador.MATRICULA))
                                    {
                                        MatriculasProcesadas.Add(identificador.MATRICULA, false);
                                        TotalFilasProcesadasIncidencia++;
                                        OnIncidenciaImportDatosPortugal(0, identificador.MATRICULA, EnumTipoLineaImportacion.MatriculaInexistente);
                                    }
                                    else
                                    {
                                        MatriculasProcesadas.Add(identificador.MATRICULA, true);
                                    }
                                }

                                indetificadorProcess = new T_G_VIA_VERDE_IDENTIFICADORES
                                {
                                    IDENTIFICADOR = identificador.id,
                                    ID_EXTRACTO = DatosImportar.id,
                                    MATRICULA = identificador.MATRICULA,
                                    REF_PAGO = identificador.REF_PAGAMENTO,
                                    TOTAL = identificador.TOTAL,
                                };

                                unitOfWork.RepositoryT_G_VIA_VERDE_IDENTIFICADORES.Insert(indetificadorProcess);
                                unitOfWork.Commit();
                            }
                            else
                            {//BORRAR TODO LO DEL IDENTIFICADOR, PARA VOLVER A IMPORTARLO.
                                List<T_G_VIA_VERDE_TRANSACCIONES> query = (from t in unitOfWork.RepositoryT_G_VIA_VERDE_TRANSACCIONES.Fetch()
                                            where t.ID_IDENTIFICADOR == indetificadorProcess.ID_IDENTIFICADOR
                                            select t).ToList();
                                if (query.Count() > 0)
                                {
                                    Global.EscribeLogApp(Global.TipoDeLog.INFO, $"<SE BORRAN TODODAS LAS TRANSACCIONES DEL IDENTIFICADOR POR YA EXISTIR EN BBDD> <IDENTIFICADOR> {contIden} de {DatosImportar.IDENTIFICADOR.Count()}  <MATRICULA> {identificador.MATRICULA} , <REF_PAGAMENTO> {identificador.REF_PAGAMENTO}");
                                    foreach (T_G_VIA_VERDE_TRANSACCIONES transaccionToDelete in query)
                                    {
                                        unitOfWork.RepositoryT_G_VIA_VERDE_TRANSACCIONES.Delete(transaccionToDelete);
                                    }
                                    unitOfWork.Commit();
                                }
                            }

                            Global.EscribeLogApp(Global.TipoDeLog.INFO, $"<INICIO TRANSACCIONES> {identificador.TRANSACCAO.Count()} <FECHA IMPORTACIÓN> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");

                            var contTran = 0;
                            foreach (TRANSACCIONES transaccion in identificador.TRANSACCAO)
                            {
                                Paso = "TRANSACCAO";
                                contTran++;

                                OnSubProcessingImportDatosPortugalEventHandler(contTran, identificador.TRANSACCAO.Count, $"{resourceView.ImportacionProcesandoLínea} IDENTFICADOR ({indetificadorProcess.ID_IDENTIFICADOR}) OPERADOR ({transaccion.OPERADOR})");

                                T_G_VIA_VERDE_TRANSACCIONES transaccionAñadir = new T_G_VIA_VERDE_TRANSACCIONES
                                {
                                    ID_IDENTIFICADOR = indetificadorProcess.ID_IDENTIFICADOR,
                                    DECUENTO = transaccion.VALOR_DESCONTO,
                                    FECHA_ENTRADA = transaccion.DATA_ENTRADA_COMPLETA,
                                    FECHA_SALIDA = transaccion.DATA_SAIDA_COMPLETA,
                                    FECHA_TARJETA = transaccion.DATA_DEBITO,
                                    IMPORTE = transaccion.IMPORTANCIA,
                                    LUGAR_ENTRADA = transaccion.ENTRADA,
                                    LUGAR_SALIDA = transaccion.SAIDA,
                                    NUM_TARJETA = transaccion.CARTAO,
                                    OPERADOR = transaccion.OPERADOR,
                                    PORCENTAJE_IMPUESTO = transaccion.TAXA_IVA,
                                    TIPO = transaccion.TIPO,

                                };
                                unitOfWork.RepositoryT_G_VIA_VERDE_TRANSACCIONES.Insert(transaccionAñadir);
                                unitOfWork.Commit();
                            }
                            OnSubProcessingImportDatosPortugalEventHandler(0, 0, "");
                            Global.EscribeLogApp(Global.TipoDeLog.INFO, $"<FINALIZADAS TRANSACCIONES> {identificador.TRANSACCAO.Count()} <FECHA IMPORTACIÓN> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
                        }
                        catch (Exception ex)
                        {
                            string mensaje = $" <Actualiza_E_CAR>. PASO ({Paso}), IDENTIFICADOR {identificador.MATRICULA}, REF_PAGAMENTO {identificador.REF_PAGAMENTO} ";
                            Global.EscribeLogApp(Global.TipoDeLog.ERROR, mensaje + Global.GetMessageError(ex));
                            OnErrorImportDatosPortugalEventHandler(FilaProcessing, $"[ERROR] {mensaje.Replace("<", "-").Replace(">", "-")}. {Environment.NewLine}{Global.GetMessageError(ex)}");
                            TotalFilasProcesadasERROR++;
                        }
                        TotalFilasProcesadasOK++;
                    }
                }
            }
            catch (Exception ex)
            {
                OnErrorImportDatosPortugalEventHandler(FilaProcessing, $"[ERROR] -Actualiza_E_CAR PASO- {Paso}. {Environment.NewLine}{Global.GetMessageError(ex)}");
                Global.EscribeLogApp(Global.TipoDeLog.ERROR,  $"<Actualiza_E_CAR> PASO {Paso}, {Global.GetMessageError(ex)}");
                TotalFilasProcesadasERROR++;
                return false;
            }


            return true;
        }
        #endregion

        public bool Archivo_VIAVERDE_ImportadoConAnterioridad(string nombreArchivo)
        {
            bool valorReturn = false;

            T_G_VIA_VERDE_EXTRACTOSSpecification spec = new T_G_VIA_VERDE_EXTRACTOSSpecification
            {
                NOMBRE_ARCHIVO_IMPORTACION = nombreArchivo,
            };

            using (var unitOfWork = new UnitOfWork())
            {
                valorReturn = unitOfWork.RepositoryT_G_VIA_VERDE_EXTRACTOS.Where(spec).Count() > 0;
            }

            return valorReturn;
        }        
        #endregion



        public bool BorrarDatosImportadosConAnterioridad(string nombreArchivo)
        {
            bool valorReturn = true;

            valorReturn = new BorradoImportacionService().BorraDatosImportacion(EnumTipoBorradoImportacion.BorrarImportacionCombustible, nombreArchivo);

            //T_G_TARJETA_COMBUSTIBLESpecification spec = new T_G_TARJETA_COMBUSTIBLESpecification
            //{
            //    NOMBRE_ARCHIVO_IMPORTACION = nombreArchivo,
            //};

            //using (var unitOfWork = new UnitOfWork())
            //{
            //    unitOfWork.RepositoryT_G_TARJETA_COMBUSTIBLE.RemoveRange(unitOfWork.RepositoryT_G_TARJETA_COMBUSTIBLE.Where(spec).ToList());
            //    unitOfWork.Commit();
            //}

            return valorReturn;
        }

        private Boolean EsCocheDirectivo(string matricula)
        {
            bool valorReturn = false;

            using (var unitOfWork = new UnitOfWork())
            {
                var tipoVehiculo = (from v in unitOfWork.RepositoryECAR_Datos_Vehiculo.Fetch().Where(o => o.Matricula == matricula)
                                select v.Tipo_Vehiculo).FirstOrDefault();

                if (tipoVehiculo != null)
                {
                    valorReturn = tipoVehiculo == GlobalCostes.VEHICULO_DIRECCION;
                }
            }
            return valorReturn;
        }

        #region eventos
        private void OnProcessingImportDatosPortugalEventHandler(int lineProsessing, int TotalLinesToProcess, string msg)
        {
            EventProcessingImportDatosPortugal?.Invoke(lineProsessing, TotalLinesToProcess, msg);
        }

        private void OnSubProcessingImportDatosPortugalEventHandler(int lineProsessing, int TotalLinesToProcess, string msg)
        {
            EventSubProcessingImportDatosPortugal?.Invoke(lineProsessing, TotalLinesToProcess, msg);
        }

        private void OnErrorImportDatosPortugalEventHandler(int lineProsessing, string msgError)
        {
            EventErrorImportDatosPortugal?.Invoke(lineProsessing, msgError);
        }

        private void OnFinishedImportDatosPortugalEventHandler(int TotalLinesToProcess, int TotalLinesProcessedOK, int TotalFilasProcesadasIncidencia, int TotalLinesProcessedERROR)
        {
            EventFinishedImportDatosPortugal?.Invoke(TotalLinesToProcess, TotalLinesProcessedOK, TotalFilasProcesadasIncidencia, TotalLinesProcessedERROR);
        }

        private void OnIncidenciaImportDatosPortugal(int lineProsessing, string matricula, EnumTipoLineaImportacion tipoIncidencia)
        {
            EventIncidenciaImportDatosPortugal?.Invoke(lineProsessing, matricula, tipoIncidencia);
        }
    }

    #endregion eventos

}
