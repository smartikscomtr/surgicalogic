using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.OutputModel.ReportOutputModel
{
    public class OvertimeReportOutputModel
    {
        public string BranchName { get; set; }
        public string OperationName { get; set; }
        public string DoctorName { get; set; }
        public string OperationRoomName { get; set; }
        public DateTime OperationStartDate { get; set; }
        public DateTime OperationEndDate { get; set; }
        public DateTime RealizedStartDate { get; set; }
        public DateTime RealizedEndDate { get; set; }
        public int OperationTimeDifference { get; set; }
    }
}
