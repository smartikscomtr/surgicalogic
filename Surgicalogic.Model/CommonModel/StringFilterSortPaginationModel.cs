using System.Linq;

namespace Surgicalogic.Model.CommonModel
{
    public class StringFilterSortPaginationModel<TSorting, TFilter> : FilterSortPaginationModel<TSorting, TFilter, string> where TSorting : struct where TFilter : struct
    {
        public static implicit operator FilterSortPaginationModel<TSorting, TFilter>(StringFilterSortPaginationModel<TSorting, TFilter> right)
        {
            return new FilterSortPaginationModel<TSorting, TFilter>
            {
                Page = right.Page,
                PageSize = right.PageSize,
                Search = right.Search,
                Sorting = right.Sorting,
                Filters = right.Filters?.Select(x => x.ToValue<object>()).ToArray()
            };
        }
    }
}