using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.OutputModel
{
    public class AppointmentCalendarOutputModel
    {
        public int Id { get; set; }
        public string AppointmentDate { get; set; }
        public int PersonnelId { get; set; }
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string IdentityNumber { get; set; }
    }
}
