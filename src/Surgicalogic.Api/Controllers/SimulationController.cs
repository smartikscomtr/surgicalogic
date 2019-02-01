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
        #region Constructor

        private readonly ISimulation _simulation;
        
        /// <summary>
        /// Simulation Constructor
        /// <para>Injection will add this entry point</para>
        /// </summary>
        public SimulationController(ISimulation simulation)
        {
            _simulation = simulation;
            
        }

        #endregion


        [HttpGet]
        [Route("Simulation/Run")]
        public async Task<ActionResult> Run()
        {
            
            List<SimulationResultModel> result = await _simulation.Run();

            return Json(result);
        }        

    }
    

}