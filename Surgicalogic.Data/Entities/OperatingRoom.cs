using Surgicalogic.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surgicalogic.Data.Entities
{
    [Table("OperatingRooms")]
    public class OperatingRoom : Entity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public Nullable<double> Width { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<double> Length { get; set; }
        public ICollection<Equipment> Equipment { get; set; }

    }
}
