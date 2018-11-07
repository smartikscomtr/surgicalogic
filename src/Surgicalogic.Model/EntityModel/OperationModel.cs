using Surgicalogic.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class OperationModel: Base.EntityModel
    {
        [Searchable(true)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int OperationTime { get; set; }
        public int OperationTypeId { get; set; }
        public DateTime Date { get; set; }
        public int PatientId { get; set; }
        public string EventNumber { get; set; }

        public OperationTypeModel OperationType { get; set; }
        public PatientModel Patient { get; set; }
        public ICollection<OperationPersonnelModel> OperationPersonels { get; set; }
        public virtual ICollection<OperationBlockedOperatingRoomModel> OperationBlockedOperatingRooms { get; set; }
    }
}