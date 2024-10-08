using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace TK_ECAR.Framework.Utils
{
    public static class ConvertExtensions
    {

        public static TR ChangeType<TR>(this object value)
        {
            return (TR)ChangeType(value, typeof(TR));
        }

        public static TR ChangeType<TR>(this object value, TR whenNull)
        {
            return (value == null || value is DBNull)
                ? whenNull
                : (TR)ChangeType(value, typeof(TR));
        }

        public static object ChangeType(this object value, Type convertToType)
        {
            if (convertToType == null)
            {
                throw new ArgumentNullException("convertToType");
            }

            // return null if the value is null or DBNull
            if (value == null || value is DBNull)
            {
                return null;
            }

            // non-nullable types, which are not supported by Convert.ChangeType(),
            // unwrap the types to determine the underlying time
            if (convertToType.IsGenericType &&
                convertToType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                convertToType = Nullable.GetUnderlyingType(convertToType);
            }

            // deal with conversion to enum types when input is a string
            if (convertToType.IsEnum && value is string)
            {
                return Enum.Parse(convertToType, value as string);
            }

            // deal with conversion to enum types when input is a integral primitive
            if (value != null && convertToType.IsEnum && value.GetType().IsPrimitive &&
                !(value is bool) && !(value is char) &&
                !(value is float) && !(value is double))
            {
                return Enum.ToObject(convertToType, value);
            }

            // use Convert.ChangeType() to do all other conversions
            return Convert.ChangeType(value, convertToType, CultureInfo.InvariantCulture);
        }

        public static string NullableToString(this object val)
        {
            return (val == null || val is DBNull)
                ? ""
                : val.ToString();
        }

        public static string NullableToFormattedString(int? val, string format)
        {
            return (val == null || val is DBNull)
                ? ""
                : ((int)val).ToString(format);
        }

        public static string NullableToFormattedString(decimal? val, string format)
        {
            return (val == null || val is DBNull)
                ? ""
                : ((decimal)val).ToString(format);
        }
    }
}
