using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class OperationModel: Base.EntityModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int OperationTime { get; set; }
        public int OperationTypeId { get; set; }
        public DateTime Date { get; set; }

        public OperationTypeModel OperationType { get; set; }
    }
}
