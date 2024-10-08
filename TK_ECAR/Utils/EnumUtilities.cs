using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Web;
using TK_ECAR.Framework;
using TK_ECAR.Models;
using resourceEnum = TK_ECAR.Content.resources.EnumResources;

namespace TK_ECAR.Utils
{
    public enum EnumDocumentoIdentificacion
    {
        [LocalizedDescription("tipoDocI_DNI", typeof(resourceEnum))]
        tipoDocI_DNI = 1,

        [LocalizedDescription("tipoDocI_NIE", typeof(resourceEnum))]
        tipoDocI_NIE = 2,

        [LocalizedDescription("tipoDocI_CIF", typeof(resourceEnum))]
        tipoDocI_CIF = 3,

        [LocalizedDescription("tipoDocI_PASAPORTE", typeof(resourceEnum))]
        tipoDocI_PASAPORTE = 4,
    }

    //Recoger la descripción de un enumerado dentro del archivo de recursos.
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string _resourceKey;
        private readonly ResourceManager _resource;
        public LocalizedDescriptionAttribute(string resourceKey, Type resourceType)
        {
            _resource = new ResourceManager(resourceType);
            _resourceKey = resourceKey;
        }

        public override string Description
        {
            get
            {
                string displayName = _resource.GetString(_resourceKey);

                return string.IsNullOrEmpty(displayName)
                    ? string.Format("[[{0}]]", _resourceKey)
                    : displayName;
            }
        }
    }

    public static class EnumUtilities
    {

        public static List<SelectChosen> GetEnumByVal<T>(int idDoc) where T : struct, IConvertible
        {
            var enumValues = GetAllEnums<T>();

            var enumValuesFiltered = new List<SelectChosen>();

            enumValuesFiltered = enumValues.Where(o => o.value == idDoc.ToString()).ToList();

            return enumValuesFiltered;
        }

        public static IEnumerable<SelectChosen> GetAllEnums<T>() where T : struct, IConvertible
        {
            var docsIdentificacion = from T enumera in Enum.GetValues(typeof(T))
                                     select new SelectChosen
                                     {
                                         PonerValuePorDelanteDeTexto = false,
                                         DevolverValueFormateado = false,
                                         text = EnumUtils<T>.GetDescription(enumera),
                                         value = Convert.ToInt32(enumera).ToString(),
                                     };


            return docsIdentificacion;
        }
    }
}