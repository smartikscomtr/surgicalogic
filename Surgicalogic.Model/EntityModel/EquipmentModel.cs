﻿using System;

namespace Surgicalogic.Model.EntityModel
{
    public class EquipmentModel : Base.EntityModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int EquipmentTypeId { get; set; }
        public bool IsPortable { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public EquipmentTypeModel EquipmentTypeModel { get; set; }
    }
}
