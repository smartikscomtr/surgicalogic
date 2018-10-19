using Surgicalogic.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Surgicalogic.Data.Entities
{
    [Table("Patients")]
    public class Patient : Entity
    {
        [StringLength(11)]
        public string IdentityNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(20)]
        public string Phone { get; set; }
        [StringLength(500)]
        public string Address { get; set; }
        public virtual ICollection<AppointmentCalendar> AppointmentCalendars { get; set; }
    }
}
