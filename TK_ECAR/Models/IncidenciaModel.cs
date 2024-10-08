using System.ComponentModel.DataAnnotations;
using System.Web;
using resources = TK_ECAR.Content.resources.TK_ECAR_Resource;

namespace TK_ECAR.Models
{
    public class IncidenciaModel
    {
        public string LoginUsuario { get; set; }

        public string NombreUsuario { get; set; }

        public string CorreoUsuario { get; set; }

        public string CodigoAplicacion { get; set; }

        public string NombreAplicacion { get; set; }

        public string DescripcionIncidencia { get; set; }

        public string CorreoCAU { get; set; }


        [Display(ResourceType = typeof(resources), Name = "lblFileToImport")]
        [DataType(DataType.Upload)]
        [UIHint("UploadMofificado")]
        public HttpPostedFileBase FileToImport { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredArchivoToImport", ErrorMessageResourceType = typeof(resources))]
        //public string FileToImport_download { get; set; }

    }
}