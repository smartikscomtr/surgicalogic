using Surgicalogic.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class SettingModel : Base.EntityModel
    {
        public string Key { get; set; }
        [Searchable]
        public string Name { get; set; }
        public int SettingDataTypeId { get; set; }
        public int? SettingValueId { get; set; }

        public int? IntValue { get; set; }
        public string StringValue { get; set; }
        public string TimeValue { get; set; }
        public double? DoubleValue { get; set; }

        public SettingValueModel SettingValue { get; set; }
    }
}
