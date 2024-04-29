using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Renderer;
using OpenBreed.Common.Interface.Drawing;

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
            View.MouseWheelAction += View_MouseWheel;
        }

        public override void Deactivate()
        {
            View.MouseWheelAction = null;
        }

        void View_MouseWheel(int delta, MyPoint location)
        {
            float currentScale = _vm.ZoomScale;
            float scaleFactor = 1.0f;

            if (Math.Sign(delta) > 0)
                scaleFactor = 2.0f;
            else if (Math.Sign(delta) < 0)
                scaleFactor = 0.5f;

            currentScale *= scaleFactor;

            if (currentScale < 0.125f)
                currentScale = 0.125f;
            else if (currentScale > 8.0f)
                currentScale = 8.0f;

            _vm.ZoomViewTo(new MyPointF(location.X, location.Y), currentScale);
        }
    }
}
