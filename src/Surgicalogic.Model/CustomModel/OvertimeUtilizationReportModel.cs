using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class OvertimeUtilizationReportModel
    {
        public int OperatingRoomId { get; set; }
        public double Overtime { get; set; }
        public double Utilization { get; set; }
    }
}
