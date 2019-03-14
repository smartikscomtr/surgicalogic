using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.OutputModel
{
    public class OperatingRoomCalendarOutputModel
    {
        public int Id { get; set; }
        public int OperatingRoomId { get; set; }
        public string OperatingRoomName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

    }
}
