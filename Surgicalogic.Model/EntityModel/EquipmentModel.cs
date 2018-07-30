using Surgicalogic.Common.CustomAttributes;
using System.Collections.Generic;

namespace Surgicalogic.Model.EntityModel
{
    public class EquipmentModel : Base.EntityModel
    {
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string Code { get; set; }
        [Searchable]
        public string Description { get; set; }
        public int EquipmentTypeId { get; set; }
        public bool IsPortable { get; set; }
        [Searchable]
        public EquipmentTypeModel EquipmentType { get; set; }
        public ICollection<OperatingRoomEquipmentModel> OperatingRoomEquipmens { get; set; }
    }
}