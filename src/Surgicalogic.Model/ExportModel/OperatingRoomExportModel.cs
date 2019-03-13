using Surgicalogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Model.ExportModel
{
    public class OperatingRoomExportModel
    {
        [Display(Name = "OperatingRoomName", ResourceType = typeof(Resource))]
        public string Name { get; set; }
        [Display(Name = "Location", ResourceType = typeof(Resource))]
        public string Location { get; set; }
        [Display(Name = "UnavailableDates", ResourceType = typeof(Resource))]
        public string UnavailableDates { get; set; }
        [Display(Name = "Width", ResourceType = typeof(Resource))]
        public Nullable<double> Width { get; set; }
        [Display(Name = "Height", ResourceType = typeof(Resource))]
        public Nullable<double> Height { get; set; }
        [Display(Name = "Length", ResourceType = typeof(Resource))]
        public Nullable<double> Length { get; set; }
    }
}
