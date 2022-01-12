using FasesDaLua.Domain;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FasesDaLua.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Day.xaml
    /// </summary>
    public partial class DayControl : UserControl
    {
        public static DependencyProperty HeaderColorProperty = DependencyProperty.Register(nameof(HeaderColor),
                                                                                           typeof(Brush),
                                                                                           typeof(DayControl),
                                                                                           new FrameworkPropertyMetadata(Brushes.White));

        public static DependencyProperty TextColorProperty = DependencyProperty.Register(nameof(TextColor),
                                                                                         typeof(Brush),
                                                                                         typeof(DayControl),
                                                                                         new FrameworkPropertyMetadata(Brushes.Black));

        public static DependencyProperty DayProperty = DependencyProperty.Register(nameof(Day),
                                                                                   typeof(int),
                                                                                   typeof(DayControl),
                                                                                   new FrameworkPropertyMetadata(DateTime.Now.Day));

        public static DependencyProperty MonthProperty = DependencyProperty.Register(nameof(Month),
                                                                                     typeof(Month),
                                                                                     typeof(DayControl),
                                                                                     new FrameworkPropertyMetadata((Month)DateTime.Now.Month));

        public static DependencyProperty YearProperty = DependencyProperty.Register(nameof(Year),
                                                                                    typeof(int),
                                                                                    typeof(DayControl),
                                                                                    new FrameworkPropertyMetadata(DateTime.Now.Year));

        public Brush HeaderColor
        {
            get => this.GetValue<Brush>(HeaderColorProperty);
            set => this.SetValue(HeaderColorProperty, value);
        }
        public Brush TextColor
        {
            get => this.GetValue<Brush>(TextColorProperty);
            set => this.SetValue(TextColorProperty, value);
        }
        public int Day
        {
            get => this.GetValue<int>(DayProperty);
            set => this.SetValue(DayProperty, value);
        }
        public Month Month
        {
            get => this.GetValue<Month>(MonthProperty);
            set => this.SetValue(MonthProperty, value);
        }
        public int Year
        {
            get => this.GetValue<int>(YearProperty);
            set => this.SetValue(YearProperty, value);
        }
        public DateTime Date { get => new DateTime(this.Year, (int)this.Month, this.Day); }
        public DayOfWeek DayOfWeek { get => this.Date.DayOfWeek; }
        public int DayOfYear { get => this.Date.DayOfYear; }
        public MoonPhase Moon { get; set; }

        public DayControl(int year, int month, int day, MoonPhase moon)
        {
            this.InitializeComponent();

            this.Year = year;
            this.Month = (Month)month;
            this.Day = day;
            this.Moon = moon;

            this.txtDayOfWeek.Text = this.GetDayOfWeekDescription(this.DayOfWeek);
            this.txtMoonPhase.Text = this.GetMoonPhaseDescription(this.Moon);

            this.Loaded += this.DayControl_Loaded;
        }

        private string GetMoonPhaseDescription(MoonPhase moon)
        {
            switch (moon)
            {
                case MoonPhase.Unknown:
                case MoonPhase.Moon_02:
                case MoonPhase.Moon_03:
                case MoonPhase.Moon_05:
                case MoonPhase.Moon_06:
                case MoonPhase.Moon_07:
                case MoonPhase.Moon_08:
                case MoonPhase.Moon_09:
                case MoonPhase.Moon_10:
                case MoonPhase.Moon_11:
                case MoonPhase.Moon_12:
                case MoonPhase.Moon_13:
                case MoonPhase.Moon_14:
                case MoonPhase.Moon_16:
                case MoonPhase.Moon_17:
                case MoonPhase.Moon_18:
                case MoonPhase.Moon_19:
                case MoonPhase.Moon_20:
                case MoonPhase.Moon_21:
                case MoonPhase.Moon_22:
                case MoonPhase.Moon_23:
                case MoonPhase.Moon_24:
                case MoonPhase.Moon_25:
                case MoonPhase.Moon_26:
                case MoonPhase.Moon_28:
                case MoonPhase.Moon_29:
                default:
                    return "";

                case MoonPhase.Moon_01:
                    return "Nova";

                case MoonPhase.Moon_04:
                    return "Crescente";

                case MoonPhase.Moon_15:
                    return "Cheia";

                case MoonPhase.Moon_27:
                    return "Minguante";
            }
        }

        private string GetDayOfWeekDescription(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "Domingo";

                case DayOfWeek.Monday:
                    return "Segunda-Feira";

                case DayOfWeek.Tuesday:
                    return "Terça-Feira";

                case DayOfWeek.Wednesday:
                    return "Quarta-Feira";

                case DayOfWeek.Thursday:
                    return "Quinta-Feira";

                case DayOfWeek.Friday:
                    return "Sexta-Feira";

                case DayOfWeek.Saturday:
                    return "Sábado";

                default:
                    throw new NotSupportedException($"Day of Week {dayOfWeek} is not supported.");
            }
        }

        private void DayControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.GetMoonImage(this.Moon);
        }

        private void GetMoonImage(MoonPhase moon)
        {
            if (moon != MoonPhase.Unknown)
            {
                this.imgMoonPhase.Source = this.GetMoonImageSource(moon);
            }
            else
            {
                throw new ArgumentException("Moon phase not recognized.");
            }
        }

        private BitmapImage GetMoonImageSource(MoonPhase moon)
        {
            var fileName = this.GetMoonImageFileName(moon);
            var filePath = $"FasesDaLua.Images.MoonPhases.{fileName}";

            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(filePath))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();

                return bitmap;
            }
        }

        private string GetMoonImageFileName(MoonPhase moon)
        {
            var numeral = (int)moon;

            if (numeral < 10)
            {
                return $"0{numeral}.png";
            }

            return $"{numeral}.png";
        }
    }
}
