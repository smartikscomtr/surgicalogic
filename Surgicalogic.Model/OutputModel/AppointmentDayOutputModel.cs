using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.OutputModel
{
    public class AppointmentDayOutputModel
    {
        public int Interval { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public string[] Disabled { get; set; }
        public int PersonPerPeriod { get; set; }
        public string[] SelectedTimes { get; set; }
    }
}
