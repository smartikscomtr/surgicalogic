using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.OutputModel
{
    public class AppointmentCalendarOutputModel
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int PersonnelId { get; set; }
        public int PatientId { get; set; }
    }
}
