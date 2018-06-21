using Surgicalogic.Services.QueryBuilder;
using Surgicalogic.Services.QueryBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Surgicalogic.Common.Extensions;
using Surgicalogic.Services.QueryBuilder.Statements;
using System.Collections;
using System.Globalization;
using Surgicalogic.Data.Entities.Base;
using System.Reflection;

namespace Surgicalogic.Services.Utils
{
    public static class QueryUtility
    {
        public static void SetFilter(
            SelectQueryBuilder query,
            object value,
            (string Table, string Column) target,
            IEnumerable<(string JoinTable, string JoinColumn, string SourceTable, string SourceColumn)> joins = null,
            InputType expectedType = InputType.Integer,
            ComparisonOperator comparisonOperator = ComparisonOperator.Equals)
        {
            var schema = query.FromClause.Table.Schema;

            object newValue;

            switch (expectedType)
            {
                case InputType.Integer:
                    newValue = GetInt32Value(value);
                    break;

                case InputType.Decimal:
                    newValue = GetDecimalValue(value);
                    break;

                case InputType.String:
                    newValue = GetStringValue(value);
                    break;

                case InputType.Boolean:
                    newValue = GetBooleanValue(value);
                    break;

                case InputType.DateTime:
                    newValue = GetDateTimeValue(value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(expectedType), expectedType, null);
            }

            var isEnumerable = newValue is IEnumerable;

            if (joins != null && joins.Any())
            {
                foreach (var join in joins)
                {
                    var alias = query.GetAlias(schema, join.JoinTable);

                    if (alias.IsNullOrEmpty())
                    {
                        alias = join.JoinTable.ToLowerInvariant();

                        var sourceAlias = query.GetAlias(schema, join.SourceTable);

                        query.JoinClause.AddStatement(
                            JoinType.InnerJoin,
                            new TableStatement(schema, join.JoinTable),
                            alias,
                            new WhereStatement(alias, join.JoinColumn, sourceAlias, join.SourceColumn));
                    }
                }
            }

            var targetAlias = query.GetAlias(schema, target.Table);

            query.WhereClause.AddStatement(LogicOperator.And, targetAlias, target.Column, newValue, isEnumerable ? ComparisonOperator.In : comparisonOperator);
        }

        public static object GetInt32Value(object value)
        {
            if (value != null)
            {
                var valueInt = value as int?;
                if (valueInt.HasValue)
                {
                    return valueInt.Value;
                }

                if (int.TryParse(value.ToString(), out var valueParsedInt))
                {
                    return valueParsedInt;
                }

                if (value is IEnumerable<int> valueEnumerable)
                {
                    if (valueEnumerable.Any())
                    {
                        return valueEnumerable;
                    }
                }
                else
                {
                    var valueString = value as string;

                    if (!valueString.IsNullOrEmpty())
                    {
                        valueEnumerable = valueString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

                        if (valueEnumerable.Any())
                        {
                            return valueEnumerable;
                        }
                    }
                }
            }

            return value;
        }

        public static object GetDecimalValue(object value)
        {
            if (value != null)
            {
                var valueDecimal = value as decimal?;
                if (valueDecimal.HasValue)
                {
                    return valueDecimal;
                }

                if (decimal.TryParse(value.ToString(), out var valueParsedDecimal))
                {
                    return valueParsedDecimal;
                }

                if (value is IEnumerable<decimal> valueEnumerable)
                {
                    if (valueEnumerable.Any())
                    {
                        return valueEnumerable;
                    }
                }
                else
                {
                    var valueString = value as string;

                    if (!valueString.IsNullOrEmpty())
                    {
                        valueEnumerable = valueString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(decimal.Parse);

                        if (valueEnumerable.Any())
                        {
                            return valueEnumerable;
                        }
                    }
                }
            }

            return value;
        }

        public static object GetStringValue(object value)
        {
            if (value != null)
            {
                var valueString = value as string;
                if (!valueString.IsNullOrEmpty())
                {
                    if (valueString.Contains(","))
                    {
                        var valueEnumerable = valueString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (valueEnumerable != null && valueEnumerable.Any())
                        {
                            return valueEnumerable;
                        }
                    }
                    else
                    {
                        return valueString;
                    }
                }
                else
                {
                    if (value is IEnumerable<string> valueEnumerable)
                    {
                        if (valueEnumerable.Any())
                        {
                            return valueEnumerable;
                        }
                    }
                }
            }

            return value;
        }

        public static object GetBooleanValue(object value)
        {
            if (value != null)
            {
                var valueBool = value as bool?;
                if (valueBool.HasValue)
                {
                    return valueBool.Value;
                }
                if (bool.TryParse(value.ToString(), out var valueParsedBool))
                {
                    return valueParsedBool;
                }

                var valueInt = value as int?;
                if (valueInt.HasValue)
                {
                    return valueInt != 0;
                }

                if (int.TryParse(value.ToString(), out var valueParsedInt))
                {
                    return valueParsedInt != 0;
                }
            }

            return value;
        }

        public static object GetDateTimeValue(object value)
        {
            if (value != null)
            {
                var valueDateTime = value as DateTime?;
                if (valueDateTime.HasValue)
                {
                    return valueDateTime.Value;
                }

                if (DateTime.TryParse(value.ToString(), out var valueParsedDateTime))
                {
                    return valueParsedDateTime;
                }
            }

            return value;
        }

        public static string GetFormattedValue(object value, bool likeOperator = false)
        {
            if (value == null)
            {
                return "NULL";
            }

            switch (value.GetType().GetNullableUnderlyingType().Name)
            {
                case "String":
                    return $"'{(likeOperator ? "%" : "")}{((string)value).Replace("'", "''")}{(likeOperator ? "%" : "")}'";

                case "DateTime":
                    return $"TO_DATE('{(DateTime)value:yyyy-MM-dd hh:mm:ss}', 'yyyy-mm-dd hh24:mi:ss')";

                case "DBNull":
                    return "NULL";

                case "Boolean":
                    return (bool)value ? "1" : "0";

                case "Byte":
                case "Int16":
                case "Int32":
                case "Int64":
                case "Decimal":
                    return Convert.ToString(value, CultureInfo.InvariantCulture);
            }

            return null;
        }

        public static IEnumerable<(string Column, object Value)> GetColumnValues<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            var type = typeof(TEntity);

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (property.Name == "Id")
                {
                    continue;
                }

                if (!property.PropertyType.IsPrimitiveLike())
                {
                    continue;
                }

                var name = property.Name;
                var value = property.GetValue(entity);

                if (property.PropertyType.IsEnum)
                {
                    value = (int)value;
                }

                yield return (name, value);
            }
        }
    }

    public enum InputType
    {
        Integer,
        String,
        Boolean,
        DateTime,
        Decimal
    }

}
