using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Filters;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{
    public class ImportacionDatosModels
    {

        [UIHint("EmpresaChosen")]
        public int Empresa { get; set; } 
        public string DescEmpresa { get; set; } 

        [Display(ResourceType = typeof(resources), Name = "lblEmpresa")]
        [Required(ErrorMessageResourceName = "RequiredEmpresa", ErrorMessageResourceType = typeof(resources))]
        public int? IDEmpresa { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblEmpresaEmisora")]
        [UIHint("EmpresaEmisoraTarjeta")]
        public string EmpresaEmisora { get; set; }
        [Required(ErrorMessageResourceName = "RequiredEmpresaEmisora", ErrorMessageResourceType = typeof(resources))]
        public int? IDEmpresaEmisora { get; set; }

        //[Display(ResourceType = typeof(resources), Name = "lblIdentificadorImportacion")]
        //[Required(ErrorMessageResourceName = "RequiredIdentificadorImportacion", ErrorMessageResourceType = typeof(resources))]
        //[StringLength(50, ErrorMessageResourceName = "MaxLenIdentificadorImportacion", ErrorMessageResourceType = typeof(resources))]
        //[IdentificadorImportacionUnico(ErrorMessageResourceName = "IdentificadorExistente",ErrorMessageResourceType = typeof(resources))]
        //public string IdentificadorImportacion { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFileToImport")]
        [DataType(DataType.Upload)]
        [UIHint("UploadMofificado")]
        public HttpPostedFileBase FileToImport { get; set; }

        [Required(ErrorMessageResourceName = "RequiredArchivoToImport", ErrorMessageResourceType = typeof(resources))]
        [ExcelExtensionCorrecta(ErrorMessageResourceName = "ExcelExtensionNoPermitida", ErrorMessageResourceType = typeof(resources))]
        public string FileToImport_download { get; set; }

        public EnumTipoImportacion TipoDeImportacionDatos { get; set; }

    }
}
