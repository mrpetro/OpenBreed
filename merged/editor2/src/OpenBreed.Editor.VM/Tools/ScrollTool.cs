using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Editor.VM.Maps;
using System.Drawing;
using OpenBreed.Common.Interface.Drawing;

namespace OpenBreed.Editor.VM.Tools
{
    public class ScrollTool : VMTool
    {
        #region Private Fields

        private readonly IScrollableVM _vm = null;

        private MyPoint _lastPos;

        #endregion Private Fields

        #region Public Constructors

        public ScrollTool(IScrollableVM vm, IToolView view) :
            base("ScrollTool", view)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));

            _vm = vm;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Activate()
        {
            View.KeyDownAction = View_KeyDown;
            View.KeyUpAction += View_KeyUp;
            View.MouseDownAction += View_MouseDown;
            View.MouseUpAction += View_MouseUp;
            View.MouseMoveAction += View_MouseMove;
        }

        public override void Deactivate()
        {
            View.KeyDownAction = null;
            View.KeyUpAction = null;
            View.MouseDownAction = null;
            View.MouseUpAction = null;
            View.MouseMoveAction = null;
        }

        #endregion Public Methods

        #region Private Methods

        void View_KeyDown()
        {
        }

        void View_KeyUp()
        {

        }

        void View_MouseDown(CursorButtons buttons, MyPoint location)
        {
            if (buttons.HasFlag(CursorButtons.Middle))
            {
                _lastPos = location;
                View.Cursor = "SizeAll";
            }
        }

        void View_MouseMove(CursorButtons buttons, MyPoint location)
        {
            if (buttons.HasFlag(CursorButtons.Middle))
            {
                int deltaX = location.X - _lastPos.X;
                int deltaY = location.Y - _lastPos.Y;
                _vm.ScrollViewBy(deltaX, deltaY);
                _lastPos = location;
                View.Invalidate();
            }
        }

        void View_MouseUp(CursorButtons buttons, MyPoint location)
        {
            View.Cursor = "Arrow";
        }

        #endregion Private Methods
    }
}
