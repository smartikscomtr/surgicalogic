using Surgicalogic.Common.CustomAttributes;
using System;
using System.Collections.Generic;

namespace Surgicalogic.Model.EntityModel
{
    public class OperatingRoomModel : Base.EntityModel
    {
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string Description { get; set; }
        [Searchable]
        public string Location { get; set; }
        public Nullable<double> Width { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<double> Length { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<OperatingRoomEquipmentModel> OperatingRoomEquipments { get; set; }
        public ICollection<OperatingRoomOperationTypeModel> OperatingRoomOperationTypes { get; set; }
        public ICollection<OperatingRoomCalendarModel> OperatingRoomCalendars { get; set; }
    }
}