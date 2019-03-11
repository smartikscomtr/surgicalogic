using Surgicalogic.Common.CustomAttributes;
using Surgicalogic.Model.EntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class OperationTypeForOperationModel : EntityModel.Base.EntityModel
    {
        [Searchable(true)]
        public string Name { get; set; }
        public int BranchId { get; set; }

        [Searchable]
        public BranchModel Branch { get; set; }
    }
}
