using Surgicalogic.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class FeedbackModel : Base.EntityModel
    {
        [Searchable]
        public string Email { get; set; }
        [Searchable]
        public string Body { get; set; }
    }
}
