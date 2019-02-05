using OpenBreed.Common.Maps;
using OpenBreed.Editor.VM.Maps.Layers;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Renderer
{
    public class LayerRenderer : RendererBase<MapLayerBaseVM>
    {
        public LayerRenderer(RenderTarget target) : base(target)
        {
        }

        public override void Render(MapLayerBaseVM renderable)
        {
            if (renderable is MapLayerGfxVM)
                Render((MapLayerGfxVM)renderable);
            else if (renderable is MapLayerActionVM)
                Render((MapLayerActionVM)renderable);
        }

        private void RenderTile(TileSetVM tileSet , int tileId, float x, float y, int tileSize)
        {
            if (tileId >= tileSet.Items.Count)
                return;

            var tileRect = tileSet.Items[tileId].Rectangle;
            Target.Gfx.DrawImage(tileSet.Bitmap, (int)x, (int)y, tileRect, GraphicsUnit.Pixel);
        }

        private void RenderDefaultTile(TileRef tileRef, float x, float y, int tileSize)
        {
            Font font = new Font("Arial", 5);

            var rectangle = new Rectangle((int)x, (int)y, tileSize, tileSize);

            Color c = Color.Black;
            Pen tileColor = new Pen(c);
            Brush brush = new SolidBrush(c);

            Target.Gfx.FillRectangle(brush, rectangle);

            c = Color.White;
            tileColor = new Pen(c);
            brush = new SolidBrush(c);

            Target.Gfx.DrawRectangle(tileColor, rectangle);
            Target.Gfx.DrawString(string.Format("{0,2:D2}", tileRef.TileId / 100), font, brush, x + 2, y + 1);
            Target.Gfx.DrawString(string.Format("{0,2:D2}", tileRef.TileId % 100), font, brush, x + 2, y + 7);
        }

        private void Render(MapLayerGfxVM renderable)
        {
            var tileSets = renderable.Layout.Owner.TileSets;
            RectangleF viewRect = Target.Gfx.ClipBounds;

            int tileSize = renderable.Layout.Owner.TileSize;
            int xFrom = renderable.Layout.GetMapIndexX(viewRect.Left);
            int xTo = renderable.Layout.GetMapIndexX(viewRect.Right);
            int yFrom = renderable.Layout.GetMapIndexY(viewRect.Top);
            int yTo = renderable.Layout.GetMapIndexY(viewRect.Bottom);

            for (int xIndex = xFrom; xIndex <= xTo; xIndex++)
            {
                for (int yIndex = yFrom; yIndex <= yTo; yIndex++)
                {
                    var tileRef = renderable.GetCell(xIndex, yIndex);
                    var x = xIndex * tileSize;
                    var y = yIndex * tileSize;

                    if (tileRef.TileSetId < tileSets.Count)
                        RenderTile(tileSets[tileRef.TileSetId], tileRef.TileId, x, y, tileSize);
                    else
                        RenderDefaultTile(tileRef, x, y, tileSize);
                }
            }
        }

        private void DrawProperty(PropSetVM propSet, int id, float x, float y, int tileSize)
        {
            if (id >= propSet.Items.Count)
                return;

            var propertyData = propSet.Items[id];

            if (!propertyData.Visibility)
                return;

            var image = propertyData.Presentation;

            var opqPen = new Pen(Color.FromArgb(128, 255, 255, 255), 10);
            var otranspen = new Pen(Color.FromArgb(128, 255, 255, 255), 10);
            var ototTransPen = new Pen(Color.FromArgb(40, 0, 255, 0), 10);

            //ColorMatrix cm = new ColorMatrix();
            //cm.Matrix33 = 0.55f;
            //ImageAttributes ia = new ImageAttributes();
            //ia.SetColorMatrix(cm);
            //gfx.DrawImage(image, new Rectangle((int)x, (int)y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, ia);

            Target.Gfx.DrawImage(image, x, y, tileSize, tileSize);

        }

        private void Render(MapLayerActionVM renderable)
        {
            RectangleF viewRect = Target.Gfx.ClipBounds;

            int tileSize = renderable.Layout.Owner.TileSize;
            int xFrom = renderable.Layout.GetMapIndexX(viewRect.Left);
            int xTo = renderable.Layout.GetMapIndexX(viewRect.Right);
            int yFrom = renderable.Layout.GetMapIndexY(viewRect.Top);
            int yTo = renderable.Layout.GetMapIndexY(viewRect.Bottom);

            var propSet = renderable.Layout.Owner.PropSet;

            if (propSet == null)
                return;

            for (int xIndex = xFrom; xIndex <= xTo; xIndex++)
            {
                for (int yIndex = yFrom; yIndex <= yTo; yIndex++)
                {
                    var propertyId = renderable.GetCell(xIndex, yIndex);
                    var x = xIndex * tileSize;
                    var y = yIndex * tileSize;

                    DrawProperty(propSet, propertyId, x, y, tileSize);
                }
            }
        }
    }
}
