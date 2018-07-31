using System.Collections.Generic;

namespace Surgicalogic.Model.OutputModel
{
    public class OperationTypeOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public List<int> Equipments { get; set; }
        public List<int> OperatingRoomIds { get; set; }
    }
}