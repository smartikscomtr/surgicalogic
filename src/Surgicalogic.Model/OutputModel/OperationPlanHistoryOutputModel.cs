using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.OutputModel
{
    public class OperationPlanListOutputModel
    {
        public string OperationName { get; set; }
        public string OperatingRoomName { get; set; }
        public DateTime? OperationStartDate { get; set; }
        public DateTime? OperationEndDate { get; set; }
     }
}
