using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.OutputModel
{
    public class AppointmentDayOutputModel
    {
        public int Interval { get; set; }
        public int MinTime { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string[] Disabled { get; set; }
    }
}
