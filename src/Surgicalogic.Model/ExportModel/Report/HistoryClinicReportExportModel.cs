using Surgicalogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Model.ExportModel
{
    public class HistoryClinicReportExportModel
    {
        [Display(Name = "Branch", ResourceType = typeof(Resource))]
        public string BranchName { get; set; }
        [Display(Name = "Doctor", ResourceType = typeof(Resource))]
        public string DoctorName { get; set; }
        [Display(Name = "PatientName", ResourceType = typeof(Resource))]
        public string PatientName { get; set; }
        [Display(Name = "Date", ResourceType = typeof(Resource))]
        public string AppointmentDate { get; set; }
    }
}
