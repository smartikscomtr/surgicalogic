using Surgicalogic.Resources;
using System.ComponentModel.DataAnnotations;

namespace Surgicalogic.Model.ExportModel
{
    public class EquipmentTypeExportModel
    {
        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string Name { get; set; }
        [Display(Name = "Description", ResourceType = typeof(Resource))]
        public string Description { get; set; }
    }
}
