using Surgicalogic.Planning.Model.InputModel;
using Surgicalogic.Planning.Model.OutputModel;
using Surgicalogic.Planning.ORTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Surgicalogic.Planning.Api.Controllers
{
    public class PlanController : ApiController
    {
        public DailyPlanOutputModel Post([FromBody]DailyPlanInputModel input)
        {
            if (input == null)
            {
                return null;
            }

            return Planner.Solve(input);
        }
    }
}