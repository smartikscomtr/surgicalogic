using Surgicalogic.Data.Entities.Base;
using System.Collections.Generic;
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
        public string PictureUrl { get; set; }
        public int? PersonnelTitleId { get; set; }
        public int? PersonnelCategoryId { get; set; }
        public int WorkTypeId { get; set; }

        public virtual PersonnelTitle PersonnelTitle { get; set; }
        public virtual PersonnelCategory PersonnelCategory { get; set; }
        public virtual WorkType WorkType { get; set; }        
        public virtual ICollection<OperationPersonnel> OperationPersonels { get; set; }
        public virtual ICollection<PersonnelBranch> PersonnelBranches { get; set; }
        public virtual ICollection<DoctorCalendar> DoctorCalendars { get; set; }
        public virtual ICollection<AppointmentCalendar> AppointmentCalendars { get; set; }
    }
}
