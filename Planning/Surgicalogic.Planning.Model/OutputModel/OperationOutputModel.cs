using Surgicalogic.Planning.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Planning.Model.OutputModel
{
    public class OperationOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Period { get; set; }
        public DoctorModel Doctor { get; set; }
        public DateTime StartDate { get; set; }
    }
}
