namespace Surgicalogic.Model.EntityModel
{
    public class OperationTypeEquipmentModel : Base.EntityModel
    {
        public int OperationTypeId { get; set; }
        public int EquipmentId { get; set; }
        public EquipmentModel Equipment { get; set; }
    }
}