using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class OperatingRoomEquipmentModel : Base.EntityModel
    {
        public int EquipmentId { get; set; }
        public EquipmentModel Equipment { get; set; }
        public int OperatingRoomId { get; set; }
        public OperatingRoomModel OperatingRoom { get; set; }
        

    }
}
