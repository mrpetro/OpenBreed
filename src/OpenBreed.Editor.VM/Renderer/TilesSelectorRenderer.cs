using OpenBreed.Editor.VM.Maps;

namespace OpenBreed.Editor.VM.Renderer
{
    public class TilesSelectorRenderer : RendererBase<MapEditorTilesSelectorVM>
    {
        #region Private Fields

        private readonly TileSetRenderer tileSetRenderer;
        private readonly TilesSelectorSelectionRenderer tilesSelectionRenderer;
        private MapEditorTilesToolVM tilesTool;

        #endregion Private Fields

        #region Public Constructors

        public TilesSelectorRenderer(MapEditorTilesToolVM tilesTool, RenderTarget target) : base(target)
        {
            this.tilesTool = tilesTool;

            tileSetRenderer = new TileSetRenderer(tilesTool, target);
            tilesSelectionRenderer = new TilesSelectorSelectionRenderer(tilesTool, target);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Render(MapEditorTilesSelectorVM renderable)
        {
            if (renderable == null)
                return;

            tileSetRenderer.Render(renderable.CurrentTileSet);
            tilesSelectionRenderer.Render(renderable);
        }

        #endregion Public Methods
    }
}