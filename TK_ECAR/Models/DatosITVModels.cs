using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Filters;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{

    public class DatosITVModel
    {
        public int ID { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblMatricula")]
        public string Matricula { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFechaUltimaITV")]
        //[Required(ErrorMessageResourceName = "RequiredFechaUltimaITV", ErrorMessageResourceType = typeof(resources))]
        [fechasITVCorrectas(fieldFechaVto = "FechaVtoITV", fieldLinea = "Linea", ErrorMessageResourceName = "FechasITV_Correctas",
                    ErrorMessageResourceType = typeof(resources))]
        [fechaUltimaITVNoObligatoriaEnPrimeraITV(fieldLinea = "Linea", ErrorMessageResourceName = "RequiredFechaUltimaITV",
                    ErrorMessageResourceType = typeof(resources))]
        [fechasITVRellenas(fieldFechaVTO = "FechaVtoITV", ErrorMessageResourceName = "RequiredAlMenosUnaFechaITV",
                    ErrorMessageResourceType = typeof(resources))]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? FechaUltimaITV { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFechaVtoITV")]
        [Required(ErrorMessageResourceName = "RequiredFechaVtoITV", ErrorMessageResourceType = typeof(resources))]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [GenericCompare(CompareToPropertyName = "FechaUltimaITV", OperatorName = GenericCompareOperator.GreaterThan,
                    ErrorMessageResourceName = "FechaVtoITVMayorFechaUltimaITV",
                    ErrorMessageResourceType = typeof(resources))]
        //[fechaVtoITVvacia(fieldLinea = "Linea",ErrorMessageResourceName = "FechaVtoITV_Vacia",
        //            ErrorMessageResourceType = typeof(resources))]
        [DataType(DataType.Date)]
        public DateTime? FechaVtoITV { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblTarifaITV")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double? TarifaITV { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblTasaITV")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double? TasaITV { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblImporteITV")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double? ImporteITV { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblPrimaConservacionITV")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double? PrimaConservacionITV { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblImporteCirculacionITV")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double? ImporteCirculacionITV { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblObservaciones")]
        public string Observaciones { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFile")]
        public string Documento { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblITV_Pasada")]
        [ValorITVyaPasada(ErrorMessageResourceName = "ITV_PasadaError", ErrorMessageResourceType = typeof(resources))]
        public bool? ITV_Pasada { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFile")]
        //[Required(ErrorMessageResourceName = "RequiredFile", ErrorMessageResourceType = typeof(resources))]
        [DataType(DataType.Upload)]
        [UIHint("UploadMofificado")]
        public HttpPostedFileBase FileUpload { get; set; }
        public string FileUpload_download { get; set; }

        public string TipoArchivo { get; set; }

        public EnumAccionEntity Accion { get; set; }

    }

    public class DatosITV_TMPModel: DatosITVModel
    {
        public int Linea { get; set; }
        public string Login { get; set; }
        public bool LineaNueva { get; set; }
        public string AccionDataTable { get; set; }
    }
}
