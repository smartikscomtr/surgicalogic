using System.Collections.Generic;

namespace Surgicalogic.Model.EntityModel
{
    public class EquipmentModel : Base.EntityModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int EquipmentTypeId { get; set; }
        public bool IsPortable { get; set; }
        public EquipmentTypeModel EquipmentType { get; set; }
        public ICollection<OperatingRoomEquipmentModel> OperatingRoomEquipmens { get; set; }
    }
}