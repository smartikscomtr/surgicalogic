namespace Surgicalogic.Model.CommonModel
{
    public class FilterModel<TFilter, TValue>
        where TFilter : struct
    {
        public TFilter Filter { get; set; }
        public TValue Value { get; set; }
        public FilterModel<TFilter, TCustomValue> ToValue<TCustomValue>()
        {
            return new FilterModel<TFilter, TCustomValue>
            {
                Filter = Filter,
                Value = (TCustomValue)(object)Value
            };
        }
    }

    public class FilterModel<TFilter> : FilterModel<TFilter, object>
        where TFilter : struct
    {
    }
}