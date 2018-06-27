using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Surgicalogic.Data.Entities.Base;
using Surgicalogic.Services.QueryBuilder.Statements;
using Surgicalogic.Services.Utils;

namespace Surgicalogic.Services.QueryBuilder
{
    public class InsertQueryBuilder<TEntity>
        where TEntity : Entity
    {
        public TableStatement Table { get; set; }

        public string BuildQuery(TEntity entity)
        {
            var columnValues = QueryUtility.GetColumnValues(entity).ToArray();

            var columns = columnValues.Select(x => $"{x.Column}");
            var values = columnValues.Select(x => QueryUtility.GetFormattedValue(x.Value));

            var sb = new StringBuilder();

            sb
                .AppendFormat(
                    "INSERT INTO {0} ({1})  OUTPUT INSERTED.Id VALUES ({2})",
                    Table.ToString(),
                    string.Join(", ", columns),
                    string.Join(", ", values))
                .AppendLine();

            return sb.ToString();
        }
    }
}
