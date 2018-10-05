using System.Collections.Generic;

namespace Surgicalogic.Model.OutputModel
{
    public class PersonnelOutputModel
    {
        public int Id { get; set; }
        public string PersonnelCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PictureUrl { get; set; }
        public int PersonnelTitleId { get; set; }
        public int PersonnelCategoryId { get; set; }
        public string BranchNames { get; set; }
        public List<int> BranchIds { get; set; }
        public int WorkTypeId { get; set; }
        public string PersonnelTitleName { get; set; }
        public string PersonnelCategoryName{ get; set; }
        public string WorkTypeName { get; set; }
    }
}