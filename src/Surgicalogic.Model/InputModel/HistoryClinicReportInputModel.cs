using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.InputModel
{
    public class HistoryClinicReportInputModel : GridInputModel
    {
        public string BranchId { get; set; }
        public string DoctorId { get; set; } 
        public string PatientId { get; set; } 
        public DateTime AppointmentStartDate { get; set; }
        public DateTime AppointmentEndDate { get; set; }
        public string LangId { get; set; }

    }
}
