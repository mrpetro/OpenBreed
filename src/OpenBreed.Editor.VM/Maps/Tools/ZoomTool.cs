using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Maps;
using System.Drawing;

namespace OpenBreed.Editor.VM.Maps.Tools
{
    public class ZoomTool : LevelTool
    {
        private readonly MapBodyEditorM _vm = null;

        public ZoomTool(MapBodyEditorM vm, IToolController controller) :
            base("ZoomTool", controller)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));

            _vm = vm;
        }

        public override void Activate()
        {
            Controller.MouseWheel += new MouseEventHandler(Controller_MouseWheel);
        }

        public override void Deactivate()
        {
            Controller.MouseWheel -= new MouseEventHandler(Controller_MouseWheel);
        }

        void Controller_MouseWheel(object sender, MouseEventArgs e)
        {
            IToolController view = (IToolController)sender;

            float currentScale = _vm.ZoomScale;
            float scaleFactor = 1.0f;

            if (Math.Sign(e.Delta) > 0)
                scaleFactor = 2.0f;
            else if (Math.Sign(e.Delta) < 0)
                scaleFactor = 0.5f;

            currentScale *= scaleFactor;

            if (currentScale < 0.125f)
            {
                //view.SetScale(0.125f);
                return;
            }
            else if (currentScale > 8.0f)
            {
                //view.SetScale(8.0f);
                return;
            }
            else
            {
                _vm.ZoomViewTo(e.Location, scaleFactor);
            }
        }
    }
}
