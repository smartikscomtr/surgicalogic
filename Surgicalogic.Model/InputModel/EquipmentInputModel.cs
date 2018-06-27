using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.InputModel
{
    public class EquipmentInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int EquipmentTypeId { get; set; }

        public bool IsPortable { get; set; }
    }
}
