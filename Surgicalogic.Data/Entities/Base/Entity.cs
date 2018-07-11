using Surgicalogic.Data.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities.Base
{
    public class Entity : IId, ICreatedByAndDate, IModifiedByAndDate, IIsActive 
    {
        [Key, Column(Order = 0)]
        public int Id { get; set; }        
        [Column(Order = 100)]
        public int CreatedBy { get; set; }
        [Column(Order = 101)]
        public DateTime CreatedDate { get; set; }
        [Column(Order = 102)]                
        public int? ModifiedBy { get; set; }
        [Column(Order = 103)]
        public DateTime? ModifiedDate { get; set; }
        [Column(Order = 104)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public bool IsActive { get; set; } = true;
    }
}
