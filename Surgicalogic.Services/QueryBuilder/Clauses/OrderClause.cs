using Surgicalogic.Services.QueryBuilder.Enums;
using Surgicalogic.Services.QueryBuilder.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surgicalogic.Services.QueryBuilder.Clauses
{
    public class OrderClause : List<OrderStatement>
    {
        public OrderStatement AddStatement(string alias, string column, Sorting direction = Sorting.Ascending)
        {
            var newStatement = new OrderStatement(alias, column, direction);

            Add(newStatement);

            return newStatement;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (this.Any())
            {
                sb.Append("ORDER BY ");

                sb.Append(string.Join(", ", this.Select(x => x.ToString())));
            }

            return sb.ToString();
        }
    }
}
