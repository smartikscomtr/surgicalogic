using System;

namespace Surgicalogic.Data.Contracts
{
    public interface IModifiedByAndDate
    {
        Nullable<int> ModifiedBy { get; set; }
        Nullable<DateTime> ModifiedDate { get; set; }
    }
}
