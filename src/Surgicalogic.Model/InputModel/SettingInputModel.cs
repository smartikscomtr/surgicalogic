using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.InputModel
{
    public class SettingInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SettingDataTypeId { get; set; }
        public int? SettingValueId { get; set; }
        public int? IntValue { get; set; }
        public string StringValue { get; set; }
        public double? DoubleValue { get; set; }
        public string TimeValue { get; set; }
    }
}
