using System.Collections.Generic;

namespace Surgicalogic.ORTools.Model
{
    public class Operation
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int Period { get; set; }
        public List<int> UnavailableRooms { get; set; }
    }
}
