using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class PersonnelBranchModel : Base.EntityModel
    {
        public int PersonnelId { get; set; }
        public int BranchId { get; set; }

        public BranchModel Branch { get; set; }
    }
}
