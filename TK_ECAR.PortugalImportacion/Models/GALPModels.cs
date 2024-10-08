using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK_ECAR.Utils;

namespace TK_ECAR.PortugalImportacion.Models
{
    public class GALPModels
    {
        public int? Sociedad
        {
            get
            {
                return Constants.CODIGO_EMPRESA_PORTUGAL;
            }
        }
        public string EJERCICIO { get; set; }
        public string TRIMESTRE { get; set; }
        public DateTime? FECHA_FACTURA { get; set; }
        public DateTime? FECHA_OPERACION { get; set; }
        private string _matricula;
        public string MATRICULA
        {
            get
            {
                if (_matricula.Length != 6)
                {
                    return _matricula;
                }
                else
                {
                    return $"{_matricula.Substring(0, 2)}-{_matricula.Substring(2, 2)}-{_matricula.Substring(4)}";
                }
            }
            set
            {
                _matricula = value;
            }
        }
        public string DES_PRODU  { get; set; }
        public string COD_PRODU { get; set; }
        public decimal KILOMETROS { get; set; }
        public decimal NUM_LITROS { get; set; }
        public decimal IMPORTE { get; set; }
        public decimal BONIF_TOTAL { get; set; }
        public decimal IVA { get; set; }

        public decimal IMP_TOTAL { get; set; }
        public DateTime FechaAlta
        {
            get
            {
                return DateTime.Now;
            }
                
        }
        public decimal KmsCiclo { get; set; }
        public string NUM_DOCUMENTO { get; set; }
        public string COD_TARJETA { get; set; }

    }
}
