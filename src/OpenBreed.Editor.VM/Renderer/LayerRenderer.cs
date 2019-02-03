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
    public class LayerRenderer : RendererBase<MapBodyBaseLayerVM>
    {
        public LayerRenderer(RenderTarget target) : base(target)
        {
        }

        public override void Render(MapBodyBaseLayerVM renderable)
        {
            if (renderable is MapBodyGfxLayerVM)
                Render((MapBodyGfxLayerVM)renderable);
            else if (renderable is MapBodyPropertyLayerVM)
                Render((MapBodyPropertyLayerVM)renderable);
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

        private void Render(MapBodyGfxLayerVM renderable)
        {
            var tileSets = renderable.Body.Owner.TileSets;
            RectangleF viewRect = Target.Gfx.ClipBounds;

            int tileSize = renderable.Body.Owner.TileSize;
            int xFrom = renderable.Body.GetMapIndexX(viewRect.Left);
            int xTo = renderable.Body.GetMapIndexX(viewRect.Right);
            int yFrom = renderable.Body.GetMapIndexY(viewRect.Top);
            int yTo = renderable.Body.GetMapIndexY(viewRect.Bottom);

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

        private void Render(MapBodyPropertyLayerVM renderable)
        {
            RectangleF viewRect = Target.Gfx.ClipBounds;

            int tileSize = renderable.Body.Owner.TileSize;
            int xFrom = renderable.Body.GetMapIndexX(viewRect.Left);
            int xTo = renderable.Body.GetMapIndexX(viewRect.Right);
            int yFrom = renderable.Body.GetMapIndexY(viewRect.Top);
            int yTo = renderable.Body.GetMapIndexY(viewRect.Bottom);

            var propSet = renderable.Body.PropSet;

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
