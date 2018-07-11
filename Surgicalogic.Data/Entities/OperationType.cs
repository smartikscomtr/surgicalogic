using Surgicalogic.Data.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("OperationTypes")]
    public class OperationType : Entity
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description {get;set;}
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}
