using Surgicalogic.Model.EntityModel;
using System;
using System.Collections.Generic;

namespace Surgicalogic.Model.InputModel
{
    public class OperatingRoomInputModel
    {
        public int Id { get; set; }
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