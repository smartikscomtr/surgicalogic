using System;
using System.Collections.Generic;
using System.Text;
using Surgicalogic.Services.QueryBuilder.Enums;

namespace Surgicalogic.Services.QueryBuilder.Statements
{
    public class JoinStatement
    {
        public JoinType JoinType { get; }
        public TableStatement Table { get; }
        public string Alias { get; }

        public WhereStatement RootStatement { get; }

        public JoinStatement(JoinType joinType, TableStatement table, string alias, WhereStatement rootStatement)
        {
            JoinType = joinType;
            Table = table;
            Alias = alias;
            RootStatement = rootStatement;
        }

        public WhereStatement AddStatement(
            LogicOperator logicOperator,
            string alias,
            string column,
            object value,
            ComparisonOperator comparisonOperator = ComparisonOperator.Equals)
        {
            var newStatement = new WhereStatement(alias, column, value, comparisonOperator);

            RootStatement.SubStatements.Add((logicOperator, newStatement));

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

            RootStatement.SubStatements.Add((logicOperator, newStatement));

            return newStatement;
        }

        public override string ToString()
        {
            return $"{JoinType.ToQueryString()} {Table} {Alias} ON {RootStatement}";
        }
    }
}
