using Surgicalogic.Model.OutputModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class OperationTimelineOutputModel : Tuple<List<OperationPlanOutputModel>, List<OperatingRoomForTimelineModel>>
    {
        public OperationTimelineOutputModel(List<OperationPlanOutputModel> one, List<OperatingRoomForTimelineModel> two)
            : base(one, two)
        {

        }

        public List<OperationPlanOutputModel> Plan { get { return this.Item1; } }
        public List<OperatingRoomForTimelineModel> Rooms { get { return this.Item2; } }
        public string MinDate { get; set; }
        public string MaxDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Period { get; set; }
        public double Overtime { get; set; }
        public DateTime WorkingHourStart { get; set; }
        public DateTime WorkingHourEnd { get; set; }
    }
}
