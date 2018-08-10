using Surgicalogic.Model.OutputModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class OperationTimelineOutputModel : Tuple<List<OperationPlanOutputModel>, List<OperatingRoomOutputModel>>
    {
        public OperationTimelineOutputModel(List<OperationPlanOutputModel> one, List<OperatingRoomOutputModel> two)
            : base(one, two)
        {

        }

        public List<OperationPlanOutputModel> Plan { get { return this.Item1; } }
        public List<OperatingRoomOutputModel> Rooms { get { return this.Item2; } }

    }
}
