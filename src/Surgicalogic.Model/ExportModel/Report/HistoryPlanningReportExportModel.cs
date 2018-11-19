using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.ExportModel.Report
{
    public class HistoryPlanningReportExportModel
    {
        public string OperationName { get; set; }
        public string OperationRoomName { get; set; }
        public string OperationDate { get; set; }
        public string RealizedStartDate { get; set; }
        public string RealizedEndDate { get; set; }
    }
}
