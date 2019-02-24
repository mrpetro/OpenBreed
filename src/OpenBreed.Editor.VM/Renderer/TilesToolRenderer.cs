using OpenBreed.Editor.VM.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Renderer
{
    public class TilesToolRenderer : RendererBase<MapEditorTilesToolVM>
    {

        private MapEditorVM _editor;

        private ViewCursorRenderer _cursorRenderer;
        private LayoutRenderer _layoutRenderer;


        public TilesToolRenderer(MapEditorVM editor, RenderTarget target) : base(target)
        {
            _editor = editor;
        }

        public override void Render(MapEditorTilesToolVM renderable)
        {
            foreach (var tile in renderable.TilesCursor)
            {
                renderable.TilesSelector.CurrentTileSet.DrawTile(Target.Gfx, tile.TileIdAfter, tile.IndexCoords.X * 16, tile.IndexCoords.Y * 16, 16);
            }
        }

    }
}
