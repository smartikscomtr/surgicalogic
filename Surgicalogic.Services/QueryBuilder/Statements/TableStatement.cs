using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Services.QueryBuilder.Statements
{
    public class TableStatement
    {
        public string Schema { get; }
        public string Name { get; }

        public TableStatement(string schema, string name)
        {
            Schema = schema;
            Name = name;
        }

        public override string ToString()
        {
            return $"\"{Name}\"";
        }
    }
}
