using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class SettingModel : Base.EntityModel
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
