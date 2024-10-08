using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{

    public class DatosVehiculoLeasePlanModel
    {
        public string Ejercicio { get; set; }

        public int? Sociedad { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblMatricula")]
        public string Matricula { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblTrimestre")]
        public string Trimestre { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFechaFactura")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FechaFactura { get; set; }

        public string NumFactura { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblImpRenting")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double? ImpRenting { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblImpMantenimiento")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double? ImpMantenimiento { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblImpAdministracion")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double? ImpAdministracion { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblImpSeguro")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double? ImpSeguro { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblImpITV")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double? ImpITV { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblImpTotal")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double? ImpTotal
        {
            get
            {
                return ImpRenting + ImpMantenimiento + ImpAdministracion + ImpSeguro + ImpITV;
            }
        }
    }


}
