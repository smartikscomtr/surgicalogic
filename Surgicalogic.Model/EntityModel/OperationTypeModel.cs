using System;

using Surgicalogic.Common.CustomAttributes;

namespace Surgicalogic.Model.EntityModel
{
    public class OperationTypeModel : Base.EntityModel
    {
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string Description { get; set; }
        public int BranchId { get; set; }
        [Searchable]
        public BranchModel Branch { get; set; }
    }
}