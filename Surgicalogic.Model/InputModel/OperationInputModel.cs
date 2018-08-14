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
        public string OperationTime { get; set; }
        public int OperationTypeId { get; set; }
        public List<int> DoctorIds { get; set; }
        public List<int> OperatingRoomIds { get; set; }
        public DateTime Date { get; set; }
    }
}