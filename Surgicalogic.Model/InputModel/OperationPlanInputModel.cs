using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.InputModel
{
    public class OperationPlanInputModel
    {
        public int Id { get; set; }
        public int OperatingRoomId { get; set; }
        public int OperationId { get; set; }
        public DateTime OperationDate { get; set; }
    }
}
