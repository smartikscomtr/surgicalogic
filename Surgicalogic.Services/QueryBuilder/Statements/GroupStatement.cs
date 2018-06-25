using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Services.QueryBuilder.Statements
{
    public class GroupStatement
    {
        public string Alias { get; }
        public string Column { get; }

        public GroupStatement(string alias, string column)
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
