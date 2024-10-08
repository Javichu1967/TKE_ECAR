using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Filters;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{
    public class DocumentacionModel
    {
        public int ID_Documento { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblCategoria")]
        [Required(ErrorMessageResourceName = "RequiredCategoria", ErrorMessageResourceType = typeof(resources))]
        public int ID_Categoria { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblNombre")]
        [Required(ErrorMessageResourceName = "RequiredNombre", ErrorMessageResourceType = typeof(resources))]
        [StringLength(250, ErrorMessageResourceName = "MaxLenNombre", ErrorMessageResourceType = typeof(resources))]
        public string Nombre { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblDescripcion")]
        [StringLength(2500, ErrorMessageResourceName = "MaxLenDescripcion", ErrorMessageResourceType = typeof(resources))]
        public string Descripcion { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblRuta")]
        //[Required(ErrorMessageResourceName = "RequiredFile", ErrorMessageResourceType = typeof(resources))]
        public string Documento { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFile")]
        [DataType(DataType.Upload)]
        [UIHint("UploadMofificado")]
        public HttpPostedFileBase FileUploadDocumentacion { get; set; }
        //[Required(ErrorMessageResourceName = "RequiredFile", ErrorMessageResourceType = typeof(resources))]
        [NoIsEmpty(ErrorMessageResourceName = "RequiredFile", ErrorMessageResourceType = typeof(resources))]
        public string FileUploadDocumentacion_download { get; set; }

        public string TipoArchivo { get; set; }

        public EnumAccionEntity Accion { get; set; }

    }

    public class DocumentacionDataTableModel : DocumentacionModel
    {
        public string expnad { get; set; }
        public int Ordenacion { get; set; }
        public string DescCategoria { get; set; }
        public string AccionDatatable { get; set; }

    }
}
