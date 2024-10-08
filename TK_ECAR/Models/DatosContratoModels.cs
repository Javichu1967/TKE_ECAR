using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Filters;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{

    public class DatosContratoModel
    {
        [Display(ResourceType = typeof(resources), Name = "lblMatricula")]
        public string Matricula { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblNumContrato")]
        [StringLength(25, ErrorMessageResourceName = "MaxLenNumContratoSeguro25", ErrorMessageResourceType = typeof(resources))]
        [Required(ErrorMessageResourceName = "RequiredNumContrato", ErrorMessageResourceType = typeof(resources))]
        public string NumContrato { get; set; }//****Num_Contrato

        [Display(ResourceType = typeof(resources), Name = "lblFechaAlta")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? FechaAlta { get; set; }//****Fecha_Alta

        [GenericCompare(CompareToPropertyName = "FechaBaja", OperatorName = GenericCompareOperator.LessThan,
                    ErrorMessageResourceName = "FechaRecogidaMenorFechabaja",
                    ErrorMessageResourceType = typeof(resources))]
        [Display(ResourceType = typeof(resources), Name = "lblFechaRecogida")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? FechaRecogida { get; set; }//****Fecha_Incorporacion

        bool? _baja;
        [Display(ResourceType = typeof(resources), Name = "lblDadoBaja")]
        [UIHint("YesNo")]
        public bool? Baja
        {
            get
            {
                if (_baja == null)
                {
                    _baja = false;
                }

                return _baja;
            }
            set
            {
                _baja = value;
            }
        }

        bool? _renovacion;
        [Display(ResourceType = typeof(resources), Name = "lblRenovacion")]
        [UIHint("YesNo")]
        public bool? Renovacion
        {
            get
            {
                if (_renovacion == null)
                {
                    _renovacion = false;
                }

                return _renovacion;
            }
            set
            {
                _renovacion = value;
            }
        }

        [Display(ResourceType = typeof(resources), Name = "lblCuotas")]
        //^[1-9]\d*$
        [RegularExpression("^[0-9]*$", ErrorMessageResourceName = "CuotasMustBeNumeric", ErrorMessageResourceType = typeof(resources))]
        [Range(0, 100, ErrorMessageResourceName = "MaxLenCuotas", ErrorMessageResourceType = typeof(resources))]
        public int? Cuotas { get; set; }//****Cuotas

        [Display(ResourceType = typeof(resources), Name = "lblFechaFinalizacion")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        //Calculado, fecha alta + cuotas (meses)
        public DateTime? FechaFinalizacion { get; set; }//****Es calculado

        [Display(ResourceType = typeof(resources), Name = "lblFechaDevolucion")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? FechaDevolucion { get; set; }//****Fecha_Devolucion

        [Display(ResourceType = typeof(resources), Name = "lblFechaBaja")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [GenericCompare(CompareToPropertyName = "FechaAlta", OperatorName = GenericCompareOperator.GreaterThan,
                    ErrorMessageResourceName = "FechaBajaMayorFechaAlta",
                    ErrorMessageResourceType = typeof(resources))]
        public DateTime? FechaBaja { get; set; }//****Fecha_Baja

        [Display(ResourceType = typeof(resources), Name = "lblFechaRecibido")]
        [DataType(DataType.Date)]
        [GenericCompare(CompareToPropertyName = "FechaAlta", OperatorName = GenericCompareOperator.GreaterThanOrEqual,
                    ErrorMessageResourceName = "FechaRecibidoMayorIgualFechaAlta",
                    ErrorMessageResourceType = typeof(resources))]
        public DateTime? FechaRecibido { get; set; }//****Fecha_Recibidos

        [Display(ResourceType = typeof(resources), Name = "lblFechaDevolución")]
        [DataType(DataType.Date)]
        [GenericCompare(CompareToPropertyName = "FechaRecogida", OperatorName = GenericCompareOperator.GreaterThanOrEqual,
                    ErrorMessageResourceName = "FechaDevolucionMayorIgualFechaRecogida",
                    ErrorMessageResourceType = typeof(resources))]
        public DateTime? FechaDevolución { get; set; }//****Fecha_Devolucion

        [Display(ResourceType = typeof(resources), Name = "lblFechaMatriculacion")]
        [DataType(DataType.Date)]
        [GenericCompare(CompareToPropertyName = "FechaRecogida", OperatorName = GenericCompareOperator.LessThanOrEqual,
                    ErrorMessageResourceName = "FechaMatriculacionMenorIgualFechaRecogida",
                    ErrorMessageResourceType = typeof(resources))]
        public DateTime? FechaMatriculacion { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblKmTotales")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceName = "InvalidKilometros", ErrorMessageResourceType = typeof(resources))]
        public int? KMTotales { get; set; }//***Km_Totales

        [Display(ResourceType = typeof(resources), Name = "lblTipoLiquidacion")]
        [UIHint("TipoLiquidacionChosen")]
        public string TipoLiquidacion { get; set; }
        [Display(ResourceType = typeof(resources), Name = "lblTipoLiquidacion")]
        public int? IDTipoLiquidacion { get; set; }//****Tipo_Liquidacion

        [Display(ResourceType = typeof(resources), Name = "lblExcesoAjuste")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceName = "InvalidExcesoAjuste", ErrorMessageResourceType = typeof(resources))]
        public int? ExcesoAjuste { get; set; }//****Exceso_ajuste

        [Display(ResourceType = typeof(resources), Name = "lblCoefExceso")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceName = "InvalidCoeficienteExceso", ErrorMessageResourceType = typeof(resources))]
        public int? CoefExceso { get; set; }//****Coef_exceso

        [Display(ResourceType = typeof(resources), Name = "lblKMExentos")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceName = "InvalidKMExentos", ErrorMessageResourceType = typeof(resources))]
        public int? KMExentos { get; set; }//****Km_Exentos

        [Display(ResourceType = typeof(resources), Name = "lblAbono")]
        //[DisplayFormat(DataFormatString = "{0:c}")]
        //[DataType(DataType.Currency)]
        [RegularExpression(@"^\d+?(\,[0-9]{1,2})?$", ErrorMessageResourceName = "InvalidAbono", ErrorMessageResourceType = typeof(resources))]
        public double? Abono { get; set; }//****Abono

        [Display(ResourceType = typeof(resources), Name = "lblCargo")]
        [RegularExpression(@"^\d+?(\,[0-9]{1,2})?$", ErrorMessageResourceName = "InvalidCargo", ErrorMessageResourceType = typeof(resources))]
        public double? Cargo { get; set; }//****Cargo

        [Display(ResourceType = typeof(resources), Name = "lblLugarEntrega")]
        [StringLength(250, ErrorMessageResourceName = "MaxLenLugarEntrega250", ErrorMessageResourceType = typeof(resources))]
        public string LugarEntrega { get; set; }//****LugarEntrega

        [Display(ResourceType = typeof(resources), Name = "lblPrioridadEntrega")]
        [StringLength(50, ErrorMessageResourceName = "MaxLenPrioridadEntrega", ErrorMessageResourceType = typeof(resources))]
        public string PrioridadEntrega { get; set; }//****PrioridadEntrega

        [Display(ResourceType = typeof(resources), Name = "lblResponsable")]
        [StringLength(100, ErrorMessageResourceName = "MaxLenResponsable100", ErrorMessageResourceType = typeof(resources))]
        public string Responsable { get; set; }//****Responsable

        [Display(ResourceType = typeof(resources), Name = "lblFechaImportacion")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]//
        [DataType(DataType.Date)]
        public DateTime? FechaImportacion { get; set; }//**** FechaRenovacion

        [Display(ResourceType = typeof(resources), Name = "lblFechaPrevistaEntrega")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]//
        [DataType(DataType.Date)]
        public DateTime? FechaPrevistaEntrega { get; set; }//**** FechaRenovacion

    }

}
