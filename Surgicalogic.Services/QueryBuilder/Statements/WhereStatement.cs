using Surgicalogic.Services.QueryBuilder.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Surgicalogic.Common.Extensions;
using Surgicalogic.Services.Utils;

namespace Surgicalogic.Services.QueryBuilder.Statements
{
    public class WhereStatement
    {
        public string Alias { get; }
        public string Column { get; }
        public ComparisonOperator Operator { get; }
        public object Value { get; }
        public string ValueAlias { get; }
        public string ValueColumn { get; }

        public List<(LogicOperator Operator, WhereStatement Statement)> SubStatements { get; }

        public WhereStatement(
            string alias,
            string column,
            object value,
            ComparisonOperator @operator = ComparisonOperator.Equals)
        {
            Alias = alias;
            Column = column;
            Operator = @operator;
            Value = value;
            ValueAlias = null;
            ValueColumn = null;

            SubStatements = new List<(LogicOperator Operator, WhereStatement Statement)>();
        }

        public WhereStatement(
            string alias,
            string column,
            string valueAlias,
            string valueColumn,
            ComparisonOperator @operator = ComparisonOperator.Equals)
        {
            Alias = alias;
            Column = column;
            Operator = @operator;
            Value = null;
            ValueAlias = valueAlias;
            ValueColumn = valueColumn;

            SubStatements = new List<(LogicOperator Operator, WhereStatement Statement)>();
        }

        public WhereStatement AddStatement(
            LogicOperator logicOperator,
            string alias,
            string column,
            object value,
            ComparisonOperator comparisonOperator = ComparisonOperator.Equals)
        {
            var newStatement = new WhereStatement(alias, column, value, comparisonOperator);

            SubStatements.Add((logicOperator, newStatement));

            return newStatement;
        }

        public WhereStatement AddStatement(
            LogicOperator logicOperator,
            string alias,
            string column,
            string valueAlias,
            string valueColumn,
            ComparisonOperator comparisonOperator = ComparisonOperator.Equals)
        {
            var newStatement = new WhereStatement(alias, column, valueAlias, valueColumn, comparisonOperator);

            SubStatements.Add((logicOperator, newStatement));

            return newStatement;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (SubStatements.Count > 0)
            {
                sb.Append("(");
            }

            if (ValueColumn.IsNullOrEmpty())
            {
                if (Operator != ComparisonOperator.IsNull && Operator != ComparisonOperator.IsNotNull)
                {
                    sb.AppendFormat("{0}.\"{1}\" {2} {3}", Alias, Column, Operator.ToQueryString(), GetFormattedValue(Operator == ComparisonOperator.Like));
                }
                else
                {
                    sb.AppendFormat("{0}.\"{1}\" {2}", Alias, Column, Operator.ToQueryString());
                }
            }
            else
            {
                sb.AppendFormat("{0}.\"{1}\" {2} {3}.\"{4}\"", Alias, Column, Operator.ToQueryString(), ValueAlias, ValueColumn);
            }

            foreach (var subStatement in SubStatements)
            {
                sb.AppendFormat(" {0} {1}", subStatement.Operator.ToQueryString(), subStatement.Statement.ToString());
            }

            if (SubStatements.Count > 0)
            {
                sb.Append(")");
            }

            return sb.ToString();
        }

        private string GetFormattedValue(bool likeOperator)
        {
            var formattedValue = QueryUtility.GetFormattedValue(Value, likeOperator);

            if (!formattedValue.IsNullOrEmpty())
            {
                if (Operator == ComparisonOperator.In)
                {
                    return $"({formattedValue})";
                }

                if (Operator == ComparisonOperator.Like)
                {
                    return $"{formattedValue}";
                }

                return formattedValue;
            }

            if (Value is IEnumerable enumerable && Operator == ComparisonOperator.In)
            {
                var sb = new StringBuilder();

                var enumerator = enumerable.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    var innerValue = QueryUtility.GetFormattedValue(enumerator.Current);

                    if (!innerValue.IsNullOrEmpty())
                    {
                        sb.AppendFormat(", {0}", innerValue);
                    }
                }

                return $"({sb.ToString().Substring(1)})";
            }

            if (Operator == ComparisonOperator.In)
            {
                return "(NULL)";
            }

            return "NULL";
        }
    }
}
