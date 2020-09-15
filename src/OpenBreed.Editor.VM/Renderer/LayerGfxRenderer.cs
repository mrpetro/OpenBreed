using OpenBreed.Model.Maps;
using OpenBreed.Editor.VM.Maps.Layers;
using OpenBreed.Editor.VM.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Renderer
{
    public class LayerGfxRenderer : RendererBase<MapLayerBaseVM>
    {

        #region Public Constructors

        public LayerGfxRenderer(RenderTarget target) : base(target)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Render(MapLayerBaseVM renderable)
        {
            Render((MapLayerGfxVM)renderable);
        }

        public void RenderDefaultTile(TileRef tileRef, float x, float y, int tileSize)
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

        public void RenderTile(TileSetVM tileSet, int tileId, float x, float y, int tileSize)
        {
            if (tileId >= tileSet.Items.Count)
                return;

            var tileRect = tileSet.Items[tileId].Rectangle;
            Target.Gfx.DrawImage(tileSet.Bitmap, (int)x, (int)y, tileRect, GraphicsUnit.Pixel);
        }

        #endregion Public Methods

        #region Private Methods

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

        #endregion Private Methods
    }
}
