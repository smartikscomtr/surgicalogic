using System.Collections.Generic;

namespace Surgicalogic.Planning.Model.InputModel
{
    public class OperationInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DoctorId { get; set; }
        public int Period { get; set; }
        public List<int> UnavailableRooms { get; set; }
    }
}
