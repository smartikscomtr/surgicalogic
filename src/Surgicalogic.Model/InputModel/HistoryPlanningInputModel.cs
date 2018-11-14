using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.InputModel
{
    public class HistoryPlanningInputModel : GridInputModel
    {
        public string OperatingRoomId { get; set; }
        public string OperationId { get; set; }
        public DateTime OperationStartDate { get; set; }
        public DateTime OperationEndDate { get; set; }
        public string IdentityNumber { get; set; }
    }
}
