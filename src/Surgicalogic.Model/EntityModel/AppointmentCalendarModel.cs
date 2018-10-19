using Surgicalogic.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class AppointmentCalendarModel : Base.EntityModel
    {
        public DateTime AppointmentDate { get; set; }
        public int PersonnelId { get; set; }

        public int PatientId { get; set; }
        public PersonnelModel Personnel { get; set; }
        [Searchable]
        public PatientModel Patient { get; set; }
    }
}
