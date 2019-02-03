using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Levels;
using System.Drawing;

namespace OpenBreed.Editor.VM.Tools
{
    public class ZoomTool : VMTool
    {
        private readonly IZoomableVM _vm = null;

        public ZoomTool(IZoomableVM vm, IToolView view) :
            base("ZoomTool", view)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));

            _vm = vm;
        }

        public override void Activate()
        {
            View.MouseWheel += View_MouseWheel;
        }

        public override void Deactivate()
        {
            View.MouseWheel -= View_MouseWheel;
        }

        void View_MouseWheel(object sender, MouseEventArgs e)
        {
            IToolView view = (IToolView)sender;

            float currentScale = _vm.ZoomScale;
            float scaleFactor = 1.0f;

            if (Math.Sign(e.Delta) > 0)
                scaleFactor = 2.0f;
            else if (Math.Sign(e.Delta) < 0)
                scaleFactor = 0.5f;

            currentScale *= scaleFactor;

            if (currentScale < 0.125f)
                currentScale = 0.125f;
            else if (currentScale > 8.0f)
                currentScale = 8.0f;

            _vm.ZoomViewTo(e.Location, currentScale);
        }
    }
}
