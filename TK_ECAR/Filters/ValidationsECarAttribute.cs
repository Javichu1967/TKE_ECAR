using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Web.Mvc;
using TK_ECAR.Framework;

namespace TK_ECAR.Filters
{
    public class ForceFileCarnetAttribute : ValidationAttribute, IClientValidatable
    {

        public string FieldDni { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo propertyDNI = validationContext.ObjectType.GetProperty(FieldDni);

            if (value == null && propertyDNI != null)
            {
                var dniPropertyValue = propertyDNI.GetValue(validationContext.ObjectInstance, null);

                if (dniPropertyValue != null && !string.IsNullOrEmpty(dniPropertyValue.ToString()) && 
                    dniPropertyValue.ToString().ToUpper().StartsWith("X"))
                {
                    return new ValidationResult(ErrorMessageString);
                }
            }

            return ValidationResult.Success;

           
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("fieldni", FieldDni);
            rule.ValidationType = "forcefilecarnet";
            yield return rule;
        }
    }

    public class RequiredConditionalAttribute : ValidationAttribute, IClientValidatable
    {

        public object RequiredIsEqual { get; set; }
        public string FieldConditional { get; set; }

         

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo propertyConditional = validationContext.ObjectType.GetProperty(FieldConditional);

            if (value == null 
                && propertyConditional != null)
            {
                var propertyValue = propertyConditional.GetValue(validationContext.ObjectInstance, null) ?? string.Empty;
                
                if  (propertyValue.ToString().Equals(RequiredIsEqual.ToString()))
                {
                    return new ValidationResult(ErrorMessageString);
                }
            }

            return ValidationResult.Success;


        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("fieldconditional", FieldConditional);
            rule.ValidationParameters.Add("requiredisequal", RequiredIsEqual );
            rule.ValidationType = "requiredconditional";
            yield return rule;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class GenericCompareAttribute : ValidationAttribute, IClientValidatable
    {
        private GenericCompareOperator operatorname = GenericCompareOperator.GreaterThanOrEqual;

        public string CompareToPropertyName { get; set; }
        public GenericCompareOperator OperatorName { get { return operatorname; } set { operatorname = value; } }
        // public IComparable CompareDataType { get; set; }

        public GenericCompareAttribute() : base() { }
        //Override IsValid
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string operstring = (OperatorName == GenericCompareOperator.GreaterThan ?
            "greater than " : (OperatorName == GenericCompareOperator.GreaterThanOrEqual ?
            "greater than or equal to " :
            (OperatorName == GenericCompareOperator.LessThan ? "less than " :
            (OperatorName == GenericCompareOperator.LessThanOrEqual ? "less than or equal to " : ""))));
            var basePropertyInfo = validationContext.ObjectType.GetProperty(CompareToPropertyName);

            var valOther = (IComparable)basePropertyInfo.GetValue(validationContext.ObjectInstance, null);

            var valThis = (IComparable)value;

            if (valThis != null && valOther != null)
            {
                if ((operatorname == GenericCompareOperator.GreaterThan && valThis.CompareTo(valOther) <= 0) ||
                    (operatorname == GenericCompareOperator.GreaterThanOrEqual && valThis.CompareTo(valOther) < 0) ||
                    (operatorname == GenericCompareOperator.LessThan && valThis.CompareTo(valOther) >= 0) ||
                    (operatorname == GenericCompareOperator.LessThanOrEqual && valThis.CompareTo(valOther) > 0))
                    return new ValidationResult(ErrorMessage);
            }
            return null;
        }


