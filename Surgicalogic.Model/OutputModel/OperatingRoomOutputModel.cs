using Surgicalogic.Model.EntityModel;
using System;
using System.Collections.Generic;

namespace Surgicalogic.Model.OutputModel
{
    public class OperatingRoomOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public Nullable<double> Width { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<double> Length { get; set; }        
        public ICollection<OperatingRoomEquipmentOutputModel> OperatingRoomEquipments { get; set; }
        public ICollection<OperatingRoomOperationTypeOutputModel> OperatingRoomOperationTypes { get; set; }
    }
}