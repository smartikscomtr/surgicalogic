﻿using System;

namespace Surgicalogic.Model.EntityModel.Base
{
    public class EntityModel
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; } = true;

    }
}