            public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            string errorMessage = FormatErrorMessage(metadata.DisplayName);
            ModelClientValidationRule compareRule = new ModelClientValidationRule();
            compareRule.ErrorMessage = errorMessage;
            compareRule.ValidationType = "genericcompare";
            compareRule.ValidationParameters.Add("comparetopropertyname", CompareToPropertyName);
            compareRule.ValidationParameters.Add("operatorname", OperatorName.ToString());
            yield return compareRule;
        }

       
    }

    //public sealed class GenericCompare001Attribute : ValidationAttribute, IClientValidatable
    //{
    //    private GenericCompareOperator operatorname = GenericCompareOperator.GreaterThanOrEqual;

    //    public string CompareToPropertyName { get; set; }
    //    public GenericCompareOperator OperatorName { get { return operatorname; } set { operatorname = value; } }
    //    // public IComparable CompareDataType { get; set; }

    //    public GenericCompare001Attribute() : base() { }
    //    //Override IsValid
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        string operstring = (OperatorName == GenericCompareOperator.GreaterThan ?
    //        "greater than " : (OperatorName == GenericCompareOperator.GreaterThanOrEqual ?
    //        "greater than or equal to " :
    //        (OperatorName == GenericCompareOperator.LessThan ? "less than " :
    //        (OperatorName == GenericCompareOperator.LessThanOrEqual ? "less than or equal to " : ""))));
    //        var basePropertyInfo = validationContext.ObjectType.GetProperty(CompareToPropertyName);

    //        var valOther = (IComparable)basePropertyInfo.GetValue(validationContext.ObjectInstance, null);

    //        var valThis = (IComparable)value;

    //        if (valThis != null && valOther != null)
    //        {
    //            if ((operatorname == GenericCompareOperator.GreaterThan && valThis.CompareTo(valOther) <= 0) ||
    //                (operatorname == GenericCompareOperator.GreaterThanOrEqual && valThis.CompareTo(valOther) < 0) ||
    //                (operatorname == GenericCompareOperator.LessThan && valThis.CompareTo(valOther) >= 0) ||
    //                (operatorname == GenericCompareOperator.LessThanOrEqual && valThis.CompareTo(valOther) > 0))
    //                return new ValidationResult(ErrorMessage);
    //        }
    //        return null;
    //    }

    //    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    //    {
    //        string errorMessage = FormatErrorMessage(metadata.DisplayName);
    //        ModelClientValidationRule compareRule = new ModelClientValidationRule();
    //        compareRule.ErrorMessage = errorMessage;
    //        compareRule.ValidationType = "genericcompare";
    //        compareRule.ValidationParameters.Add("comparetopropertyname", CompareToPropertyName);
    //        compareRule.ValidationParameters.Add("operatorname", OperatorName.ToString());
    //        yield return compareRule;
    //    }

    //}

    #region Validación Tarjeta Combustible
    public class tarjetacombustibleasociadaAttribute : ValidationAttribute, IClientValidatable
    {

        public string fieldMatricula { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            PropertyInfo propertyMatricula = validationContext.ObjectType.GetProperty(fieldMatricula);

            //LO COMENTO, PORQUE NO VA A HACERSE VALIDACIÓN EN EL SERVIDOR. SOLO EN EL CLIENTE
            //if (value == null && propertyMatricula != null)
            //{
            //    var dniPropertyValue = propertyDNI.GetValue(validationContext.ObjectInstance, null);

            //    if (dniPropertyValue != null && !string.IsNullOrEmpty(dniPropertyValue.ToString()) &&
            //        dniPropertyValue.ToString().ToUpper().StartsWith("X"))
            //    {
            //        return new ValidationResult(ErrorMessageString);
            //    }
            //}

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
       ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("fielmatricula", fieldMatricula);
            rule.ValidationType = "tarjetacombustibleasociada";
            yield return rule;
        }
    }
    #endregion Validación Tarjeta Combustible

    #region Validación Matrículas iguales
    public class matriculaSustituidaAttribute : ValidationAttribute, IClientValidatable
    {
        public string fieldMatricula { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo propertyMatricula = validationContext.ObjectType.GetProperty(fieldMatricula);

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("fielmatricula", fieldMatricula);
            rule.ValidationType = "matriculasustituida";
            yield return rule;
        }
    }
    #endregion Validación Matrículas iguales

    #region Validación Matrícula existente
    public class matriculaExistenteAttribute : ValidationAttribute, IClientValidatable
    {
        public string fieldAccion { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo propertyAccion = validationContext.ObjectType.GetProperty(fieldAccion);

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
       ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("fielaccion", fieldAccion);
            rule.ValidationType = "matriculaexistente";
            yield return rule;
        }
    }
    #endregion Validación Matrícula existente

    #region Validación Matrícula sustitución vacía
    public class matriculaSustitucionVaciaAttribute : ValidationAttribute, IClientValidatable
    {
        public string fieldEsSustitucion { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo propertyfieldEsSustitucion = validationContext.ObjectType.GetProperty(fieldEsSustitucion);

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
       ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("fielessustitucion", fieldEsSustitucion);
            rule.ValidationType = "matriculasustitucionvacia";
            yield return rule;
        }
    }
    #endregion Validación Matrícula sustitución vacía

    #region Validación Fecha Vto. ITV vacía.
    /// <summary>
    /// No puede dejarse la Fecha de Vto. de la ITV vacía, si ya hay una que lo está.
    /// </summary>
    //public class fechaVtoITVvaciaAttribute : ValidationAttribute, IClientValidatable
    //{
    //    public string fieldLinea { get; set; }
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        PropertyInfo propertyfieldLinea = validationContext.ObjectType.GetProperty(fieldLinea);

    //        return ValidationResult.Success;
    //    }

    //    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
    //   ModelMetadata metadata, ControllerContext context)
    //    {
    //        var rule = new ModelClientValidationRule();
    //        rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
    //        rule.ValidationParameters.Add("fiellinea", fieldLinea);
    //        rule.ValidationType = "fechavtoitvvacia";
    //        yield return rule;
    //    }
    //}
    #endregion Validación Fecha Vto. ITV vacía

    #region Validación Fechas ITV correctas.
    /// <summary>
    /// No se puede poner una fecha de última ITV y Vto que comprenda otras existentes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class fechasITVCorrectasAttribute : ValidationAttribute, IClientValidatable
    {
        public string fieldFechaVto { get; set; }
        public string fieldLinea { get; set; }

        //public fechasITVCorrectasAttribute(string fechaVto, string linea) : base()
        //{
        //    fieldFechaVto = fechaVto;
        //    fieldLinea = linea;
        //}
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo propertyfieldfieldFechVto = validationContext.ObjectType.GetProperty(fieldFechaVto);
            PropertyInfo propertyfieldLinea = validationContext.ObjectType.GetProperty(fieldLinea);

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
       ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("fielfechavto", fieldFechaVto);
            rule.ValidationParameters.Add("fiellinea", fieldLinea);
            rule.ValidationType = "fechasitvcorrectas";
            yield return rule;
        }
    }
    #endregion Validación Fechas ITV correctas

    #region Validación Fechas ITV rellenas.
    /// <summary>
    /// No se puede poner una fecha de última ITV y Vto que comprenda otras existentes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class fechasITVRellenasAttribute : ValidationAttribute, IClientValidatable
    {
        public string fieldFechaVTO { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo propertyfieldLinea = validationContext.ObjectType.GetProperty(fieldFechaVTO);

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
        ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("fielfechavto", fieldFechaVTO);
            rule.ValidationType = "fechasitvrellenas";
            yield return rule;
        }
    }
    #endregion Validación Fechas ITV rellenas

    #region Validación Fecha primera ITV.
    /// <summary>
    /// Sólo puede dejarse la Fecha de la última ITV vacía, cuando es la primera línea.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class fechaUltimaITVNoObligatoriaEnPrimeraITVAttribute : ValidationAttribute, IClientValidatable
    {
        public string fieldLinea { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo propertyfieldLinea = validationContext.ObjectType.GetProperty(fieldLinea);

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
        ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("fiellinea", fieldLinea);
            rule.ValidationType = "fechaultimaitvnoobligatoriaenprimeraitv";
            yield return rule;
        }
    }
    #endregion Validación Fecha primera ITV

    #region Validación Identificador Importación.
    /// <summary>
    /// No se puede poner un identificador de importación de vehículos de flota que ya se haya usado.
    /// </summary>
    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IdentificadorImportacionUnicoAttribute : ValidationAttribute, IClientValidatable
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
        ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationType = "identificadorimportacionunico";
            yield return rule;
        }
    }
    #endregion Validación Identificador Importación


    #region Validación Documento Vehículo.
    /// <summary>
    /// No se puede poner un identificador de importación de vehículos de flota que ya se haya usado.
    /// </summary>
    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DocumentoVehiculoRequeridoAttribute : ValidationAttribute, IClientValidatable
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
        ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationType = "documentovehiculorequerido";
            yield return rule;
        }
    }
    #endregion Validación Documento Vehículo

    #region Validación Documento Vehículo.
    /// <summary>
    /// No se puede poner un identificador de importación de vehículos de flota que ya se haya usado.
    /// </summary>
    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NoIsEmptyAttribute : ValidationAttribute, IClientValidatable
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
        ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationType = "noisempty";
            yield return rule;
        }
    }
    #endregion Validación Documento Vehículo

    #region Validación PIN Tarjeta Combustible Obligatorio para Portugal.
    /// <summary>
    /// Sólo puede dejarse la Fecha de la última ITV vacía, cuando es la primera línea.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TarjetaCombustiblePinObligatorioAttribute : ValidationAttribute, IClientValidatable
    {
        public string fieldEmpresa { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo propertyfieldLinea = validationContext.ObjectType.GetProperty(fieldEmpresa);

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
        ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("fielempresa", fieldEmpresa);
            rule.ValidationType = "tarjetacombustiblepinobligatorio";
            yield return rule;
        }
    }
    #endregion Validación PIN Tarjeta Combustible Obligatorio para Portugal

    #region Validación ITV pasada.
    /// <summary>
    /// No se puede poner un identificador de importación de vehículos de flota que ya se haya usado.
    /// </summary>
    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ValorITVyaPasadaAttribute : ValidationAttribute, IClientValidatable
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
        ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationType = "valoritvyapasada";
            yield return rule;
        }
    }
    #endregion Validación ITV pasada.

    #region Validación Extensión documento Excel.
    /// <summary>
    /// No se puede importar archivos ".xls".
    /// </summary>
    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ExcelExtensionCorrectaAttribute : ValidationAttribute, IClientValidatable
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
        ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationType = "excelextensioncorrecta";
            yield return rule;
        }
    }
    #endregion Validación Extensión documento Excel.
}


