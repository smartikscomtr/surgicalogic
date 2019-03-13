using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.InputModel
{
    public class OvertimeUtilizationReportInputModel : GridInputModel
    {
        public DateTime OperationStartDate { get; set; }
        public DateTime OperationEndDate { get; set; }
        public string LangId { get; set; }

    }
}
