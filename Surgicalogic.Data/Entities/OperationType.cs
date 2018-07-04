using Surgicalogic.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Surgicalogic.Data.Entities
{
    [Table("OperationTypes")]
    public class OperationType : Entity
    {
        public string Name { get; set; }
        public string Description {get;set;}
        public Branch Branch { get; set; }

    }
}
