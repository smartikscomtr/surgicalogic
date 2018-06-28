using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Planning.Model.InputModel
{
    public class SettingsInputModel
    {
        public int RoomsPeriod { get; set; }
        public int MaximumPeriod { get; set; }
        public int StartingHour { get; set; }
        public int StartingMinute { get; set; }
        public int PeriodInMinutes { get; set; }
    }
}
