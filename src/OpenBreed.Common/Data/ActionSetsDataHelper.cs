using OpenBreed.Model.Tiles;
using OpenBreed.Database.Interface.Items.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Model.Actions;
using OpenBreed.Database.Interface.Items.Actions;
using System.Drawing.Imaging;

namespace OpenBreed.Common.Data
{
    public class ActionSetsDataHelper
    {
        public static ActionSetModel FromEmbeddedData(DataProvider provider, IActionSetEntry entry)
        {
            var model = new ActionSetModel();

            foreach (var actionEntry in entry.Actions)
            {
                var newAction = new ActionModel();
                newAction.Name = actionEntry.Name;
                newAction.Id = actionEntry.Id;
                newAction.Visibility = actionEntry.Presentation.Visibility;
                newAction.Description = actionEntry.Description;
                FromEntry(newAction, actionEntry.Presentation);
                model.Items.Add(newAction);
            }


            //Items.UpdateAfter(() =>
            //{
            //    Items.Clear();

            //    foreach (var item in entry.Actions)
            //    {
            //        var newAction = NewItem();
            //        newAction.FromModel(item);
            //        Items.Add(newAction);
            //    }
            //});

            return model;
        }


        public static string ColorToHex(Color c)
        {
            return "#" + c.A.ToString("X2") + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static void FromEntry(ActionModel model, IActionPresentation presentationEntry)
        {
            Image image;
            string message;

            model.Visibility = presentationEntry.Visibility;
            model.Color = HexToColor(presentationEntry.Color);

            if (!TryLoadImage(presentationEntry.Image, out image, out message))
                SetPresentationDefault(model, model.Color);
            else
                model.Icon = image;
        }

        public static Color HexToColor(string hex)
        {
            return ColorTranslator.FromHtml(hex);
        }

        public static void SetPresentationDefault(ActionModel model, Color color)
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
                gfx.DrawString(string.Format("{0,2:D2}", model.Id), font, brush, 1, 3);
            }

            model.Icon = bitmap;
        }

        public static void ToEntry(ActionModel actionModel, IActionEntry actionEntry)
        {
            actionEntry.Id = actionModel.Id;
            actionEntry.Description = actionModel.Description;
            actionEntry.Name = actionModel.Name;
            actionEntry.Presentation.Visibility = actionModel.Visibility;
            actionEntry.Presentation.Color = ColorToHex(actionModel.Color);
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
    }
}
