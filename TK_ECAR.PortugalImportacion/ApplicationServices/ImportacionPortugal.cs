using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK_ECAR.Domain;
using TK_ECAR.Infraestructure;
using TK_ECAR.Framework;
using log4net;
using System.Xml.Serialization;
using System.IO;
using TK_ECAR.PortugalImportacion.Models;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;
using System.Threading;
using OfficeOpenXml;
using TK_ECAR.Utils;
using TK_ECAR.PortugalImportacion.Global;

namespace TK_ECAR.PortugalImportacion.ApplicationServices
{
    class ImportacionPortugal
    {
        #region Constantes
        private Dictionary<string, string> conversor_codigo_columnaLEASEPLAN = new Dictionary<string, string>();

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

        private const int VEHICULO_DIRECCION = 8;

        #region ViaVerde XML
        public EXTRACTO SerializeViaVerde(string fileToImport)
        {
            EXTRACTO DatosImportar = new EXTRACTO();
            var Paso = "";
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
                foreach (XmlNode node in nodeExtracto.ChildNodes)
                {
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

                }

                //string xmlString = System.IO.File.ReadAllText("C://Workspaces//Projects//TK_ECAR//TK_ECAR.PortugalImportacion//ArchivosImportacion//EXTRACTO_48049369_09_2016.xml");

                //xmlString = xmlString.Substring(45);

                //XmlSerializer serializer = new XmlSerializer(typeof(EXTRACTO));
                //StringReader rdr = new StringReader(xmlString);  // “texto” es el XML pasado a una variable
                //EXTRACTO objResultante = (EXTRACTO)serializer.Deserialize(rdr);


                //XmlSerializer serializer = new XmlSerializer(typeof(schema));

                //StreamReader readerViaVerde = new StreamReader("C://Workspaces//Projects//TK_ECAR//TK_ECAR.PortugalImportacion//ArchivosImportacion//EXTRACTO_48049369_09_2016.xml");
                //readerViaVerde.BaseStream.Position = 0;
                //var deserializedEMSConfig = serializer.Deserialize(readerViaVerde) as schema;
                //readerViaVerde.Close();

            }
            catch (Exception ex)
            {
                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"<SerializeViaVerde> {Paso}, {ex.Message}");
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
                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"<DevuelveTransaccion> {Paso}, {ex.Message}");
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
        public bool Actualiza_E_CAR(EXTRACTO DatosImportar)
        {
            var Paso = "using (var unitOfWork = new UnitOfWork())";
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    Paso = "cliente";
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

                    Paso = "extracto";
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
                        };
                        unitOfWork.RepositoryT_G_VIA_VERDE_EXTRACTOS.Insert(extracto);
                        unitOfWork.Commit();
                    }

                    GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<EXTRACTO> <ID_EXTRACTO> {extracto.ID_EXTRACTO}, <TOTAL> {extracto.TOTAL} {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");

                    var contIden = 0;
                    foreach (Identificador identificador in DatosImportar.IDENTIFICADOR)
                    {
                        contIden++;
                        Paso = "IDENTIFICADOR";
                        try
                        {
                            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<IDENTIFICADOR> {contIden} de {DatosImportar.IDENTIFICADOR.Count()}  <MATRICULA> {identificador.MATRICULA} , <REF_PAGAMENTO> {identificador.REF_PAGAMENTO} <FECHA IMPORTACIÓN> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
                            var indetificadorProcess = new T_G_VIA_VERDE_IDENTIFICADORES();

                            indetificadorProcess = unitOfWork.RepositoryT_G_VIA_VERDE_IDENTIFICADORES.Fetch()
                                                    .Where(x => x.ID_EXTRACTO == DatosImportar.id && x.IDENTIFICADOR == identificador.id)
                                                    .FirstOrDefault();
                            if (indetificadorProcess == null)
                            {
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
                                    GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<SE BORRAN TODODAS LAS TRANSACCIONES DEL IDENTIFICADOR POR YA EXISTIR EN BBDD> <IDENTIFICADOR> {contIden} de {DatosImportar.IDENTIFICADOR.Count()}  <MATRICULA> {identificador.MATRICULA} , <REF_PAGAMENTO> {identificador.REF_PAGAMENTO}");
                                    foreach (T_G_VIA_VERDE_TRANSACCIONES transaccionToDelete in query)
                                    {
                                        unitOfWork.RepositoryT_G_VIA_VERDE_TRANSACCIONES.Delete(transaccionToDelete);
                                    }
                                    unitOfWork.Commit();
                                }
                            }

                            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<INICIO TRANSACCIONES> {identificador.TRANSACCAO.Count()} <FECHA IMPORTACIÓN> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");

                            var contTran = 0;
                            foreach (TRANSACCIONES transaccion in identificador.TRANSACCAO)
                            {
                                Paso = "TRANSACCAO";
                                contTran++;

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
                            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<FINALIZADAS TRANSACCIONES> {identificador.TRANSACCAO.Count()} <FECHA IMPORTACIÓN> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
                        }
                        catch (Exception ex)
                        {
                            string mensaje = $" <Actualiza_E_CAR>. PASO ({Paso}), IDENTIFICADOR {identificador.MATRICULA}, REF_PAGAMENTO {identificador.REF_PAGAMENTO} ";
                            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, mensaje + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR,  $"<Actualiza_E_CAR> PASO {Paso}, {ex.Message}");
                return false;
            }


            return true;
        }
        #endregion
        #endregion

        #region GALP

        public bool ImportaDatosGALP(string fileToImport)
        {
            //int COL_Sociedad = 8150;
            //VAR int COL_EJERCICIO = ;
            //int COL_TRIMESTRE1 = 27;
            int COL_TRIMESTRE2 = 28;
            int COL_FECHA_FACTURA = 26;
            int COL_FECHA_OPERACION1 = 30; //COL_FECHA_OPERACION1 + COL_FECHA_OPERACION2
            int COL_FECHA_OPERACION2 = 31;
            int COL_MATRICULA = 21;
            int COL_DES_PRODU = 42;
            int COL_COD_PRODU = 41;
            int COL_KILOMETROS = 37;
            int COL_NUM_LITROS = 43;
            int COL_IMPORTE = 50;
            int COL_BONIF_TOTAL = 48;
            int COL_IVA = 51;
            int COL_IMP_TOTAL1 = 50;
            int COL_IMP_TOTAL2 = 48;
            //int COL_FechaAlta = Datetime.now;
            int COL_KmsCiclo = 38;
            int COL_DOCUMENTO = 40;
            int COL_TARJETA = 4;

            var fileinfo = new FileInfo(fileToImport);

            using (ExcelPackage package = new ExcelPackage(fileinfo))
            {
                int FilaProcess = 0;
                string Paso = "";
                //try
                //{
                //Añadimos la columna de procesamiento
                ExcelWorksheet ws = null;
                ws = package.Workbook.Worksheets[1];
                for (int r = 2; r <= ws.Dimension.End.Row; r++)
                {
                    try
                    {
                        Paso = "ESTABLECIENDO DATO";
                        FilaProcess = r;
                        GALPModels datosGALP = new GALPModels
                        {
                            BONIF_TOTAL = Convert.ToDecimal(ws.Cells[r, COL_BONIF_TOTAL].Value),
                            COD_PRODU = (string)ws.Cells[r, COL_COD_PRODU].Value,
                            DES_PRODU = (string)ws.Cells[r, COL_DES_PRODU].Value,
                            FECHA_FACTURA = Convert.ToDateTime(ws.Cells[r, COL_FECHA_FACTURA].Text),
                            FECHA_OPERACION = Convert.ToDateTime(ws.Cells[r, COL_FECHA_OPERACION1].Text + " " + ws.Cells[r, COL_FECHA_OPERACION2].Text),
                            IMPORTE = Convert.ToDecimal(ws.Cells[r, COL_IMPORTE].Value),
                            IMP_TOTAL = Convert.ToDecimal(ws.Cells[r, COL_IMP_TOTAL1].Value) + Convert.ToDecimal(ws.Cells[r, COL_IMP_TOTAL2].Value),
                            IVA = Convert.ToDecimal(ws.Cells[r, COL_IVA].Value),
                            KILOMETROS = Convert.ToDecimal(ws.Cells[r, COL_KILOMETROS].Value),
                            KmsCiclo = Convert.ToDecimal(ws.Cells[r, COL_KmsCiclo].Value),
                            MATRICULA = (string)ws.Cells[r, COL_MATRICULA].Value,
                            NUM_LITROS = Convert.ToDecimal(ws.Cells[r, COL_NUM_LITROS].Value),
                            NUM_DOCUMENTO = (string)ws.Cells[r, COL_DOCUMENTO].Value,
                            COD_TARJETA = (string)ws.Cells[r, COL_TARJETA].Value,
                        };

                        datosGALP.EJERCICIO = (datosGALP.FECHA_OPERACION.Value.Month >= 10 ?
                                                (datosGALP.FECHA_OPERACION.Value.Year.ToString() + "-" + (datosGALP.FECHA_OPERACION.Value.Year + 1).ToString()) :
                                                ((datosGALP.FECHA_OPERACION.Value.Year - 1).ToString() + "-" + datosGALP.FECHA_OPERACION.Value.Year.ToString()));

                        var mes = Convert.ToDateTime(ws.Cells[r, COL_TRIMESTRE2].Text).Month;
                        if (mes <= 3)
                        {
                            datosGALP.TRIMESTRE = "1º";
                        }
                        else if (mes >= 10)
                        {
                            datosGALP.TRIMESTRE = "4º";
                        }
                        else if (mes > 3 && mes <= 6)
                        {
                            datosGALP.TRIMESTRE = "2º";
                        }
                        else
                        {
                            datosGALP.TRIMESTRE = "3º";
                        }

                        Paso = "ACTUALIZANDO BBDD";
                        ActualizaTarjetaCombustibleECAR(datosGALP);
                    }
                    catch (Exception ex)
                    {
                        GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"<ImportarDatosGALP> <FICHERO> {fileToImport}, FILA {FilaProcess}, PASO {Paso}, {ex.Message}");
                        //return false;
                    }
                }
                //}
                //catch (Exception ex)
                //{
                //    GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"<ImportarDatosGALP> <FICHERO> {fileToImport}, FILA {FilaProcess}, {ex.Message}");
                //    return false;
                //}
            }
            return true;
        }

        private bool ActualizaTarjetaCombustibleECAR(GALPModels datosGALP)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                T_G_TARJETA_COMBUSTIBLE datos = new T_G_TARJETA_COMBUSTIBLE
                {
                    BONIF_TOTAL = (double)datosGALP.BONIF_TOTAL,
                    COD_PRODU = datosGALP.COD_PRODU,
                    DES_PRODU = datosGALP.DES_PRODU,
                    EJERCICIO = datosGALP.EJERCICIO,
                    FechaAlta = DateTime.Now,
                    FECHA_FACTURA = datosGALP.FECHA_FACTURA,
                    FECHA_OPERACION = datosGALP.FECHA_OPERACION,
                    IMPORTE = (double)datosGALP.IMPORTE,
                    IMP_TOTAL = (double)datosGALP.IMP_TOTAL,
                    IVA = (double)datosGALP.IVA,
                    KILOMETROS = (double)datosGALP.KILOMETROS,
                    KmsCiclo = (double)datosGALP.KmsCiclo,
                    NUM_LITROS = (double)datosGALP.NUM_LITROS,
                    MATRICULA = datosGALP.MATRICULA,
                    Sociedad = Constants.CODIGO_EMPRESA_PORTUGAL,
                    TRIMESTRE = datosGALP.TRIMESTRE,
                    COD_TARJETA = datosGALP.COD_TARJETA,
                    NUM_DOCUMENTO =  datosGALP.NUM_DOCUMENTO,
                };

                unitOfWork.RepositoryT_G_TARJETA_COMBUSTIBLE.Insert(datos);
                unitOfWork.Commit();
            }


            return true;
        }

        #endregion

        #region LEASEPLAN
        public bool ImportaDatosLEASEPLAN(string fileToImport)
        {
            try
            {
                var fileinfo = new FileInfo(fileToImport);

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

                List<LeasePlanPortugal> datosLeasePlanPortugal = CargaLeasePlanPortugal(fileinfo.FullName);

                AñadeLEASEPLAN(datosLeasePlanPortugal);
            }
            catch (Exception ex)
            {
                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"<ImportaDatosLEASEPLAN> <FICHERO> {fileToImport}, <PASO> CARGA DATOS, {ex.Message}");
                return false;
            }
            return true;
        }


        private List<LeasePlanPortugal> CargaLeasePlanPortugal(string archivoLeasePlan)
        {
            string line;
            string lineToProcess = "";
            int linea = 0;

            List<LeasePlanPortugal> datosLeasePlan = new List<LeasePlanPortugal>();

            System.IO.StreamReader file = null;

            file = new System.IO.StreamReader(archivoLeasePlan);
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    lineToProcess = line;
                    linea++;
                    LeasePlanPortugal dato = new LeasePlanPortugal();
                    dato = GetLeasePlanPortugalFromLine(line);

                    var itemExistente = datosLeasePlan.Where(o => o.FACTURA == dato.FACTURA &&
                                                            o.FECHA_FACTURA == dato.FECHA_FACTURA &&
                                                            o.MATRICULA == dato.MATRICULA).FirstOrDefault();

                    if (itemExistente == null)
                    {
                        datosLeasePlan.Add(dato);
                    }
                    else
                    {
                        ActualizaImporteElemento(datosLeasePlan.Where(o => o.FACTURA == dato.FACTURA &&
                                                            o.FECHA_FACTURA == dato.FECHA_FACTURA &&
                                                            o.MATRICULA == dato.MATRICULA).FirstOrDefault(), dato);
                    }
                }

                catch (Exception ex)
                {
                    GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"<CargaLeasePlanPortugal> <FICHERO> {archivoLeasePlan}, <Nº LINEA> {linea}, <TEXTO> {lineToProcess}, {ex.Message}");
                    //return false;
                }
            }

            file.Close();

            return datosLeasePlan;
        }


        private LeasePlanPortugal GetLeasePlanPortugalFromLine(string line)
        {
            LeasePlanPortugal leaseplan = new LeasePlanPortugal();

            leaseplan.FACTURA = GetDatoFromLineaLeasePlan(line, PLAN_INICIO_FACTURA, PLAN_CARACTERES_FACTURA, '0');
            leaseplan.FECHA_FACTURA = GetFechaFromLineaLeasePlan(line, PLAN_INICIO_F_FACTURA, PLAN_CARACTERES_F_FACTURA);
            leaseplan.MATRICULA = GetDatoFromLineaLeasePlan(line, PLAN_INICIO_MATRICULA, PLAN_CARACTERES_MATRICULA);
            leaseplan.EJERCICIO = DevuelveEjercicioDeFecha(leaseplan.FECHA_FACTURA);
            leaseplan.TRIMESTRE = DevuelveTrimestreDeFecha(leaseplan.FECHA_FACTURA);
            leaseplan.DIRECTIVO = EsCocheDirectivo(leaseplan.MATRICULA);
            RellenaImporte(line, leaseplan);

            return leaseplan;
        }

        private string GetDatoFromLineaLeasePlan(string line, int inicio, int numCaracteres, char quitarPorLaIzquierda = '?')
        {
            return line.Substring(inicio-1, numCaracteres).Trim().TrimStart(quitarPorLaIzquierda);
        }

        private DateTime GetFechaFromLineaLeasePlan(string line, int inicio, int numCaracteres)
        {
            string valor = line.Substring(inicio - 1, numCaracteres).Trim();
            return Convert.ToDateTime($"{valor.Substring(6)}/{valor.Substring(4, 2)}/{valor.Substring(0, 4)}");
        }

        private string DevuelveEjercicioDeFecha(DateTime fecha)
        {
            return (fecha.Month >= 10 ?
                                    (fecha.Year.ToString() + "-" + (fecha.Year + 1).ToString()) :
                                    ((fecha.Year - 1).ToString() + "-" + fecha.Year.ToString()));
        }


        private string DevuelveTrimestreDeFecha(DateTime fecha)
        {
            var mes = fecha.Month;
            if (mes <= 3)
            {
                return "1º";
            }
            else if (mes >= 10)
            {
                return "4º";
            }
            else if (mes > 3 && mes <= 6)
            {
                return "2º";
            }
            else
            {
                return "3º";
            }

        }

        private void RellenaImporte(string line, LeasePlanPortugal lease)
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

            string codigo = GetDatoFromLineaLeasePlan(line, PLAN_INICIO_CODIGO, PLAN_CARACTERES_CODIGO);
            lease.CONCEPTO = codigo;
            if (conversor_codigo_columnaLEASEPLAN.ContainsKey(codigo))
            {
                string valorBase = GetDatoFromLineaLeasePlan(line, PLAN_INICIO_VALOR_BASE, PLAN_CARACTERES_VALOR_BASE);
                string valorBaseConImpuesto = GetDatoFromLineaLeasePlan(line, PLAN_INICIO_VALOR_BASE_CON_IVA, PLAN_CARACTERES_VALOR_BASE_CON_IVA);
                valorBase = $"{valorBase.Substring(0, PLAN_CARACTERES_VALOR_BASE - 2)},{valorBase.Substring(PLAN_CARACTERES_VALOR_BASE - 2)}";
                valorBaseConImpuesto = $"{valorBaseConImpuesto.Substring(0, PLAN_CARACTERES_VALOR_BASE_CON_IVA - 2)},{valorBaseConImpuesto.Substring(PLAN_CARACTERES_VALOR_BASE_CON_IVA-2)}";

                if (valorBase == valorBaseConImpuesto)
                {
                    lease.IMPUESTO = 0;
                }
                else
                {
                    lease.IMPUESTO = 23;
                }

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
                }
            }
        }

        private void ActualizaImporteElemento(LeasePlanPortugal leaseExistente, LeasePlanPortugal lease)
        {
            leaseExistente.IMP_SEGURO = Math.Round(leaseExistente.IMP_SEGURO + lease.IMP_SEGURO, 2);
            leaseExistente.IMP_SEGURO_IVA = Math.Round(leaseExistente.IMP_SEGURO_IVA + lease.IMP_SEGURO_IVA, 2);
            leaseExistente.IMP_MANTENIMIENTO = Math.Round(leaseExistente.IMP_MANTENIMIENTO + lease.IMP_MANTENIMIENTO,2);
            leaseExistente.IMP_MANTENIMIENTO_IVA = Math.Round(leaseExistente.IMP_MANTENIMIENTO_IVA + lease.IMP_MANTENIMIENTO_IVA, 2);
            leaseExistente.IMP_ALQUILER = Math.Round(leaseExistente.IMP_ALQUILER + lease.IMP_ALQUILER, 2);
            leaseExistente.IMP_ALQUILER_IVA = Math.Round(leaseExistente.IMP_ALQUILER_IVA + lease.IMP_ALQUILER_IVA, 2);
            leaseExistente.IMP_ADMINISTRACION = Math.Round(leaseExistente.IMP_ADMINISTRACION + lease.IMP_ADMINISTRACION, 2);
            leaseExistente.IMP_ADMINISTRACION_IVA = Math.Round(leaseExistente.IMP_ADMINISTRACION_IVA + lease.IMP_ADMINISTRACION_IVA, 2);

            if (leaseExistente.IMPUESTO != lease.IMPUESTO && leaseExistente.IMPUESTO == 0)
            {
                leaseExistente.IMPUESTO = lease.IMPUESTO;
            }
        }

        private bool AñadeLEASEPLAN(List<LeasePlanPortugal> Lease)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var cont = 0;
                var matricula = "";
                foreach (LeasePlanPortugal leaseplan in Lease)
                {
                    cont++;
                    try
                    {
                        matricula = leaseplan.MATRICULA;
                        T_G_DATOS_LEASING datos = new T_G_DATOS_LEASING
                        {
                            Administracion = leaseplan.IMP_ADMINISTRACION,
                            Administracion_IVA = leaseplan.IMP_ADMINISTRACION_IVA,
                            Alquiler = leaseplan.IMP_ALQUILER,
                            Alquiler_IVA = leaseplan.IMP_ALQUILER_IVA,
                            Asistencia_Carretera = leaseplan.IMP_ASISTEN_CARRETERA,
                            Asistencia_Carretera_IVA = leaseplan.IMP_ASISTEN_CARRETERA_IVA,
                            Canarias = leaseplan.CANARIAS,
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
                        };

                        unitOfWork.RepositoryT_G_DATOS_LEASING.Insert(datos);
                        unitOfWork.Commit();
                    }
                    catch (Exception ex)
                    {
                        GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"<AñadeLEASEPLAN> <ELEMENTO> {cont}, <MATRICULA> {matricula}, {ex.Message}");
                        //return false;
                    }
                }
            }


            return true;
        }

        #endregion

        private Boolean EsCocheDirectivo(string matricula)
        {
            bool valorReturn = false;

            using (var unitOfWork = new UnitOfWork())
            {
                var tipoVehiculo = (from v in unitOfWork.RepositoryDatos_Vehiculo.Fetch().Where(o => o.Matricula == matricula)
                                select v.Tipo_Vehiculo).FirstOrDefault();

                if (tipoVehiculo != null)
                {
                    valorReturn = tipoVehiculo == VEHICULO_DIRECCION;
                }
            }
            return valorReturn;
        }
    }
}
