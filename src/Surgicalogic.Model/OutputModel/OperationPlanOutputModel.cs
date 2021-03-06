﻿using Surgicalogic.Model.EntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.OutputModel
{
    public class OperationPlanOutputModel
    {
        public int id { get; set; }
        public int group { get; set; }
        public string content { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string operationPlanId { get; set; }
        public string className { get; set; }
        public DateTime RealizedStartDate { get; set; }
        public DateTime RealizedEndDate { get; set; }
    }
}
