﻿using Surgicalogic.Data.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("Equipments")]
    public class Equipment : Entity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Code { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public int EquipmentTypeId { get; set; }
        public bool IsPortable { get; set; }
        public virtual EquipmentType EquipmentType { get; set; }
        public virtual ICollection<OperatingRoomEquipment> OperatingRoomEquipments { get; set; }
        public virtual ICollection<OperationTypeEquipment> OperationTypeEquipment { get; set; }
    }
}
