using Surgicalogic.Common.CustomAttributes;
using Surgicalogic.Model.EntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class OperationGridModel : EntityModel.Base.EntityModel
    {
        [Searchable(true)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int OperationTime { get; set; }
        public int OperationTypeId { get; set; }
        public DateTime Date { get; set; }
        public int PatientId { get; set; }
        public string EventNumber { get; set; }

        [Searchable]
        public OperationTypeForOperationModel OperationType { get; set; }
    }
}
