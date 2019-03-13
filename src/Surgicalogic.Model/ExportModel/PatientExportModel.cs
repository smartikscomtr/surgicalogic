using Surgicalogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Model.ExportModel
{
    public class PatientExportModel
    {
        [Display(Name = "IdentityNumber", ResourceType = typeof(Resource))]
        public string IdentityNumber { get; set; }
        [Display(Name = "FirstName", ResourceType = typeof(Resource))]
        public string FirstName { get; set; }
        [Display(Name = "LastName", ResourceType = typeof(Resource))]
        public string LastName { get; set; }
        [Display(Name = "Phone", ResourceType = typeof(Resource))]
        public string Phone { get; set; }
        [Display(Name = "Address", ResourceType = typeof(Resource))]
        public string Address { get; set; }
    }
}
