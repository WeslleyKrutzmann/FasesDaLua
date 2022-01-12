using System.Windows;
using System.Windows.Media.Media3D;

namespace FasesDaLua.EventArgs
{
    public class StartMoonSpinEventArgs : System.EventArgs
    {
        public Vector Speed { get; set; }
        public Point Position { get; set; }
        public Point3D ArroundPoint { get; set; }

        public StartMoonSpinEventArgs(Vector speed, Point position, Point3D arroundPoint)
        {
            this.Speed = speed;
            this.Position = position;
            this.ArroundPoint = arroundPoint;
        }
    }
}
