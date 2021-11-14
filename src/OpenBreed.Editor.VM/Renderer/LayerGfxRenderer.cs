using OpenBreed.Editor.VM.Maps;
using OpenBreed.Model.Maps;
using System.Drawing;

namespace OpenBreed.Editor.VM.Renderer
{
    public class LayerGfxRenderer : RendererBase<MapLayoutModel>
    {
        #region Private Fields

        private const MapLayerType layerType = MapLayerType.Gfx;

        private MapEditorTilesToolVM tilesTool;

        #endregion Private Fields

        #region Public Constructors

        public LayerGfxRenderer(MapEditorTilesToolVM tilesTool, RenderTarget target) : base(target)
        {
            this.tilesTool = tilesTool;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Render(MapLayoutModel renderable)
        {
            var layerIndex = renderable.GetLayerIndex(layerType);

            RectangleF viewRect = Target.ClipBounds;

            //TODO: Get this from model
            int tileSize = 16;
            int xFrom, xTo, yFrom, yTo;
            renderable.GetClipIndices(viewRect, out xFrom, out yFrom, out xTo, out yTo);

            for (int xIndex = xFrom; xIndex <= xTo; xIndex++)
            {
                for (int yIndex = yFrom; yIndex <= yTo; yIndex++)
                {
                    var tileId = renderable.GetCellValue(layerIndex, xIndex, yIndex);
                    var x = xIndex * tileSize;
                    var y = yIndex * tileSize;

                    tilesTool.Parent.DrawTile(Target, tileId, x, y, tileSize);
                }
            }
        }

        #endregion Public Methods
    }
}