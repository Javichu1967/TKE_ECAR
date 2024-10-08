using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Filters;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{

    public class DatosGeneralesComunesModel
    {
        [UIHint("EmpresaChosen")]
        public int Empresa { get; set; } //Sociedad****

        [Display(ResourceType = typeof(resources), Name = "lblEmpresa")]
        [Required(ErrorMessageResourceName = "RequiredEmpresa", ErrorMessageResourceType = typeof(resources))]
        public int? IDEmpresa { get; set; } //Sociedad****
        public int? IDEmpresaInicial { get; set; } //Sociedad****

        [Required(ErrorMessageResourceName = "RequiredMatricula", ErrorMessageResourceType = typeof(resources))]
        [StringLength(15, ErrorMessageResourceName = "MaxLenMatricula", ErrorMessageResourceType = typeof(resources))]
        [Display(ResourceType = typeof(resources), Name = "lblMatricula")]
        [matriculaExistente(fieldAccion = "Accion", ErrorMessageResourceName = "msgVehiculoExistente", ErrorMessageResourceType = typeof(resources))]
        public string Matricula { get; set; } //Matricula****

        public string MatriculaInicial { get; set; } 

        [Display(ResourceType = typeof(resources), Name = "lblTipoVehiculo")]
        [UIHint("TipoVehiculoChosen")]
        public string TipoVehiculo { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblTipoVehiculo")]
        public int? IDTipoVehiculo { get; set; } //IDTipoVehiculo****
    }

    public class DatosGeneralesModel: DatosGeneralesComunesModel
    {
        [Display(ResourceType = typeof(resources), Name = "lblMarca")]
        [StringLength(50, ErrorMessageResourceName = "MaxLenMarca", ErrorMessageResourceType = typeof(resources))]
        public string Marca { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblMarca")]
        [UIHint("MarcaVehiculoChosen")]
        public int? MarcaVehiculo { get; set; }
        public int? IDMarca { get; set; } //Marca****

        [Display(ResourceType = typeof(resources), Name = "lblModelo")]
        [StringLength(50, ErrorMessageResourceName = "MaxLenModelo", ErrorMessageResourceType = typeof(resources))]
        public string Modelo { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblModelo")]
        [UIHint("ModeloVehiculoChosen")]
        //public int? ModeloVehiculo { get; set; } //IDModelo****
        public int? IDModelo { get; set; } //IDModelo****

        [Display(ResourceType = typeof(resources), Name = "lblExtras")]
        [StringLength(250, ErrorMessageResourceName = "MaxLenExtras250", ErrorMessageResourceType = typeof(resources))]
        public string Extras { get; set; } //Extras****

        [Display(ResourceType = typeof(resources), Name = "lblNumBastidor")]
        [StringLength(50, ErrorMessageResourceName = "MaxLenNumBastidor50", ErrorMessageResourceType = typeof(resources))]
        public string Bastidor { get; set; } //Bastidor

        [Display(ResourceType = typeof(resources), Name = "lblCECO")]
        [UIHint("CentroCosteChosen")]
        public string CentroCoste { get; set; } //CC****
        [Required(ErrorMessageResourceName = "RequiredCentroCoste", ErrorMessageResourceType = typeof(resources))]
        public string IDCentroCoste { get; set; } //CC****
        public string IDCentroCosteInicial { get; set; } //CC****

        [Display(ResourceType = typeof(resources), Name = "lblEmpresaLeasing")]
        [UIHint("LeasingSeguroChosen")]
        public int? EmpresaLeasing { get; set; }//****EmpresaLeasing
        public int? IDEmpresaLeasing { get; set; }//****EmpresaLeasing

        [Display(ResourceType = typeof(resources), Name = "lblConductor")]
        [UIHint("ConductoresIdChosen")]
        public string Conductor { get; set; }//****Conductor
        public int? IDConductor { get; set; }//****Conductor
        public int? IDConductorInicial { get; set; }//****Conductor

        [Display(ResourceType = typeof(resources), Name = "lblCompañiaSeguros")]
        [UIHint("LeasingSeguroChosen")]
        public int? Cia_Seguro { get; set; }//****Cia_Seguro
        public int? IDCia_Seguro { get; set; }//****Cia_Seguro

        [Display(ResourceType = typeof(resources), Name = "lblCompañiaSeguros")]
        public string Seguro_Compañia { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblPolizaSeguro")]
        [StringLength(50, ErrorMessageResourceName = "MaxLenPolizaSeguro50", ErrorMessageResourceType = typeof(resources))]
        public string Seguro_Poliza { get; set; }//****Poliza_Seguro

        [Display(ResourceType = typeof(resources), Name = "lblImporteSeguro")]
        [RegularExpression(@"^\d+?(\,[0-9]{1,2})?$", ErrorMessageResourceName = "InvalidImporteSeguro", ErrorMessageResourceType = typeof(resources))]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public double? Seguro_Importe { get; set; }//****Importe_Seguro

        [Display(ResourceType = typeof(resources), Name = "lblFechaVtoSeguro")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? Seguro_FechaVencimiento { get; set; }//****Vto_Seguro

        [Display(ResourceType = typeof(resources), Name = "lblTipoSeguro")]
        public int? IDTipoSeguro { get; set; }//****Tipo_Seguro
        [UIHint("TipoSeguroChosen")]
        [Display(ResourceType = typeof(resources), Name = "lblTipoSeguro")]
        public string Seguro_Tipo { get; set; }//****Tipo_Seguro

        [Display(ResourceType = typeof(resources), Name = "lblObservaciones")]
        public string Observaciones { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblMatriculaSustituida")]
        [StringLength(15, ErrorMessageResourceName = "MaxLenMatriculaSustituida", ErrorMessageResourceType = typeof(resources))]
        [UIHint("MatriculaSustituidaChosen")]
        public string Veh_sustituido { get; set; }//****Veh_sustituido
        [matriculaSustituida(fieldMatricula = "Matricula", ErrorMessageResourceName = "msgMatriculaSustituida", ErrorMessageResourceType = typeof(resources))]
        [matriculaSustitucionVacia(fieldEsSustitucion = "EsVehiculoDeSustitucion", ErrorMessageResourceName = "RequiredMatriculaSustituida", ErrorMessageResourceType = typeof(resources))]
        public string MatriculaSustituida { get; set; }//****Veh_sustituido
        public bool EsVehiculoDeSustitucion { get; set; }//****Veh_sustituido

        [Display(ResourceType = typeof(resources), Name = "lblDepartamento")]
        [UIHint("DepartamentosChosen")]
        public string Departamento { get; set; }//****Departamento
        public string IDDepartamento { get; set; }//****Departamento

        [Display(ResourceType = typeof(resources), Name = "lblDelegacion")]
        public string IDDelegacion { get; set; }//****Delegacion
        [UIHint("DelegacionesChosen")]
        public string Delegacion { get; set; }//****Delegacion

        public string F_D { get; set; } //NADIE SABE QUÉ ES ESTE CAMPO. POSIBLEMENTE NO VALGA PARA NADA.

        [Display(ResourceType = typeof(resources), Name = "lblDirectivo")]
        public int? Directivo { get; set; }//****Directivo

        [Display(ResourceType = typeof(resources), Name = "lblFechaAlta")]
//        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]//
//        [DataType(DataType.Date)]
        public DateTime? FechaAltaRegistro { get; set; }//**** Falta

        [Display(ResourceType = typeof(resources), Name = "lblTipoRuta")]
        [UIHint("TipoRutaChosen")]
        public int? TipoRuta { get; set; }
        public int? IDTipoRuta { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblTarjetaCombustible")]
        [UIHint("TarjetaCombustibleChosen")]
        public int? TarjetaCombustible { get; set; }//****IDTarjetaCombustible
        [tarjetacombustibleasociada(fieldMatricula = "Matricula", ErrorMessageResourceName = "msgErrTarjetaAsociadaMatricula", ErrorMessageResourceType = typeof(resources))]
        public int? IDTarjetaCombustible { get; set; }//****IDTarjetaCombustible
        public int? IDTarjetaCombustibleInicial { get; set; }//****IDTarjetaCombustible
        public string PIN_Tarjeta { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFechaRenovacion")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]//
        [DataType(DataType.Date)]
        public DateTime? FechaRenovacion { get; set; }//**** FechaRenovacion

        [Display(ResourceType = typeof(resources), Name = "lblEquipamiento")]
        [StringLength(250, ErrorMessageResourceName = "MaxLenEquipamiento250", ErrorMessageResourceType = typeof(resources))]
        public string Equipamiento { get; set; }//****Equipamiento


        [Display(ResourceType = typeof(resources), Name = "lblCarburante")]
        [UIHint("CarburanteChosen")]
        public int? Carburante { get; set; }//****IDCarburante
        public int? IDCarburante { get; set; }//****IDCarburante

        [Display(ResourceType = typeof(resources), Name = "lblUbicacion")]
        [UIHint("UbicacionesChosen")]
        public int? Ubicacion { get; set; }//****Ubicacion
        public int? IDUbicacion { get; set; }//****Ubicacion

        [Display(ResourceType = typeof(resources), Name = "lblIdentificadorImportacion")]
        [StringLength(50, ErrorMessageResourceName = "MaxLenIdentificadorImportacion", ErrorMessageResourceType = typeof(resources))]
        public string IdentificadorImportacion { get; set; }//****LugarEntrega

        public string UsuarioImportacion { get; set; }//**** UsuarioImportacion


        public EnumAccionEntity Accion { get; set; }
    }


    //    public class MtoDatosGeneralesModel : DatosGeneralesComunesModel
    //  {
    //      [Display(ResourceType = typeof(resources), Name = "lblMarca")]
    //      public int ID_Marca { get; set; }
    //
    //      [Display(ResourceType = typeof(resources), Name = "lblMarca")]
    //      public int ID_Modelo { get; set; }
    //  }



}
