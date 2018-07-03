using Surgicalogic.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("EquipmentTypes")]
    public class EquipmentType : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Equipment> Equipments { get; set; }
    }
}
