using Surgicalogic.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class PersonnelTitleModel : Base.EntityModel
    {
        [Searchable(true)]
        public string Name { get; set; }
    }
}
