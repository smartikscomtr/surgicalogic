using System;

namespace SurgicaLogic.ORTools.Model
{
    public class OperationPlan
    {
        public int OperationId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
