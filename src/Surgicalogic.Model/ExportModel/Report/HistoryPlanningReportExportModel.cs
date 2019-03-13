using Surgicalogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Model.ExportModel.Report
{
    public class HistoryPlanningReportExportModel
    {
        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string OperationName { get; set; }
        [Display(Name = "OperatingRoomName", ResourceType = typeof(Resource))]
        public string OperationRoomName { get; set; }
        [Display(Name = "OperationDate", ResourceType = typeof(Resource))]
        public string OperationDate { get; set; }
        [Display(Name = "StartDate", ResourceType = typeof(Resource))]
        public string RealizedStartDate { get; set; }
        [Display(Name = "EndDate", ResourceType = typeof(Resource))]
        public string RealizedEndDate { get; set; }
    }
}
