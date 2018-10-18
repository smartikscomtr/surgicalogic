using Surgicalogic.Model.EntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class OperationForReportModel : EntityModel.Base.EntityModel
    {
        public string Name { get; set; }
        public int OperationTime { get; set; }
        public OperationTypeModel OperationType { get; set; }
        public ICollection<OperationPersonnelForReportModel> OperationPersonels { get; set; }
    }
}
