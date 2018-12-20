using System.Collections.Generic;

namespace Surgicalogic.Planning.Model.InputModel
{
    public class OperationInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] DoctorIds { get; set; }
        public int OperationTime { get; set; }
        public int Period { get; set; }
        public List<int> UnavailableRooms { get; set; }
    }
}