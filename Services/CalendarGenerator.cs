using FasesDaLua.Domain;
using FasesDaLua.Domain.Entities;
using FasesDaLua.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace FasesDaLua.Services
{
    public class CalendarGenerator : ServiceBase
    {
        public System.Globalization.Calendar Calendar { get; }
        public Size CalendarSize { get => new Size(768, 1122); }
        public MoonPhase LastMoonPhase { get; private set; }
        public CalendarConfiguration CalendarConfiguration { get; set; }
        public DateTime LastMoonPhaseDate { get; set; }

        private int DaysToNextMoonPhase { get; set; }

        public CalendarGenerator()
        {
            this.Calendar = new GregorianCalendar();
        }

        public List<DayControl> GetDays(int month, int year, Size dayControlSize)
        {
            var days = this.Calendar.GetDaysInMonth(year, month);
            return this.GetDays(year, month, days, dayControlSize);
        }

        public List<DayControl> GetDays(int year, Size dayControlSize)
        {
            var months = this.Calendar.GetMonthsInYear(year);
            var daysPerYear = new List<DayControl>();

            for (var month = 1; month <= months; month++)
            {
                var days = this.GetDaysInMonth(year, month);
                var daysPerMonth = this.GetDays(year, month, days, dayControlSize);

                if (daysPerMonth != null && daysPerMonth.Count > 0)
                {
                    daysPerYear.AddRange(daysPerMonth);
                }
            }

            return daysPerYear;
        }

        public List<DayControl> GetDays(CalendarConfiguration calendarConfiguration)
        {
            this.CalendarConfiguration = calendarConfiguration;

            if (this.CalendarConfiguration.IsCreateFullCalendar)
            {
                return this.GetDays(this.CalendarConfiguration.ReferenceYear,
                                    this.CalendarConfiguration.DayControlSize);
            }

            var daysPerYear = new List<DayControl>();

            foreach (var month in this.CalendarConfiguration.Months)
            {
                var year = this.CalendarConfiguration.ReferenceYear;

                var days = this.GetDaysInMonth(year, (int)month);
                var daysPerMonth = this.GetDays(year, (int)month, days, this.CalendarConfiguration.DayControlSize);

                if (daysPerMonth != null && daysPerMonth.Count > 0)
                {
                    daysPerYear.AddRange(daysPerMonth);
                }
            }

            return daysPerYear;
        }

        public int GetDaysInYear(int year)
        {
            return this.Calendar.GetDaysInYear(year);
        }

        public int GetDaysInMonth(int year, int month)
        {
            return this.Calendar.GetDaysInMonth(year, month);
        }

        public int GetMonthInYear(int year)
        {
            return this.Calendar.GetMonthsInYear(year);
        }

        public DayOfWeek GetDayOfWeek(DateTime date)
        {
            return this.Calendar.GetDayOfWeek(date);
        }

        public Month GetMonth(DateTime date)
        {
            return (Month)date.Month;
        }

        private List<DayControl> GetDays(int year, int month, int days, Size dayControlSize)
        {
            var dayPerMonth = new List<DayControl>();

            for (var day = 1; day <= days; day++)
            {
                var currentDateTime = new DateTime(year, month, day);

                var moonPhase = this.GetMoonPhase(currentDateTime);

                if (moonPhase != MoonPhase.None)
                {
                    this.LastMoonPhase = moonPhase;
                    this.LastMoonPhaseDate = currentDateTime;
                    this.DaysToNextMoonPhase = this.GetDaysToNextMoon(moonPhase);
                }

                var dayControl = new DayControl(year, month, day, moonPhase);

                if (dayControl != null)
                {
                    dayControl.Width = dayControlSize.Width;
                    dayControl.Height = dayControlSize.Height;

                    dayPerMonth.Add(dayControl);
                }
            }

            return dayPerMonth;
        }

        private int GetDaysToNextMoon(MoonPhase moonPhase)
        {
            switch (moonPhase)
            {
                case MoonPhase.NewMoon:
                    return 7;

                case MoonPhase.CrescentMoon:
                case MoonPhase.FullMoon:
                case MoonPhase.WaningMoon:
                    return 8;

                default:
                    throw new NotSupportedException($"Moon phase {moonPhase} is not supported in this method.");
            }
        }

        private MoonPhase GetMoonPhase(DateTime currentDateTime)
        {
            if (currentDateTime == this.CalendarConfiguration.FirstNewMoonDate)
            {
                return MoonPhase.NewMoon;
            }

            if (currentDateTime == (this.LastMoonPhaseDate.AddDays(this.DaysToNextMoonPhase)))
            {
                return this.GetNextMoonPhase(this.LastMoonPhase);
            }

            return MoonPhase.None;
        }

        private MoonPhase GetNextMoonPhase(MoonPhase lastMoonPhase)
        {
            switch (lastMoonPhase)
            {
                case MoonPhase.NewMoon:
                    return MoonPhase.CrescentMoon;

                case MoonPhase.CrescentMoon:
                    return MoonPhase.FullMoon;

                case MoonPhase.FullMoon:
                    return MoonPhase.WaningMoon;

                case MoonPhase.WaningMoon:
                    return MoonPhase.NewMoon;

                default:
                    return MoonPhase.None;
            }
        }

        public FixedDocument GetCalendar(int year)
        {
            var document = new FixedDocument();
            document.DocumentPaginator.PageSize = this.CalendarSize;

            var calendarFp = this.GetCalendarFixedPage(year);

            if (calendarFp != null)
            {
                var pageContent = new PageContent();
                ((IAddChild)pageContent).AddChild(calendarFp);
                document.Pages.Add(pageContent);

                return document;
            }

            return null;
        }

        private FixedPage GetCalendarFixedPage(int year)
        {
            var element = this.GetCalendarElement<Grid>("FasesDaLua.Resources.Templates.Calendar.MoonCalendar.xaml");

            element.UpdateLayout();
            element.Measure(this.CalendarSize);
            element.Arrange(new Rect(new Point(), this.CalendarSize));

            var gdDays = element.FindName<Grid>("gdDias");

            var recipientSize = new Size(gdDays.ActualWidth, gdDays.ActualHeight);

            //var days = this.GetDays(year, recipientSize);
            //element.DataContext = new MoonCalendarViewModel(days, Month.January, 2022);

            var page = new FixedPage
            {
                Background = Brushes.White,
                Width = this.CalendarSize.Width,
                Height = this.CalendarSize.Height
            };

            page.Children.Add(element);
            page.UpdateLayout();
            page.Measure(this.CalendarSize);
            page.Arrange(new Rect(new Point(), this.CalendarSize));



            return page;
        }

        public T GetCalendarElement<T>(string elementName) where T : FrameworkElement
        {
            using (var xaml = new Xaml())
            {
                var xamlElement = xaml.GetXamlElement<T>(elementName);

                if (xamlElement != null)
                {
                    return xamlElement;
                }

                return null;
            }
        }
    }
}
