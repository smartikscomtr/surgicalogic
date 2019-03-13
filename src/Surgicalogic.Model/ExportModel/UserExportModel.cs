using Surgicalogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Model.ExportModel
{
    public class UserExportModel
    {
        [Display(Name = "Username", ResourceType = typeof(Resource))]
        public string UserName { get; set; }
        [Display(Name = "Mail", ResourceType = typeof(Resource))]
        public string Email { get; set; }
    }
}
