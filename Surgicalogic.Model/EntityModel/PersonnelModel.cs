using Surgicalogic.Common.CustomAttributes;

namespace Surgicalogic.Model.EntityModel
{
    public class PersonnelModel : Base.EntityModel
    {
        [Searchable]
        public string PersonnelCode { get; set; }
        [Searchable]
        public string FirstName { get; set; }
        [Searchable]
        public string LastName { get; set; }
        public int PersonnelTitleId { get; set; }
        public int BranchId { get; set; }
        public int WorkTypeId { get; set; }
        [Searchable]
        public PersonnelTitleModel PersonnelTitle { get; set; }
       // [Searchable]
        public BranchModel Branch { get; set; }
        [Searchable]
        public WorkTypeModel WorkType { get; set; }
    }
}