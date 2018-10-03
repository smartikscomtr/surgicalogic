using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.ExportModel
{
    public class OperationTypeExportModel
    {
        public string Name { get; set; }
        public bool SuitableForMultipleOperation { get; set; }
        public string Description { get; set; }
    }
}
