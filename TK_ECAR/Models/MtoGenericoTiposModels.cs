using System.ComponentModel.DataAnnotations;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{
    public class MtoGenericoTiposModels
    {
        public int ID_Tipo { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblMarca")]
        [Required(ErrorMessageResourceName = "RequiredMarca", ErrorMessageResourceType = typeof(resources))]
        public int? ID_Tipo_FOREIGN { get; set; } //SI TIENE CAMPO CON CLAVE EXTERNA. COMO MODELOS, CON EL ID_MARCA.

        [Required(ErrorMessageResourceName = "RequiredMarca", ErrorMessageResourceType = typeof(resources))]
        public string Descripcion_FOREIGN { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblDescripcion")]
        [StringLength(100, ErrorMessageResourceName = "MaxLenDescripcionMtoTipo", ErrorMessageResourceType = typeof(resources))]
        [Required(ErrorMessageResourceName = "RequiredDescripcion", ErrorMessageResourceType = typeof(resources))]
        public string Descripcion { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblTieneITV")]
        [Required(ErrorMessageResourceName = "RequiredTieneITV", ErrorMessageResourceType = typeof(resources))]
        public bool TieneITV { get; set; }

        private bool _permiteBorrado = true;
        public bool PermiteBorrado
        {
            get { return _permiteBorrado; }
            set { _permiteBorrado = value; }
        }

        public bool Baja { get; set; }

        public EnumMtoTiposGenerales TipoMtoGenerico { get; set; }

        public EnumAccionEntity Accion { get; set; }
    }
}
