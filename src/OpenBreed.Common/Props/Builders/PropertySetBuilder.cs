using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using OpenBreed.Common.Drawing;

namespace OpenBreed.Common.Props.Builders
{
    public class PropertySetBuilder
    {
        internal string Name;
        internal List<PropertyModel> Properties;

        public PropertySetBuilder()
        {
            Properties = new List<PropertyModel>();
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public PropertySetBuilder AddProperty(PropertyModel property)
        {
            Properties.Add(property);
            return this;
        }

        public static PropertySetBuilder NewPropertySet()
        {
            return new PropertySetBuilder();
        }

        public PropertySetBuilder SetDefaultProperties()
        {
            Properties.Clear();

            Bitmap bmp = GetDefaultBitmap();

            int tilesNoX = bmp.Width / 16;
            int tilesNoY = bmp.Height / 16;

            for (int tileIndex = 0; tileIndex < tilesNoX * tilesNoY; tileIndex++)
            {

                int tileIndexX = tileIndex % tilesNoX;
                int tileIndexY = tileIndex / tilesNoX;

                var rect = new Rectangle(tileIndexX * 16, tileIndexY * 16, 16, 16);
                byte[] data = BitmapHelper.ToBytes(bmp, rect);

                //Start building new tile
                var tileBuilder = PropertyBuilder.NewProperty();
                tileBuilder.SetId(tileIndex);
                //tileBuilder.SetData(data);
                Properties.Add(tileBuilder.Build());
            }

            return this;
        }

        public PropertySetModel Build()
        {
            return new PropertySetModel(this);
        }

        private Bitmap GetDefaultBitmap()
        {
            Bitmap tilesBitmap = new Bitmap(128, 128, PixelFormat.Format32bppArgb);

            int tileSize = 16;
            int tilesNoX = tilesBitmap.Width / tileSize;
            int tilesNoY = tilesBitmap.Height / tileSize;

            using (Graphics gfx = Graphics.FromImage(tilesBitmap))
            {
                Font font = new Font("Arial", 7);

                for (int j = 0; j < tilesNoY; j++)
                {
                    for (int i = 0; i < tilesNoX; i++)
                    {
                        int tileId = i + j * tilesNoX;

                        Rectangle rectangle = new Rectangle(i * tileSize, j * tileSize, tileSize - 1, tileSize - 1);

                        Color c = Color.Black;
                        Pen tileColor = new Pen(c);
                        Brush brush = new SolidBrush(c);

                        gfx.FillRectangle(brush, rectangle);

                        c = Color.White;
                        tileColor = new Pen(c);
                        brush = new SolidBrush(c);

                        gfx.DrawRectangle(tileColor, rectangle);
                        gfx.DrawString(string.Format("{0,2:D2}", tileId), font, brush, i * tileSize + 1, j * tileSize + 3);
                        //gfx.DrawString(string.Format("{0,2:D2}", tileId % 100), font, brush, i * tileSize + 2, j * tileSize + 7);
                    }
                }
            }

            return tilesBitmap;
        }
    }
}
