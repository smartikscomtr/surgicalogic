using Surgicalogic.Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("OperationPersonnels")]
    public class OperationPersonnel : Entity 
    {        
        public int OperationId { get; set; }
        public Operation Operation { get; set; }     
        public int PersonnelId { get; set; }
        public Personnel Personnel { get; set; }
    }
}
