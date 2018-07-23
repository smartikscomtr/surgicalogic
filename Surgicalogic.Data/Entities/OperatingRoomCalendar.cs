using Surgicalogic.Data.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("OperatingRoomCalendars")]
    public class OperatingRoomCalendar : Entity
    {
        public int OperatingRoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public OperatingRoom OperatingRoom { get; set; }
    }
}
