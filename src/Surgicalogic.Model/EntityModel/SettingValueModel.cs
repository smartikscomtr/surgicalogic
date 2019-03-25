using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class SettingValueModel : Base.EntityModel
    {
        public int RelatedSettingId { get; set; }
        public string Value { get; set; }
    }
}
