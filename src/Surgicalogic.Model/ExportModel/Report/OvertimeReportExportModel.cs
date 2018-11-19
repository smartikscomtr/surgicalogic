using Surgicalogic.Model.CustomModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.ExportModel.Report
{
    public class OvertimeReportExportModel
    {
        public string BranchName { get; set; }
        public string OperationName { get; set; }
        public string DoctorName { get; set; }
        public string OperationRoomName { get; set; }
        public string OperationStartDate { get; set; }
        public string OperationEndDate { get; set; }
        public string RealizedStartDate { get; set; }
        public string RealizedEndDate { get; set; }
        public string OperationTimeDifference { get; set; }
    }
}
