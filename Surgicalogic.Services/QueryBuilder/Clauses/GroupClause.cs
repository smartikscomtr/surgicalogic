using Surgicalogic.Services.QueryBuilder.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surgicalogic.Services.QueryBuilder.Clauses
{
    public class GroupClause : List<GroupStatement>
    {
        public override string ToString()
        {
            var sb = new StringBuilder();

            if (this.Any())
            {
                sb.Append("GROUP BY ");

                sb.Append(string.Join(", ", this.Select(x => x.ToString())));
            }

            return sb.ToString();
        }
    }
}
