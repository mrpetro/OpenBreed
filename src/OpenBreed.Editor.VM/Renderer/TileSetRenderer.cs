using OpenBreed.Editor.VM.Maps;
using OpenBreed.Model.Tiles;

namespace OpenBreed.Editor.VM.Renderer
{
    public class TileSetRenderer : RendererBase<TileSetModel>
    {
        #region Private Fields

        private MapEditorTilesToolVM tilesTool;

        #endregion Private Fields

        #region Public Constructors

        public TileSetRenderer(MapEditorTilesToolVM tilesTool, RenderTarget target) : base(target)
        {
            this.tilesTool = tilesTool;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Render(TileSetModel renderable)
        {
            if (renderable == null)
                return;

            var tileSize = renderable.TileSize;
            int xMax = renderable.TilesNoX;
            int yMax = renderable.TilesNoY;

            for (int j = 0; j < yMax; j++)
            {
                for (int i = 0; i < xMax; i++)
                {
                    int gfxId = i + xMax * j;
                    tilesTool.Parent.DrawTile(Target, gfxId, i * tileSize, j * tileSize, tileSize);
                }
            }
        }

        #endregion Public Methods
    }
}