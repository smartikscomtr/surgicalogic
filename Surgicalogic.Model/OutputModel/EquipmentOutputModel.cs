using System.Collections.Generic;

namespace Surgicalogic.Model.OutputModel
{
    public class EquipmentOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EquipmentTypeId { get; set; }
        public bool IsPortable { get; set; }
        public string EquipmentTypeName { get; set; }
        public ICollection<OperatingRoomEquipmentOutputModel> OperatingRoomEquipments { get; set; }
    }
}