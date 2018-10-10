using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.OutputModel
{
    public class PatientOutputModel
    {
        public int Id { get; set; }
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public List<int> AppointmentCalendars { get; set; }
    }
}
