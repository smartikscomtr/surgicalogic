using Surgicalogic.Services.QueryBuilder.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Surgicalogic.Common.Extensions;

namespace Surgicalogic.Services.QueryBuilder.Clauses
{
    public class SelectClause : List<SelectStatement>
    {
        public bool IsDistinct { get; set; }

        public string MainAlias { get; set; }

        public SelectClause(string mainAlias = null)
        {
            MainAlias = mainAlias;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("SELECT ");

            if (IsDistinct)
            {
                sb.Append("DISTINCT ");
            }

            sb.Append(!this.Any() ? (MainAlias.IsNullOrEmpty() ? "*" : $"{MainAlias}.*") : string.Join(", ", this.Select(x => x.ToString())));

            return sb.ToString();
        }
    }
}
