using Surgicalogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Model.ExportModel.Report
{
    public class OvertimeUtilizationReportExportModel
    {
        [Display(Name = "OperatingRoomName", ResourceType = typeof(Resource))]
        public string OperatingRoom { get; set; }
        [Display(Name = "Overtime", ResourceType = typeof(Resource))]
        public string Overtime { get; set; }
        [Display(Name = "Utilization", ResourceType = typeof(Resource))]
        public string Utilization { get; set; }
    }
}
