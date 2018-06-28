using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Planning.Model.InputModel
{
    public class OperationPlanInputModel
    {
        public int OperationId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
