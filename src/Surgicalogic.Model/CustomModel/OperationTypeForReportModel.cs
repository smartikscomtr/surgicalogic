using Surgicalogic.Model.EntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class OperationTypeForReportModel : EntityModel.Base.EntityModel
    {
        public BranchModel Branch { get; set; } 
    }
}
