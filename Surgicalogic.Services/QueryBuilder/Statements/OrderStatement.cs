using Surgicalogic.Services.QueryBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Services.QueryBuilder.Statements
{
    public class OrderStatement
    {
        public string Alias { get; }
        public string Column { get; }
        public Sorting Direction { get; set; }

        public OrderStatement(string alias, string column, Sorting direction = Sorting.Ascending)
        {
            Alias = alias;
            Column = column;
            Direction = direction;
        }

        public override string ToString()
        {
            return $"{Alias}.\"{Column}\" {(Direction == Sorting.Descending ? "DESC" : "ASC")}";
        }
    }
}
