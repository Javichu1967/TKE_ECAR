using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TK_ECAR.Framework.PDF.PDFConstants;

namespace TK_ECAR.Framework.PDF
{
    class PDFCellUtility
    {
        private static Font bold = FontFactory.GetFont("Arial", Font.BOLD, Font.BOLD, BaseColour.blue);
        private static Font normal = FontFactory.GetFont("Arial", Font.DEFAULTSIZE, Font.NORMAL, BaseColour.blue);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTexto"></param>
        /// <param name="colSpan"></param>
        /// <param name="rowSpan"></param>
        /// <param name="alineacion"></param>
        /// <param name="bNegrita"></param>
        /// <returns></returns>
        public static PdfPCell cell(String strTexto, int colSpan, int rowSpan, int alineacion, bool bNegrita, bool backcolor = false, 
                                    BorderCellPDF border = BorderCellPDF.BOTTOM_BORDER | BorderCellPDF.TOP_BORDER | 
                                    BorderCellPDF.LEFT_BORDER | BorderCellPDF.RIGHT_BORDER, 
                                    int fontSize = 9)
        {

        //public const int BOTTOM_BORDER = 2;
        //public const int BOX = 15;
        //public const int LEFT_BORDER = 4;
        //public const int NO_BORDER = 0;
        //public const int RIGHT_BORDER = 8;
        //public const int TOP_BORDER = 1;


        Font _font;
            if (bNegrita)
            {
                _font = bold;
                _font.Size = fontSize;
            }
            else
            {
                _font = normal;
                _font.Size = fontSize;
            }

            //Chunk texto = new Chunk(strTexto, negrita);
            //Phrase frase = new Phrase(texto);


            PdfPCell cell = new PdfPCell(new Paragraph(strTexto, _font));
            cell.Colspan = colSpan;
            cell.Rowspan = rowSpan;
            cell.HorizontalAlignment = alineacion; //0=Left, 1=Centre, 2=Right
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.BorderColor = BaseColour.darkgrey;
            //cell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
            cell.Border = (int)border;
            //cell.BorderColor = (borderVisible ? BaseColour.darkgrey : BaseColour.white);
            //cell.Border = PdfPCell.BOTTOM_BORDER;

            if (backcolor)
            {
                cell.BackgroundColor = BaseColour.lightgrey;
            }
            return cell;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTexto"></param>
        /// <param name="colSpan"></param>
        /// <param name="rowSpan"></param>
        /// <param name="alineacion"></param>
        /// <param name="bNegrita"></param>
        /// <returns></returns>
        public static PdfPCell cellSign(String strTexto)
        {
            Font negrita = FontFactory.GetFont("Arial", Font.DEFAULTSIZE, Font.NORMAL);
            negrita.Size = 10;
            Chunk texto = new Chunk(strTexto, negrita);
            Phrase frase = new Phrase(texto);
            PdfPCell cell = new PdfPCell(frase);
            cell.Border = PdfPCell.NO_BORDER;
            cell.Colspan = 1;
            cell.Rowspan = 1;
            cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            return cell;
        }
        /// <summary>
        /// creamos una celda con otro estilo de letra más grande y fondo gris.
        /// </summary>
        /// <param name="strTexto"></param>
        /// <param name="span"></param>
        /// <param name="alineacion"></param>
        /// <param name="bNegrita"></param>
        /// <returns></returns>
        public static PdfPCell cellTitle(String strTexto, int span)
        {
            Font negrita;

            //negrita = new Font(Font.FontFamily.COURIER, Font.DEFAULTSIZE, Font.BOLD, BaseColor.BLACK);          
            negrita = FontFactory.GetFont("Arial", Font.DEFAULTSIZE, Font.BOLD, BaseColour.black);
            negrita.Size = 4;
            //Font lightblue = new Font(Font.COURIER, 9f, Font.NORMAL, new Color(43, 145, 175));
            Chunk texto = new Chunk(strTexto, negrita);
            Phrase frase = new Phrase(texto);
            PdfPCell cell = new PdfPCell(frase);
            cell.Colspan = span;
            cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT; //0=Left, 1=Centre, 2=Right
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            //cell.BackgroundColor = BaseColor.BLACK;            
            return cell;
        }

        /// <summary>
        /// creamos una celda que se ajusta a las medidas de un subtitulo dentro de los informes pdf
        /// </summary>
        /// <param name="strTexto"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public static PdfPCell celdacSubTitulo(String strTexto, int span)
        {
            Font negrita;

            //negrita = new Font(Font.FontFamily.COURIER, Font.DEFAULTSIZE, Font.BOLD, BaseColor.BLACK);
            negrita = FontFactory.GetFont("Verdana", Font.DEFAULTSIZE, Font.BOLD);
            negrita.Size = 10;
            //Font lightblue = new Font(Font.COURIER, 9f, Font.NORMAL, new Color(43, 145, 175));
            Chunk texto = new Chunk(strTexto, negrita);
            Phrase frase = new Phrase(texto);
            PdfPCell cell = new PdfPCell(frase);
            cell.Colspan = span;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            //cell.BackgroundColor = #8996a0;
            return cell;
        }

        /// <summary>
        /// creamos una celda que se ajusta a las medidas de un subtitulo dentro de los informes pdf
        /// </summary>
        /// <param name="strTexto"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public static PdfPCell celdaSubTitulo(String strTexto, int colSpan, int rowSpan, int alineacion, bool bNegrita)
        {
            Font negrita;
            if (bNegrita) negrita = FontFactory.GetFont("Verdana", Font.DEFAULTSIZE, Font.BOLD);
            else negrita = FontFactory.GetFont("Verdana", Font.DEFAULTSIZE, Font.NORMAL);
            negrita.Size = 10;
            //Font lightblue = new Font(Font.COURIER, 9f, Font.NORMAL, new Color(43, 145, 175));
            Chunk texto = new Chunk(strTexto, negrita);
            Phrase frase = new Phrase(texto);
            PdfPCell cell = new PdfPCell(frase);
            cell.Colspan = colSpan;
            cell.Rowspan = rowSpan;
            cell.HorizontalAlignment = alineacion; //0=Left, 1=Centre, 2=Right
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            // cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            return cell;
        }


        #region "borde"
        //public PdfPCell BorderCellPDF(PdfPCell CeldaPDF, PDFConstants.BorderCellPDF borde)
        //{
        //    //Creamos un Objeto de tipo PdfPCell en el cual agregamos directo nuestro texto como vera utilizo Paragraph con un tipo de letra en mi caso fijo pero si lo desean pueden enviarlo como parametro, un tamaño y un color de letra que se envian como parametro

        //    //Este codigo es referente a los border, como podran ver tengo varias combinancaciones para poner el borde deseado

        //    if (borde == PDFConstants.BorderCellPDF.NO_BORDER)
        //    {
        //        CeldaPDF.Border = 0;
        //    }
        //    else
        //    {
        //        if (borde == PDFConstants.BorderCellPDF.BOTTOM_BORDER)
        //        {
        //            CeldaPDF.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
        //        }
        //        else
        //        {
        //            if (borde == PDFConstants.BorderCellPDF.TOP_BORDER)
        //            {
        //                CeldaPDF.Border = iTextSharp.text.Rectangle.TOP_BORDER;
        //            }
        //            else
        //            {
        //                if (borde == PDFConstants.BorderCellPDF.LEFT_BORDER)
        //                {
        //                    CeldaPDF.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
        //                }
        //                else
        //                {
        //                    if (borde == PDFConstants.BorderCellPDF.RIGHT_BORDER)
        //                    {
        //                        CeldaPDF.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
        //                    }
        //                    else
        //                    {
        //                        if (borde == PDFConstants.BorderCellPDF.ALL_BORDER)
        //                        {
        //                            CeldaPDF.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
        //                        }
        //                        else
        //                        {
        //                            if (borde == PDFConstants.BorderCellPDF.BOTTOM_LEFT_BORDER)
        //                            {
        //                                CeldaPDF.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER;
        //                            }
        //                            else
        //                            {
        //                                if (borde == PDFConstants.BorderCellPDF.BOTTOM_RIGHT_BORDER)
        //                                {
        //                                    CeldaPDF.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
        //                                }
        //                                else
        //                                {
        //                                    if (borde == PDFConstants.BorderCellPDF.TOP_LEFT_BORDER)
        //                                    {
        //                                        CeldaPDF.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER;
        //                                    }
        //                                    else
        //                                    {
        //                                        if (borde == PDFConstants.BorderCellPDF.TOP_RIGHT_BORDER)
        //                                        {
        //                                            CeldaPDF.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return CeldaPDF;
        //}
        #endregion
    }
}
