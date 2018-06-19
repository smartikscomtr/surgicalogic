using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class BranchTypeModel : Base.EntityModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
}