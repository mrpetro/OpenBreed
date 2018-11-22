using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Input;

namespace OpenBreed.Editor.VM
{
    public interface ITool
    {
        bool IsActive { get; set; }
        string Name { get; }

        void KeyDown(object sender, KeyEventArgs e);
        void KeyUp(object sender, KeyEventArgs e);
        void MouseDown(object sender, MouseEventArgs e);
        void MouseMove(object sender, MouseEventArgs e);
        void MouseUp(object sender, MouseEventArgs e);
        void MouseWheel(object sender, MouseEventArgs e);
    }
}
