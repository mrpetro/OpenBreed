using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Model.Maps;
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

        public ViewRenderer(MapEditorVM editor, IRenderTarget target, IPensProvider pensProvider) : base(target)
        {
            _editor = editor;

            _layoutRenderer = new LayoutRenderer(editor, target);
            _cursorRenderer = new ViewCursorRenderer(editor, target, pensProvider);
            _tilesToolRenderer = new TilesToolRenderer(editor, target);
        }

        public override void Render(MapEditorViewVM renderable)
        {
            Target.Transform = renderable.Transformation;

            _layoutRenderer.Render((MapLayoutModel)renderable.Layout);
            _tilesToolRenderer.Render(_editor.TilesTool);
            _cursorRenderer.Render(renderable.Cursor);
        }

        #endregion Public Constructors

    }
}
