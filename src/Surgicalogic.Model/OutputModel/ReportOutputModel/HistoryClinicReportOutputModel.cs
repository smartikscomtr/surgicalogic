using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.OutputModel.ReportOutputModel
{
    public class HistoryClinicReportOutputModel
    {
        public string BranchName { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public DateTime AppointmentDate { get; set; }

        public PersonnelForReportModel Personnel { get; set; }
        public PatientModel Patient { get; set; }

    }
}
