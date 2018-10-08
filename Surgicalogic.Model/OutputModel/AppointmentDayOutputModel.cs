using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.OutputModel
{
    public class AppointmentDayOutputModel
    {
        public int Interval { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string[] Disabled { get; set; }
    }
}
