using Surgicalogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Model.ExportModel
{
    public class FeedbackExportModel
    {
        [Display(Name = "Mail", ResourceType = typeof(Resource))]
        public string Email { get; set; }
        [Display(Name = "Content", ResourceType = typeof(Resource))]
        public string Body { get; set; }
    }
}
