using OpenBreed.Model.Tiles;
using OpenBreed.Database.Interface.Items.Tiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Model.Actions;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Drawing;
using System.Drawing;

namespace OpenBreed.Common.Data
{
    public class ActionSetsDataHelper
    {
        public static ActionSetModel FromEmbeddedData(IDrawingFactory drawingFactory, IDrawingContextProvider drawingContextProvider, IImageProvider imageProvider, IModelsProvider provider, IDbActionSet entry)
        {
            var model = new ActionSetModel();

            foreach (var actionEntry in entry.Actions)
            {
                var newAction = new ActionModel();
                newAction.Name = actionEntry.Name;
                newAction.Id = actionEntry.Id;
                newAction.Visibility = actionEntry.Presentation.Visibility;
                newAction.Description = actionEntry.Description;
                FromEntry(drawingFactory, drawingContextProvider, imageProvider, newAction, actionEntry.Presentation);
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


        public static string ColorToHex(MyColor c)
        {
            return "#" + c.A.ToString("X2") + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static void FromEntry(IDrawingFactory drawingFactory, IDrawingContextProvider drawingContextProvider, IImageProvider imageProvider, ActionModel model, IDbActionPresentation presentationEntry)
        {
            IImage image;
            string message;

            model.Visibility = presentationEntry.Visibility;
            model.Color = HexToColor(presentationEntry.Color);

            if (!TryLoadImage(imageProvider, presentationEntry.Image, out image, out message))
                SetPresentationDefault(drawingFactory, drawingContextProvider, model, model.Color);
            else
                model.Icon = image;
        }

        public static MyColor HexToColor(string hex)
        {
            return MyColor.FromHex(hex);
        }

        public static void SetPresentationDefault(IDrawingFactory drawingFactory, IDrawingContextProvider drawingContextProvider, ActionModel model, MyColor color)
        {
            var bitmap = drawingFactory.CreateBitmap(32, 32, MyPixelFormat.Format32bppArgb);

            using (var gfx = drawingContextProvider.FromImage(bitmap))
            {
                //TODO:
                //gfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

                var font = drawingFactory.CreateFont("Arial", 7);

                var rectangle = new MyRectangle(0, 0, bitmap.Width - 1, bitmap.Height - 1);

                var c = color;
                //Color c = Color.FromArgb(50, 0, 0, 0);// Color.Black;
                var tileColor = drawingFactory.CreatePen(c);
                var brush = drawingFactory.CreateSolidBrush(c);

                gfx.FillRectangle(brush, rectangle);

                c = MyColor.White;
                tileColor = drawingFactory.CreatePen(c);
                brush = drawingFactory.CreateSolidBrush(c);

                gfx.DrawRectangle(tileColor, rectangle);
                gfx.DrawString(string.Format("{0,2:D2}", model.Id), font, brush, 1, 3);
            }

            model.Icon = bitmap;
        }

        public static void ToEntry(ActionModel actionModel, IDbAction actionEntry)
        {
            actionEntry.Id = actionModel.Id;
            actionEntry.Description = actionModel.Description;
            actionEntry.Name = actionModel.Name;
            actionEntry.Presentation.Visibility = actionModel.Visibility;
            actionEntry.Presentation.Color = ColorToHex(actionModel.Color);
        }

        public static bool TryLoadImage(IImageProvider imageProvider, string imagePath, out IImage image, out string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imagePath))
                {
                    image = null;
                    message = "Image path is null or empty.";
                    return false;
                }

                image = imageProvider.FromFile(imagePath);
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
