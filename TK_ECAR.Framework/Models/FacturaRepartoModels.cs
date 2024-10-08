using System;
using System.Collections.Generic;
using System.Globalization;
using TK_ECAR.Framework.Utils;

namespace TK_ECAR.Framework.Models
{

    public class FacturaRepartoModels
    {
        public string Matricula { get; set; }
        public bool Directivo { get; set; }
        public int IDEmpresaLeasing { get; set; }
        public string NombreEmpresaLeasing { get; set; }
        public string NIFEmpresaLeasing { get; set; }
        public int IDEmpresaFatura { get; set; }
        public string NombreEmpresaFatura { get; set; }
        public string IDDelegacionFatura { get; set; }
        public string NombreDelegacionFatura { get; set; }
        public string CentroCosteCoste { get; set; }
        public string NombreCentroCoste { get; set; }
        public string NumFactura { get; set; }
        public DateTime FechaFactura { get; set; }
        public string MesFactura
        {
            get
            {
                if (FechaFactura == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    //var mes = FechaFactura.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                    //var año = FechaFactura.Year.ToString();
                    //return $"{mes}-{año}";
                    return String.Format("{0:y}", FechaFactura);
                }
            }
        }


        public string AñoMesFactura
        {
            get
            {
                if (FechaFactura == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    var mes = FechaFactura.Month.ToString("00");
                    var año = FechaFactura.Year.ToString();
                    return $"{año} {mes}";
                }
            }
        }

        public double ImporteRenting { get; set; }
        public double ImporteMantenimiento { get; set; }
        public double ImporteNeumaticos { get; set; }
        public double ImporteAdministracion { get; set; }
        public double ImporteSeguro { get; set; }
        public double ImporteITV { get; set; }

        public double Impuesto { get; set; }

        public double TotalFactura { get; set; }
        public double TotalImporteImpuesto { get; set; }

        public bool Canarias { get; set; }

        public double ImporteCuentaContable(string cuenta)
        {
            double valorReturn = 0.0;

            List<GlobalCostes.TipoObjetoCoste> costes = GlobalCostes.GetTiposCosteCuentaContableAsociada(IDEmpresaFatura, cuenta);

            foreach(GlobalCostes.TipoObjetoCoste coste in costes)
            {
                switch (coste)
                {
                    case GlobalCostes.TipoObjetoCoste.Administracion:
                        valorReturn += ImporteAdministracion;
                        break;
                    case GlobalCostes.TipoObjetoCoste.Alquiler:
                        valorReturn += ImporteRenting;
                        break;
                    case GlobalCostes.TipoObjetoCoste.ITV:
                        valorReturn += ImporteITV;
                        break;
                    case GlobalCostes.TipoObjetoCoste.Mantenimiento:
                        valorReturn += ImporteMantenimiento;
                        break;
                    case GlobalCostes.TipoObjetoCoste.Neumaticos:
                        valorReturn += ImporteNeumaticos;
                        break;
                    case GlobalCostes.TipoObjetoCoste.Seguro:
                        valorReturn += ImporteSeguro;
                        break;
                }
            }

            return valorReturn;
        }
    }
}
