using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class OperationPlanModel : Base.EntityModel
    {
        public int OperatingRoomId { get; set; }
        public int OperationId { get; set; }
        public DateTime OperationDate { get; set; }

        public OperationModel Operation { get; set; }
        public OperatingRoomModel OperatingRoom { get; set; }
    }
}
