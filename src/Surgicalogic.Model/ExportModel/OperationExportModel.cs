using Surgicalogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Model.ExportModel
{
    public class OperationExportModel
    {
        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string Name { get; set; }
        [Display(Name = "Description", ResourceType = typeof(Resource))]
        public string Description { get; set; }
        [Display(Name = "Duration", ResourceType = typeof(Resource))]
        public string OperationTime { get; set; }
    }
}
