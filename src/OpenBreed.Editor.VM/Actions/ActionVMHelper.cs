using OpenBreed.Common.Actions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Actions
{
    public static class ActionVMHelper
    {
        public static void SetPresentationDefault(ActionVM prop,  Color color)
        {
            Bitmap bitmap = new Bitmap(32, 32, PixelFormat.Format32bppArgb);

            using (Graphics gfx = Graphics.FromImage(bitmap))
            {
                gfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

                Font font = new Font("Arial", 7);

                Rectangle rectangle = new Rectangle(0, 0, bitmap.Width - 1, bitmap.Height - 1);

                Color c = color;
                //Color c = Color.FromArgb(50, 0, 0, 0);// Color.Black;
                Pen tileColor = new Pen(c);
                Brush brush = new SolidBrush(c);

                gfx.FillRectangle(brush, rectangle);

                c = Color.White;
                tileColor = new Pen(c);
                brush = new SolidBrush(c);

                gfx.DrawRectangle(tileColor, rectangle);
                gfx.DrawString(string.Format("{0,2:D2}", prop.Id), font, brush, 1, 3);
            }

            prop.Icon = bitmap;
        }

        public static bool TryLoadImage(string imagePath, out Image image, out string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imagePath))
                {
                    image = null;
                    message = "Image path is null or empty.";
                    return false;
                }

                image = Image.FromFile(imagePath);
                message = null;
                return true;
            }
            catch (Exception ex)
            {
                image = null;
                message = $"Unable to load image from path {imagePath}. Reason: {ex.Message}";
            }

            return false;
        }

        public static void ToModel(ActionVM vm, IActionPresentation model)
        {
            model.Visibility = vm.Visibility;
            model.Color = ColorTranslator.ToHtml(vm.Color);
        }

        public static void FromModel(ActionVM vm, IActionPresentation model)
        {
            Image image;
            string message;

            vm.Visibility = model.Visibility;
            vm.Color = ColorTranslator.FromHtml(model.Color);

            if (!TryLoadImage(model.Image, out image, out message))
                SetPresentationDefault(vm, vm.Color);
            else
                vm.Icon = image;
        }
    }
}
