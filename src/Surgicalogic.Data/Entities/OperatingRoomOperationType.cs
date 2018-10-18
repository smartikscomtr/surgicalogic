using Surgicalogic.Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("OperatingRoomOperationTypes")]
    public class OperatingRoomOperationType : Entity
    {
        public int OperatingRoomId { get; set; }
        public OperatingRoom OperatingRoom { get; set; }
        public int OperationTypeId { get; set; }
        public OperationType OperationType { get; set; }
    }
}
