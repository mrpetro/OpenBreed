using OpenBreed.Common.Interface.Drawing;
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
        private readonly IPensProvider pensProvider;

        #endregion Private Fields

        #region Public Constructors

        public ViewCursorRenderer(MapEditorVM editor, IRenderTarget target, IPensProvider pensProvider) : base(target)
        {
            _editor = editor;
            this.pensProvider = pensProvider;
        }

        public override void Render(MapViewCursorVM renderable)
        {
            if (!renderable.Visible)
                return;

            Target.DrawRectangle(pensProvider.Red, new MyRectangle(renderable.WorldSnapCoords, new MySize(16, 16)));
        }

        #endregion Public Constructors

    }
}
