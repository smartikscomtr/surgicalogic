using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace Surgicalogic.Common.Extensions
{
    public static class ReflectionExtensions
    {
        public static Type GetNullableUnderlyingType(this Type type)
        {
            if (type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return type.GetGenericArguments()[0];
            }

            return type;
        }

        public static bool IsPrimitiveLike(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            type = type.GetNullableUnderlyingType();

            return
                type.IsPrimitive
                || type.IsEnum
                || type == typeof(String)
                || type == typeof(Decimal)
                || type == typeof(DateTime)
                || type == typeof(Guid)
                || type == typeof(Enum)
                || type == typeof(TimeSpan)
                || type == typeof(DateTimeOffset);
        }

        public static string TableName(this Type type)
        {
            var attribute = type.GetCustomAttribute<TableAttribute>();

            if (attribute == null || attribute.Name.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Tablo adı bulunamadı.");
            }

            if (attribute.Schema.IsNullOrEmpty())
            {
                return attribute.Name;
            }

            return $"{attribute.Schema}.{attribute.Name}";
        }
    }
}
