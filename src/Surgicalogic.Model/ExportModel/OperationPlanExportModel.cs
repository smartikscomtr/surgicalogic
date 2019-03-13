using Surgicalogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Model.ExportModel
{
    public class OperationPlanExportModel
    {
        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string OperationName { get; set; }
        [Display(Name = "OperatingRoomName", ResourceType = typeof(Resource))]
        public string OperatingRoomName { get; set; }
        [Display(Name = "Date", ResourceType = typeof(Resource))]
        public string OperationDate { get; set; }
    }
}
