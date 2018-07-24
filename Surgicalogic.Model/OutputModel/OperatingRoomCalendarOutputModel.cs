using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.OutputModel
{
    public class OperatingRoomCalendarOutputModel
    {
        public int Id { get; set; }
        public int OperatingRoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
