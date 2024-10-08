using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TK_ECAR.Models.Portugal
{
    public class EXTRACTO
    {
        public string id { get; set; }
        public int MES_EMISSAO { get; set; }
        public int AÑO_EMISSAO { get; set; }
        public CLIENTE Cliente { get; set; }
        public  List<Identificador> IDENTIFICADOR { get; set; }
        public decimal TOTAL { get; set; }
        public decimal TOTAL_IVA { get; set; }
        public string NOMBRE_ARCHIVO_IMPORTACION { get; set; }

    }

    public class CLIENTE
    {
        public string id { get; set; }
        public string NIF { get; set; }
        public string NOME { get; set; }
        public string MORADA { get; set; }
        public string LOCALIDADE { get; set; }
        public string CODIGO_POSTAL { get; set; }
    }

    public class Identificador
    {
        public string id { get; set; }
        private string _matricula = string.Empty;
        public string MATRICULA
        { get
            { return _matricula; }

            set
            {
                if (value.IndexOf('-') == -1)
                {
                    _matricula = _matricula.Trim().Trim(' ');
                    _matricula = $"{value.Substring(0, 2)}-{value.Substring(2, 2)}-{value.Substring(4)}"; 
                }
                else
                {
                    _matricula = value;
                }
            }
        }
        public string REF_PAGAMENTO { get; set; }

        public List<TRANSACCIONES> TRANSACCAO { get; set; }
        public decimal TOTAL { get; set; }

    }


    public class TRANSACCIONES
    {
        public DateTime? DATA_ENTRADA { get; set; }
        public string HORA_ENTRADA { get; set; }

        public DateTime? DATA_ENTRADA_COMPLETA
        { get
            {
                DateTime? valorReturn = null;
                if (DATA_ENTRADA != null)
                {
                    if (!string.IsNullOrEmpty(HORA_ENTRADA))
                    {
                        valorReturn = Convert.ToDateTime(DATA_ENTRADA.ToString().Replace("0:00:00", "")  + HORA_ENTRADA);
                    }
                    else
                    {
                        valorReturn = DATA_ENTRADA;
                    }
                }
                return valorReturn;
            }
        }

        public string ENTRADA { get; set; }
        public DateTime? DATA_SAIDA { get; set; }
        public string HORA_SAIDA { get; set; }
        public DateTime? DATA_SAIDA_COMPLETA
        {
            get
            {
                DateTime? valorReturn = null;
                if (DATA_SAIDA != null)
                {
                    if (!string.IsNullOrEmpty(HORA_SAIDA))
                    {
                        valorReturn = Convert.ToDateTime(DATA_SAIDA.ToString().Replace("0:00:00", "") + HORA_SAIDA);
                    }
                    else
                    {
                        valorReturn = DATA_SAIDA;
                    }
                }
                return valorReturn;
            }
        }
        public string SAIDA { get; set; }
        public decimal IMPORTANCIA { get; set; }
        public decimal VALOR_DESCONTO { get; set; }
        public int TAXA_IVA { get; set; }
        public string OPERADOR { get; set; }
        public string TIPO { get; set; }
        public DateTime? DATA_DEBITO { get; set; }
        public string CARTAO { get; set; }

    }

}
