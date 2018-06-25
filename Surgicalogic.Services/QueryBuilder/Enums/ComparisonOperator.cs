using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Services.QueryBuilder.Enums
{
    public enum ComparisonOperator
    {
        Equals,
        NotEquals,
        Like,
        NotLike,
        GreaterThan,
        GreaterOrEquals,
        LessThan,
        LessOrEquals,
        In,
        IsNull,
        IsNotNull
    }
}
