using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.InputModel
{
    public class HistoryClinicReportInputModel : GridInputModel
    {
        public int BranchId { get; set; }
        public int DoctorId { get; set; } 
        public int PatientId { get; set; } 
        public DateTime AppointmentStartDate { get; set; }
        public DateTime AppointmentEndDate { get; set; }
    }
}
