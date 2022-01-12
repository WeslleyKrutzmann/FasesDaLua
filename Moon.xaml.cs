using FasesDaLua.EventArgs;
using System;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Xps.Packaging;

namespace FasesDaLua
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Moon : Window
    {
        private const double MOON_ROTATION_DAYS = 29.53;
        private const double COMPLETE_ROTATION_AZIMUTH = 365;
        private const double MILISSECONDS_IN_ONE_SECOND = 1000;

        private Timer TimerMoonPrint { get; set; }
        private MoonController MoonController { get; set; }
        private double PrintInterval => this.GetPrintInterval();
        private FixedDocument Document { get; set; }
        private double MoonStageCount { get; set; } = 15;
        private Size ImageSize { get; set; } = new Size(1920, 1080);
        private double CurrentAzimuth { get; set; }
        private double AzimuthInOneDay { get => COMPLETE_ROTATION_AZIMUTH / MOON_ROTATION_DAYS; }

        public Moon()
        {
            this.InitializeComponent();

            this.moonSphere.Material = this.GetMoonMaterial(false);
            this.Loaded += this.Moon_Loaded;
        }

        private void Moon_Loaded(object sender, RoutedEventArgs e)
        {
            this.moonView.CameraController.InfiniteSpin = true;

            this.MoonController = new MoonController(this);

            this.MoonController.StartMoonSpin += this.MoonController_StartMoonSpin;
            this.MoonController.StopMoonSpin += this.MoonController_StopMoonSpin;
            this.MoonController.TakeMoonPrint += this.MoonController_TakeMoonPrint;
            this.MoonController.SunAzimuthUpdated += MoonController_SunAzimuthUpdated;

            this.MoonController.Show();
        }

        private void MoonController_SunAzimuthUpdated(object sender, SunAzimuthUpdatedEventArgs e)
        {
            this.AlterSunAzimuth(this.AzimuthInOneDay);
        }

        private void MoonController_TakeMoonPrint(object sender, TakeMoonPrintEventArgs e)
        {
            this.PrintMoonState();
        }

        private void CreateMoonPrint()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Cursor = Cursors.Wait;
                this.MoonStageCount += 1;
                this.CreateNextMoonStage(this.AzimuthInOneDay);
                this.Cursor = Cursors.Arrow;
            });
        }

        private void CreateNextMoonStage(double azimuthInOneDay)
        {
            this.CreateNewMoonStage(this.gdRoot);
        }

        private void MoonController_StopMoonSpin(object sender, StopMoonSpinEventArgs e)
        {
            this.IsMoonSpinStarted = false;
            this.TimerAlterAzimuth.Stop();
        }

        private void MoonController_StartMoonSpin(object sender, StartMoonSpinEventArgs e)
        {
            this.StartMoonSpin(e.Speed, e.Position, e.ArroundPoint);
        }

        private void StartMoonSpin(Vector speed, Point position, Point3D arroundPoint)
        {
            this.TimerAlterAzimuth = new Timer();
            this.TimerAlterAzimuth.Interval = 100;
            this.TimerAlterAzimuth.Elapsed += TimerAlterAzimuth_Elapsed;
            this.TimerAlterAzimuth.Start();

            this.sun.Azimuth = 100;
        }

        private void TimerAlterAzimuth_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.AlterSunAzimuth(this.AzimuthInOneDay);
        }

        private void AlterSunAzimuth(double azimuth)
        {
            this.Dispatcher.InvokeAsync(() =>
            {
                this.sun.Azimuth -= azimuth;
            });
        }

        private void PrintMoonState()
        {
            this.CreateMoonPrint();
        }

        private void CreateNewMoonStage(Grid element)
        {
            this.CreateMoonStageImage(element, this.ImageSize, this.MoonStageCount);
        }

        private Material GetMoonMaterial(bool isEmissive)
        {
            var globe_brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/moon.jpg")));

            var material = new DiffuseMaterial(globe_brush);
            var ambientColor = Colors.DarkGray;
            ambientColor.A = (byte)Convert.ToInt32("BF", 16);

            material.AmbientColor = ambientColor;

            return material;
        }

        private double GetPrintInterval()
        {
            var seconds = COMPLETE_ROTATION_AZIMUTH / MOON_ROTATION_DAYS;
            var roundedSeconds = Math.Round(seconds, 3);

            return roundedSeconds * MILISSECONDS_IN_ONE_SECOND;
        }

        private string DestinationFolder
        {
            get => this.GetDestinationFolder();
        }
        public bool IsMoonSpinStarted { get; private set; }
        public Timer TimerAlterAzimuth { get; private set; }

        public void CreateMoonStageImage(Visual visual, Size imageSize, double currentStage)
        {
            var bmp = this.GetMoonStageBitmap(visual, imageSize);

            var pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(bmp));

            var fileName = $"{this.MoonStageCount}.png";
            var filePath = Path.Combine(this.GetDestinationFolder(), fileName);

            using (var fs = File.OpenWrite(filePath))
            {
                pngEncoder.Save(fs);
            }
        }

        private RenderTargetBitmap GetMoonStageBitmap(Visual visual, Size imageSize)
        {
            return this.GetVisualBitmap(visual, imageSize);
        }

        private RenderTargetBitmap GetVisualBitmap(Visual visual, Size imageSize)
        {
            var bmp = new RenderTargetBitmap((int)imageSize.Width, (int)imageSize.Height, 96, 96, PixelFormats.Default);
            bmp.Render(visual);

            bmp.Freeze();

            return bmp;
        }

        private string GetDestinationFolder()
        {
            var baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Moon Stages");

            if (!Directory.Exists(baseDirectory))
            {
                Directory.CreateDirectory(baseDirectory);
            }

            return baseDirectory;
        }

        private void SaveDocument(FixedDocument document, string filePath)
        {
            this.SaveXpsDocument(document, filePath);
        }

        private void SaveXpsDocument(FixedDocument fixedDocument, string filePath)
        {
            using (var xpsd = new XpsDocument(filePath, FileAccess.ReadWrite))
            {
                var xw = XpsDocument.CreateXpsDocumentWriter(xpsd);
                xw.Write(fixedDocument);
                xpsd.Close();
            }
        }
    }
}
