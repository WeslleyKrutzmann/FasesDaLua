using System.Windows;
using System.Windows.Media.Media3D;

namespace FasesDaLua.Domain.Entities
{
    public class MoonSpinConfiguration
    {
        public Vector Speed { get; set; }
        public Point Position { get; set; }
        public Point3D ArroundPoint { get; set; }

        public MoonSpinConfiguration()
        {

        }
    }
}
