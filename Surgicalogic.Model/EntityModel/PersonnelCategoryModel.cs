using Surgicalogic.Common.CustomAttributes;
using System.Collections.Generic;

namespace Surgicalogic.Model.EntityModel
{
    public class PersonnelCategoryModel : Base.EntityModel
    {
        [Searchable(true)]
        public string Name { get; set; }
        public bool SuitableForMultipleOperation { get; set; }
        [Searchable]
        public string Description { get; set; }
    }
}