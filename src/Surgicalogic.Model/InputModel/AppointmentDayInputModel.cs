﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.InputModel
{
    public class AppointmentDayInputModel : GridInputModel
    {
        public int DoctorId { get; set; }
        public DateTime SelectedDate { get; set; }
    }
}
