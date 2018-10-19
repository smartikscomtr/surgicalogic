using Surgicalogic.Data.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("Feedbacks")]
    public class Feedback : Entity
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(1000)]
        public string Body { get; set; }
    }
}
