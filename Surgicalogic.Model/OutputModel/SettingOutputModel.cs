using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.OutputModel
{
    public class SettingOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SettingDataTypeId { get; set; }
        public int? IntValue { get; set; }
        public string StringValue { get; set; }
        public string TimeValue { get; set; }
        public string Value { get; set; }
    }
}
