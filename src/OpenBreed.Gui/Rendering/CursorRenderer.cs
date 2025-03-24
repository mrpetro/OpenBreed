using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Rendering;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
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
        public void Render(IInteractionCursor cursor, IRenderView view)
        {
            var cPos = cursor.Position;
            //var cPos = view.GetViewToWorldCoords(cursorPos);

            var cSize = 10;
            view.Context.Primitives.DrawCircle(view, new Vector2(cPos.X, cPos.Y), cSize, Color4.Red, filled: false);
            view.Context.Primitives.DrawPoint(view, new Vector2(cPos.X, cPos.Y), Color4.Red, PointType.Cross, cSize);
            view.Context.Fonts.Render(view, new Box2(view.Box.Min, view.Box.Max), (v,c) => RenderTexts(cPos, v, c));
        }

        private void RenderTexts(Vector2 pos, IRenderView view, Box2 clipBox)
        {
            var font = view.Context.Fonts.GetOSFont("ARIAL", 12);

            view.Context.Fonts.RenderStart(view, pos);
            view.Context.Fonts.RenderPart(view, font.Id, $"({pos.X},{pos.Y})", Vector2.Zero, Color4.Green, 100, clipBox);
            view.Context.Fonts.RenderEnd(view);
        }
    }
}
