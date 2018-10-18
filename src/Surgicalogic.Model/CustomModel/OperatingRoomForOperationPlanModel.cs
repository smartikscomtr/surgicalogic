using Surgicalogic.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class OperatingRoomForOperationPlanModel : EntityModel.Base.EntityModel
    {
        [Searchable(true)]
        public string Name { get; set; }
    }
}
