using OpenBreed.Editor.VM.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Renderer
{
    public class ViewCursorRenderer : RendererBase<MapViewCursorVM>
    {
        #region Private Fields

        private readonly MapEditorVM _editor;

        #endregion Private Fields

        #region Public Constructors

        public ViewCursorRenderer(MapEditorVM editor, RenderTarget target) : base(target)
        {
            _editor = editor;
        }

        public override void Render(MapViewCursorVM renderable)
        {
            Target.Gfx.DrawRectangle(System.Drawing.Pens.Red, new Rectangle(renderable.WorldCoords, new Size(16, 16)));
        }

        #endregion Public Constructors

    }
}
