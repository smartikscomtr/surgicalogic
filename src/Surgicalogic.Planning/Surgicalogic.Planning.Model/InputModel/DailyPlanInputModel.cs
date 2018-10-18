using System.Collections.Generic;

namespace Surgicalogic.Planning.Model.InputModel
{
    public class DailyPlanInputModel
    {
        public SettingsInputModel Settings { get; set; }
        public List<RoomInputModel> Rooms { get; set; }
        public List<OperationInputModel> Operations { get; set; }
    }
}