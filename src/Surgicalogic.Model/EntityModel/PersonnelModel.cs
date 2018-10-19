using Surgicalogic.Common.CustomAttributes;
using System.Collections.Generic;

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
        public string FullName { get; set; }
        public string PictureUrl { get; set; }
        public int? PersonnelTitleId { get; set; }
        public int? PersonnelCategoryId { get; set; }
        public int WorkTypeId { get; set; }
        [Searchable]
        public PersonnelTitleModel PersonnelTitle { get; set; }
        [Searchable]
        public PersonnelCategoryModel PersonnelCategory { get; set; }
        [Searchable]
        public WorkTypeModel WorkType { get; set; }

        public ICollection<PersonnelBranchModel> PersonnelBranches { get; set; }
        public ICollection<DoctorCalendarModel> DoctorCalendars { get; set; }
        public ICollection<AppointmentCalendarModel> AppointmentCalendars { get; set; }
    }
}