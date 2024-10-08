using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{

    public class DatosVehiculoSOLREDModel
    {
        public string Ejercicio { get; set; }

        public int? Sociedad { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblMatricula")]
        public string Matricula { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFechaFactura")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FechaFactura { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFechaOperacion")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FechaOperacion { get; set; }

        public string DescProducto { get; set; }

        public double? Kilometros { get; set; }

        public double? Litros { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblImporte")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double? Importe { get; set; }

        public string NumDocumento { get; set; }
        public string CodTarjeta { get; set; }


    }


}
