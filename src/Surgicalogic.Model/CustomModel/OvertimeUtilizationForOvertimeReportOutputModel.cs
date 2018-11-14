using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class OvertimeUtilizationForOvertimeReportOutputModel
    {
        public int OperatingRoomId { get; set; }
        public double Overtime { get; set; }
        public string OperatingRoom { get; set; }
        public string Utilization { get; set; }
    }
}
