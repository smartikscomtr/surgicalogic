using Surgicalogic.Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("OperationTypeEquipments")]
    public class OperationTypeEquipment : Entity
    {
        public int OperationTypeId { get; set; }
        public OperationType OperationType { get; set; }
        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }
    }
}
