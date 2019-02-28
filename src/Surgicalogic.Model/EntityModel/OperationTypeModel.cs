using System;
using System.Collections.Generic;
using Surgicalogic.Common.CustomAttributes;

namespace Surgicalogic.Model.EntityModel
{
    public class OperationTypeModel : Base.EntityModel
    {
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string Description { get; set; }
        public int BranchId { get; set; }
        public double? CoefficientOfVariation { get; set; }

        [Searchable]
        public BranchModel Branch { get; set; }
        public ICollection<OperatingRoomOperationTypeModel> OperatingRoomOperationTypes { get; set; }

        public ICollection<OperationTypeEquipmentModel> OperationTypeEquipment { get; set; }
    }
}