using Surgicalogic.Common.CustomAttributes;
using Surgicalogic.Model.EntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class OperationForOperationPlanModel : EntityModel.Base.EntityModel
    {
        [Searchable(true)]
        public string Name { get; set; }
        public int OperationTime { get; set; }
        public string EventNumber { get; set; }


        public PatientModel Patient { get; set; }
    }
}
