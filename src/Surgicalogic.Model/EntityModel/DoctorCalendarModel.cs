using Surgicalogic.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class DoctorCalendarModel : Base.EntityModel
    {
        [Searchable]
        public DateTime StartDate { get; set; }
        [Searchable]
        public DateTime EndDate { get; set; }
        [Searchable]
        public int PersonnelId { get; set; }
        public PersonnelModel Personnel { get; set; }
    }
}
