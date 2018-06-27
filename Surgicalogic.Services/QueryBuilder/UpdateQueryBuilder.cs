using Surgicalogic.Data.Entities.Base;
using Surgicalogic.Services.QueryBuilder.Statements;
using System;
using System.Collections.Generic;
using System.Text;
using Surgicalogic.Services.Utils;
using System.Linq;

namespace Surgicalogic.Services.QueryBuilder
{
    public class UpdateQueryBuilder<TEntity>
        where TEntity : Entity
    {
        public TableStatement Table { get; set; }

        public string BuildQuery(TEntity entity)
        {
            var columnValues = QueryUtility.GetColumnValues(entity)
                .Select(x => $"{x.Column} = {QueryUtility.GetFormattedValue(x.Value)}")
                .ToArray();

            var sb = new StringBuilder();

            sb.AppendFormat("UPDATE {0} SET", Table.ToString()).AppendLine();
            sb.AppendLine(string.Join(", ", columnValues));
            sb.AppendFormat("WHERE \"Id\" = {0}", entity.Id).AppendLine();

            return sb.ToString();
        }

        public string BuildQuery(IEnumerable<(string Column, object Value)> set, IEnumerable<(string Column, object Value)> where)
        {
            var setValues =
                set
                    .Select(x => $"{x.Column} = {QueryUtility.GetFormattedValue(x.Value)}")
                    .ToArray();

            var whereValues =
                where
                    .Select(x => $"{x.Column} = {QueryUtility.GetFormattedValue(x.Value)}")
                    .ToArray();

            var sb = new StringBuilder();

            sb.AppendFormat("UPDATE {0} SET", Table).AppendLine();
            sb.AppendLine(string.Join(", ", setValues));
            sb.AppendFormat("WHERE {0}", string.Join(" OR ", whereValues)).AppendLine();

            return sb.ToString();
        }
    }
}
