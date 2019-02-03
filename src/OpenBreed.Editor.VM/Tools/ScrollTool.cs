using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Maps;
using System.Drawing;

namespace OpenBreed.Editor.VM.Tools
{
    public class ScrollTool : VMTool
    {
        #region Private Fields

        private readonly IScrollableVM _vm = null;

        private Point _lastPos;

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
            View.KeyDown += View_KeyDown;
            View.KeyUp += View_KeyUp;
            View.MouseDown += View_MouseDown;
            View.MouseUp += View_MouseUp;
            View.MouseMove += View_MouseMove;
        }

        public override void Deactivate()
        {
            View.KeyDown -= View_KeyDown;
            View.KeyUp -= View_KeyUp;
            View.MouseDown -= View_MouseDown;
            View.MouseUp -= View_MouseUp;
            View.MouseMove -= View_MouseMove;
        }

        #endregion Public Methods

        #region Private Methods

        void View_KeyDown(object sender, KeyEventArgs e)
        {
            IToolView view = (IToolView)sender;
        }

        void View_KeyUp(object sender, KeyEventArgs e)
        {
            IToolView view = (IToolView)sender;
        }

        void View_MouseDown(object sender, MouseEventArgs e)
        {
            IToolView view = (IToolView)sender;

            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                _lastPos = e.Location;
                view.Cursor = Cursors.SizeAll;
            }
        }

        void View_MouseMove(object sender, MouseEventArgs e)
        {
            IToolView view = (IToolView)sender;

            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                int deltaX = e.Location.X - _lastPos.X;
                int deltaY = e.Location.Y - _lastPos.Y;
                _vm.ScrollViewBy(deltaX, deltaY);
                _lastPos = e.Location;
                view.Invalidate();
            }
        }
        void View_MouseUp(object sender, MouseEventArgs e)
        {
            IToolView view = (IToolView)sender;

            view.Cursor = Cursors.Arrow;
        }

        #endregion Private Methods
    }
}
