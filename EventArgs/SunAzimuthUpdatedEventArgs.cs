namespace FasesDaLua.EventArgs
{
    public class SunAzimuthUpdatedEventArgs : System.EventArgs
    {
        public double Azimuth { get; set; }

        public SunAzimuthUpdatedEventArgs(double azimuth)
        {
            this.Azimuth = azimuth;
        }
    }
}
