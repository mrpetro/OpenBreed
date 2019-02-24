using OpenBreed.Editor.VM.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Renderer
{
    public class ViewRenderer : RendererBase<MapEditorViewVM>
    {
        #region Private Fields

        private MapEditorVM _editor;

        private ViewCursorRenderer _cursorRenderer;
        private LayoutRenderer _layoutRenderer;
        private TilesToolRenderer _tilesToolRenderer;

        #endregion Private Fields

        #region Public Constructors

        public ViewRenderer(MapEditorVM editor, RenderTarget target) : base(target)
        {
            _editor = editor;

            _layoutRenderer = new LayoutRenderer(editor, target);
            _cursorRenderer = new ViewCursorRenderer(editor, target);
            _tilesToolRenderer = new TilesToolRenderer(editor, target);
        }

        public override void Render(MapEditorViewVM renderable)
        {
            Target.Gfx.Transform = renderable.Transformation;

            if (renderable.Layout == null)
                return;

            _layoutRenderer.Render(renderable.Layout);


            _editor.TilesTool.DrawBuffer(Target.Gfx, 16);

            _tilesToolRenderer.Render(_editor.TilesTool);

            if (renderable.Cursor.Visible)
                _cursorRenderer.Render(renderable.Cursor);
        }

        #endregion Public Constructors

    }
}
