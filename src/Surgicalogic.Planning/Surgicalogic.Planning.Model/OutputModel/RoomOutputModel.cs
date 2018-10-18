using System.Collections.Generic;

namespace Surgicalogic.Planning.Model.OutputModel
{
    public class RoomOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<OperationOutputModel> Operations { get; set; }
    }
}