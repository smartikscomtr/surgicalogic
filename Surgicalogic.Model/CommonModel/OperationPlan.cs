using System;

namespace Surgicalogic.Model.CommonModel
{
    public class OperationPlan
    {
        public int OperationId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
