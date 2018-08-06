using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.InputModel
{
    public class OperationInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OperationTime { get; set; }
        public int OperationTypeId { get; set; }
        public DateTime Date { get; set; }
    }
}