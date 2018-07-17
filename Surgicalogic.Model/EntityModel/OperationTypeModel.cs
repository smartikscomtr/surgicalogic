using System;

namespace Surgicalogic.Model.EntityModel
{
    public class OperationTypeModel : Base.EntityModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int BranchId { get; set; }
        public BranchModel Branch { get; set; }
    }
}