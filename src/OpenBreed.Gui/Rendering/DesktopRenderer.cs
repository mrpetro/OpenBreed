using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Presentations;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Rendering
{
    public class DesktopRenderer : ElementRenderer<IDesktop>
    {
        #region Protected Methods

        protected override void Render(IDesktop element, IRenderView view)
        {
            //var box = element.LocalBox;
            //var size = box.Size;
            //var center = element.Position.AsVector();

            //var lightColor = ButtonPresentation.LightSideColor;
            //var flatColor = ButtonPresentation.FlatSideColor;
            //var darkColor = ButtonPresentation.DarkSideColor;

            //if (element.IsHovered)
            //{
            //    lightColor = lightColor.Multiply(0.9f);
            //    flatColor = flatColor.Multiply(0.9f);
            //    darkColor = darkColor.Multiply(0.9f);
            //}

            //view.PushMatrix();

            //view.Translate(center);

            //view.Context.Primitives.DrawRectangle(
            //    view,
            //    box,
            //    flatColor, filled: true);

            //view.Context.Primitives.DrawRectangle(
            //    view,
            //    new Vector2(-1, 1),
            //    new Vector2(size.X - 2, size.Y - 2),
            //    lightColor, filled: true);

            //view.Context.Primitives.DrawRectangle(
            //    view,
            //    new Vector2(1, -1),
            //    new Vector2(size.X - 2, size.Y - 2),
            //    darkColor, filled: true);

            //view.Context.Primitives.DrawRectangle(
            //    view,
            //    Vector2.Zero,
            //    new Vector2(size.X - 4, size.Y - 4),
            //    flatColor, filled: true);

            //view.PopMatrix();

        }

        #endregion Protected Methods
    }
}