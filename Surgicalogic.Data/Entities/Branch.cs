using Surgicalogic.Common.CustomAttributes;
using Surgicalogic.Data.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("Branches")]
    public class Branch : Entity
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Dependent("BranchId")]
        public virtual ICollection<OperationType> OperationTypes { get; set; }
        public virtual ICollection<PersonnelBranch> PersonnelBranches { get; set; }
    }
}
