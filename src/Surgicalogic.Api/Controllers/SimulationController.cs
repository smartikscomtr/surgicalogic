using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Surgicalogic.Api.Helpers;
using Surgicalogic.Common.Settings;
using Surgicalogic.Contracts.Helpers;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Planning.Model.InputModel;
using Surgicalogic.Planning.Model.OutputModel;

namespace Surgicalogic.Api.Controllers
{
    public class SimulationController : Controller
    {
        private readonly ISimulation _simulation;
        
        public SimulationController(ISimulation simulation)
        {
            _simulation = simulation;
            
        }
        
        [HttpGet]
        [Route("Simulation/Run")]
        public async Task<ResultModel<SimulationResultModel>> Run(GridInputModel input)
        { 
            return await _simulation.Run(input);
        }

        //[HttpGet]
        //[Route("Simulation/TodayRun")]
        //public async Task<ResultModel<SimulationResultModel>> TodayRun(GridInputModel input)
        //{
        //    return await _simulation.TodayRun(input);
        //}
    }
}