namespace Surgicalogic.Model.EntityModel
{
    public class OperationTypeModel : Base.EntityModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public BranchModel BranchModel { get; set; }
    }
}