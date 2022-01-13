using FasesDaLua.EventArgs;
using System;
using System.IO;
using System.Windows;

namespace FasesDaLua
{
    /// <summary>
    /// Interaction logic for MoonControl.xaml
    /// </summary>
    public partial class MoonController : Window
    {
        private string _calendarDestinationPath;

        public event EventHandler<StartMoonSpinEventArgs> StartMoonSpin;
        public event EventHandler<StopMoonSpinEventArgs> StopMoonSpin;
        public event EventHandler<TakeMoonPrintEventArgs> TakeMoonPrint;
        public event EventHandler<SunAzimuthUpdatedEventArgs> SunAzimuthUpdated;

        private string CalendarDestinationPath
        {
            get
            {
                if (String.IsNullOrEmpty(_calendarDestinationPath))
                {
                    _calendarDestinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Calendar");

                    if (!Directory.Exists(_calendarDestinationPath))
                    {
                        Directory.CreateDirectory(_calendarDestinationPath);
                    }
                }

                return _calendarDestinationPath;
            }
        }

        public MoonController(Window owner)
        {
            this.InitializeComponent();
            this.Owner = owner;

            this.Closing += MoonController_Closing;
        }

        private void MoonController_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner?.Close();
        }

        private void btnStartSpin_Click(object sender, RoutedEventArgs e)
        {
            this.InvokeStartMoonSpinEvent(sender);
        }

        private void InvokeStartMoonSpinEvent(object sender)
        {
            var spinConfiguration = MoonSpinPopup.GetSpinConfiguration(this);

            if (spinConfiguration != null)
            {
                this.StartMoonSpin?.Invoke(sender, new StartMoonSpinEventArgs(spinConfiguration.Speed, spinConfiguration.Position, spinConfiguration.ArroundPoint));
            }
        }

        public void MoonController_SunAzimuthUpdated(object sender, SunAzimuthUpdatedEventArgs e)
        {
            this.Dispatcher.Invoke(() => 
            {
                this.txtAzimuth.Text = e.Azimuth.ToString();
            });
        }

        private void btnStopSpin_Click(object sender, RoutedEventArgs e)
        {
            this.InvokeStopMoonSpinEvent(sender);
        }

        private void InvokeStopMoonSpinEvent(object sender)
        {
            this.StopMoonSpin?.Invoke(sender, new StopMoonSpinEventArgs());
        }

        private void btnStartPrints_Click(object sender, RoutedEventArgs e)
        {
            this.TakeMoonPrint?.Invoke(sender, new TakeMoonPrintEventArgs());
        }

        private void btnCreateCalendar_Click(object sender, RoutedEventArgs e)
        {
            var calendar = new Calendar(this);
            calendar.ShowDialog();
        }

        private void BtnClosePopup_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.optionsPopup.IsOpen)
            {
                this.optionsPopup.IsOpen = false;
            }
        }

        private void btnNextPhase_Click(object sender, RoutedEventArgs e)
        {
            this.SunAzimuthUpdated?.Invoke(this, new SunAzimuthUpdatedEventArgs(0));
        }
    }
}
