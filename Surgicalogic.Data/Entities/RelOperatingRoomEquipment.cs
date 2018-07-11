using Surgicalogic.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Data.Entities
{
    public class RelOperatingRoomEquipment : Entity 
    {
        public int OperatingRoomId { get; set; }
        public int EquipmentId { get; set; }
    }
}
