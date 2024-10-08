using System;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Globalization;
using System.Net;
using System.IO;

namespace TK_ECAR.Framework.PDF
{
    public class PDFEvents
    {

        public class PDFEventsUtility : PdfPageEventHelper
        {
            public delegate void NewPagePDFEventHandler(int numPage, Document document);
            //Evento de nueva de página.
            public event NewPagePDFEventHandler EventNewPagePDF;

            private Uri urlLogoTKE;
            private string strMensajeCabecera;
            private string strMensajeCabecera1;
            private string strMensajeCabecera2;
            private Boolean rotate = false;
            private string UserPDF = string.Empty;

            public PDFEventsUtility(Uri urlLogo, string strMensajeCabecera, bool rotarPagina)
            {
                this.urlLogoTKE = urlLogo;
                this.strMensajeCabecera = strMensajeCabecera;
                this.rotate = rotarPagina;
            }

            public PDFEventsUtility(Uri urlLogo, string _strMensajeCabecera, string _strMensajeCabecera1, string _strMensajeCabecera2, bool rotarPagina)
            {
                this.urlLogoTKE = urlLogo;
                this.strMensajeCabecera = _strMensajeCabecera;
                this.strMensajeCabecera1 = _strMensajeCabecera1;
                this.strMensajeCabecera2 = _strMensajeCabecera2;
                this.rotate = rotarPagina;
            }


            /// <summary>
            /// añadir logo al pdf
            /// </summary>
            /// <param name="doc"></param>
            /// <returns></returns>
            public Image getLogo(Document doc)
            {
                Image imagen = null;

                if (urlLogoTKE != null)
                {
                    HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(urlLogoTKE);
                    HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();
                    Stream sPathFile1 = aResponse.GetResponseStream();

                    System.Drawing.Image _imagen = new System.Drawing.Bitmap(sPathFile1);
                    imagen = Image.GetInstance((System.Drawing.Image)new System.Drawing.Bitmap(_imagen, 130, 35), Color.WHITE);

                    imagen.ScalePercent(80f);//(70f)
                    if (rotate)
                    {
                        imagen.SetAbsolutePosition(doc.PageSize.Width - imagen.Width - 25f, doc.PageSize.Height - 86f); //Posicion en el eje cartesiano de X y Y
                    }
                    else
                    {
                        imagen.SetAbsolutePosition(doc.PageSize.Width - (imagen.Width + 25f), doc.PageSize.Height-50f); //-86 Posicion en el eje cartesiano de X y Y                        
                    }
                }

                return imagen;
            }

            /// <summary>
            /// añade el título al lado del logo
            /// </summary>
            /// <param name="writer"></param>
            /// <param name="iAlto"></param>
            /// <param name="iAncho"></param>
            public void getHeaderReport(PdfWriter writer, float iAlto, float iAncho, Document doc)
            {
                // step 4: we grab the ContentByte and do some stuff with it
                PdfContentByte cb = writer.DirectContent;
                BaseFont bf2;

                bf2 = FontFactory.GetFont("Arial", Font.DEFAULTSIZE, Font.BOLD).GetCalculatedBaseFont(false);
                cb.SetFontAndSize(bf2, Font.DEFAULTSIZE);
                cb.SetColorStroke(BaseColour.black);
                cb.SetColorFill(BaseColour.black);
                cb.SetLineWidth(3);
                cb.LineTo(30, cb.YTLM);
                // we tell the ContentByte we're ready to draw text
                cb.BeginText();
                // we draw some text on a certain position

                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, strMensajeCabecera, iAncho, iAlto - 25, 0);

                float[] widths = new float[] { 500f };
                if (!string.IsNullOrEmpty(strMensajeCabecera1))
                {
                    doc.Add(new Paragraph(50f, " "));

                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 500;
                    table.LockedWidth = true;
                    table.SetWidths(widths);
                    Font fb2 = FontFactory.GetFont("Arial", Font.DEFAULTSIZE, Font.BOLD);
                    PdfPCell cell = new PdfPCell(new Phrase(strMensajeCabecera1, fb2));
                    cell.Colspan = 0; // either 1 if you need to insert one cell
                    cell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    cell.BackgroundColor = BaseColour.lightgrey;
                    table.AddCell(cell);

                    doc.Add(table);
                    //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, strMensajeCabecera1, iAncho, iAlto - 36, 0);
                }

                if (!string.IsNullOrEmpty(strMensajeCabecera2))
                {
                    doc.Add(new Paragraph(10f, " "));

                    PdfPTable table1 = new PdfPTable(1);
                    table1.TotalWidth = 500;
                    table1.LockedWidth = true;
                    table1.SetWidths(widths);
                    Font fb3 = FontFactory.GetFont("Arial", Font.DEFAULTSIZE, Font.BOLD);
                    fb3.Size = 12;
                    PdfPCell cell1 = new PdfPCell(new Phrase(strMensajeCabecera2, fb3));
                    cell1.Colspan = 0; // either 1 if you need to insert one cell
                    cell1.BorderColor = BaseColour.white;
                    cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    table1.AddCell(cell1);

                    doc.Add(table1);

                    doc.Add(new Paragraph(15f, " "));
                }
                // we tell the contentByte, we've finished drawing text
                cb.EndText();
            }


            public void setHeaderReport(Document doc)
            {
                PdfPTable table = new PdfPTable(1);
                // Añadir la cabecera

                Font fb2 = FontFactory.GetFont("Arial", Font.DEFAULTSIZE, Font.BOLD);
                PdfPCell cell = new PdfPCell(new Phrase("setHeaderReport", fb2));
                cell.Colspan = 0; // either 1 if you need to insert one cell
                cell.Border = 0;
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(cell);

                doc.Add(table);
                doc.Add(new Paragraph(" "));
                doc.Add(table);
            }

            /// <summary>
            /// se crea el footer para el final de pagina. Fecha y Nº de página
            /// </summary>
            /// <param name="writer"></param>
            /// <param name="iAlto"></param>
            /// <param name="iAncho"></param>
            public void getFooterReport(PdfWriter writer, float iAlto, float iAncho)
            {

                // step 4: we grab the ContentByte and do some stuff with it
                PdfContentByte cb = writer.DirectContent;
                BaseFont bf2 = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);
                cb.SetFontAndSize(bf2, 9);
                cb.SetColorStroke(BaseColour.mediumgrey);
                cb.SetColorFill(BaseColour.mediumgrey);

                cb.SetLineWidth(3);
                cb.LineTo(1, cb.YTLM);
                // we tell the ContentByte we're ready to draw text
                cb.BeginText();
                // we draw some text on a certain position
                cb.SetTextMatrix(iAncho, iAlto);
                cb.ShowText($"Pag. - {writer.PageNumber} -");
                // we tell the contentByte, we've finished drawing text
                cb.EndText();
            }

 
            public void setHeaderStringSec(string str)
            {
                this.strMensajeCabecera1 = str;
            }

            /// <summary>
            /// Gestión de eventos de nueva página
            /// </summary>
            /// <param name="writer"></param>
            /// <param name="document"></param>
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);

                document.Add(getLogo(document));

                getHeaderReport(writer, (document.PageSize.Height - 50), (document.PageSize.Width / 2), document);

                getFooterReport(writer, 10, 10);

                OnProcessNewPagePDFEventHandler(writer.PageNumber, document);
            }

            #region eventos
            private void OnProcessNewPagePDFEventHandler(int numPage, Document document)
            {
                EventNewPagePDF?.Invoke(numPage, document);
            }

            #endregion
        }
    }

}




