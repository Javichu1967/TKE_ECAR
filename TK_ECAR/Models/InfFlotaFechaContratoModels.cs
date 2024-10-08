using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using TK_ECAR.Framework.Utils;
using static TK_ECAR.Framework.Utils.GlobalCostes;

namespace TK_ECAR.Models
{

    public class InformeContratosRentigPorFechaFinalizacionModels
    {
        public FilterInformeFlotaModel FiltroUtilizado { get; set; }
        public List<InfFlotaFechaContratoModels> vehiculosLeasing { get; set; }
    }
    public class InfFlotaFechaContratoModels
    {
        public string MarcaVehiculo { get; set; }
        public string ModeloVehiculo { get; set; }
        public int TipoVehiculo { get; set; }
        public int CodEmpresa { get; set; }
        public string Empresa { get; set; }
        public string Delegacion { get; set; }
        public string DirTerritorial { get; set; }
        public string CodCeco { get; set; }
        public string NombreCeco { get; set; }
        public string codCecoMasNombre
        {
            get
            {
                return $"{CodCeco} - {NombreCeco}";
            }
        }
        public string Matricula { get; set; }
        public string Bastidor { get; set; }
        public string CiaLeasing { get; set; }
        public string NumContratoLeasing { get; set; }
        public DateTime FechaVtoLeasing { get; set; }
        public string CiaSeguro { get; set; }
        public string Poliza_Seguro { get; set; }
        public double? ImpSeguro { get; set; }
        public string ImpSeguro_FORMATEADO
        {
            get
            {
                return ConvertExtensions.NullableToFormattedString((decimal?)(ImpSeguro == null ? 0.0 : ImpSeguro), "###,###,##0.00");
            }
        }
        public DateTime? FechaVtoSeguro { get; set; }
        public string MesAñoVtoLeasing
        {
            get
            {
                if (FechaVtoLeasing == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    var mes = FechaVtoLeasing.Month.ToString("00");
                    var año = FechaVtoLeasing.Year.ToString();
                    return $"{año} {mes}";
                }
            }
        }

        public string MesAñoVtoLeasingText
        {
            get
            {
                if (FechaVtoLeasing == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return $"{FechaVtoLeasing.ToString("MMMM", Thread.CurrentThread.CurrentCulture)} {FechaVtoLeasing.Year.ToString()}" ;
                }
            }
        }

    }
}
