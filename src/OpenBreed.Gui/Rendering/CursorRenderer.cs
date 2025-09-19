using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Rendering;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions.Renderers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Rendering
{
    public class CursorRenderer : ICursorRenderer
    {
        private readonly IFontMan fontMan;

        public CursorRenderer(IFontMan fontMan)
        {
            this.fontMan = fontMan;
        }

        public void Render(IInteractionCursor cursor, IRenderView view)
        {
            var cPos = cursor.Position;
            //var cPos = view.GetViewToWorldCoords(cursorPos);

            var cSize = 10;
            //view.Context.Primitives.DrawCircle(view, new Vector2(cPos.X, cPos.Y), cSize, Color4.Red, filled: false);
            //view.Context.Primitives.DrawPoint(view, new Vector2(cPos.X, cPos.Y), Color4.Red, PointType.Cross, cSize);
            //view.Context.Fonts.Render(view, new Box2(view.Box.Min, view.Box.Max), (v,c) => RenderTexts(cPos, v, c));
        }
    }
}
