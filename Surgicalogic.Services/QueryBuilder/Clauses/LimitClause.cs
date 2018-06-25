using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Services.QueryBuilder.Clauses
{
    public class LimitClause
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
