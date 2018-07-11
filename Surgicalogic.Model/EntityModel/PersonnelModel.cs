namespace Surgicalogic.Model.EntityModel
{
    public class PersonnelModel : Base.EntityModel
    {
        public string PersonnelCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PersonnelTitleId { get; set; }
        public int BranchId { get; set; }
        public int WorkTypeId { get; set; }
        public PersonnelTitleModel PersonnelTitleModel { get; set; }
        public BranchModel BranchModel { get; set; }
        public WorkTypeModel WorkTypeModel { get; set; }
    }
}