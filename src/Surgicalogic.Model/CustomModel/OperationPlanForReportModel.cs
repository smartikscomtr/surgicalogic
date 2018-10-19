using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class OperationPlanForReportModel : EntityModel.Base.EntityModel
    {
        public DateTime OperationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RealizedStartDate { get; set; }
        public DateTime RealizedEndDate { get; set; }

        
        public OperationForReportModel Operation { get; set; }
        public OperatingRoomForOperationPlanModel OperatingRoom { get; set; }
    }
}
