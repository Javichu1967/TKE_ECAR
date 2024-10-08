using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{
    public class Via_Verde_Extractos
    {
        public string ID_EXTRACTO { get; set; }
        public int MES_EMISION { get; set; }
        public int AÑO_EMISION { get; set; }
        public string ID_CLIENTE { get; set; }
        public  decimal? TOTAL { get; set; }
        public  decimal? TOTAL_IVA { get; set; }
        public List<Via_Verde_Identificadores> IDENTIFICADORES { get; set; }

        public ClientesModels CLIENTE { get; set; }
    }

    public class Via_Verde_Identificadores
    {
        public int ID_IDENTIFICADOR { get; set; }
        public string IDENTIFICADOR { get; set; }
        public string ID_EXTRACTO { get; set; }
        public string MATRICULA { get; set; }
        public string REF_PAGO { get; set; }
        public decimal TOTAL { get; set; }
        public List<Via_Verde_Transacciones> TRANSACCIONES { get; set; }
    }


    public class Via_Verde_Transacciones
    {
        public int ID_TRANSACCION { get; set; }
        public int ID_IDENTIFICADOR { get; set; }
        public DateTime? FECHA_ENTRADA { get; set; }
        public string LUGAR_ENTRADA { get; set; }
        public  DateTime? FECHA_SALIDA { get; set; }
        public string LUGAR_SALIDA { get; set; }
        public  decimal? IMPORTE { get; set; }
        public  decimal? DECUENTO { get; set; }
        public  decimal? PORCENTAJE_IMPUESTO { get; set; }
        public string OPERADOR { get; set; }
        public string TIPO { get; set; }
        public  DateTime? FECHA_TARJETA { get; set; }
        public string NUM_TARJETA { get; set; }

    }


    public class ViaVerdeDatatable
    {
        public string MATRICULA { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblTotalImporte")]
        public decimal? TOTAL_IMPORTE { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblTotalDescuento")]
        public decimal? TOTAL_DESCUENTO { get; set; }

        public List<ViaVerdeLineaDatatable> LineasDataTable { get; set; }
    }

    public class ViaVerdeLineaDatatable
    {
        public string REF_PAGO { get; set; }
        public DateTime? FECHA_ENTRADA { get; set; }
        public string LUGAR_ENTRADA { get; set; }
        public DateTime? FECHA_SALIDA { get; set; }
        public string LUGAR_SALIDA { get; set; }
        public decimal? IMPORTE { get; set; }
        public decimal? DECUENTO { get; set; }
        public string OPERADOR { get; set; }
    }

}
