using Surgicalogic.Common.CustomAttributes;

namespace Surgicalogic.Model.EntityModel
{
    public class EquipmentTypeModel : Base.EntityModel
    {
        [Searchable(true)]
        public string Name { get; set; }
        [Searchable]
        public string Description { get; set; }
    }
}