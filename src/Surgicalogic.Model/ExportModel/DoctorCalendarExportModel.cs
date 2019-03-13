using Surgicalogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Model.ExportModel
{
    public class DoctorCalendarExportModel
    {
        [Display(Name = "StartDate", ResourceType = typeof(Resource))]
        public string StartDate { get; set; }
        [Display(Name = "EndDate", ResourceType = typeof(Resource))]
        public string EndDate { get; set; }
        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string PersonnelName { get; set; }
    }
}
