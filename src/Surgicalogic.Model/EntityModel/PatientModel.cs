using Surgicalogic.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class PatientModel : Base.EntityModel
    {
        [Searchable(true)]
        public string IdentityNumber { get; set; }
        [Searchable(true)]
        public string FirstName { get; set; }
        [Searchable(true)]
        public string LastName { get; set; }
        [Searchable]
        public string Phone { get; set; }
        [Searchable]
        public string Address { get; set; }
    }
}
