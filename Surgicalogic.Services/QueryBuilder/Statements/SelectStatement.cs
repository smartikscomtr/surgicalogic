using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Services.QueryBuilder.Statements
{
    public class SelectStatement
    {
        public string Alias { get; }
        public string Column { get; }

        public SelectStatement(string alias, string column)
        {
            Alias = alias;
            Column = column;
        }

        public override string ToString()
        {
            return $"{Alias}.\"{Column}\"";
        }
    }
}
