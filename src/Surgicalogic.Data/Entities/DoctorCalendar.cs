using Surgicalogic.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Surgicalogic.Data.Entities
{
    [Table("DoctorCalendars")]
    public class DoctorCalendar : Entity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PersonnelId { get; set; }
        public virtual Personnel Personnel { get; set; }
    }
}
