using System.Collections.Generic;

namespace Surgicalogic.Model.InputModel
{
    public class OperationTypeInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BranchId { get; set; }
        public List<int> OperatingRoomIds { get; set; }
        public List<int> Equipments { get; set; }
    }
}