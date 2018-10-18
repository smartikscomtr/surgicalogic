using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.CustomModel
{
    public class OperationPersonnelForReportModel : EntityModel.Base.EntityModel
    {
        public PersonnelForReportModel Personnel { get; set; }
    }
}
