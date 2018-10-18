using Surgicalogic.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Surgicalogic.Data.Entities
{

    [Table("PersonnelTitles")]
    public class PersonnelTitle : Entity
    {
        [StringLength(50)]
        public string Name { get; set; }
    }
}
