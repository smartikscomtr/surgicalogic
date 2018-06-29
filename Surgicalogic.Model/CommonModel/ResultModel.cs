using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CommonModel
{
    public class ResultModel<TModel>
    {
        public dynamic Result { get; set; }
        public decimal TotalCount { get; set; }
        public Info Info { get; set; }
    }
}
