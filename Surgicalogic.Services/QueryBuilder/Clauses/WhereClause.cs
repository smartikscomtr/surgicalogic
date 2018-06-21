using Surgicalogic.Services.QueryBuilder.Enums;
using Surgicalogic.Services.QueryBuilder.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Services.QueryBuilder.Clauses
{
    public class WhereClause
    {
        private WhereStatement RootStatement { get; set; }

        public WhereStatement AddStatement(
            LogicOperator logicOperator,
            string alias,
            string column,
            object value,
            ComparisonOperator comparisonOperator = ComparisonOperator.Equals)
        {
            var newStatement = new WhereStatement(alias, column, value, comparisonOperator);

            if (RootStatement == null)
            {
                RootStatement = newStatement;
            }
            else
            {
                RootStatement.SubStatements.Add((logicOperator, newStatement));
            }

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

            if (RootStatement == null)
            {
                RootStatement = newStatement;
            }
            else
            {
                RootStatement.SubStatements.Add((logicOperator, newStatement));
            }

            return newStatement;
        }

        public override string ToString()
        {
            return RootStatement != null ? $"WHERE {RootStatement}" : string.Empty;
        }
    }
}
