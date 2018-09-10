using Google.OrTools.LinearSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surgicalogic.Planning.ORTools
{
    public class LinearPlanner
    {
        Solver solver = new Solver("SurgicaLogic", Solver.CBC_MIXED_INTEGER_PROGRAMMING);
    }
}