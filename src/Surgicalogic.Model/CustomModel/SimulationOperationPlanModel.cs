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
        public int StartPeriot { get; set; }
        public int EndPeriot { get; set; }
        public int OperationPeriot { get; set; }
        public ICollection<int> OperationPersonelIds { get; set; }
        public bool InUse { get; set; } = false;
        public bool IsFinished { get; set; } = false;

    }
}
