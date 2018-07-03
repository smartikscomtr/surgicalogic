using Surgicalogic.Data.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("Personnels")]
    public class Personnel : Entity
    {
        [Required]
        [StringLength(100)]
        public string PersonnelCode { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        public int PersonnelTitleId { get; set; }
        public int BranchId { get; set; }
        public int WorkTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public virtual PersonnelTitle PersonnelTitle { get; set; }
        public virtual WorkType WorkType { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
