using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{

    public class PreavisosAlertasModel
    {

        public int idTipoAlerta { get; set; }
        public string DescTipoAlerta { get; set; }
        public int Prioridad { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblDiasPreaviso")]
        [Range(0, 365, ErrorMessageResourceName = "MaxLenDiasPreavisoTipoAlerta", ErrorMessageResourceType = typeof(resources))]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceName = "MustBeNumeric", ErrorMessageResourceType = typeof(resources))]
        [Required(ErrorMessageResourceName = "RequiredPreaviso", ErrorMessageResourceType = typeof(resources))]
        public Nullable<int> DiasPreaviso { get; set; }

        public bool Automatica { get; set; }

        public string AccionDatatable { get; set; }
    }
}

