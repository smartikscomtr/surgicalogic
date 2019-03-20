using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Data.Entities
{
    public class SettingValue : Base.Entity
    {
        public int RelatedSettingId { get; set; }

        [StringLength(100)]
        public string Value { get; set; }
    }
}
