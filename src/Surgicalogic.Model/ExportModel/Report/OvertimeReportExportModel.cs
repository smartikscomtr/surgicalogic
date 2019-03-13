using Surgicalogic.Model.CustomModel;
using Surgicalogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Model.ExportModel.Report
{
    public class OvertimeReportExportModel
    {
        [Display(Name = "Branch", ResourceType = typeof(Resource))]
        public string BranchName { get; set; }
        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string OperationName { get; set; }
        [Display(Name = "Doctor", ResourceType = typeof(Resource))]
        public string DoctorName { get; set; }
        [Display(Name = "OperatingRoomName", ResourceType = typeof(Resource))]
        public string OperationRoomName { get; set; }
        [Display(Name = "StartDate", ResourceType = typeof(Resource))]
        public string OperationStartDate { get; set; }
        [Display(Name = "EndDate", ResourceType = typeof(Resource))]
        public string OperationEndDate { get; set; }
        [Display(Name = "RealizedStartDate", ResourceType = typeof(Resource))]
        public string RealizedStartDate { get; set; }
        [Display(Name = "RealizedEndDate", ResourceType = typeof(Resource))]
        public string RealizedEndDate { get; set; }
        [Display(Name = "OperationTimeDifference", ResourceType = typeof(Resource))]
        public string OperationTimeDifference { get; set; }
    }
}
