using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CustomModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Contracts.Stores
{
    public interface IOperationGridStoreService : IStoreService<Operation, OperationGridModel>
    {
    }
}
