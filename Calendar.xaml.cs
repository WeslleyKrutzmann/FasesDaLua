using FasesDaLua.Domain;
using FasesDaLua.Domain.Entities;
using FasesDaLua.Services;
using FasesDaLua.Utils;
using FasesDaLua.Views;
using FasesDaLua.Views.UserControls;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace FasesDaLua
{
    /// <summary>
    /// Interaction logic for Calendar.xaml
    /// </summary>
    public partial class Calendar : WindowBase
    {
        private const int DAYS_IN_WEEK = 7;
        private const int WEEKS_IN_MONTH = 6;

        private int _year = DateTime.Now.Year + 1;
        private string _months = String.Empty;
        private bool _isCreateFullCalendar = true;
        private string _monthYearReference = "";

        private Size PrintSize { get; set; } = new Size(768, 1122);
        public int Year
        {
            get => this._year;
            set
            {
                this._year = value;
                this.NotifyPropertyChanged(nameof(this.Year));
            }
        }
        public string Months
        {
            get => this._months;
            set
            {
                this._months = value;
                this.NotifyPropertyChanged(nameof(this.Months));
            }
        }
        public bool IsCreateFullCalendar
        {
            get => this._isCreateFullCalendar;
            set
            {
                this._isCreateFullCalendar = value;
                this.NotifyPropertyChanged(nameof(this.IsCreateFullCalendar));
            }
        }
        public Size DayControlSize { get; set; }
        public CalendarConfiguration CalendarConfiguration { get; private set; }
        public double HorizontalGap { get; set; }
        public double VerticalGap { get; set; }
        public DayOfWeek LastDayOfWeek { get; set; }
        public string MonthYearReference
        {
            get => this._monthYearReference;
            set
            {
                this._monthYearReference = value;
                this.NotifyPropertyChanged(nameof(this.MonthYearReference));
            }
        }

        public Month CurrentMonth { get; private set; }
        public Queue<Month> MonthsQueue { get; private set; }

        public Calendar(Window owner)
        {
            this.InitializeComponent();

            this.Owner = owner;
            this.DataContext = this;
            this.Loaded += this.Calendar_Loaded;
        }

        private void Calendar_Loaded(object sender, RoutedEventArgs e)
        {
            var calendarSize = this.GetCalendarSize();

            this.gdCcalendar.Width = calendarSize.Width;
            this.gdCcalendar.Height = calendarSize.Height;

            this.gdCcalendar.UpdateLayout();

            this.LoadBackgroundImage();
        }

        private Size GetCalendarSize()
        {
            var maxHeight = this.gdCcalendar.ActualHeight;
            var maxWidth = this.gdCcalendar.ActualWidth;

            var newWidth = 0D;
            var newHeight = 0D;

            if (this.PrintSize.Width > this.PrintSize.Height)
            {
                newWidth = maxWidth;
                newHeight = (this.PrintSize.Height * newWidth) / this.PrintSize.Width;

                if (newHeight > maxHeight)
                {
                    newHeight = maxHeight;
                    newWidth = (this.PrintSize.Width * newHeight) / this.PrintSize.Height;
                }
            }
            else
            {
                newHeight = maxHeight;
                newWidth = (this.PrintSize.Width * newHeight) / this.PrintSize.Height;

                if (newWidth > maxWidth)
                {
                    newWidth = maxWidth;
                    newHeight = (this.PrintSize.Height * newWidth) / this.PrintSize.Width;
                }
            }

            return new Size(newWidth, newHeight);
        }

        private void LoadBackgroundImage()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "bgCalendar.svg");

            using (var imagem = new MagickImage(filePath))
            {
                this.imgBackground.ImageSource = imagem.ToBitmapSource();
            }
        }

        private Size GetDayControlSize()
        {
            var horizontalDivisor = DAYS_IN_WEEK + 0.5;
            var verticalDivisor = WEEKS_IN_MONTH + 0.5;

            var dayControlwidth = Math.Round(this.inkCalendar.ActualWidth / horizontalDivisor);
            var dayControlheight = Math.Round(this.inkCalendar.ActualHeight / verticalDivisor);

            this.HorizontalGap = (this.inkCalendar.ActualWidth - (dayControlwidth * DAYS_IN_WEEK)) / (DAYS_IN_WEEK + 1);
            this.VerticalGap = (this.inkCalendar.ActualHeight - (dayControlheight * WEEKS_IN_MONTH)) / (WEEKS_IN_MONTH + 1);

            return new Size(dayControlwidth, dayControlheight);
        }

        private void BtnCreateCalendar_Click(object sender, RoutedEventArgs e)
        {
            this.CreateCalendar();
        }

        private void CreateCalendar()
        {
            this.DayControlSize = this.GetDayControlSize();

            this.CalendarConfiguration = new CalendarConfiguration
            {
                ReferenceYear = this.Year,
                Months = this.GetMonths(this.Months),
                IsCreateFullCalendar = this.IsCreateFullCalendar,
                DayControlSize = this.DayControlSize,
                FirstNewMoonDate = new DateTime(2022, 1, 2)
            };

            this.CreateCalendarAsync();
        }

        private void CreateCalendarAsync()
        {
            using (var cg = new CalendarGenerator())
            {
                this.inkCalendar.Children.Clear();

                this.CalendarConfiguration.Days = cg.GetDays(this.CalendarConfiguration);

                if (this.CalendarConfiguration.Days != null && this.CalendarConfiguration.Days.Count > 0)
                {
                    if (this.CalendarConfiguration.Months != null && this.CalendarConfiguration.Months.Count > 0)
                    {
                        this.MonthsQueue = new Queue<Month>(this.CalendarConfiguration.Months);
                    }

                    if (this.CalendarConfiguration.IsCreateFullCalendar)
                    {
                        var months = new List<Month> { Month.January, Month.February, Month.March, Month.April, Month.May, Month.June, Month.July, Month.August, Month.September, Month.October, Month.November, Month.December };
                        this.MonthsQueue = new Queue<Month>();

                        foreach (var month in months)
                        {
                            this.MonthsQueue.Enqueue(month);
                        }
                    }

                    this.AddDaysToCalendar(this.MonthsQueue.Dequeue());
                }
            }
        }

        private void AddDaysToCalendar(Month month)
        {
            this.inkCalendar.Children.Clear();

            this.MonthYearReference = this.GetMonthYearReference(month, this.CalendarConfiguration.ReferenceYear);
            this.CurrentMonth = month;

            var daysInMonth = this.CalendarConfiguration.Days.Where(d => d.Month == month).ToList();
            var currentWeek = 1;

            foreach (var day in daysInMonth)
            {
                var position = this.GetDayPosition(day, currentWeek);

                InkCanvas.SetLeft(day, position.X);
                InkCanvas.SetTop(day, position.Y);

                this.inkCalendar.Children.Add(day);
                this.LastDayOfWeek = day.DayOfWeek;

                if (this.LastDayOfWeek == DayOfWeek.Saturday)
                {
                    currentWeek++;
                }

                if (daysInMonth.IndexOf(day) == (daysInMonth.Count - 1))
                {
                    day.MoonImageLoaded += Day_MoonImageLoaded;
                }
            }

            this.gdCcalendar.UpdateLayout();
        }

        private void Day_MoonImageLoaded(object sender, RoutedEventArgs e)
        {
            this.SaveCalendar(this.CurrentMonth);
            this.AddDaysToCalendar(this.MonthsQueue.Dequeue());
        }
        
        private void Day_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void SaveCalendar(Month month)
        {
            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Calendar");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var filePath = Path.Combine(directory, $"{month}.png");

            var imageSize = new Size(this.gdCcalendar.ActualWidth, this.gdCcalendar.ActualHeight);

            VisualUtils.SaveVisualBitmap(this.gdCcalendar, imageSize, filePath);
        }

        private string GetMonthYearReference(Month month, int referenceYear)
        {
            return $"{DateTimeUtils.GetMonthDescription(month)} - {referenceYear}";
        }

        private Point GetDayPosition(DayControl day, int currentWeek)
        {
            var x = this.GetXPosition(day.DayOfWeek);
            var y = this.GetYPosition(day.DayOfWeek, currentWeek);

            return new Point(x, y);
        }

        private double GetYPosition(DayOfWeek dayOfWeek, int currentWeek)
        {
            if (currentWeek == 1)
            {
                return this.VerticalGap;
            }

            return (this.VerticalGap * currentWeek) + (this.DayControlSize.Height * (currentWeek - 1));
        }

        private double GetXPosition(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return this.HorizontalGap;

                case DayOfWeek.Monday:
                    return (this.HorizontalGap * 2) + this.DayControlSize.Width;

                case DayOfWeek.Tuesday:
                    return (this.HorizontalGap * 3) + (this.DayControlSize.Width * 2);

                case DayOfWeek.Wednesday:
                    return (this.HorizontalGap * 4) + (this.DayControlSize.Width * 3);

                case DayOfWeek.Thursday:
                    return (this.HorizontalGap * 5) + (this.DayControlSize.Width * 4);

                case DayOfWeek.Friday:
                    return (this.HorizontalGap * 6) + (this.DayControlSize.Width * 5);

                case DayOfWeek.Saturday:
                    return (this.HorizontalGap * 7) + (this.DayControlSize.Width * 6);

                default:
                    throw new NotSupportedException($"Day of Week {dayOfWeek} is not supported.");
            }
        }

        private void ExecuteAsync(Action action)
        {
            var th = new Thread(() =>
            {
                action.Invoke();
            });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private List<Month> GetMonths(string months)
        {
            if (!String.IsNullOrEmpty(months) && !this.IsCreateFullCalendar)
            {
                var monthsArray = months.Split(',');
                var monthsList = new List<Month>();

                if (monthsArray.Length > 0)
                {
                    foreach (var month in monthsArray)
                    {
                        monthsList.Add(DateTimeUtils.GetMonth(month));
                    }
                }

                return monthsList;
            }

            return null;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
