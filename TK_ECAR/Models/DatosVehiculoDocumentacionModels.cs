using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Filters;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{

    public class DatosVehiculoDocumentacionModel
    {
        public int idDocumento { get; set; }

        public string Matricula { get; set; }

        public int IdAlerta { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblCategoria")]
        [Required(ErrorMessageResourceName = "RequiredCategoria", ErrorMessageResourceType = typeof(resources))]
        public int idCategoria { get; set; }

        public string DescCategoria { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblDescripcion")]
        [StringLength(2500, ErrorMessageResourceName = "MaxLenDescripcion", ErrorMessageResourceType = typeof(resources))]
        public string DescDocumento { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblRuta")]
        //[Required(ErrorMessageResourceName = "RequiredFile", ErrorMessageResourceType = typeof(resources))]
        //[DocumentoVehiculoRequerido(ErrorMessageResourceName = "RequiredFile", ErrorMessageResourceType = typeof(resources))]
        public string Documento { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFile")]
        //[Required(ErrorMessageResourceName = "RequiredFile", ErrorMessageResourceType = typeof(resources))]
        //[DocumentoVehiculoRequerido(ErrorMessageResourceName = "RequiredFile", ErrorMessageResourceType = typeof(resources))]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase FileUploadDocumentacion { get; set; }

        public DateTime FechaAlta { get; set; }

        public string Accion { get; set; }
    }


}
