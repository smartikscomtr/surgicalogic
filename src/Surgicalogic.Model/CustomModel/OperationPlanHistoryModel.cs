 using Surgicalogic.Common.CustomAttributes;
using Surgicalogic.Model.CustomModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class OperationPlanHistoryModel : EntityModel.Base.EntityModel
    {
        public int OperatingRoomId { get; set; }
        public int OperationId { get; set; }
        public DateTime OperationDate { get; set; }
        public DateTime RealizedStartDate { get; set; }
        public DateTime RealizedEndDate { get; set; }


        [Searchable]
        public OperationForOperationPlanModel Operation { get; set; }
        [Searchable]
        public OperatingRoomForOperationPlanModel OperatingRoom { get; set; }
    }
}
