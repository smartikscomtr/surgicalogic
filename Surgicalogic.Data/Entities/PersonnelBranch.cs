using Surgicalogic.Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("PersonnelBranches")]
    public class PersonnelBranch : Entity
    {
        public int PersonnelId { get; set; }
        public Personnel Personnel { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}
