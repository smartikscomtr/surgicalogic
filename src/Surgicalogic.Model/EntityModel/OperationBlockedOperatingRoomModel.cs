using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class OperationBlockedOperatingRoomModel : Base.EntityModel
    {
        public int OperationId { get; set; }
        public int OperatingRoomId { get; set; }

        public OperatingRoomModel OperatingRoom { get; set; }
    }
}
