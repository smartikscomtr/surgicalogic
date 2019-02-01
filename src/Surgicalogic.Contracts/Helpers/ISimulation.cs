using Surgicalogic.Model.CustomModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Surgicalogic.Contracts.Helpers
{
    public interface ISimulation
    {
        Task<List<SimulationResultModel>> Run();
    }
}
