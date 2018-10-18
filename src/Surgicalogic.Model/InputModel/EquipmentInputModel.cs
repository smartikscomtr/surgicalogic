using System.Collections.Generic;

namespace Surgicalogic.Model.InputModel
{
    public class EquipmentInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int EquipmentTypeId { get; set; }
        public bool IsPortable { get; set; }
        public List<int> OperatingRoomIds { get; set; }
    }
}