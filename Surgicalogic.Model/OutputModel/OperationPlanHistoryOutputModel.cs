using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.OutputModel
{
    public class OperationPlanHistoryOutputModel
    {
        public string OperationName { get; set; }
        public string OperatingRoomName { get; set; }
        public DateTime OperationDate { get; set; }

        public OperationForOperationPlanModel Operation { get; set; }
        public OperatingRoomForOperationPlanModel OperatingRoom { get; set; }
    }
}
