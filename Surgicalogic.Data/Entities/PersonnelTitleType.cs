using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Surgicalogic.Data.Entities.Base;

namespace Surgicalogic.Data.Entities
{
    [Table("PersonnelTitleTypes")]
    public class PersonnelTitleType : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }        
    }
}
