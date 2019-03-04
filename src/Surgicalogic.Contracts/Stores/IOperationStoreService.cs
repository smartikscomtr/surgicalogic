using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;

namespace Surgicalogic.Contracts.Stores
{
    public interface IOperationStoreService : IStoreService<Operation, OperationModel>
    {
        Task<List<OperationNameModel>> GetOperationNamesForHistory();
        Task<List<OperationModel>> GetOperationsByDateAsync(DateTime operationDate);
        Task<List<OperationModel>> GetByIdListAsync(int[] updatedItemIds);
        Task<bool> IsDuplicateEventNumber(string eventNumber, int id);
    }
}