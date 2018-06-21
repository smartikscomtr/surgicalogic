using Surgicalogic.Services.QueryBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Services.QueryBuilder
{
    public static class OperatorExtensions
    {
        public static string ToQueryString(this ComparisonOperator @operator)
        {
            switch (@operator)
            {
                case ComparisonOperator.Equals:
                    return "=";

                case ComparisonOperator.NotEquals:
                    return "!=";

                case ComparisonOperator.Like:
                    return "LIKE";

                case ComparisonOperator.NotLike:
                    return "NOT LIKE";

                case ComparisonOperator.GreaterThan:
                    return ">";

                case ComparisonOperator.GreaterOrEquals:
                    return ">=";

                case ComparisonOperator.LessThan:
                    return "<";

                case ComparisonOperator.LessOrEquals:
                    return "<=";

                case ComparisonOperator.In:
                    return "IN";

                case ComparisonOperator.IsNull:
                    return "IS NULL";

                case ComparisonOperator.IsNotNull:
                    return "IS NOT NULL";

                default:
                    throw new ArgumentOutOfRangeException(nameof(@operator), @operator, $"Unknown {nameof(ComparisonOperator)} value.");
            }
        }

        public static string ToQueryString(this JoinType joinType)
        {
            switch (joinType)
            {
                case JoinType.InnerJoin:
                    return "INNER JOIN";

                case JoinType.OuterJoin:
                    return "OUTER JOIN";

                case JoinType.LeftJoin:
                    return "LEFT JOIN";

                case JoinType.RightJoin:
                    return "RIGHT JOIN";

                default:
                    throw new ArgumentOutOfRangeException(nameof(joinType), joinType, $"Unknown {nameof(JoinType)} value.");
            }
        }

        public static string ToQueryString(this LogicOperator logicOperator)
        {
            switch (logicOperator)
            {
                case LogicOperator.And:
                    return "AND";

                case LogicOperator.Or:
                    return "OR";

                default:
                    throw new ArgumentOutOfRangeException(nameof(logicOperator), logicOperator, $"Unknown {nameof(LogicOperator)} value.");
            }
        }
    }
}
