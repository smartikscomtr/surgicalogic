using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.InputModel
{
    public class OvertimeReportInputModel : GridInputModel
    {
        public int BranchId { get; set; }
        public int DoctorId { get; set; }
        public DateTime RealizedStartDate { get; set; }
        public DateTime RealizedEndDate { get; set; }
    }
}
