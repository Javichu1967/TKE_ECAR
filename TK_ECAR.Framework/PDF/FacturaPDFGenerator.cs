using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Globalization;
using System.IO;
using TK_ECAR.Framework.Models;
using TK_ECAR.Framework.Utils;
using System.Text;
using System.Linq;

namespace TK_ECAR.Framework.PDF
{
    public class FacturaPDFGenerator
    {
        private enum formatoCabeceraDetalle { SinCabecera, CabeceraNormal, CabeceraContinuacion };

        private PDFConstants.OrientationPDF Orientation;

        private double TotalCecoRenting = 0; private double TotalCecoMantenimiento = 0; private double TotalCecoNeumáticos = 0;
        private double TotalCecoAdministracion = 0; private double TotalCecoSeguro = 0; private double TotalCecoITV = 0;

        private double TotalDelegRenting = 0; private double TotalDelegMantenimiento = 0; private double TotalDelegNeumáticos = 0;
        private double TotalDelegAdministracion = 0; private double TotalDelegSeguro = 0; private double TotalDelegITV = 0;

        private double TotalParcialRenting = 0; private double TotalParcialMantenimiento = 0; private double TotalParcialNeumáticos = 0;
        private double TotalParcialAdministracion = 0; private double TotalParcialSeguro = 0; private double TotalParcialITV = 0;

        private double TotalGeneralRenting = 0; private double TotalGeneralMantenimiento = 0; private double TotalGeneralNeumáticos = 0;
        private double TotalGeneralAdministracion = 0; private double TotalGeneralSeguro = 0; private double TotalGeneralITV = 0;

        private PDFConstants.BorderCellPDF bordeCompleto = PDFConstants.BorderCellPDF.BOTTOM_BORDER | PDFConstants.BorderCellPDF.LEFT_BORDER |
                                            PDFConstants.BorderCellPDF.RIGHT_BORDER | PDFConstants.BorderCellPDF.TOP_BORDER;

        private formatoCabeceraDetalle escribirCabeceraTabla = formatoCabeceraDetalle.SinCabecera;

        private string TextoConDirectivo = "(SIN vehíc.de dirección)";

        private string centroCosteAnt = string.Empty;

        private bool EscribiendoTotales = false;
        private string paso = string.Empty;

