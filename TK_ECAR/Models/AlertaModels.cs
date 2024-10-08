using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Filters;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{
    public class AlertaModel
    {
        [ScaffoldColumn(false)]
        public int IdAlerta { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblMatricula")]
        [Required(ErrorMessageResourceName = "RequiredMatricula", ErrorMessageResourceType = typeof(resources))]
        [StringLength(15, ErrorMessageResourceName = "MaxLenMatricula", ErrorMessageResourceType = typeof(resources))]
        [UIHint("Matricula")]
        public string Matricula { get; set; }

         
        [Display(ResourceType = typeof(resources), Name = "lblObservaciones")]
        public virtual string Observaciones { get; set; }


        [Display(ResourceType = typeof(resources), Name = "lblFile")]
        [DataType(DataType.Upload)]
        public  virtual HttpPostedFileBase FileUpload { get; set; }

        public string FileDownload { get; set; }

        protected EnumTipoAlerta _idtipoAlerta = EnumTipoAlerta.Otras;
        [ScaffoldColumn(false)]
        public int IdTipoAlerta { get {return (int)_idtipoAlerta; } }

    }
    public class DatosOtraNotificacionModel : AlertaModel
    {
        [Required(ErrorMessageResourceName = "RequiredObservacionesMotivoSolicitud", ErrorMessageResourceType = typeof(resources))]
        [Display(ResourceType = typeof(resources), Name = "lblObservaciones")]
        public override string Observaciones { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblTipoPeticion")]
        [Required(ErrorMessageResourceName = "RequiredTipoPeticion", ErrorMessageResourceType = typeof(resources))]
        public int? IdTipoClasificacion { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblObservacionesRespuesta")]
        public string ObservacionesRespuesta { get; set; }

        [DataType(DataType.Upload)]
        [Display(ResourceType = typeof(resources), Name = "lblFileRespuesta")]
        public HttpPostedFileBase FileUploadRespuesta { get; set; }

        public string FileDownloadRespuesta { get; set; }
    }

    public class DatosMultaModel : AlertaModel
    {
        
        public DatosMultaModel()
        {
            _idtipoAlerta = EnumTipoAlerta.Multa;
        }

        [Display(ResourceType = typeof(resources), Name = "lblFechaDenuncia")]
        [Required(ErrorMessageResourceName = "RequiredFechaDenuncia", ErrorMessageResourceType = typeof(resources))]
        [DataType(DataType.Date, ErrorMessageResourceName = "TypeFechaDenuncia", ErrorMessageResourceType = typeof(resources))]
        public DateTime? FechaDenuncia { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblHora")]
        [Required(ErrorMessageResourceName = "RequiredHoraDenuncia", ErrorMessageResourceType = typeof(resources))]
        [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3])$", ErrorMessageResourceName = "TypeHoras", ErrorMessageResourceType = typeof(resources))]
        public int HoraDenuncia { get; set; }
        [RegularExpression(@"^([0-5][0-9])$", ErrorMessageResourceName = "TypeHoras", ErrorMessageResourceType = typeof(resources))]
        public int MinutosDenuncia { get; set; }


        [Display(ResourceType = typeof(resources), Name = "lblImporte")]
        [DataType(DataType.Currency, ErrorMessageResourceName = "TypeImporte", ErrorMessageResourceType = typeof(resources))]
        //[RegularExpression(@"(([1-9][0-9]{0,2}(.[0-9]{3})*)|[0-9]+)?(\,[0-9]{1,2})?$", ErrorMessageResourceName = "TypeImporte", ErrorMessageResourceType = typeof(resources))]
        [RegularExpression(@"^\d+?(\,[0-9]{1,2})?$", ErrorMessageResourceName = "TypeImporte", ErrorMessageResourceType = typeof(resources))]
        [Range(0, 999999999.99, ErrorMessageResourceName = "RangeImporte", ErrorMessageResourceType = typeof(resources))]
        public decimal? Importe { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblExpediente")]
        [StringLength(250, ErrorMessageResourceName = "MaxLenExpediente", ErrorMessageResourceType = typeof(resources))]
        public string Expediente { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblLugar")]
        [StringLength(250, ErrorMessageResourceName = "MaxLenLugar", ErrorMessageResourceType = typeof(resources))]
        public string Lugar { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblInfraccion")]
        [StringLength(250, ErrorMessageResourceName = "MaxLenInfraccion", ErrorMessageResourceType = typeof(resources))]
        public string Infraccion { get; set; }
    }

    public class DatosSOLREDModel : AlertaModel
    {

         
        public DatosSOLREDModel()
        {
            _idtipoAlerta = EnumTipoAlerta.TarjetaSOLRED;
        }
        

        [Display(ResourceType = typeof(resources), Name = "lblFechaSolicitud")]
        [Required(ErrorMessageResourceName = "RequiredFechaSolicitud", ErrorMessageResourceType = typeof(resources))]
        [DataType(DataType.Date, ErrorMessageResourceName = "TypeFechaSolicitud", ErrorMessageResourceType = typeof(resources))]
        public DateTime? FechaSolicitud { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblMotivo")]
        [Required(ErrorMessageResourceName = "RequiredMotivoSolicitud", ErrorMessageResourceType = typeof(resources))]
        public int? IdMotivoSolicitud { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFile")]
        [DataType(DataType.Upload)]
        [RequiredConditional(FieldConditional = "IdMotivoSolicitud", RequiredIsEqual = (int)EnumTipoSolicitudSOLRED.Robo, ErrorMessageResourceName = "RequiredFicheroDenunciaSOLRED", ErrorMessageResourceType = typeof(resources))]
        public override HttpPostedFileBase FileUpload { get; set; }


    }

    public class DatosRoboModel : AlertaModel
    {
        public DatosRoboModel()
        {
            _idtipoAlerta = EnumTipoAlerta.Robos;
        }
        [Display(ResourceType = typeof(resources), Name = "lblFechaRobo")]
        [Required(ErrorMessageResourceName = "RequiredFechaRobo", ErrorMessageResourceType = typeof(resources))]
        [DataType(DataType.Date, ErrorMessageResourceName = "TypeFechaRobo", ErrorMessageResourceType = typeof(resources))]
        public DateTime? FechaRobo { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFile")]
        [DataType(DataType.Upload)]
        [RequiredConditional(FieldConditional = "FileUpload", RequiredIsEqual = null, ErrorMessageResourceName = "RequiredFicheroDenunciaSOLRED", ErrorMessageResourceType = typeof(resources))]
        public override HttpPostedFileBase FileUpload { get; set; }

    }
    public class DatosCambioConductorModel : AlertaModel
    {
        public DatosCambioConductorModel()
        {
            _idtipoAlerta = EnumTipoAlerta.CambioConductor;
        }
         
        [Display(ResourceType = typeof(resources), Name = "lblNombre")]
        [Required(ErrorMessageResourceName = "RequiredEmpleado", ErrorMessageResourceType = typeof(resources))]
        [UIHint("Empleado")]
        public string Empleado { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblNumEmpleado")]
        //[Required(ErrorMessageResourceName = "RequiredNumEmpleado", ErrorMessageResourceType = typeof(resources))]
        //[UIHint("NumEmpleado")]
        public int? NumEmpleado { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblDni")]
      
        public string Dni { get; set; }
         
        [Display(ResourceType = typeof(resources), Name = "lblProvincia")]
      
        public string Provincia { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblPoblacion")]
        public string Poblacion { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblDomicilio")]
        public string Domicilio { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblCodigoPostal")]
        //[RegularExpression(@"^([1-9]{2}|[0-9][1-9]|[1-9][0-9])[0-9]{3}$", ErrorMessageResourceName = "TypeCodigoPostal", ErrorMessageResourceType = typeof(resources))]
        [StringLength(8, ErrorMessageResourceName = "MaxLenCodPostal", ErrorMessageResourceType = typeof(resources))]
        public string CodigoPostal { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFechaNacimiento")]
        [DataType(DataType.Date, ErrorMessageResourceName = "TypeFechaFechaNacimiento", ErrorMessageResourceType = typeof(resources))]
        [ReadOnly(true)]
        public DateTime? FechaNacimiento { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFechaCaducidadCarnet")]
        [DataType(DataType.Date, ErrorMessageResourceName = "TypeFechaCaducidadCarnet", ErrorMessageResourceType = typeof(resources))]
        public DateTime? FechaVencimientoCarnet { get; set; }

      
        [Display(ResourceType = typeof(resources), Name = "lblFile")]
        [DataType(DataType.Upload)]
        [ForceFileCarnet(FieldDni =   "Dni", ErrorMessageResourceName = "ForceFileCarnetValidation", ErrorMessageResourceType = typeof(resources))]
        public override HttpPostedFileBase FileUpload { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblMotivoCambioConductor")]
        public string Motivo { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFileEmiteRenting")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase FileUploadEmiteRenting { get; set; }

        public string FileDownloadEmiteRenting { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblObservaciones")]
        public string ObservacionesValidarSolicitud { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFechaEfecto")]
        [Required(ErrorMessageResourceName = "RequiredFechaEfecto", ErrorMessageResourceType = typeof(resources))]
        [DataType(DataType.Date, ErrorMessageResourceName = "TypeFechaFechaEfecto", ErrorMessageResourceType = typeof(resources))]
        public DateTime? FechaEfecto { get; set; }

    }


    public class ConfirmarConductorMultaModel 
    {
        public int IdAlerta { get; set; }

        public int IdEstado { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblNombre")]
        [Required(ErrorMessageResourceName = "RequiredNombre", ErrorMessageResourceType = typeof(resources))]
        [StringLength(150, ErrorMessageResourceName = "MaxLenNombreConductor", ErrorMessageResourceType = typeof(resources))]
        public string Nombre { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblDniConductor")]
        [Required(ErrorMessageResourceName = "RequiredDNI", ErrorMessageResourceType = typeof(resources))]
        [StringLength(20, ErrorMessageResourceName = "MaxLenDNI", ErrorMessageResourceType = typeof(resources))]
        public string DNI { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblPermisoConducir")]
        [Required(ErrorMessageResourceName = "RequiredNumPermisoConducir", ErrorMessageResourceType = typeof(resources))]
        [StringLength(50, ErrorMessageResourceName = "MaxLenNumPermiso", ErrorMessageResourceType = typeof(resources))]
        public string NumPermisoConducir { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblNacionalidadPermiso")]
        [StringLength(50, ErrorMessageResourceName = "MaxLenNacionalidadPermiso", ErrorMessageResourceType = typeof(resources))]
        public string NacionalidadPermiso { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblValidezPermisoESP")]
        [UIHint("YesNo")]
        public bool ValidezPermisoESP { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblPais")]
        [StringLength(50, ErrorMessageResourceName = "MaxLenPais", ErrorMessageResourceType = typeof(resources))]
        public string Pais { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblProvincia")]
        [Required(ErrorMessageResourceName = "RequiredProvincia", ErrorMessageResourceType = typeof(resources))]
        [StringLength(50, ErrorMessageResourceName = "MaxLenProvincia", ErrorMessageResourceType = typeof(resources))]
        public string Provincia { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblPoblacion")]
        [Required(ErrorMessageResourceName = "RequiredPoblacion", ErrorMessageResourceType = typeof(resources))]
        [StringLength(50, ErrorMessageResourceName = "MaxLenPoblacion", ErrorMessageResourceType = typeof(resources))]
        public string Poblacion { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblDomicilio")]
        [Required(ErrorMessageResourceName = "RequiredDomicilio", ErrorMessageResourceType = typeof(resources))]
        [StringLength(150, ErrorMessageResourceName = "MaxLenDomicilio", ErrorMessageResourceType = typeof(resources))]
        public string Domicilio { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblCodigoPostal")]
        //[RegularExpression(@"^([1-9]{2}|[0-9][1-9]|[1-9][0-9])[0-9]{3}$", ErrorMessageResourceName = "TypeCodigoPostal", ErrorMessageResourceType = typeof(resources))]
        [StringLength(8, ErrorMessageResourceName = "MaxLenCodPostal", ErrorMessageResourceType = typeof(resources))]
        public string CodigoPostal { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblAutorizacionPermiso")]
        [UIHint("YesNo")]
        public bool AutorizacionPermiso { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFileCarnet")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase FileUploadCarnet { get; set; }

        public string  FileDownloadCarnet { get; set; }
    }


    public class AlertaEmail
    {
        public string Tipo { get; set; }
        public string Estado { get; set; }
        public string Matricula { get; set; }
        public string Accion { get; set; }
        public string Modelo { get; set; }
        public string Ceco { get; set; }
        public int Prioridad { get; set; }

        public string Usuario { get; set; }

        public string Login { get; set; }

    }

}
