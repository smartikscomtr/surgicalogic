using Surgicalogic.Data.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities.Base
{
    public class Entity : IId, ICreatedByAndDate, IModifiedByAndDate, IIsActive 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int Id { get; set; }                
        public int CreatedBy { get; set; }        
        public DateTime CreatedDate { get; set; }        
        public int? ModifiedBy { get; set; }        
        public DateTime? ModifiedDate { get; set; }        
        public bool IsActive { get; set; } = true;
    }
}
