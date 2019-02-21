using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class SimulationResultModel
    {
        public int OperatingRoomId { get; set; }
        public string OperatingRoomName { get; set; }
        public double Usage { get; set; }
        public int OverTime { get; set; }
        public decimal WaitingTime { get; set; } 
    }
}
