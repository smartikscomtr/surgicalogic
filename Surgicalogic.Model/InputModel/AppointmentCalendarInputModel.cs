using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.InputModel
{
    public class AppointmentCalendarInputModel
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int PersonnelId { get; set; }
        public int PatientId { get; set; }
    }
}
