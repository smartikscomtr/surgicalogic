using System;

namespace Surgicalogic.Data.Contracts
{
    public interface ICreatedByAndDate
    {
        int CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
    }
}