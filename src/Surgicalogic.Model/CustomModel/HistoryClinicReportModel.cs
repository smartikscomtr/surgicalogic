using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class HistoryClinicReportModel
    {
        public int BranchId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
