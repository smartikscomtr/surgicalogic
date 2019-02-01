using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class SimulationResultGroupModel
    {
        public int OperatingRoomId { get; set;}
        public List<SimulationResultModel> SimulationResultModels { get; set; }
    }
}
