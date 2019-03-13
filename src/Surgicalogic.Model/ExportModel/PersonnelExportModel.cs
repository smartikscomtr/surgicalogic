using Surgicalogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Model.ExportModel
{
    public class PersonnelExportModel
    {
        [Display(Name = "Code", ResourceType = typeof(Resource))]
        public string PersonnelCode { get; set; }
        [Display(Name = "Title", ResourceType = typeof(Resource))]
        public string Title { get; set; }
        [Display(Name = "FirstName", ResourceType = typeof(Resource))]
        public string FirstName { get; set; }
        [Display(Name = "LastName", ResourceType = typeof(Resource))]
        public string LastName { get; set; }
        [Display(Name = "Category", ResourceType = typeof(Resource))]
        public string Category { get; set; }
        [Display(Name = "Branch", ResourceType = typeof(Resource))]
        public string Branch { get; set; }
        [Display(Name = "WorkType", ResourceType = typeof(Resource))]
        public string WorkType { get; set; }
    }
}
