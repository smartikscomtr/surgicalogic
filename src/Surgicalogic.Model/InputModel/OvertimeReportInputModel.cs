using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.InputModel
{
    public class OvertimeReportInputModel : GridInputModel
    {
        public string BranchId { get; set; }
        public string DoctorId { get; set; }
        public DateTime RealizedStartDate { get; set; }
        public DateTime RealizedEndDate { get; set; }
        public string LangId { get; set; }

    }
}
