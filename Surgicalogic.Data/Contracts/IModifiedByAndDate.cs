using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Data.Contracts
{
    public interface IModifiedByAndDate
    {
        Nullable<int> ModifiedBy { get; set; }
        Nullable<DateTime> ModifiedDate { get; set; }
    }
}
