using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.OutputModel
{
    public class OperationPlanOutputModel
    {
        public int OperatingRoomId { get; set; }
        public int OperationId { get; set; }
        public string OperationName { get; set; }
        public string OperatingRoomName { get; set; }
        public DateTime OperationDate { get; set; }
    }
}
