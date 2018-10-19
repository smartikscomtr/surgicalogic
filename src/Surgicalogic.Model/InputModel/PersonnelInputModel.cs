using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Surgicalogic.Model.InputModel
{
    public class PersonnelInputModel
    {
        public int Id { get; set; }
        public string PersonnelCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
        public int? PersonnelTitleId { get; set; }
        public int? PersonnelCategoryId { get; set; }
        public string Branches { get; set; }
        public int WorkTypeId { get; set; }
        public List<int> DoctorCalendars { get; set; }
        public List<int> AppointmentCalendars { get; set; }
    }
}