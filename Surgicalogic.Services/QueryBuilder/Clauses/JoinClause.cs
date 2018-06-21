using Surgicalogic.Services.QueryBuilder.Enums;
using Surgicalogic.Services.QueryBuilder.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surgicalogic.Services.QueryBuilder.Clauses
{
    public class JoinClause : List<JoinStatement>
    {
        public JoinStatement AddStatement(JoinType joinType, TableStatement table, string alias, WhereStatement rootStatement)
        {
            var newStatement = new JoinStatement(joinType, table, alias, rootStatement);

            Add(newStatement);

            return newStatement;
        }

        public override string ToString()
        {
            return string.Join(" ", this.Select(x => x.ToString()));
        }
    }
}
