using FasesDaLua.Domain;
using FasesDaLua.Domain.Entities;
using FasesDaLua.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FasesDaLua.ViewModel
{
    public class MoonCalendarViewModel : BaseViewModel
    {
        private string _monthAndYear = "";

        public CalendarConfiguration CalendarConfiguration { get; set; }
        public List<DayControl> Mondays { get; set; }
        public List<DayControl> Thursdays { get; set; }
        public List<DayControl> Wednesdays { get; set; }
        public List<DayControl> Tuesdays { get; set; }
        public List<DayControl> Fridays { get; set; }
        public List<DayControl> Saturdays { get; set; }
        public List<DayControl> Sundays { get; set; }
        public string MonthAndYear 
        { 
            get => this._monthAndYear;
            set 
            {
                this._monthAndYear = value;
                this.NotifyPropertyChanged(nameof(this.MonthAndYear));
            }
        }

        public MoonCalendarViewModel(List<DayControl> days, Month month, int year)
        {
            this.PopulateDays(days, month, year);
        }

        private void PopulateDays(List<DayControl> daysInMonth, Month month, int year)
        {
            if (daysInMonth != null && daysInMonth.Count > 0)
            {
                this.MonthAndYear = $"{month} {year}";

                this.Mondays = daysInMonth.Where(d => d.Date.DayOfWeek == DayOfWeek.Monday).ToList();
                this.Thursdays = daysInMonth.Where(d => d.Date.DayOfWeek == DayOfWeek.Thursday).ToList();
                this.Wednesdays = daysInMonth.Where(d => d.Date.DayOfWeek == DayOfWeek.Wednesday).ToList();
                this.Tuesdays = daysInMonth.Where(d => d.Date.DayOfWeek == DayOfWeek.Tuesday).ToList();
                this.Fridays = daysInMonth.Where(d => d.Date.DayOfWeek == DayOfWeek.Friday).ToList();
                this.Saturdays = daysInMonth.Where(d => d.Date.DayOfWeek == DayOfWeek.Saturday).ToList();
                this.Sundays = daysInMonth.Where(d => d.Date.DayOfWeek == DayOfWeek.Sunday).ToList();

                this.NotifyPropertyChanged(nameof(this.Mondays));
                this.NotifyPropertyChanged(nameof(this.Thursdays));
                this.NotifyPropertyChanged(nameof(this.Wednesdays));
                this.NotifyPropertyChanged(nameof(this.Tuesdays));
                this.NotifyPropertyChanged(nameof(this.Fridays));
                this.NotifyPropertyChanged(nameof(this.Saturdays));
                this.NotifyPropertyChanged(nameof(this.Sundays));
            }
        }
    }
}
