using Surgicalogic.Services.QueryBuilder.Clauses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Services.QueryBuilder
{
    public class SelectQueryBuilder
    {
        public const string MainTableAlias = "main";

        public SelectClause SelectClause { get; } = new SelectClause(MainTableAlias);
        public FromClause FromClause { get; } = new FromClause { Alias = MainTableAlias };
        public JoinClause JoinClause { get; } = new JoinClause();
        public WhereClause WhereClause { get; } = new WhereClause();
        public GroupClause GroupClause { get; } = new GroupClause();
        public OrderClause OrderClause { get; } = new OrderClause();
        public LimitClause LimitClause { get; set; }

        public string GetAlias(string schema, string tableName)
        {
            if (FromClause.Table.Schema == schema && FromClause.Table.Name == tableName)
            {
                return FromClause.Alias;
            }

            foreach (var joinStatement in JoinClause)
            {
                if (joinStatement.Table.Schema == schema && joinStatement.Table.Name == tableName)
                {
                    return joinStatement.Alias;
                }
            }

            return null;
        }

        public string BuildQuery()
        {
            if (FromClause.Table == null)
            {
                return null;
            }

            var sb = new StringBuilder();

            sb.AppendLine(SelectClause.ToString());
            sb.AppendLine(FromClause.ToString());
            sb.AppendLine(JoinClause.ToString());
            sb.AppendLine(WhereClause.ToString());
            sb.AppendLine(GroupClause.ToString());
            sb.AppendLine(OrderClause.ToString());

            if (LimitClause != null)
            {
                var take = LimitClause.PageSize;
                var skip = LimitClause.Page - 1;

                skip = skip < 0 ? 0 : skip; // PageSize 1 iken Skip edilecek kayıt sayısı 0 olması lazım
                skip = skip * take; // Skip edilecek kayıt bir sayfadaki kayıtlar * sayfa sayısı kadar olması lazım

                return $@"
                    SELECT K.*
                    FROM (
                        SELECT T.*, ROWNUM RowNumber
                        FROM (
                            {sb}
                        ) T
                        WHERE ROWNUM <= {skip + take}
                    ) K
                    WHERE K.RowNumber > {skip}
                ";
            }

            return sb.ToString();
        }

        public string BuildCountQuery()
        {
            var sb = new StringBuilder();

            sb.AppendLine("SELECT COUNT(1) Count");
            if (FromClause != null)
            {
                sb.AppendLine(FromClause?.ToString());
            }
            sb.AppendLine(JoinClause.ToString());
            if (WhereClause != null)
            {
                sb.AppendLine(WhereClause?.ToString());
            }
            sb.AppendLine(GroupClause.ToString());

            return sb.ToString();
        }
    }
}
