using Surgicalogic.Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("OperatingRoomEquipments")]
    public class OperatingRoomEquipment : Entity 
    {        
        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }     
        public int OperatingRoomId { get; set; }
        public OperatingRoom OperatingRoom { get; set; }
    }
}
