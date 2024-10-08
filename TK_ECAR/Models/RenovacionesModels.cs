using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Filters;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{
     
    
    public class RenovarModel
    {
        [ScaffoldColumn(false)]
        public int IdAlerta { get; set; }

        [ScaffoldColumn(false)]
        public int IdEstado { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblMatricula")]
        [Required(ErrorMessageResourceName = "RequiredMatricula", ErrorMessageResourceType = typeof(resources))]
        [StringLength(25, ErrorMessageResourceName = "MaxLenMatricula", ErrorMessageResourceType = typeof(resources))]
        [UIHint("Matricula")]
        public string Matricula { get; set; }

        public int IdTipoAlerta { get; set; }

        public string FileDownload { get; set; }

        [ScaffoldColumn(false)]
        public bool ExisteRenovacion { get; set; }
    }
    public class RenovarITVModel : RenovarModel
    {       

        [Display(ResourceType = typeof(resources), Name = "lblFechaITV")]
        [Required(ErrorMessageResourceName = "RequiredFechaITV", ErrorMessageResourceType = typeof(resources))]
        [DataType(DataType.Date, ErrorMessageResourceName = "TypeFechaITV", ErrorMessageResourceType = typeof(resources))]
        public DateTime? FechaITV { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFechaCaducidadITV")]
        [Required(ErrorMessageResourceName = "RequiredFechaCaducidadITV", ErrorMessageResourceType = typeof(resources))]
        [DataType(DataType.Date, ErrorMessageResourceName = "TypeFechaCaducidadITV", ErrorMessageResourceType = typeof(resources))]
        [GenericCompare(CompareToPropertyName = "FechaITV", OperatorName = GenericCompareOperator.GreaterThan,
                    ErrorMessageResourceName = "FechaITVMayorFechaCaducidadITV",
                    ErrorMessageResourceType = typeof(resources))]
        public DateTime? FechaCaducidadITV { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFileReciboITV")]
        [DataType(DataType.Upload)]
        [Required(ErrorMessageResourceName = "RequiredFileReciboITV", ErrorMessageResourceType = typeof(resources))]
        public  HttpPostedFileBase FileUpload { get; set; }

        
    }

    public class RenovarCarnetModel : RenovarModel
    {       
 
        [Display(ResourceType = typeof(resources), Name = "lblFechaCaducidadCarnet")]
        [Required(ErrorMessageResourceName = "RequiredFechaCaducidadCarnet", ErrorMessageResourceType = typeof(resources))]
        [DataType(DataType.Date, ErrorMessageResourceName = "TypeFechaCaducidadCarnet", ErrorMessageResourceType = typeof(resources))]
        public DateTime? FechaCaducidadCarnet { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFileCarnet")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase FileUpload { get; set; }

        public string Conductor { get; set; }

        public int CodigoConductor { get; set; }

        public string DNI { get; set; }
    }

   

    public class RenovarRentingModel : RenovarModel
    {

        [RequiredConditional(FieldConditional ="Renovar", RequiredIsEqual = true,
            ErrorMessageResourceName = "RequiredSeleccionarConductor", ErrorMessageResourceType = typeof(resources))]
        public int? CodConductor { get; set; }      

   

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(resources), Name = "lblRenovarRenting")]
        
        public bool  Renovar { get; set; }
    }
}
