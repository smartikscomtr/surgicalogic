﻿using Surgicalogic.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("Branches")]
    public class Branch : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<OperationType> OperationTypes { get; set; }

    }
}
