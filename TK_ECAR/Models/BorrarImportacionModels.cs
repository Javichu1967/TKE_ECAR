using System.ComponentModel.DataAnnotations;
using resources = TK_ECAR.Content.resources.ModelsResources;
using TK_ECAR.Framework;

namespace TK_ECAR.Models
{
    public class BorrarImportacionModels
    {
        public EnumTipoBorradoImportacion TipoBorrado { get; set; }

        //[Display(ResourceType = typeof(resources), Name = "lblEmpresa")]
        //[UIHint("EmpresaChosen")]
        //public int Empresa { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredEmpresa", ErrorMessageResourceType = typeof(resources))]
        //public int IDEmpresa { get; set; }

        [Required(ErrorMessageResourceName = "RequiredArchivoParaBorrar", ErrorMessageResourceType = typeof(resources))]
        [Display(ResourceType = typeof(resources), Name = "lblArchivoParaBorrar")]
        [UIHint("NombreArchivoParaBorrarChosen")]
        public string NombreArchivoParaBorrar { get; set; }
    }
}
