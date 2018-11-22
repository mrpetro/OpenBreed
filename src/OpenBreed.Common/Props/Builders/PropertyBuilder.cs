using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using OpenBreed.Common.Logging;

namespace OpenBreed.Common.Props.Builders
{
    public class PropertyBuilder
    {
        internal int Id;
        internal bool Visibility;
        internal Image Presentation;
        internal string Description;
        internal string Name;

        public PropertyBuilder SetId(int id)
        {
            Id = id;
            return this;
        }

        public PropertyBuilder SetVisibility(bool visibility)
        {
            Visibility = visibility;
            return this;
        }

        public PropertyBuilder SetName(string name)
        {
            Name = name;
            return this;
        }

        public PropertyBuilder SetDescription(string description)
        {
            Description = description;
            return this;
        }

        public PropertyBuilder SetPresentationDefault(Color color)
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
                gfx.DrawString(string.Format("{0,2:D2}", Id), font, brush, 1, 3);
            }

            Presentation = bitmap;

            return this;
        }

        public Image TryLoadImage(string imagePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imagePath))
                    return null;

                return Image.FromFile(imagePath);
            }
            catch (Exception ex)
            {
                LogMan.Instance.LogError(string.Format("Unable to load image from path {0}. Reason: {1}", imagePath, ex.Message));
            }

            return null;
        }

        public PropertyBuilder SetPresentation(string imagePath, Color color)
        {
            var image = TryLoadImage(imagePath);

            if (image == null)
            {
                SetPresentationDefault(color);
                return this;
            }

            Presentation = image;
            return this;
        }

        public static PropertyBuilder NewProperty()
        {
            return new PropertyBuilder();
        }

        public PropertyModel Build()
        {
            return new PropertyModel(this);
        }
    }
}
