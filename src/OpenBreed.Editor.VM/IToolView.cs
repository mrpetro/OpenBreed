using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Editor.VM.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Editor.VM
{
    public interface IToolView
    {
        Action KeyDownAction { get; set; }
        Action KeyUpAction { get; set; }
        Action<CursorButtons, MyPoint> MouseDownAction { get; set; }
        Action<CursorButtons, MyPoint> MouseUpAction { get; set; }
        Action<CursorButtons, MyPoint> MouseMoveAction { get; set; }
        Action<int, MyPoint> MouseWheelAction { get; set; }
        Action MouseEnterAction { get; set; }
        Action MouseLeaveAction { get; set; }
        Action PaintAction { get; set; }

        string Cursor { get; set; }

        void Invalidate();
    }
}
