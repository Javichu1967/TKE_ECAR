using System;
using System.Collections.Generic;
using System.Globalization;
using TK_ECAR.Framework.Utils;
using static TK_ECAR.Framework.Utils.GlobalCostes;

namespace TK_ECAR.Models
{
    public class FacturaDataTableModels
    {
        public string EmpresaFactura { get; set; }
        public string EmpresaLeasing { get; set; }
        public string NumFactura { get; set; }
        public DateTime FechaFactura { get; set; }
        public double BaseFactura { get; set; }
        public string BaseFactura_FORMATEADA
        {
            get
            {
                return ConvertExtensions.NullableToFormattedString((decimal?)BaseFactura, "###,###,##0.00");
            }
        }
        public double ImpuestoFactura { get; set; }
        public string ImpuestoFactura_FORMATEADA
        {
            get
            {
                return ConvertExtensions.NullableToFormattedString((decimal?)ImpuestoFactura, "###,###,##0.00");
            }
        }
        public double TotalFactura { get; set; }
        public string TotalFactura_FORMATEADA
        {
            get
            {
                return ConvertExtensions.NullableToFormattedString((decimal?)TotalFactura, "###,###,##0.00");
            }
        }
    }
}
