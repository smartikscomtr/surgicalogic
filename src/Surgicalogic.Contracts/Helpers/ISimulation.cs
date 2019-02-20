using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Helpers
{
    public interface ISimulation
    {
        Task<ResultModel<SimulationResultModel>> Run(GridInputModel input, DateTime selectDate);
    }
}
