using Surgicalogic.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class PatientModel : Base.EntityModel
    {
        [Searchable]
        public string IdentityNumber { get; set; }
        [Searchable]
        public string FirstName { get; set; }
        [Searchable]
        public string LastName { get; set; }
        [Searchable]
        public string Phone { get; set; }
        [Searchable]
        public string Address { get; set; }
        public ICollection<AppointmentCalendarModel> AppointmentCalendars { get; set; }
    }
}
