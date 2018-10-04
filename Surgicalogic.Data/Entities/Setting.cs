using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Data.Entities
{
    public class Setting : Base.Entity
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public int SettingDataTypeId { get; set; }
        public int? IntValue { get; set; }
        public string StringValue { get; set; }
        public string TimeValue { get; set; }

        public SettingDataType SettingDataType { get; set; }
    }
}
