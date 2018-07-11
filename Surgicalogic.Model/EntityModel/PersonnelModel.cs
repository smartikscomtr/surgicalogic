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
        public PersonnelTitleModel PersonnelTitle { get; set; }
        public BranchModel Branch { get; set; }
        public WorkTypeModel WorkType { get; set; }
    }
}