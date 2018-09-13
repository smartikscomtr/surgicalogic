using Surgicalogic.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class AppointmentCalendarModel : Base.EntityModel
    {
        [Searchable]
        public DateTime AppointmentDate { get; set; }
        [Searchable]
        public int PersonnelId { get; set; }
        [Searchable]
        public int PatientId { get; set; }
        public PersonnelModel Personnel { get; set; }
        public PatientModel Patient { get; set; }
    }
}
