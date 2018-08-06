﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.OutputModel
{
    public class OperationOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OperationTime { get; set; }
        public string OperationTypeName { get; set; }
        public int OperationTypeId { get; set; }
        public DateTime Date { get; set; }
    }
}
