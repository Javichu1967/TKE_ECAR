using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{

    public class GestoresFlotaModel
    {
        [Display(ResourceType = typeof(resources), Name = "lblNumEmpleado")]
        public int NumeroEmpleado { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblPuesto")]
        [Required(ErrorMessageResourceName = "RequiredPuesto", ErrorMessageResourceType = typeof(resources))]
        [StringLength(100, ErrorMessageResourceName = "MaxLenPuesto", ErrorMessageResourceType = typeof(resources))]
        public string Puesto { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblTelefono1")]
        [Required(ErrorMessageResourceName = "RequiredTelefono1", ErrorMessageResourceType = typeof(resources))]
        [StringLength(50, ErrorMessageResourceName = "MaxLenTelefono1", ErrorMessageResourceType = typeof(resources))]
        public string Telefono1 { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblTelefono2")]
        [StringLength(50, ErrorMessageResourceName = "MaxLenTelefono2", ErrorMessageResourceType = typeof(resources))]
        public string Telefono2 { get; set; }

        public string Foto { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFoto")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase FicheroFoto { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblNombre")]
        [Required(ErrorMessageResourceName = "RequiredNombre", ErrorMessageResourceType = typeof(resources))]
        [UIHint("Empleado")]
        public string Nombre { get; set; }

        public EnumAccionEntity Accion { get; set; }

        public DateTime FechaAlta { get; set; }
    }
}

