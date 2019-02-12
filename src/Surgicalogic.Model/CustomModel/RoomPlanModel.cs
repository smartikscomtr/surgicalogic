using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class RoomPlanModel
    {
        public int OperatingRoomId { get; set; }
        public string OperatingRoomName { get; set; }
        public List<SimulationOperationPlanModel> SimulationOperationPlanModels { get; set; }
    }
}
