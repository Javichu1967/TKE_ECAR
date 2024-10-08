using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{

    //public class ConductoresOnlyDataTableModel //Creo esta clase, porque me está dando problemas de tamaño el datatable.
    //{
    //    public int Cod_Conductor { get; set; }
    //    public string NombreCompleto { get; set; }
    //    public string CECO { get; set; }
    //    public string AccionDatatable { get; set; }
        
    //}


    public class ConductorDataTableModel
    {
        public int Cod_Conductor { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblNombre")]
        [Required(ErrorMessageResourceName = "RequiredNombre", ErrorMessageResourceType = typeof(resources))]
        [StringLength(50, ErrorMessageResourceName = "MaxLenNombre50", ErrorMessageResourceType = typeof(resources))]
        public string Nombre { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblDni")]
        [Required(ErrorMessageResourceName = "RequiredDNI", ErrorMessageResourceType = typeof(resources))]
        [StringLength(20, ErrorMessageResourceName = "MaxLenDNI", ErrorMessageResourceType = typeof(resources))]
        public string DNI { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblCarnetConducir")]
        [Required(ErrorMessageResourceName = "RequiredCarnetConducir", ErrorMessageResourceType = typeof(resources))]
        [StringLength(20, ErrorMessageResourceName = "MaxLenCarnetConducir", ErrorMessageResourceType = typeof(resources))]
        public string NumeroCarnetConducir { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblDireccion")]
        [StringLength(100, ErrorMessageResourceName = "MaxLenDireccion", ErrorMessageResourceType = typeof(resources))]
        public string Direccion { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblPoblacion")]
        [StringLength(50, ErrorMessageResourceName = "MaxLenPoblacion", ErrorMessageResourceType = typeof(resources))]
        public string Poblacion { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblProvincia")]
        [StringLength(50, ErrorMessageResourceName = "MaxLenProvincia", ErrorMessageResourceType = typeof(resources))]
        public string Provincia { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblCodigoPostal")]
        [StringLength(8, ErrorMessageResourceName = "MaxLenCodPostal", ErrorMessageResourceType = typeof(resources))]
        public string Cod_Postal { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFechaNacimiento")]
        [DataType(DataType.Date)]
        public DateTime? Fecha_Nacimiento { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFechaCarnet")]
        [DataType(DataType.Date)]
        public DateTime? Fecha_Carnet { get; set; }
        
        [Display(ResourceType = typeof(resources), Name = "lblFechaVtoCarnet")]
        [DataType(DataType.Date)]
        public DateTime? Fecha_Vencimiento_Carnet { get; set; }

    }


    public class ConductorECARModel: ConductorDataTableModel
    {
        public int? TipoDocumentoIdentificacion { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblTipoDocIdentificacion")]
        [Required(ErrorMessageResourceName = "RequiredTipoDocIdentificacion", ErrorMessageResourceType = typeof(resources))]
        [UIHint("TipoDocIdentificacion")]
        public int? IdTipoDocumentoIdentificacion { get; set; }

        public int? IDEmpresa { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblApellidos")]
        [Required(ErrorMessageResourceName = "RequiredApellidos", ErrorMessageResourceType = typeof(resources))]
        [StringLength(50, ErrorMessageResourceName = "MaxLenApellidos50", ErrorMessageResourceType = typeof(resources))]
        public string Apellidos { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblTelefono")]
        [StringLength(15, ErrorMessageResourceName = "MaxLenTelefono", ErrorMessageResourceType = typeof(resources))]
        public string Tlfn { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblMovil")]
        [StringLength(15, ErrorMessageResourceName = "MaxLenMovil", ErrorMessageResourceType = typeof(resources))]
        public string Movil { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblNombre")]
        [UIHint("Conductor")]
        public string Conductor { get; set; }

        public int? NumEmpleado { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblCECO")]
        //[StringLength(10, ErrorMessageResourceName = "MaxLenCeco", ErrorMessageResourceType = typeof(resources))]
        [UIHint("CentroCosteConductor")]
        public string CECO { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblEmail")]
        //[RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessageResourceName = "TypeEmail", ErrorMessageResourceType = typeof(resources))]
        [EmailAddress(ErrorMessageResourceName = "TypeEmail", ErrorMessageResourceType = typeof(resources))]
        public string Email { get; set; }
        public DateTime? Fecha_Alta { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblPersonalInterno")]
        [Required(ErrorMessageResourceName = "RequiredPersonalInterno", ErrorMessageResourceType = typeof(resources))]
        public bool PersonalInterno { get; set; }

        public string DescEmpresa { get; set; }

        public string DescCECO { get; set; }

        public string NombreCompleto
        {
            get { return $"{Nombre} {Apellidos}"; }
        }

        public string CECOFormated
        {
            get
            {
                if (CECO != null)
                {
                    return CECO.TrimStart('0');
                }
                else
                {
                    return "";
                }
            }
        }

        //En la importación de vehículos si el conductor no tiene nº empleado en la Hoja Excel,
        //  se marca este campo.
        private bool? _pendienteDefinir = false;
        [Display(ResourceType = typeof(resources), Name = "lblPendienteDefinir")]
        public bool? PendienteDefinir {
            get
            {
               return _pendienteDefinir;
            }
            set
            {
                _pendienteDefinir = (value == null ? false : value);
            }
        }

        public EnumAccionEntity Accion { get; set; }

    }





}
