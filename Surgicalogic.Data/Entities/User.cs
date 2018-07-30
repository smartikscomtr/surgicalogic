using Microsoft.AspNetCore.Identity;
using Surgicalogic.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Data.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
