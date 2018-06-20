using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surgicalogic.ORTools.Model
{
    public class SurgeryPlan
    {
        public int RoomCount { get; set; }
        public int RoomsPeriod { get; set; }
        public int MaximumPeriod { get; set; }
        public int StartingHour { get; set; }
        public int StartingMinute { get; set; }
        public int PeriodInMinutes { get; set; }
        public List<Operation> Operations { get; set; }
    }
}
