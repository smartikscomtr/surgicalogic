using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Data.Entities
{
    public class Setting : Base.Entity
    {
        [Required]
        [StringLength(100)]
        public string Key { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public int SettingDataTypeId { get; set; }
        public int? IntValue { get; set; }
        public string StringValue { get; set; }
        public string TimeValue { get; set; }

        public SettingDataType SettingDataType { get; set; }
    }
}
