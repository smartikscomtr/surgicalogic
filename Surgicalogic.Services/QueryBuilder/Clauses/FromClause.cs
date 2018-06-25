using Surgicalogic.Services.QueryBuilder.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Services.QueryBuilder.Clauses
{
    public class FromClause
    {
        public TableStatement Table { get; set; }
        public string Alias { get; set; }

        public override string ToString()
        {
            return $"FROM {Table} {Alias}";
        }
    }
}
