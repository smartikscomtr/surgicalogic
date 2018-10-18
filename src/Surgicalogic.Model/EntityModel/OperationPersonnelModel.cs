using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.EntityModel
{
    public class OperationPersonnelModel : Base.EntityModel
    {
        public int OperationId { get; set; }
        public int PersonnelId { get; set; }
        public PersonnelModel Personnel { get; set; }
    }
}
