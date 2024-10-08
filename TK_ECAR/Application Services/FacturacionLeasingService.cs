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
using TK_ECAR.Framework.Models;
using TK_ECAR.Framework.Utils;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;
using TK_ECAR.Models.Portugal;
using TK_ECAR.Utils;
using TKUtilidades;
using resourceView = TK_ECAR.Content.resources.TK_ECAR_Resource;

namespace TK_ECAR.ApplicationServices
{
    class Ceco_Delegacion
    {
        public string idCeco { get; set; }
        public string NombreCeco { get; set; }
        public string idDeleg { get; set; }
        public string NombreDeleg { get; set; }
    }

    class empLeasing
    {
        public string NIF { get; set; }
        public string Nombre { get; set; }
    }
    public class FacturacionLeasingService
    {

        #region Eventos Facturación
        //Definimos evento.
        public delegate void ProcessingFacturaDatosLeasingEventHandler(int lineProsessing, int TotalLinesToProcess, string msg);
        //Evento de procesado de archivo.
        public event ProcessingFacturaDatosLeasingEventHandler EventProcessingFacturaDatosLeasing;

        public delegate void SubProcessingFacturaDatosLeasingEventHandler(int lineProsessing, int TotalLinesToProcess, string msg);
        public event SubProcessingFacturaDatosLeasingEventHandler EventSubProcessingFacturaDatosLeasing;

        public delegate void ErrorFacturaDatosLeasingEventHandler(int lineaProsessing, string msgError);
        public event ErrorFacturaDatosLeasingEventHandler EventErrorFacturaDatosLeasing;

        public delegate void IncidenciaFacturaDatosLeasingEventHandler(int lineaProsessing, string matricula, EnumTipoLineaImportacion tipoIncidencia);
        public event IncidenciaFacturaDatosLeasingEventHandler EventIncidenciaFacturaDatosLeasing;

        public delegate void FinishedFacturaDatosLeasingEventHandler(int TotalLinesToProcess, int TotalLinesProcessedOK, int TotalFilasProcesadasIncidencia, int TotalLinesProcessedERROR);
        public event FinishedFacturaDatosLeasingEventHandler EventFinishedFacturaDatosLeasing;
        #endregion Eventos Importación

        //private const int VEHICULO_DIRECCION = 8;

        private string Paso = string.Empty;
        private int FilaProcessing = 0;
        private int TotalFilas = 0;
        private int TotalFilasProcesadasOK = 0;
        private int TotalFilasProcesadasExistentes = 0;
        private int TotalFilasProcesadasIncidencia = 0;
        private int TotalFilasProcesadasERROR = 0;

        #region FACTURACIÓN LEASEPLAN
        public bool FacturacionDatosLEASING(GeneracionFacturacionModels modelo, List<FacturaModels> FacturasLeasing, List<FacturaRepartoModels> RepartoFacturasLeasing)
        {
            bool valorReturn = true;
            string fileToImport = string.Empty;
            
            TotalFilasProcesadasERROR = 0;

            //Dejo que sea un Lista, por si en un futuro se cambia a chosen multiselección.
            List<int?> empresas = new List<int?> { modelo.IDEmpresa }; //.Split(',').Select(x => (int?)int.Parse(x)).ToList();
            List<int?> empresasLeasing = new List<int?> { modelo.IDEmpresaLeasing }; //.Split(',').Select(x => (int?)int.Parse(x)).ToList();

            OnProcessingFacturaLeasingEventHandler(0, 0, $"{resourceView.ImportacionComienza} {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");

            try
            {
                Paso = $"Leyendo datos Leasing";

                var cecosUsuario = ((UserModel)Util.GetItemFromMemory("userProfile")).CecosModelList.Select(x=>x.IdCeco).ToList();
                valorReturn = GeneraFacturaLeasing((DateTime)modelo.FechaFacturacion, cecosUsuario, empresas, empresasLeasing,
                                            FacturasLeasing, RepartoFacturasLeasing);

            }
            catch (Exception ex)
            {
                OnErrorFacturaLeasingEventHandler(FilaProcessing, $"[ERROR] {resourceView.ImportacionProcesandoLínea}{FilaProcessing} - {Paso}. {Environment.NewLine}{Global.GetMessageError(ex)}");
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"<FacturacionDatosLEASING> <FICHERO> {fileToImport}, <PASO> CARGA DATOS, {Global.GetMessageError(ex)}");
                valorReturn = false;
            }

