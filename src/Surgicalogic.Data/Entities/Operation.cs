using Surgicalogic.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Data.Entities
{
    public class Operation : Entity
    {
        public string EventNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OperationTypeId { get; set; }
        public int OperationTime { get; set; }
        public DateTime Date { get; set; }
        public int PatientId { get; set; }
        public virtual OperationType OperationType { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual ICollection<OperationPersonnel> OperationPersonels { get; set; }
        public virtual ICollection<OperationBlockedOperatingRoom> OperationBlockedOperatingRooms { get; set; }
    }
}
