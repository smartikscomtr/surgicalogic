using System;
using System.Collections.Generic;

namespace Surgicalogic.Model.EntityModel
{
    public class OperatingRoomModel : Base.EntityModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public Nullable<double> Width { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<double> Length { get; set; }
        public int EquipmentId { get; set; }
        public ICollection<EquipmentModel> Equipment { get; set; }
    }
}