            finally
            {
                OnFinishedFacturaLeasingEventHandler(TotalFilas, TotalFilasProcesadasOK, TotalFilasProcesadasIncidencia, TotalFilasProcesadasERROR);
            }

            return valorReturn;
        }


        /// <summary>
        /// Genera las facturas y el reparto. Devuelve los datos en FacturasLeasing y RepartoFacturasLeasing, respectivamente
        /// </summary>
        /// <param name="fechaFactura"></param>
        /// <param name="lCentrosCoste"></param>
        /// <param name="empresasFacturadas"></param>
        /// <param name="empresasLeasing"></param>
        /// <param name="FacturasLeasing"></param>
        /// <param name="RepartoFacturasLeasing"></param>
        /// <returns>True si no ha habido errores</returns>
        public bool GeneraFacturaLeasing(DateTime fechaFactura, List<string> lCentrosCoste,
                                            List<int?> empresasFacturadas, List<int?> empresasLeasing, 
                                            List<FacturaModels> FacturasLeasing, List<FacturaRepartoModels> RepartoFacturasLeasing)
        {
            bool valorReturn = true;
            //var _facturacionLeasing = new List<FacturaModels>();
            //var _repartoFacturasLeasing = new List<FacturaRepartoModels>();

            using (var unitOfWork = new UnitOfWork())
            {
                try
                {
                    Paso = $"Estableciendo Especificaciones";

                    T_G_DATOS_LEASINGSpecification spec = new T_G_DATOS_LEASINGSpecification();
                    var firstDayOfMonth = new DateTime(fechaFactura.Year, fechaFactura.Month, 1);
                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                    spec.Fecha_FacturaFrom = firstDayOfMonth;
                    spec.Fecha_FacturaTo = lastDayOfMonth;

                    if (empresasFacturadas.Count > 0)
                    {
                        spec.SociedadIN = empresasFacturadas;
                    }
                    if (empresasLeasing.Count > 0)
                    {
                        spec.EmpresaLeasingIN = empresasLeasing;
                    }

                    //int empresaAnt = 0;
                    //int empLeasingAnt = 0;
                    var facturaAnt = string.Empty;
                    Dictionary<int, string> empresasTratadas = new Dictionary<int, string>();
                    Dictionary<int, empLeasing> empleasingTratadas = new Dictionary<int, empLeasing>();
                    Dictionary<string, Ceco_Delegacion> CentrosCosteTratados = new Dictionary<string, Ceco_Delegacion>();
                    Dictionary<string, string> DelegacionesTratadas = new Dictionary<string, string>();

                    Paso = $"Iniciando lectura datos leasing";

                    var LineasFacturaLeasing = new LeasingService().GetLeasing(fechaFactura, lCentrosCoste,
                                                empresasFacturadas, empresasLeasing);
                    TotalFilas = LineasFacturaLeasing.Count();

                    Paso = $"Procesando datos leasing";
                    foreach (T_G_DATOS_LEASING lineaLeasing in LineasFacturaLeasing.ToList())
                    {
                        FilaProcessing++;
                        
                        OnProcessingFacturaLeasingEventHandler(FilaProcessing, TotalFilas, $"{resourceView.ImportacionProcesandoLínea} FACTURA-{lineaLeasing.Num_Factura}-");

                        //Generamos facturas por empresa leasing y factura.
                        //if (empresaAnt != lineaLeasing.Sociedad || empLeasingAnt != lineaLeasing.EmpresaLeasing || facturaAnt != lineaLeasing.Num_Factura)
                        if (facturaAnt != lineaLeasing.Num_Factura)
                        {
                            Paso = $"Asignando total factura {facturaAnt}";
                            LetTotalFactura_TotalImpuesto(RepartoFacturasLeasing ,FacturasLeasing ,facturaAnt);

                            Paso = $"Añadiendo nueva factura";

                            Dictionary<string, string> porcentajesEnFactura = new Dictionary<string, string>();

                            Paso = $"Comprobando empresas tratadas en factura {lineaLeasing.Num_Factura}";
                            CompruebaEmpresasTratadas(unitOfWork, (int)lineaLeasing.Sociedad, empresasTratadas);

                            Paso = $"Comprobando empresas leasing tratadas en factura {lineaLeasing.Num_Factura}";
                            CompruebaEmpresasLeasingTratadas(unitOfWork, (int)lineaLeasing.EmpresaLeasing, empleasingTratadas);
                            //empresaAnt = (int)lineaLeasing.Sociedad;
                            //empLeasingAnt = (int)lineaLeasing.EmpresaLeasing;
                            facturaAnt = lineaLeasing.Num_Factura;
                        }

                        //Inicio creación de la factura ***
                        Paso = $"Procesando factura {lineaLeasing.Num_Factura}";
                        //FacturaModels factura = FacturasLeasing.Where(x => x.IDEmpresaFatura == (int)lineaLeasing.Sociedad
                        //                                            && x.IDEmpresaLeasing == (int)lineaLeasing.EmpresaLeasing
                        //                                            && x.NumeroFactura == lineaLeasing.Num_Factura)
                        //                                            .FirstOrDefault();
                        FacturaModels factura = FacturasLeasing.Where(x => x.IDEmpresaLeasing == (int)lineaLeasing.EmpresaLeasing
                                                                    && x.NumeroFactura == lineaLeasing.Num_Factura)
                                                                    .FirstOrDefault();
                        if (factura == null)
                        {
                            FacturasLeasing.Add(new FacturaModels
                            {
                                CentroCosteCoste = GlobalCostes.CENTRO_COSTE_FACTURA,
                                NumeroFactura = lineaLeasing.Num_Factura,
                                FechaFactura = (DateTime)lineaLeasing.Fecha_Factura,
                                FechaVencimientoFactura = (DateTime)lineaLeasing.Fecha_Factura,
                                IDEmpresaFatura = (int)lineaLeasing.Sociedad,
                                IDEmpresaLeasing = (int)lineaLeasing.EmpresaLeasing,
                                NombreEmpresaFatura = empresasTratadas[(int)lineaLeasing.Sociedad],
                                NombreEmpresaLeasing = empleasingTratadas[(int)lineaLeasing.EmpresaLeasing].Nombre,
                                Canarias = lineaLeasing.Canarias,
                                CIFEmpresaLeasing = empleasingTratadas[(int)lineaLeasing.EmpresaLeasing].NIF,
                            });
                            //factura = FacturasLeasing.Where(x => x.IDEmpresaFatura == (int)lineaLeasing.Sociedad
                            //                                            && x.IDEmpresaLeasing == (int)lineaLeasing.EmpresaLeasing
                            //                                            && x.NumeroFactura == lineaLeasing.Num_Factura)
                            //                                            .FirstOrDefault();
                            factura = FacturasLeasing.Where(x => x.IDEmpresaLeasing == (int)lineaLeasing.EmpresaLeasing
                                                            && x.NumeroFactura == lineaLeasing.Num_Factura)
                                                            .FirstOrDefault();
                        }

                        var PorcentajeImpuestoLinea = (lineaLeasing.Impuesto * 100).ToString();
                        Paso = $"Comprobando porcentaje impuesto ({PorcentajeImpuestoLinea}) para asiento en factura {lineaLeasing.Num_Factura}";
                        if (factura.AsientosFactura == null || !factura.AsientosFactura.ContainsKey(PorcentajeImpuestoLinea))
                        {
                            Paso = $"Añadiendo asiento con porcentaje impuesto ({PorcentajeImpuestoLinea}) en factura {lineaLeasing.Num_Factura}";
                            factura.AddAsientosFactura(PorcentajeImpuestoLinea);
                        }

                        Paso = $"Añadiendo importes en asiento con % de impuesto ({PorcentajeImpuestoLinea}) en factura {lineaLeasing.Num_Factura}";
                        AñadeImportesConceptoFacturacion(factura.AsientosFactura[PorcentajeImpuestoLinea], lineaLeasing);
                        //*** Fin creación factura.

                        //Inicio Reparo ***
                        //No debe haber para una misma fecha de facturación más de una matrícula.
                        Paso = $"Creando reparto en factura {lineaLeasing.Num_Factura}";
                        var detalleReparto = new FacturaRepartoModels
                        {
                            NumFactura = lineaLeasing.Num_Factura,
                            NIFEmpresaLeasing = empleasingTratadas[(int)lineaLeasing.EmpresaLeasing].NIF,
                            CentroCosteCoste = lineaLeasing.ECAR_Datos_Vehiculo.CC ?? "",
                            Matricula = lineaLeasing.Matricula,
                            Directivo = lineaLeasing.Directivo,
                            Canarias = lineaLeasing.Canarias,
                            FechaFactura = (DateTime)lineaLeasing.Fecha_Factura,
                            ImporteAdministracion = lineaLeasing.Administracion == null ? 0 : (double)lineaLeasing.Administracion,
                            ImporteITV = lineaLeasing.ITV == null ? 0 : (double)lineaLeasing.ITV,
                            ImporteMantenimiento = lineaLeasing.Mantenimiento == null ? 0 : (double)lineaLeasing.Mantenimiento,
                            ImporteNeumaticos = lineaLeasing.Neumaticos == null ? 0 : (double)lineaLeasing.Neumaticos,
                            ImporteRenting = lineaLeasing.Alquiler == null ? 0 : (double)lineaLeasing.Alquiler,
                            ImporteSeguro = lineaLeasing.Seguro == null ? 0 : (double)lineaLeasing.Seguro,
                            Impuesto = Convert.ToDouble(lineaLeasing.Impuesto),
                        };

                        if (lineaLeasing.Directivo)
                        {
                            detalleReparto.ImporteAdministracion += DameImporteImpuestoNoDeducible((double)(lineaLeasing.Administracion_IVA- detalleReparto.ImporteAdministracion));
                            detalleReparto.ImporteITV += DameImporteImpuestoNoDeducible((double)(lineaLeasing.ITV_IVA- detalleReparto.ImporteITV));
                            detalleReparto.ImporteMantenimiento += DameImporteImpuestoNoDeducible((double)(lineaLeasing.Mantenimiento_IVA - lineaLeasing.Mantenimiento));
                            detalleReparto.ImporteNeumaticos += DameImporteImpuestoNoDeducible((double)(lineaLeasing.Neumaticos_IVA) - lineaLeasing.Neumaticos);
                            detalleReparto.ImporteRenting += DameImporteImpuestoNoDeducible((double)(lineaLeasing.Alquiler_IVA) - lineaLeasing.Alquiler);
                            detalleReparto.ImporteSeguro += DameImporteImpuestoNoDeducible((double)(lineaLeasing.Seguro_IVA) - lineaLeasing.Seguro);
                        }

                        //var ceco = unitOfWork.RepositoryECAR_Datos_Vehiculo.Fetch().Where(x => x.Matricula == lineaLeasing.Matricula).FirstOrDefault();
                        //detalleReparto.CentroCosteCoste = lineaLeasing.ECAR_Datos_Vehiculo.CC;
                        Paso = $"Obteniendo nombre CECO ({lineaLeasing.ECAR_Datos_Vehiculo.CC}) reparto en factura {lineaLeasing.Num_Factura}";
                        var cecoDeleg = GetNombreCeco(unitOfWork, lineaLeasing.ECAR_Datos_Vehiculo.CC, CentrosCosteTratados, DelegacionesTratadas);
                        detalleReparto.NombreCentroCoste = cecoDeleg.NombreCeco;

                        Paso = $"Obteniendo nombre Empresa ({lineaLeasing.Sociedad}) reparto en factura {lineaLeasing.Num_Factura}";
                        detalleReparto.IDEmpresaFatura = (int)lineaLeasing.Sociedad;
                        detalleReparto.NombreEmpresaFatura = empresasTratadas[detalleReparto.IDEmpresaFatura];

                        Paso = $"Obteniendo nombre Empresa leasing ({lineaLeasing.EmpresaLeasing}) reparto en factura {lineaLeasing.Num_Factura}";
                        detalleReparto.IDEmpresaLeasing = (int)lineaLeasing.EmpresaLeasing;
                        detalleReparto.NombreEmpresaLeasing = empleasingTratadas[detalleReparto.IDEmpresaLeasing].Nombre;

                        Paso = $"Comprobando delegaciones ({cecoDeleg.idDeleg}) tratadas en reparto {lineaLeasing.Num_Factura}";
                        //CompruebaDelegacionesTratadas(unitOfWork, ceco.Delegacion, DelegacionesTratadas);
                        detalleReparto.IDDelegacionFatura = cecoDeleg.idDeleg;
                        detalleReparto.NombreDelegacionFatura = cecoDeleg.NombreDeleg;

                        RepartoFacturasLeasing.Add(detalleReparto);

                        //*** Fin reparto.
                        TotalFilasProcesadasOK++;
                        TotalFilasProcesadasExistentes++;
                    }
                    Paso = $"Asignando total última factura {facturaAnt}";
                    LetTotalFactura_TotalImpuesto(RepartoFacturasLeasing, FacturasLeasing, facturaAnt);
                }


                catch (Exception ex)
                {
                    OnErrorFacturaLeasingEventHandler(FilaProcessing, $"[ERROR] {resourceView.ImportacionProcesandoLínea}{FilaProcessing} - {Paso}. {Environment.NewLine}{Global.GetMessageError(ex)}");
                    Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"<GetFacturaLeasing> <PASO> {Paso}, {Global.GetMessageError(ex)}");
                    TotalFilasProcesadasERROR++;
                    valorReturn = false;
                }

                finally
                {
                    OnFinishedFacturaLeasingEventHandler(TotalFilas, TotalFilasProcesadasOK, TotalFilasProcesadasIncidencia, TotalFilasProcesadasERROR);
                }

                return valorReturn;
            }
        }


        private double DameImporteImpuestoNoDeducible(double? importeImpuesto)
        {
            double valorReturn = 0.0;

            if (importeImpuesto != null && importeImpuesto != 0.0)
            {
                valorReturn = GlobalCostes.Truncate((double)importeImpuesto / 2);
            }

            return valorReturn;
        }

        /// <summary>
        /// Añadir el concepto de facturación, según los campos de leasing.
        /// </summary>
        /// <param name="Asiento"></param>
        /// <param name="_lineaLeasing"></param>
        private void AñadeImportesConceptoFacturacion(AsientoFactura Asiento, T_G_DATOS_LEASING _lineaLeasing)
        {
            Paso = $"Añadiendo ConceptoFacturacion Administracion en factura {_lineaLeasing.Num_Factura}";
            AñadeConcepto(Asiento, _lineaLeasing, GlobalCostes.TipoObjetoCoste.Administracion, (float)_lineaLeasing.Administracion, (float)_lineaLeasing.Administracion_IVA);

            Paso = $"Añadiendo ConceptoFacturacion Seguro en factura {_lineaLeasing.Num_Factura}";
            AñadeConcepto(Asiento, _lineaLeasing, GlobalCostes.TipoObjetoCoste.Seguro, (float)_lineaLeasing.Seguro, (float)_lineaLeasing.Seguro_IVA);

            Paso = $"Añadiendo ConceptoFacturacion Mantenimiento en factura {_lineaLeasing.Num_Factura}";
            AñadeConcepto(Asiento, _lineaLeasing, GlobalCostes.TipoObjetoCoste.Mantenimiento, (float)_lineaLeasing.Mantenimiento, (float)_lineaLeasing.Mantenimiento_IVA);

            Paso = $"Añadiendo ConceptoFacturacion Neumáticos en factura {_lineaLeasing.Num_Factura}";
            AñadeConcepto(Asiento, _lineaLeasing, GlobalCostes.TipoObjetoCoste.Neumaticos, (float)_lineaLeasing.Neumaticos, (float)_lineaLeasing.Neumaticos_IVA);

            Paso = $"Añadiendo ConceptoFacturacion Alquiler en factura {_lineaLeasing.Num_Factura}";
            AñadeConcepto(Asiento, _lineaLeasing, GlobalCostes.TipoObjetoCoste.Alquiler, (float)_lineaLeasing.Alquiler, (float)_lineaLeasing.Alquiler_IVA);

            Paso = $"Añadiendo ConceptoFacturacion ITV en factura {_lineaLeasing.Num_Factura}";
            AñadeConcepto(Asiento, _lineaLeasing, GlobalCostes.TipoObjetoCoste.ITV, (float)_lineaLeasing.ITV, (float)_lineaLeasing.ITV_IVA);
        }

        private void AñadeConcepto(AsientoFactura Asiento, T_G_DATOS_LEASING _lineaLeasing, GlobalCostes.TipoObjetoCoste tipoCoste, float Importe, float ImporteIVA)
        {
            //if(_lineaLeasing.Num_Factura == "13381531")
            //{

            //}

            var importeIVA_No_Deducible = 0.0;
            var importeImpuestoDeducible = 0.0;
            var ImporteImpuesto = 0.0;//Math.Ceiling((ImporteIVA - Importe) * 100) / 100f;
            if (_lineaLeasing.Directivo && ImporteIVA > 0.000001)
            { //El 50% de los conceptos de IVA, si es coche de dirección, se pone en el IVA NO deducible.
                Paso = $"importeIVA_No_Deducible de {tipoCoste} en factura {_lineaLeasing.Num_Factura}";
                ImporteImpuesto = GlobalCostes.Truncate(ImporteIVA - Importe, 2);
                importeIVA_No_Deducible = GlobalCostes.Truncate((ImporteImpuesto * 50) / 100, 2);
                importeImpuestoDeducible = GlobalCostes.Truncate(ImporteImpuesto - importeIVA_No_Deducible,2);
            }
            else
            {
                ImporteImpuesto = ImporteIVA - Importe;
                importeImpuestoDeducible = ImporteImpuesto;
            }

            Asiento.AddConceptoFacturacion((int)_lineaLeasing.Sociedad, tipoCoste, Importe, ImporteImpuesto, importeIVA_No_Deducible, importeImpuestoDeducible);
        }

        private void LetTotalFactura_TotalImpuesto(List<FacturaRepartoModels> reparto, List<FacturaModels> FacturasLeasing, string factura)
        {
            if (!string.IsNullOrEmpty(factura))
            {
                FacturaModels _facturaLeasing = FacturasLeasing.Where(x => x.NumeroFactura == factura).FirstOrDefault();
                double _totalFactura = _facturaLeasing.TotalFactura;
                double _totalImpuesto = _facturaLeasing.Impuesto;

                foreach (FacturaRepartoModels detalle in reparto.Where(x => x.NumFactura == factura))
                {
                    detalle.TotalFactura = _totalFactura;
                    detalle.TotalImporteImpuesto = _totalImpuesto;
                }
            }
        }

        #endregion FACTURACIÓN LEASEPLAN

        #region métodos privados
        private Ceco_Delegacion GetNombreCeco(UnitOfWork unitOfWork, string ceco, Dictionary<string, Ceco_Delegacion> _centrosCosteTratados, Dictionary<string, string> _delegacionesTratadas)
        {
            var valorReturn = new Ceco_Delegacion();
            var objCeco = new Ceco_Delegacion();
            var cecoFormateado = ceco;

            if (Global.EsNumerico(ceco))
            {
                cecoFormateado = new String('0', 10 - ceco.Length) + ceco;
            }
            if (!_centrosCosteTratados.ContainsKey(ceco))
            {
                var CC = unitOfWork.RepositorySAPHR_CentrosCoste.Fetch().Where(x => x.IdCeco == cecoFormateado).FirstOrDefault();

                objCeco.idCeco = ceco;
                objCeco.NombreCeco = CC.Nombre;

                if (string.IsNullOrEmpty(CC.IdDelegacion))
                {
                    if (!_delegacionesTratadas.ContainsKey(ceco))
                    {
                        _delegacionesTratadas.Add(ceco, GlobalCostes.TEXTO_SIN_DELEGACION);
                    }
                    objCeco.idDeleg = ceco;
                    objCeco.NombreDeleg = _delegacionesTratadas[ceco];
                    _centrosCosteTratados.Add(ceco, objCeco);
                }
                else
                {
                    if (!_delegacionesTratadas.ContainsKey(CC.IdDelegacion))
                    {
                        var objDeleg = unitOfWork.RepositorySAPHR_Delegaciones.Fetch()
                                                    .Where(x => x.IdDelegacion == CC.IdDelegacion)
                                                    .FirstOrDefault();
                        _delegacionesTratadas.Add(CC.IdDelegacion, objDeleg.Nombre);

                        objCeco.idDeleg = CC.IdDelegacion;
                        objCeco.NombreDeleg = _delegacionesTratadas[CC.IdDelegacion];
                    }
                    else
                    {
                        objCeco.idDeleg = CC.IdDelegacion;
                        objCeco.NombreDeleg = _delegacionesTratadas[CC.IdDelegacion];
                    }
                    _centrosCosteTratados.Add(ceco, objCeco);
                }
            }

            valorReturn = _centrosCosteTratados[ceco];

            return valorReturn;
        }

        private void CompruebaEmpresasTratadas(UnitOfWork unitOfWork, int idempresa, Dictionary<int, string> _empresasTratadas)
        {
            if (!_empresasTratadas.ContainsKey(idempresa))
            {
                _empresasTratadas.Add(idempresa, unitOfWork.RepositorySAPHR_Empresas.Fetch()
                                            .Where(x => x.CodigoEmpresa == idempresa)
                                            .FirstOrDefault().Nombre);
            }
        }

        private void CompruebaEmpresasLeasingTratadas(UnitOfWork unitOfWork, int idempresa, Dictionary<int, empLeasing> _empresasLeasingTratadas)
        {
            if (!_empresasLeasingTratadas.ContainsKey(idempresa))
            {
                EmpresasVehiculosModels emp = new EmpresasVehiculosService().GetEmpVehiculoByID(idempresa);

                _empresasLeasingTratadas.Add((int)idempresa, new empLeasing { NIF = emp.NIF, Nombre = emp.Nombre});
            }
        }

        private void CompruebaDelegacionesTratadas(UnitOfWork unitOfWork, string iddelegacion, Dictionary<string, string> _delegacionesTratadas)
        {
            if (!_delegacionesTratadas.ContainsKey(iddelegacion))
            {
                _delegacionesTratadas.Add(iddelegacion, unitOfWork.RepositorySAPHR_Delegaciones.Fetch()
                                            .Where(x => x.IdDelegacion == iddelegacion)
                                            .FirstOrDefault().Nombre);
            }
        }

        #endregion métodos privados


        #region eventos
        private void OnProcessingFacturaLeasingEventHandler(int lineProsessing, int TotalLinesToProcess, string msg)
        {
            EventProcessingFacturaDatosLeasing?.Invoke(lineProsessing, TotalLinesToProcess, msg);
        }

        private void OnSubProcessingFacturaLeasingEventHandler(int lineProsessing, int TotalLinesToProcess, string msg)
        {
            EventSubProcessingFacturaDatosLeasing?.Invoke(lineProsessing, TotalLinesToProcess, msg);
        }

        private void OnErrorFacturaLeasingEventHandler(int lineProsessing, string msgError)
        {
            EventErrorFacturaDatosLeasing?.Invoke(lineProsessing, msgError);
        }

        private void OnFinishedFacturaLeasingEventHandler(int TotalLinesToProcess, int TotalLinesProcessedOK, int TotalFilasProcesadasIncidencia, int TotalLinesProcessedERROR)
        {
            EventFinishedFacturaDatosLeasing?.Invoke(TotalLinesToProcess, TotalLinesProcessedOK, TotalFilasProcesadasIncidencia, TotalLinesProcessedERROR);
        }

        private void OnIncidenciaFacturaLeasing(int lineProsessing, string matricula, EnumTipoLineaImportacion tipoIncidencia)
        {
            EventIncidenciaFacturaDatosLeasing?.Invoke(lineProsessing, matricula, tipoIncidencia);
        }
    }

    #endregion eventos

}
