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

        public event EventHandler<RoutedEventArgs> MoonImageLoaded;

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
                case MoonPhase.NewMoon:
                    return "Nova";

                case MoonPhase.CrescentMoon:
                    return "Crescente";

                case MoonPhase.FullMoon:
                    return "Cheia";

                case MoonPhase.WaningMoon:
                    return "Minguante";

                default:
                    return "";
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
            if (moon != MoonPhase.None)
            {
                this.imgMoonPhase.Source = this.GetMoonImageSource(moon);
            }

            this.MoonImageLoaded?.Invoke(this, new RoutedEventArgs());
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
