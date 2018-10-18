using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.InputModel
{
    public class OperationPlanInputModel
    {
        public int Id { get; set; }
        public int OperationId { get; set; }
        public DateTime Start { get; set; }
        public int Length { get; set; }
        public int RoomId { get; set; }
    }

    public class OperationPlanListModel
    {
        public List<OperationPlanInputModel> Operations { get; set; }
    }
}
