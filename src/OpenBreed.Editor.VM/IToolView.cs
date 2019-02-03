using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenBreed.Editor.VM
{
    public interface IToolView
    {
        event KeyEventHandler KeyDown;
        event KeyEventHandler KeyUp;
        event MouseEventHandler MouseDown;
        event MouseEventHandler MouseUp;
        event MouseEventHandler MouseMove;
        event MouseEventHandler MouseWheel;
        event EventHandler MouseEnter;
        event EventHandler MouseLeave;
        event PaintEventHandler Paint;

        Cursor Cursor { get; set; }

        void Invalidate();
    }
}