        #region Asientos
        public MemoryStream ExportFacturaPDF(List<FacturaModels> FacturasLeasing, PDFConstants.OrientationPDF orientation, string paramPathImageHeaderTKE, CultureInfo currentCulture)
        {

            MemoryStream memStream = new MemoryStream();

            //Inicializamos la variable de documento nuevo
            Document doc = new Document();//PageSize.A4,30,20,30,20);

            Orientation = orientation;

            try
            {
                paso = "Inicializando documento";
                //obtenemos el path configurable desde base de datos donde se encuentra la imagen.
                string pathImageHeaderTKE = paramPathImageHeaderTKE;
                PdfWriter writer = PdfWriter.GetInstance(doc, memStream);

                Uri urlLogoTKE = new Uri(pathImageHeaderTKE);

                //Cambiamos la orientación de la página.
                bool rotate = Orientation == PDFConstants.OrientationPDF.HORIZONTAL;

                if (rotate)
                {
                    doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                }
                else
                {
                    doc.SetPageSize(iTextSharp.text.PageSize.A4);
                }

                //doc.SetMargins(2f, 2f, 100f, 20f);

                //Se le pasa la información necesaria a la calse de los eventos del pdf, para poder usarlos allí
                //"FacturasLeasing" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf"
                PDFEvents.PDFEventsUtility evEnts = new PDFEvents.PDFEventsUtility(urlLogoTKE, "CONTRATOS DE RENTING DE AUTOMÓVILES", rotate);

                writer.PageEvent = evEnts;
                writer.CloseStream = false;

                //creamos el documento
                doc.Open();

                int cont = 0;
                PdfPTable table = new PdfPTable(1);
                foreach (FacturaModels factura in FacturasLeasing)
                {
                    foreach (KeyValuePair<string, AsientoFactura> asiento in factura.AsientosFactura.OrderBy(x=>x.Key))
                    {
                        paso = $"-Factura {factura.NumeroFactura}- -Asiento {asiento.Key}-";
                        if (cont > 0)
                        {
                            doc.NewPage();
                        }
                        cont++;

                        doc.Add(new Paragraph(110f, " "));
                        table = new PdfPTable(1);
                        table.AddCell(PDFCellUtility.cell($"Empresa: {factura.IDEmpresaFatura} - {factura.NombreEmpresaFatura}", 1, 1, PdfPCell.ALIGN_LEFT, true, false, PDFConstants.BorderCellPDF.NO_BORDER));
                        doc.Add(table);
                        doc.Add(new Paragraph(10f, " "));

                        table = new PdfPTable(1);
                        table.AddCell(PDFCellUtility.cell($"{factura.NombreEmpresaLeasing} - CUOTA FOR FECHA DE VENCIMIENTO", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER));
                        doc.Add(table);
                        doc.Add(new Paragraph(6f, " "));

                        table = new PdfPTable(1);
                        table.AddCell(PDFCellUtility.cell($"ASIENTO {factura.NombreEmpresaLeasing} - CUOTAS RENTING", 1, 1, PdfPCell.ALIGN_CENTER, true, false, PDFConstants.BorderCellPDF.NO_BORDER));
                        doc.Add(table);
                        doc.Add(new Paragraph(10f, " "));

                        table = new PdfPTable(2);
                        table.AddCell(PDFCellUtility.cell($"MES: {factura.MesFactura.ToUpper()}", 1, 1, PdfPCell.ALIGN_CENTER, true, false, PDFConstants.BorderCellPDF.NO_BORDER));
                        table.AddCell(PDFCellUtility.cell($"VTO: {factura.FechaVencimientoFactura.ToString("dd/MM/yyyy")}", 1, 1, PdfPCell.ALIGN_CENTER, true, false, PDFConstants.BorderCellPDF.NO_BORDER));
                        doc.Add(table);
                        doc.Add(new Paragraph(40f, " "));

                        var textoPeninsula_Canarias = factura.Canarias ? "CANARIAS" : "PENINSULA";
                        table = new PdfPTable(2);
                        table.AddCell(PDFCellUtility.cell(textoPeninsula_Canarias, 1, 1, PdfPCell.ALIGN_LEFT, true, false, PDFConstants.BorderCellPDF.NO_BORDER, 11));
                        table.AddCell(PDFCellUtility.cell($"FACTURA: {factura.NumeroFactura}", 1, 1, PdfPCell.ALIGN_CENTER, true, false, PDFConstants.BorderCellPDF.NO_BORDER, 11));
                        doc.Add(table);
                        doc.Add(new Paragraph(15f, " "));

                        table = new PdfPTable(5);
                        table.TotalWidth = 400;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 110f, 70f, 70f, 70f, 70f };
                        table.SetWidths(widths);
                        table.AddCell(PDFCellUtility.cell("", 1, 1, PdfPCell.ALIGN_LEFT, false, false, PDFConstants.BorderCellPDF.NO_BORDER));
                        table.AddCell(PDFCellUtility.cell("CUENTA", 1, 1, PdfPCell.ALIGN_CENTER, true, false));
                        table.AddCell(PDFCellUtility.cell("GASTO", 1, 1, PdfPCell.ALIGN_CENTER, true, false));
                        table.AddCell(PDFCellUtility.cell("IMPUESTO (no deducible)", 1, 1, PdfPCell.ALIGN_CENTER, true, false));
                        table.AddCell(PDFCellUtility.cell("TOTAL", 1, 1, PdfPCell.ALIGN_CENTER, true, false));
                        //doc.Add(table);

                        foreach (KeyValuePair<string, ConceptoFactura> concepto in asiento.Value.DesgloseFactura.OrderBy(key=>key.Key))
                        {
                            table.AddCell(PDFCellUtility.cell(concepto.Value.NombreCuentaContable, 1, 1, PdfPCell.ALIGN_LEFT, true, false));
                            table.AddCell(PDFCellUtility.cell(concepto.Value.CuentaContable, 1, 1, PdfPCell.ALIGN_CENTER, true, false));
                            table.AddCell(PDFCellUtility.cell($"{concepto.Value.Importe.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_RIGHT, false, false));
                            table.AddCell(PDFCellUtility.cell($"{concepto.Value.ImporteImpuestoNoDeducible.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_RIGHT, false, false));
                            table.AddCell(PDFCellUtility.cell($"{concepto.Value.TotalGastoMasImporteImpuestoNoDeducible.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_RIGHT, false, false));
                        }
                        //doc.Add(table);

                        //Totales concepto.
                        table.AddCell(PDFCellUtility.cell("", 1, 1, PdfPCell.ALIGN_CENTER, false, false, PDFConstants.BorderCellPDF.TOP_BORDER, 9));
                        table.AddCell(PDFCellUtility.cell("TOTAL", 1, 1, PdfPCell.ALIGN_CENTER, true, false));
                        table.AddCell(PDFCellUtility.cell($"{asiento.Value.TotalGasto.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_RIGHT, true, false));
                        table.AddCell(PDFCellUtility.cell($"{asiento.Value.TotalImporteImpuestoNoDeducible.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_RIGHT, true, false));
                        table.AddCell(PDFCellUtility.cell($"{(asiento.Value.TotalGastoMasTotalImporteImpuestoNoDeducible).ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_RIGHT, true, false));
                        doc.Add(table);

                        doc.Add(new Paragraph(30f, " "));

                        ////Total Asiento.
                        table = new PdfPTable(5);
                        table.TotalWidth = 400;
                        table.LockedWidth = true;
                        widths = new float[] { 100f, 70f, 70f, 80f, 80f };
                        table.SetWidths(widths);
                        table.AddCell(PDFCellUtility.cell("", 1, 1, PdfPCell.ALIGN_CENTER, true, false, PDFConstants.BorderCellPDF.NO_BORDER));
                        table.AddCell(PDFCellUtility.cell("", 1, 1, PdfPCell.ALIGN_CENTER, true, false, PDFConstants.BorderCellPDF.NO_BORDER));
                        table.AddCell(PDFCellUtility.cell("", 1, 1, PdfPCell.ALIGN_CENTER, true, false, PDFConstants.BorderCellPDF.NO_BORDER));
                        table.AddCell(PDFCellUtility.cell("Impuesto (deducible)", 1, 1, PdfPCell.ALIGN_CENTER, true, false, PDFConstants.BorderCellPDF.NO_BORDER));
                        table.AddCell(PDFCellUtility.cell("Total + Impuesto (deducible)", 1, 1, PdfPCell.ALIGN_CENTER, true, false, PDFConstants.BorderCellPDF.NO_BORDER));

                        table.AddCell(PDFCellUtility.cell("", 1, 1, PdfPCell.ALIGN_CENTER, true, false, PDFConstants.BorderCellPDF.NO_BORDER));
                        table.AddCell(PDFCellUtility.cell("", 1, 1, PdfPCell.ALIGN_CENTER, true, false, PDFConstants.BorderCellPDF.NO_BORDER));
                        table.AddCell(PDFCellUtility.cell("", 1, 1, PdfPCell.ALIGN_CENTER, true, false, PDFConstants.BorderCellPDF.NO_BORDER));
                        table.AddCell(PDFCellUtility.cell($"{asiento.Value.TotalImporteImpuestoDeducible.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_RIGHT, true, true));
                        table.AddCell(PDFCellUtility.cell($"{asiento.Value.TotalConceptos.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_RIGHT, true, true));
                        doc.Add(table);

                        doc.Add(new Paragraph(6f, " "));

                        table = new PdfPTable(5);
                        table.TotalWidth = 400;
                        table.LockedWidth = true;
                        widths = new float[] { 100f, 70f, 70f, 80f, 80f };
                        table.SetWidths(widths);
                        table.AddCell(PDFCellUtility.cell("", 1, 1, PdfPCell.ALIGN_CENTER, true, false, PDFConstants.BorderCellPDF.NO_BORDER));
                        table.AddCell(PDFCellUtility.cell("", 1, 1, PdfPCell.ALIGN_CENTER, true, false, PDFConstants.BorderCellPDF.NO_BORDER));
                        table.AddCell(PDFCellUtility.cell("", 1, 1, PdfPCell.ALIGN_CENTER, true, false, PDFConstants.BorderCellPDF.NO_BORDER));
                        table.AddCell(PDFCellUtility.cell("Total factura", 1, 1, PdfPCell.ALIGN_CENTER, true, false, PDFConstants.BorderCellPDF.NO_BORDER));
                        table.AddCell(PDFCellUtility.cell($"{asiento.Value.TotalConceptos.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_RIGHT, true, true));
                        doc.Add(table);

                        doc.Add(new Paragraph(60f, " "));

                        table = new PdfPTable(1);
                        table.AddCell(PDFCellUtility.cell($"CENTRO DE COSTE {GlobalCostes.CENTRO_COSTE_FACTURA}", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER));
                        doc.Add(table);
                    }
                }

            }
            catch (Exception oEx)
            {
                throw new Exception($"-ExportFacturaPDF- {paso} {Environment.NewLine} {oEx.Message}");
            }
            finally
            {
                doc.Close();
            }

            return memStream;
        }
        #endregion Asientos

        #region Reparto
        public MemoryStream ExportRepartofacturaPDF(List<FacturaRepartoModels> RepartoLeasing, PDFConstants.OrientationPDF orientation, string paramPathImageHeaderTKE, CultureInfo currentCulture)
        {

            MemoryStream memStream = new MemoryStream();

            //Inicializamos la variable de documento nuevo
            Document doc = new Document();//PageSize.A4,30,20,30,20);

            Orientation = orientation;

            try
            {
                paso = "Inicializando documento";
                //obtenemos el path configurable desde base de datos donde se encuentra la imagen.
                string pathImageHeaderTKE = paramPathImageHeaderTKE;
                PdfWriter writer = PdfWriter.GetInstance(doc, memStream);

                Uri urlLogoTKE = new Uri(pathImageHeaderTKE);

                //Cambiamos la orientación de la página.
                bool rotate = Orientation == PDFConstants.OrientationPDF.HORIZONTAL;

                if (rotate)
                {
                    doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                }
                else
                {
                    doc.SetPageSize(iTextSharp.text.PageSize.A4);
                }

                //doc.SetMargins(2f, 2f, 100f, 20f);

                //Se le pasa la información necesaria a la calse de los eventos del pdf, para poder usarlos allí

                PDFEvents eventosPDF = new PDFEvents();

                PDFEvents.PDFEventsUtility evEnts = new PDFEvents.PDFEventsUtility(urlLogoTKE,
                                                    "CONTRATOS DE RENTING DE AUTOMÓVILES", $"{RepartoLeasing[1].NombreEmpresaLeasing} - REPARTO",
                                                    $"MES: {RepartoLeasing[1].MesFactura}", rotate);

                evEnts.EventNewPagePDF += this.OnEventNewPagePDF;

                writer.PageEvent = evEnts;
                writer.CloseStream = false;

                //creamos el documento
                doc.Open();

                PdfPTable table = new PdfPTable(1);

                var cont = 0;
                centroCosteAnt = string.Empty;
                var canariasAnt = true;
                var delegacionAnt = string.Empty;
                //float[] widths = new float[] { 110f, 70f, 70f, 70f, 70f, 70f, 70f, 70f };
                float[] widths = new float[] { 133f, 93f, 93f, 93f, 93f, 93f };
                //Iteramos sobre los datos de Leasing obtenidos.
                foreach (FacturaRepartoModels reparto in RepartoLeasing)
                {
                    paso = $"-Matrícula {reparto.Matricula}-";
                    //Como se escribe la cabecera primero, en el primer elemento ponemos si es PENÍNSULA o CANARIAS.
                    if (cont == 0)
                    {
                        canariasAnt = reparto.Canarias;
                        centroCosteAnt = $"{reparto.CentroCosteCoste}@{reparto.NombreCentroCoste}";
                        delegacionAnt = $"{reparto.IDDelegacionFatura}@{reparto.NombreDelegacionFatura}";

                        EscribeTextoCambioCanarias(doc, reparto);
                        EscribeTextoCambioDelegacion(doc, delegacionAnt.Split('@')[0], delegacionAnt.Split('@')[1]);
                        EscribeTextoCambioCentroCoste(doc, centroCosteAnt.Split('@')[0], centroCosteAnt.Split('@')[1]);

                        escribirCabeceraTabla = formatoCabeceraDetalle.CabeceraNormal;
                    }
                    //Controlamos si hay cambio de PENÍNSULA o CANARIAS.
                    if (canariasAnt != reparto.Canarias)
                    {
                        EscribirTotalesCeco(doc);
                        InicializaTotalesCeco();

                        EscribirTotalesDeleg(doc, delegacionAnt);
                        InicializaTotalesDeleg();

                        EscribirTotalesParciales(doc, canariasAnt ? "CANARIAS" : "PENINSULA");
                        doc.Add(new Paragraph(40f, " "));

                        InicializaTotalesParciales();
                        canariasAnt = reparto.Canarias;
                        EscribeTextoCambioCanarias(doc, reparto);

                        delegacionAnt = $"{reparto.IDDelegacionFatura}@{reparto.NombreDelegacionFatura}";
                        EscribeTextoCambioDelegacion(doc, delegacionAnt.Split('@')[0], delegacionAnt.Split('@')[1]);

                        centroCosteAnt = $"{reparto.CentroCosteCoste}@{reparto.NombreCentroCoste}";
                        EscribeTextoCambioCentroCoste(doc, centroCosteAnt.Split('@')[0], centroCosteAnt.Split('@')[1]);

                        escribirCabeceraTabla = formatoCabeceraDetalle.CabeceraNormal;
                    }
                    //Controlamos si hay cambio de delegación.
                    if (delegacionAnt != $"{reparto.IDDelegacionFatura}@{reparto.NombreDelegacionFatura}")
                    {
                        EscribirTotalesCeco(doc);
                        InicializaTotalesCeco();

                        EscribirTotalesDeleg(doc, delegacionAnt);
                        InicializaTotalesDeleg();

                        delegacionAnt = $"{reparto.IDDelegacionFatura}@{reparto.NombreDelegacionFatura}";
                        EscribeTextoCambioDelegacion(doc, delegacionAnt.Split('@')[0], delegacionAnt.Split('@')[1]);

                        centroCosteAnt = $"{reparto.CentroCosteCoste}@{reparto.NombreCentroCoste}";
                        EscribeTextoCambioCentroCoste(doc, centroCosteAnt.Split('@')[0], centroCosteAnt.Split('@')[1]);

                        escribirCabeceraTabla = formatoCabeceraDetalle.CabeceraNormal;
                    }
                    //Controlamos si hay cambio de centro de coste.
                    if (centroCosteAnt != $"{reparto.CentroCosteCoste}@{reparto.NombreCentroCoste}")
                    {
                        EscribirTotalesCeco(doc);
                        InicializaTotalesCeco();

                        doc.Add(new Paragraph(5f, " "));
                        centroCosteAnt = $"{reparto.CentroCosteCoste}@{reparto.NombreCentroCoste}";
                        EscribeTextoCambioCentroCoste(doc, centroCosteAnt.Split('@')[0], centroCosteAnt.Split('@')[1]);

                        escribirCabeceraTabla = formatoCabeceraDetalle.CabeceraNormal;
                    }

                    //Acumulamos totales.
                    TotalCecoRenting += reparto.ImporteRenting; TotalCecoMantenimiento += reparto.ImporteMantenimiento;
                    TotalCecoNeumáticos += reparto.ImporteNeumaticos; TotalCecoAdministracion += reparto.ImporteAdministracion;
                    TotalCecoSeguro += reparto.ImporteSeguro; TotalCecoITV += reparto.ImporteITV;

                    TotalDelegRenting += reparto.ImporteRenting; TotalDelegMantenimiento += reparto.ImporteMantenimiento;
                    TotalDelegNeumáticos += reparto.ImporteNeumaticos; TotalDelegAdministracion += reparto.ImporteAdministracion;
                    TotalDelegSeguro += reparto.ImporteSeguro; TotalDelegITV += reparto.ImporteITV;

                    TotalParcialRenting += reparto.ImporteRenting; TotalParcialMantenimiento += reparto.ImporteMantenimiento;
                    TotalParcialNeumáticos += reparto.ImporteNeumaticos; TotalParcialAdministracion += reparto.ImporteAdministracion;
                    TotalParcialSeguro += reparto.ImporteSeguro; TotalParcialITV += reparto.ImporteITV;

                    TotalGeneralRenting += reparto.ImporteRenting; TotalGeneralMantenimiento += reparto.ImporteMantenimiento;
                    TotalGeneralNeumáticos += reparto.ImporteNeumaticos; TotalGeneralAdministracion += reparto.ImporteAdministracion;
                    TotalGeneralSeguro += reparto.ImporteSeguro; TotalGeneralITV += reparto.ImporteITV;

                    //Comprobar si hay que escribir la cabecera de la tabla de detalle.
                    if (escribirCabeceraTabla != formatoCabeceraDetalle.SinCabecera)
                    {
                        EscribeCabeceraTabla(doc, centroCosteAnt);
                        escribirCabeceraTabla = formatoCabeceraDetalle.SinCabecera;
                    }

                    //Escribimos el detalle.
                    //table = new PdfPTable(8);
                    table = new PdfPTable(6);
                    table.TotalWidth = 500;
                    table.LockedWidth = true;
                    //widths = new float[] { 100f, 57f, 57f, 57f, 57f, 57f, 57f, 57f };
                    widths = new float[] { 119f, 76f, 76f, 76f, 76f, 76f };
                    table.SetWidths(widths);
                    table.AddCell(PDFCellUtility.cell($"{reparto.Matricula} {(reparto.Directivo ? "(Directivo)" : "") } ", 1, 1, PdfPCell.ALIGN_LEFT, false, false, bordeCompleto, 6));
                    table.AddCell(PDFCellUtility.cell($"{reparto.ImporteRenting.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, false, false, bordeCompleto, 6));
                    table.AddCell(PDFCellUtility.cell($"{reparto.ImporteMantenimiento.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, false, false, bordeCompleto, 6));
                    //table.AddCell(PDFCellUtility.cell($"{reparto.ImporteNeumaticos.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, false, false, bordeCompleto, 6));
                    //table.AddCell(PDFCellUtility.cell($"{(reparto.ImporteMantenimiento + reparto.ImporteNeumaticos).ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, false, false, bordeCompleto, 6));
                    table.AddCell(PDFCellUtility.cell($"{reparto.ImporteAdministracion.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, false, false, bordeCompleto, 6));
                    table.AddCell(PDFCellUtility.cell($"{reparto.ImporteSeguro.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, false, false, bordeCompleto, 6));
                    table.AddCell(PDFCellUtility.cell($"{reparto.ImporteITV.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, false, false, bordeCompleto, 6));
                    doc.Add(table);

                    if (reparto.Directivo)
                    {
                        TextoConDirectivo = "(CON vehíc.de dirección)";
                    }

                    cont++;
                }
                EscribirTotalesCeco(doc);
                EscribirTotalesDeleg(doc, delegacionAnt);
                EscribirTotalesParciales(doc, canariasAnt ? "CANARIAS" : "PENINSULA");
                EscribirTotalesGenerales(doc);
            }
            catch (Exception oEx)
            {
                throw new Exception($"-ExportRepartofacturaPDF- {paso} {Environment.NewLine} {oEx.Message}");
            }
            finally
            {
                doc.Close();
            }

            return memStream;
        }


        private void EscribeCabeceraTabla(Document _doc, string _centroCosteAnt)
        {
            if (escribirCabeceraTabla == formatoCabeceraDetalle.CabeceraContinuacion)
            {
                EscribeTextoCambioCentroCoste(_doc, _centroCosteAnt.Split('@')[0], $"{_centroCosteAnt.Split('@')[1]} ... (continuación)");
            }

            PdfPTable table = new PdfPTable(6);
            table.TotalWidth = 500;
            table.LockedWidth = true;
            //float[] widths = new float[] { 100f, 57f, 57f, 57f, 57f, 57f, 57f, 57f };
            float[] widths = new float[] { 119f, 76f, 76f, 76f, 76f, 76f };
            table.SetWidths(widths);

            table.AddCell(PDFCellUtility.cell("Matricula", 1, 1, PdfPCell.ALIGN_CENTER, true, true, bordeCompleto, 7));
            table.AddCell(PDFCellUtility.cell("Renting", 1, 1, PdfPCell.ALIGN_CENTER, true, true, bordeCompleto, 7));
            table.AddCell(PDFCellUtility.cell("Mantenimiento", 1, 1, PdfPCell.ALIGN_CENTER, true, true, bordeCompleto, 7));
            //table.AddCell(PDFCellUtility.cell("Neumáticos", 1, 1, PdfPCell.ALIGN_CENTER, true, true, bordeCompleto, 7));
            //table.AddCell(PDFCellUtility.cell("Mant+Neum.", 1, 1, PdfPCell.ALIGN_CENTER, true, true, bordeCompleto, 7));
            table.AddCell(PDFCellUtility.cell("Administración", 1, 1, PdfPCell.ALIGN_CENTER, true, true, bordeCompleto, 7));
            table.AddCell(PDFCellUtility.cell("Seguro", 1, 1, PdfPCell.ALIGN_CENTER, true, true, bordeCompleto, 7));
            table.AddCell(PDFCellUtility.cell("I.T.V.", 1, 1, PdfPCell.ALIGN_CENTER, true, true, bordeCompleto, 7));
            _doc.Add(table);
        }

        private void EscribeTextoCambioCanarias(Document _doc, FacturaRepartoModels _reparto)
        {
            var textoPeninsula_Canarias = _reparto.Canarias ? "*** CANARIAS ***" : "*** PENINSULA ***";
            PdfPTable table = new PdfPTable(1);
            table.TotalWidth = 500;
            table.LockedWidth = true;
            float[] widths = new float[] { 500f };
            table.SetWidths(widths);

            table.AddCell(PDFCellUtility.cell(textoPeninsula_Canarias, 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 11));
            _doc.Add(table);
            _doc.Add(new Paragraph(5f, " "));
        }

        private void EscribeTextoCambioDelegacion(Document _doc, string NombreDeleg, string CodDeleg)
        {
            _doc.Add(new Paragraph(10f, " "));
            var texto = $"{NombreDeleg} - {CodDeleg}";
            PdfPTable table = new PdfPTable(1);
            table.TotalWidth = 500;
            table.LockedWidth = true;
            float[] widths = new float[] { 500f };
            table.SetWidths(widths);

            if (CodDeleg == GlobalCostes.TEXTO_SIN_DELEGACION)
            {
                table.AddCell(PDFCellUtility.cell(" ", 1, 1, PdfPCell.ALIGN_LEFT, true, false, PDFConstants.BorderCellPDF.NO_BORDER, 10));
            }
            else
            {
                table.AddCell(PDFCellUtility.cell($"Delegación {texto}", 1, 1, PdfPCell.ALIGN_LEFT, true, false, PDFConstants.BorderCellPDF.NO_BORDER, 10));
            }
            _doc.Add(table);
            _doc.Add(new Paragraph(3f, " "));
        }

        private void EscribeTextoCambioCentroCoste(Document _doc, string NombreCeco, string CodCeco)
        {
            var texto = $"{NombreCeco} - {CodCeco}";
            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = 500;
            table.LockedWidth = true;
            float[] widths = new float[] { 10f, 490f };
            table.SetWidths(widths);

            table.AddCell(PDFCellUtility.cell("", 1, 1, PdfPCell.ALIGN_LEFT + PdfPCell.ALIGN_MIDDLE, true, false, PDFConstants.BorderCellPDF.NO_BORDER));
            table.AddCell(PDFCellUtility.cell($"Centro Coste {texto}", 1, 1, PdfPCell.ALIGN_LEFT, true, false, PDFConstants.BorderCellPDF.NO_BORDER));
            _doc.Add(table);
            _doc.Add(new Paragraph(2f, " "));
        }

        private void InicializaTotalesCeco()
        {
            TotalCecoRenting = 0; TotalCecoMantenimiento = 0; TotalCecoNeumáticos = 0;
            TotalCecoAdministracion = 0; TotalCecoSeguro = 0; TotalCecoITV = 0;
            TextoConDirectivo = "(SIN vehíc.de dirección)";
        }

        private void InicializaTotalesDeleg()
        {
            TotalDelegRenting = 0; TotalDelegMantenimiento = 0; TotalDelegNeumáticos = 0;
            TotalDelegAdministracion = 0; TotalDelegSeguro = 0; TotalDelegITV = 0;
        }

        private void InicializaTotalesParciales()
        {
            TotalParcialRenting = 0; TotalParcialMantenimiento = 0; TotalParcialNeumáticos = 0;
            TotalParcialAdministracion = 0; TotalParcialSeguro = 0; TotalParcialITV = 0;
        }

        private void EscribirTotalesCeco(Document _doc)
        {
            _doc.Add(new Paragraph(2f, " "));

            PdfPTable table = new PdfPTable(6);
            table.TotalWidth = 500;
            table.LockedWidth = true;
            //float[] widths = new float[] { 100f, 57f, 57f, 57f, 57f, 57f, 57f, 57f };
            float[] widths = new float[] { 119f, 76f, 76f, 76f, 76f, 76f };
            table.SetWidths(widths);

            table.AddCell(PDFCellUtility.cell($"Total Centro Coste {TextoConDirectivo}", 1, 1, PdfPCell.ALIGN_LEFT, true, false, PDFConstants.BorderCellPDF.NO_BORDER, 8));
            table.AddCell(PDFCellUtility.cell($"{TotalCecoRenting.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            table.AddCell(PDFCellUtility.cell($"{TotalCecoMantenimiento.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            //table.AddCell(PDFCellUtility.cell($"{TotalCecoNeumáticos.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            //table.AddCell(PDFCellUtility.cell($"{(TotalCecoMantenimiento + TotalCecoNeumáticos).ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            table.AddCell(PDFCellUtility.cell($"{TotalCecoAdministracion.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            table.AddCell(PDFCellUtility.cell($"{TotalCecoSeguro.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            table.AddCell(PDFCellUtility.cell($"{TotalCecoITV.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            EscribiendoTotales = true;
            _doc.Add(table);
        }

        private void EscribirTotalesDeleg(Document _doc, string Deleg)
        {
            if (Deleg.IndexOf(GlobalCostes.TEXTO_SIN_DELEGACION) <= 0)
            {
                _doc.Add(new Paragraph(2f, " "));

                PdfPTable table = new PdfPTable(6);
                table.TotalWidth = 500;
                table.LockedWidth = true;
                //float[] widths = new float[] { 100f, 57f, 57f, 57f, 57f, 57f, 57f, 57f };
                float[] widths = new float[] { 119f, 76f, 76f, 76f, 76f, 76f };
                table.SetWidths(widths);

                table.AddCell(PDFCellUtility.cell("Total Delegación", 1, 1, PdfPCell.ALIGN_LEFT, true, false, PDFConstants.BorderCellPDF.NO_BORDER, 7));
                table.AddCell(PDFCellUtility.cell($"{TotalDelegRenting.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
                table.AddCell(PDFCellUtility.cell($"{TotalDelegMantenimiento.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
                //table.AddCell(PDFCellUtility.cell($"{TotalDelegNeumáticos.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
                //table.AddCell(PDFCellUtility.cell($"{(TotalDelegMantenimiento + TotalDelegNeumáticos).ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
                table.AddCell(PDFCellUtility.cell($"{TotalDelegAdministracion.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
                table.AddCell(PDFCellUtility.cell($"{TotalDelegSeguro.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
                table.AddCell(PDFCellUtility.cell($"{TotalDelegITV.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
                EscribiendoTotales = true;
                _doc.Add(table);
            }
        }

        private void EscribirTotalesParciales(Document _doc, string TextoTotalParcial)
        {
            _doc.Add(new Paragraph(10f, " "));

            PdfPTable table = new PdfPTable(6);
            table.TotalWidth = 500;
            table.LockedWidth = true;
            //float[] widths = new float[] { 100f, 57f, 57f, 57f, 57f, 57f, 57f, 57f };
            float[] widths = new float[] { 119f, 76f, 76f, 76f, 76f, 76f };
            table.SetWidths(widths);

            table.AddCell(PDFCellUtility.cell($"Total {TextoTotalParcial}", 1, 1, PdfPCell.ALIGN_LEFT, true, false, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            table.AddCell(PDFCellUtility.cell($"{TotalParcialRenting.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            table.AddCell(PDFCellUtility.cell($"{TotalParcialMantenimiento.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            //table.AddCell(PDFCellUtility.cell($"{TotalParcialNeumáticos.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            //table.AddCell(PDFCellUtility.cell($"{(TotalParcialMantenimiento + TotalParcialNeumáticos).ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            table.AddCell(PDFCellUtility.cell($"{TotalParcialAdministracion.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            table.AddCell(PDFCellUtility.cell($"{TotalParcialSeguro.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            table.AddCell(PDFCellUtility.cell($"{TotalParcialITV.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            EscribiendoTotales = true;
            _doc.Add(table);

        }

        private void EscribirTotalesGenerales(Document _doc)
        {
            _doc.Add(new Paragraph(12f, " "));

            PdfPTable table = new PdfPTable(6);
            table.TotalWidth = 500;
            table.LockedWidth = true;
            //float[] widths = new float[] { 100f, 57f, 57f, 57f, 57f, 57f, 57f, 57f };
            float[] widths = new float[] { 119f, 76f, 76f, 76f, 76f, 76f };
            table.SetWidths(widths);

            table.AddCell(PDFCellUtility.cell($"Total General", 1, 1, PdfPCell.ALIGN_LEFT, true, false, PDFConstants.BorderCellPDF.NO_BORDER, 8));
            table.AddCell(PDFCellUtility.cell($"{TotalGeneralRenting.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            table.AddCell(PDFCellUtility.cell($"{TotalGeneralMantenimiento.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            //table.AddCell(PDFCellUtility.cell($"{TotalGeneralNeumáticos.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            //table.AddCell(PDFCellUtility.cell($"{(TotalGeneralMantenimiento + TotalGeneralNeumáticos).ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            table.AddCell(PDFCellUtility.cell($"{TotalGeneralAdministracion.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            table.AddCell(PDFCellUtility.cell($"{TotalGeneralSeguro.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            table.AddCell(PDFCellUtility.cell($"{TotalGeneralITV.ToString("###,###,##0.00")} €", 1, 1, PdfPCell.ALIGN_CENTER, true, true, PDFConstants.BorderCellPDF.NO_BORDER, 7));
            EscribiendoTotales = true;
            _doc.Add(table);
        }

        private void OnEventNewPagePDF(int lineProsessing, Document document)
        {
            //escribirCabeceraTabla = formatoCabeceraDetalle.CabeceraContinuacion;
            if (centroCosteAnt != string.Empty)
            {
                escribirCabeceraTabla = (escribirCabeceraTabla == formatoCabeceraDetalle.SinCabecera ? formatoCabeceraDetalle.CabeceraContinuacion : escribirCabeceraTabla);
                EscribeCabeceraTabla(document, centroCosteAnt);
                if (EscribiendoTotales)
                {
                    document.Add(new Paragraph(1f, " "));
                    EscribiendoTotales = false;
                }
                escribirCabeceraTabla = formatoCabeceraDetalle.SinCabecera;
            }
        }

        #endregion Reparto

        #region SAP
        public MemoryStream ExportRepartofacturaSAP_TXT(List<FacturaRepartoModels> RepartoLeasing)
        {
            var memStream = new MemoryStream();
            TextWriter tw = new StreamWriter(memStream);
            try
            {
                paso = "Inicializando documento";
                var globalCostes = new GlobalCostes();
                var cuenta = string.Empty;
                var importe = string.Empty;

                foreach (FacturaRepartoModels reparto in RepartoLeasing)
                {
                    paso = $"-Factura {reparto.NumFactura}-";
                    StringBuilder linea = new StringBuilder();
                    linea.Append($"{GetTextFormattingLength(reparto.NIFEmpresaLeasing, ' ', 10)}");
                    linea.Append($"{GetTextFormattingLength(reparto.NumFactura, ' ', 10)}");
                    linea.Append($"{GetTextFormattingLength(reparto.CentroCosteCoste, ' ', 6)}");
                    linea.Append((reparto.Directivo ? GlobalCostes.VEHICULO_DIRECCION.ToString() : " "));

                    AddValuesCuentaToLine(linea, reparto, GlobalCostes.TipoObjetoCoste.Alquiler);

                    AddValuesCuentaToLine(linea, reparto, GlobalCostes.TipoObjetoCoste.Mantenimiento);

                    AddValuesCuentaToLine(linea, reparto, GlobalCostes.TipoObjetoCoste.Administracion);

                    AddValuesCuentaToLine(linea, reparto, GlobalCostes.TipoObjetoCoste.Seguro);

                    AddValuesCuentaToLine(linea, reparto, GlobalCostes.TipoObjetoCoste.ITV);

                    linea.Append($"{reparto.FechaFactura.ToString("yyyyMMdd")}");
                    linea.Append($"{reparto.Impuesto.ToString("000.00").Replace(",", ".")}");
                    linea.Append($"{reparto.TotalFactura.ToString("0000000000.00").Replace(",", ".")}");
                    linea.Append($"{reparto.TotalImporteImpuesto.ToString("0000000000.00").Replace(",", ".")}");
                    linea.Append($"{GetTextFormattingLength(reparto.NombreCentroCoste, ' ', 30)}");

                    tw.WriteLine(linea);
                }
                tw.Flush();
            }

            catch (Exception oEx)
            {
                throw new Exception($"-ExportRepartofacturaSAP_TXT- {paso} {Environment.NewLine} {oEx.Message}");
            }

            return memStream;
        }

        private string GetTextFormattingLength(string texto, char caracter, int tamaño)
        {
            if (string.IsNullOrEmpty(texto))
            {
                texto = string.Empty;
            }
            else
            {
                texto = texto.Trim(' ');
                if (texto.Length > tamaño)
                {
                    texto = texto.Substring(0, tamaño-1);
                }
            }
            string valorReturn = $"{texto.Trim(' ')}{new String(caracter, tamaño - texto.Trim(' ').Length)}";

            return valorReturn;
        }

        private void AddValuesCuentaToLine(StringBuilder linea, FacturaRepartoModels reparto, GlobalCostes.TipoObjetoCoste coste)
        {

            var cuenta = GlobalCostes.GetCodigoCuentaContableAsociada(reparto.IDEmpresaFatura, coste);
            linea.Append($"{GetTextFormattingLength(cuenta, ' ', 8)}");
            var importe = reparto.ImporteCuentaContable(cuenta).ToString("000000.00").Replace(",", ".");
            linea.Append($"{GetTextFormattingLength(importe, ' ', 9)}");

        }
        #endregion SAP
    }
}
