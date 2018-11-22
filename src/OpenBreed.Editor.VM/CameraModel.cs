using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace OpenBreed.Editor.VM
{
    public class CameraModel
    {
        public double Zoom { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int Left {get; set;}
        public int Right { get; set; }
        public int Down { get; set; }
        public int Up { get; set; }

        public void MoveBy(int p, int p_2)
        {
            throw new NotImplementedException();
        }
    }
}
