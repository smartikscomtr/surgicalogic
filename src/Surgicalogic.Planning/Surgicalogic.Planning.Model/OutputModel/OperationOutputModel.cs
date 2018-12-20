using System;

namespace Surgicalogic.Planning.Model.OutputModel
{
    public class OperationOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OperationTime { get; set; }
        public int Period { get; set; }
        public int[] DoctorIds { get; set; }
        public DateTime StartDate { get; set; }
    }
}