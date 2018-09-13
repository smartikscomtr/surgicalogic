using Surgicalogic.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Surgicalogic.Data.Entities
{
    [Table("AppointmentCalendars")]
    public class AppointmentCalendar : Entity
    {
        public DateTime AppointmentDate { get; set; }
        public int PersonnelId { get; set; }
        public int PatientId { get; set; }
        public virtual Personnel Personnel { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
