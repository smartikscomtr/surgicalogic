using Surgicalogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Model.ExportModel
{
    public class AppointmentCalendarExportModel
    {
        [Display(Name = "AppointmentDate", ResourceType = typeof(Resource))]
        public string AppointmentDate { get; set; }
        [Display(Name = "PersonnelName", ResourceType = typeof(Resource))]
        public string PersonnelName { get; set; }
        [Display(Name = "PatientName", ResourceType = typeof(Resource))]
        public string PatientName { get; set; }
    }
}
