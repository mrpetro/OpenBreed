using OpenBreed.Gui.Interface.Elements;
using OpenBreed.Gui.Interface.Presentations;
using OpenBreed.Rendering.Interface.Extensions;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Rendering
{
    public class ButtonRenderer : ElementRenderer<IButton>
    {
        #region Protected Methods

        protected override void Render(IButton element, IRenderView view)
        {
            var body = element.Body;

            var lightColor = ButtonPresentation.LightSideColor;
            var flatColor = ButtonPresentation.FlatSideColor;
            var darkColor = ButtonPresentation.DarkSideColor;

            if (element.IsHovered)
            {
                lightColor = lightColor.Multiply(0.9f);
                flatColor = flatColor.Multiply(0.9f);
                darkColor = darkColor.Multiply(0.9f);
            }

            var elementCenter = element.Position.AsVector();

            view.Context.Primitives.DrawRectangle(
                view,
                elementCenter,
                new Vector2(body.Width, body.Height),
                flatColor, filled: true);

            if (element.IsPressed)
            {
                view.Context.Primitives.DrawRectangle(
                    view,
                    elementCenter + new Vector2(1, -1),
                    new Vector2(body.Width - 2, body.Height - 2),
                    lightColor, filled: true); ;

                view.Context.Primitives.DrawRectangle(
                    view,
                    elementCenter + new Vector2(-1, 1),
                    new Vector2(body.Width - 2, body.Height - 2),
                    darkColor, filled: true);
            }
            else
            {
                view.Context.Primitives.DrawRectangle(
                    view,
                    elementCenter + new Vector2(-1, 1),
                    new Vector2(body.Width - 2, body.Height - 2),
                    lightColor, filled: true);

                view.Context.Primitives.DrawRectangle(
                    view,
                    elementCenter + new Vector2(1, -1),
                    new Vector2(body.Width - 2, body.Height - 2),
                    darkColor, filled: true);
            }

            view.Context.Primitives.DrawRectangle(
                view,
                elementCenter,
                new Vector2(body.Width - 4, body.Height - 4),
                flatColor, filled: true);
        }

        #endregion Protected Methods
    }
}