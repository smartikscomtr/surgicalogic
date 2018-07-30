using Surgicalogic.Common.CustomAttributes;
using Surgicalogic.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("WorkTypes")]
    public class WorkType : Entity
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Dependent("WorkTypeId")]
        public virtual ICollection<Personnel> Personnels { get; set; }
    }
}
