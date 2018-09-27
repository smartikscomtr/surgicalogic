using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Data.Entities
{
    public class Setting : Base.Entity
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
