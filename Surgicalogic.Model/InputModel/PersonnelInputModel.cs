using System;

namespace Surgicalogic.Model.InputModel
{
    public class PersonnelInputModel
    {
        public int Id { get; set; }
        public string PersonnelCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PersonnelTitleId { get; set; }
        public int BranchId { get; set; }
        public int WorkTypeId { get; set; }
    }
}