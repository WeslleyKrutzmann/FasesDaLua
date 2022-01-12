using FasesDaLua.Domain.Entities;
using FasesDaLua.Views;
using System;
using System.Windows;
using System.Windows.Media.Media3D;

namespace FasesDaLua
{
    /// <summary>
    /// Interaction logic for Popup.xaml
    /// </summary>
    public partial class MoonSpinPopup : WindowBase
    {
        private double _xSpeed = 0;
        private double _ySpeed = 0;
        private double _xPosition = 0;
        private double _yPosition = 0;

        public MoonSpinConfiguration SpinConfiguration { get; set; }
        public double XSpeed
        {
            get => this._xSpeed;
            set
            {
                this._xSpeed = value;
                this.NotifyPropertyChanged(nameof(this.XSpeed));
            }
        }
        public double YSpeed
        {
            get => this._ySpeed;
            set
            {
                this._ySpeed = value;
                this.NotifyPropertyChanged(nameof(this.YSpeed));
            }
        }
        public double XPosition
        {
            get => this._xPosition;
            set
            {
                this._xPosition = value;
                this.NotifyPropertyChanged(nameof(this.XPosition));
            }
        }
        public double YPosition
        {
            get => this._yPosition;
            set
            {
                this._yPosition = value;
                this.NotifyPropertyChanged(nameof(this.YPosition));
            }
        }

        public MoonSpinPopup(Window owner) : base()
        {
            this.InitializeComponent();
            this.Owner = owner;
            this.DataContext = this;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            this.ConfirmMoonSpinConfiguration();
        }

        private void ConfirmMoonSpinConfiguration()
        {
            this.SpinConfiguration = new MoonSpinConfiguration
            {
                Speed = new Vector(this.XSpeed, this.YSpeed),
                Position = new Point(this.XPosition, this.YPosition),
                ArroundPoint = new Point3D(0, 0, 0)
            };

            this.DialogResult = true;
            this.Close();
        }
        
        private void BtnClosePopup_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        public static MoonSpinConfiguration GetSpinConfiguration(Window owner)
        {
            var popup = new MoonSpinPopup(owner);
            popup.ShowDialog();

            if (popup.DialogResult == true)
            {
                return popup.SpinConfiguration;
            }

            return null;
        }

    }
}
