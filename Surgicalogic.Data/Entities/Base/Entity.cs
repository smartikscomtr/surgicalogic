using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surgicalogic.Data.Entities.Base
{
    public class Entity
    {        
        [Key]
        public int Id { get; set; }
    }
}
