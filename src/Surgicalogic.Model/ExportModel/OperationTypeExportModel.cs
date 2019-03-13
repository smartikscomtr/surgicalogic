using Surgicalogic.Common.CustomAttributes;
using Surgicalogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Model.ExportModel
{
    public class OperationTypeExportModel
    {
        [Display(Name = "OperationTypeName", ResourceType = typeof(Resource))]
        public string Name { get; set; }
        [Display(Name = "Branch", ResourceType = typeof(Resource))]
        public string Branch { get; set; }
    }
}
