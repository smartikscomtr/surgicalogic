using Surgicalogic.Common.CustomAttributes;
using System.Collections.Generic;

namespace Surgicalogic.Model.EntityModel
{
    public class PersonnelTitleModel : Base.EntityModel
    {
        [Searchable]
        public string Name { get; set; }
        public bool SuitableForMultipleOperation { get; set; }
        [Searchable]
        public string Description { get; set; }
        public virtual ICollection<PersonnelModel> Personnels { get; set; }
    }
}