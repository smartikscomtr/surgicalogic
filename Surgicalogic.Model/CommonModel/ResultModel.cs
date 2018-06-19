using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CommonModel
{
    public class ResultModel<TModel>
    {
        public IEnumerable<TModel> Result { get; set; }
        public decimal TotalCount { get; set; }
    }
}
