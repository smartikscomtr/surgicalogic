using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Data.Entities
{
    public class OperationPlan : Base.Entity
    {
        public int OperatingRoomId { get; set; }
        public int OperationId { get; set; }
        public DateTime OperationDate { get; set; }

        public virtual Operation Operation { get; set; }
        public virtual OperatingRoom OperatingRoom { get; set; }
    }
}
