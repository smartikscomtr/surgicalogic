using Surgicalogic.Planning.Model.InputModel;
using Surgicalogic.Planning.Model.OutputModel;
using Surgicalogic.Planning.ORTools;
using System.Web.Http;

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

            var result = Planner.Solve(input);
            return result;
        }
    }
}