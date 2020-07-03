using System;
using System.Globalization;

namespace FWTL.Common.Extensions
{
    public static class StringExtensions
    {
        public static T To<T>(this string source)
        {
            return (T)Convert.ChangeType(source, typeof(T), CultureInfo.InvariantCulture);
        }

        public static T? ToEnum<T>(this string source) where T : struct
        {
            if (Enum.TryParse(source, out T value))
            {
                return value;
            }

            return null;
        }

        public static T? ToN<T>(this string source) where T : struct
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                return (T)Convert.ChangeType(source, typeof(T), CultureInfo.InvariantCulture);
            }

            return null;
        }
    }
}