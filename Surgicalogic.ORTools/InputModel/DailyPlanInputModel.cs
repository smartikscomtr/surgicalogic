using Smartiks.Teydeb.Surgicalogic.ConsoleApp.InputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartiks.Teydeb.Surgicalogic.ConsoleApp.Model
{
    public class DailyPlanInputModel
    {
        public Settings Settings { get; set; }
        public List<RoomInputModel> Rooms { get; set; }
        public List<OperationInputModel> Operations { get; set; }

    }
}
