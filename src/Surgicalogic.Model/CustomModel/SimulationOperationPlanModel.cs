using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class SimulationOperationPlanModel
    {
        public int OperationId { get; set; }
        public int OperatingRoomId { get; set; }
        public string OperatingRoomName { get; set; }
        public DateTime OperationDate { get; set; }        
        public int OperationTime { get; set; }
        public int ActualOperationTime { get;set;}
        public int StartPeriod { get; set; }
        public int EndPeriod { get; set; }
        public int OperationPeriod { get; set; }
        public ICollection<int> OperationPersonelIds { get; set; }
        public bool InUse { get; set; } = false;
        public bool IsFinished { get; set; } = false;
        public double? CoefficientOfVariation { get; set; }

    }
}
