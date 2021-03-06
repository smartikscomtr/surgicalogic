﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class OvertimeUtilizationForOvertimeReportOutputModel
    {
        public int OperatingRoomId { get; set; }
        public string Overtime { get; set; }
        public string OperatingRoom { get; set; }
        public double Utilization { get; set; }
    }
}
