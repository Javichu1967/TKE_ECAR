using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{
    public class CategoriasPreguntasModel
    {
        public int ID_Categoria { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblEmpresa")]
        [Required(ErrorMessageResourceName = "RequiredEmpresa", ErrorMessageResourceType = typeof(resources))]
        public int ID_Empresa { get; set; }

        public string DescEmpresa { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblNombre")]
        [Required(ErrorMessageResourceName = "RequiredNombre", ErrorMessageResourceType = typeof(resources))]
        [StringLength(50, ErrorMessageResourceName = "MaxLenNombre50", ErrorMessageResourceType = typeof(resources))]
        public string Nombre { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblOrden")]
        [Required(ErrorMessageResourceName = "RequiredOrdenacion", ErrorMessageResourceType = typeof(resources))]
        public int Ordenacion { get; set; }

        public EnumAccionEntity Accion { get; set; }
    }

    public class CategoriasPreguntasDataTableModel : CategoriasPreguntasModel
    {
        public string AccionDatatable { get; set; }
    }

}
