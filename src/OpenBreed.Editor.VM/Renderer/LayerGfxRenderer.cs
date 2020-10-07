using OpenBreed.Model.Maps;
using OpenBreed.Editor.VM.Maps.Layers;
using OpenBreed.Editor.VM.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Model.Tiles;

namespace OpenBreed.Editor.VM.Renderer
{
    public class LayerGfxRenderer : RendererBase<MapLayerBaseVM>
    {

        #region Public Constructors

        private MapEditorTilesToolVM tilesTool;

        public LayerGfxRenderer(MapEditorTilesToolVM tilesTool, RenderTarget target) : base(target)
        {
            this.tilesTool = tilesTool;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Render(MapLayerBaseVM renderable)
        {
            Render((MapLayerGfxVM)renderable);
        }

        #endregion Public Methods

        #region Private Methods

        private void Render(MapLayerGfxVM renderable)
        {
            RectangleF viewRect = Target.ClipBounds;

            int tileSize = renderable.Layout.Parent.TileSize;
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

                    tilesTool.Parent.DrawTile(Target, tileRef.TileId, x, y, tileSize);
                }
            }
        }

        #endregion Private Methods
    }
}
