using Surgicalogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Model.ExportModel
{
    public class PersonnelTitleExportModel
    {
        [Display(Name = "Title", ResourceType = typeof(Resource))]
        public string Name { get; set; }
    }
}
 