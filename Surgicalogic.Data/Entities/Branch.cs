using Surgicalogic.Data.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("Branches")]
    public class Branch : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
