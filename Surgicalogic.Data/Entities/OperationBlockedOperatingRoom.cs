using Surgicalogic.Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("OperationBlockedOperatingRooms")]
    public class OperationBlockedOperatingRoom : Entity 
    {        
        public int OperationId { get; set; }
        public Operation Operation { get; set; }     
        public int OperatingRoomId { get; set; }
        public OperatingRoom OperatingRoom { get; set; }
    }
}