using Surgicalogic.Common.CustomAttributes;
using Surgicalogic.Data.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("PersonnelTitles")]
    public class PersonnelTitle : Entity
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public bool SuitableForMultipleOperation { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Dependent("PersonnelTitleId")]
        public virtual ICollection<Personnel> Personnels { get; set; }
    }
}
