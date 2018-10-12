﻿using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Planning.Engine;
using Surgicalogic.Planning.Model.InputModel;
using Surgicalogic.Planning.Model.OutputModel;

namespace Surgicalogic.Planning.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Plan")]
    public class PlanController : Controller
    {
        public DailyPlanOutputModel Post([FromBody]DailyPlanInputModel input)
        {
            if (input == null)
            {
                return null;
            }

            var result = MIPPlanner.Solve(input);
            return result;
        }
    }
}