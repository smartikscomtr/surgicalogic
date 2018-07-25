using Surgicalogic.Common.CustomAttributes;
using Surgicalogic.Data.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("EquipmentTypes")]
    public class EquipmentType : Entity
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Dependent("EquipmentTypeId")]
        public virtual ICollection<Equipment> Equipments { get; set; }
    }
}
