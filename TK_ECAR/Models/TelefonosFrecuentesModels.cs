using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{
    public class TelefonosFrecuentesModels
    {

        [Display(ResourceType = typeof(resources), Name = "lblEmpresa")]
        [Required(ErrorMessageResourceName = "RequiredEmpresa", ErrorMessageResourceType = typeof(resources))]
        public int ID_Empresa { get; set; }

        public string DescEmpresa { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblTelefono")]
        [Required(ErrorMessageResourceName = "RequiredTelefono", ErrorMessageResourceType = typeof(resources))]
        [StringLength(20, ErrorMessageResourceName = "MaxLenTelefono", ErrorMessageResourceType = typeof(resources))]
        public string NUMERO_TELEFONO { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblDescripcion")]
        [Required(ErrorMessageResourceName = "RequiredDescripcion", ErrorMessageResourceType = typeof(resources))]
        [StringLength(100, ErrorMessageResourceName = "MaxLenDescTelefono", ErrorMessageResourceType = typeof(resources))]
        public string DESCRIPCION { get; set; }

        public string AccionDataTable { get; set; }
        public EnumAccionEntity Accion { get; set; }

    }
}
