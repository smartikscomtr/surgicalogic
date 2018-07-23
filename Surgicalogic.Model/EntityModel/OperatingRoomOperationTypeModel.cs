using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class OperatingRoomOperationTypeModel : Base.EntityModel
    {
        public int OperatingRoomId { get; set; }
        public int OperationTypeId { get; set; }
        public OperationTypeModel OperationType { get; set; }
    }
}
