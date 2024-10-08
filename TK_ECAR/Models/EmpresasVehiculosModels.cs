using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{
    public class EmpresasVehiculosModels
    {
        public int IDEmpresa { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblNombre")]
        [Required(ErrorMessageResourceName = "RequiredNombre", ErrorMessageResourceType = typeof(resources))]
        [StringLength(100, ErrorMessageResourceName = "MaxLenNombre100", ErrorMessageResourceType = typeof(resources))]
        public string Nombre { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblDireccion")]
        [StringLength(100, ErrorMessageResourceName = "MaxLenDireccion", ErrorMessageResourceType = typeof(resources))]
        public string Direccion { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblNIF")]
        [StringLength(50, ErrorMessageResourceName = "MaxLenNIF", ErrorMessageResourceType = typeof(resources))]
        public string NIF { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblPoblacion")]
        [StringLength(100, ErrorMessageResourceName = "MaxLenPoblacion100", ErrorMessageResourceType = typeof(resources))]
        public string Poblacion { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblCodigoPostal")]
        [StringLength(8, ErrorMessageResourceName = "MaxLenCodPostal", ErrorMessageResourceType = typeof(resources))]
        public string CodPostal { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblPersonaContacto")]
        [StringLength(100, ErrorMessageResourceName = "MaxLenPersonaContacto", ErrorMessageResourceType = typeof(resources))]
        public string PersonaContacto { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblEmail")]
        [StringLength(100, ErrorMessageResourceName = "MaxLenEmail", ErrorMessageResourceType = typeof(resources))]
        [EmailAddress(ErrorMessageResourceName = "TypeEmail", ErrorMessageResourceType = typeof(resources))]
        public string Email { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblTelefono_1")]
        [StringLength(15, ErrorMessageResourceName = "MaxLenTelefono_1", ErrorMessageResourceType = typeof(resources))]
        public string Telefono1 { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblTelefono_2")]
        [StringLength(15, ErrorMessageResourceName = "MaxLenTelefono_2", ErrorMessageResourceType = typeof(resources))]
        public string Telefono2 { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblRenting")]
        [Required(ErrorMessageResourceName = "RequiredRenting", ErrorMessageResourceType = typeof(resources))]
        public bool Renting { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblAseguradora")]
        [Required(ErrorMessageResourceName = "RequiredAseguradora", ErrorMessageResourceType = typeof(resources))]
        public bool Aseguradora { get; set; }
        public bool Baja { get; set; }
        public EnumAccionEntity Accion { get; set; }
    }

    public class EmpresasVehiculosDataTableModel : EmpresasVehiculosModels
    {
        public string AccionDatatable { get; set; }
    }

}
