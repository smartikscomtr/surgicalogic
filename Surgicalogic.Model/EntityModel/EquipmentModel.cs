using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class EquipmentModel : Base.EntityModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public int EquipmentTypeId { get; set; }
        public bool IsPortable { get; set; }        
        public EquipmentTypeModel EquipmentTypeModel { get; set; }
    }
}
