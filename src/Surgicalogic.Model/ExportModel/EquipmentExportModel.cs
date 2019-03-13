using Surgicalogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Model.ExportModel
{
    public class EquipmentExportModel
    {
        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string Name { get; set; }
        [Display(Name = "Code", ResourceType = typeof(Resource))]
        public string Code { get; set; }
        [Display(Name = "Type", ResourceType = typeof(Resource))]
        public string EquipmentType { get; set; }
        [Display(Name = "IsPortable", ResourceType = typeof(Resource))]
        public string IsPortable { get; set; }
    }
}
