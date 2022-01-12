﻿using System.Collections.Generic;
using System.Windows;

namespace FasesDaLua.Domain.Entities
{
    public class CalendarConfiguration
    {
        public int ReferenceYear { get; set; }
        public bool IsCreateFullCalendar { get; set; }
        public List<Month> Months { get; set; }
        public Size DayControlSize { get; set; }
        public MoonPhase FirstMoonPhase { get; set; }

        public CalendarConfiguration()
        {

        }
    }
}
