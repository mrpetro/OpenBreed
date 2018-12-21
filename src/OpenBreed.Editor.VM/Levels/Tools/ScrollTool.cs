using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Levels;
using System.Drawing;

namespace OpenBreed.Editor.VM.Levels.Tools
{
    public class ScrollTool : LevelTool
    {
        private readonly LevelBodyEditorVM _vm = null;

        private Point m_LastPos;

        public ScrollTool(LevelBodyEditorVM vm, IToolController controller) :
            base("ScrollTool", controller)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));

            _vm = vm;
        }

        public override void Activate()
        {
            Controller.KeyDown += new KeyEventHandler(Controller_KeyDown);
            Controller.KeyUp += new KeyEventHandler(Controller_KeyUp);
            Controller.MouseDown += new MouseEventHandler(Controller_MouseDown);
            Controller.MouseUp += new MouseEventHandler(Controller_MouseUp);
            Controller.MouseMove += new MouseEventHandler(Controller_MouseMove);
        }

        public override void Deactivate()
        {
            Controller.KeyDown -= new KeyEventHandler(Controller_KeyDown);
            Controller.KeyUp -= new KeyEventHandler(Controller_KeyUp);
            Controller.MouseDown -= new MouseEventHandler(Controller_MouseDown);
            Controller.MouseUp -= new MouseEventHandler(Controller_MouseUp);
            Controller.MouseMove -= new MouseEventHandler(Controller_MouseMove);
        }

        void Controller_MouseMove(object sender, MouseEventArgs e)
        {
            IToolController view = (IToolController)sender;

            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                int deltaX = e.Location.X - m_LastPos.X;
                int deltaY = e.Location.Y - m_LastPos.Y;
                _vm.ScrollViewBy(deltaX, deltaY);
                m_LastPos = e.Location;
                view.Invalidate();
            }
        }

        void Controller_MouseDown(object sender, MouseEventArgs e)
        {
            IToolController view = (IToolController)sender;

            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                m_LastPos = e.Location;
                view.Cursor = Cursors.SizeAll;
            }
        }

        void Controller_MouseUp(object sender, MouseEventArgs e)
        {
            IToolController view = (IToolController)sender;

            view.Cursor = Cursors.Arrow;
        }

        void Controller_KeyUp(object sender, KeyEventArgs e)
        {
            IToolController view = (IToolController)sender;
        }

        void Controller_KeyDown(object sender, KeyEventArgs e)
        {
            IToolController view = (IToolController)sender;
        }

    }
}
