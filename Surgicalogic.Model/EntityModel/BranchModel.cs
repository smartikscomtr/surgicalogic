using Surgicalogic.Common.CustomAttributes;

namespace Surgicalogic.Model.EntityModel
{
    public class BranchModel : Base.EntityModel
    {
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string Description { get; set; }
    }
}