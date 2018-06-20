using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.OutputModel
{
    public class EquipmentOutputModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int EquipmentTypeId { get; set; }
        public int EquipmentTypeName { get; set; }
        public bool IsPortable { get; set; }

        public virtual EquipmentTypeOutputModel EquipmentType { get; set; }
    }
}
