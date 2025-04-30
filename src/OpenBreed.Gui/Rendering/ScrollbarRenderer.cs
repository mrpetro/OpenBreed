using OpenBreed.Gui.Abstractions.Constants;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Presentations;
using OpenBreed.Gui.Elements;
using OpenBreed.Rendering.Interface.Extensions;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Rendering
{
    public class ScrollbarRenderer : ElementRenderer<IScrollbar>
    {
        #region Protected Methods

        protected override void Render(IScrollbar element, IRenderView view)
        {
            var elementBox = element.LocalBox;
            var handleBox = element.HandleBox;
            var size = elementBox.Size;
            var center = element.Position.AsVector();

            var lightColor = ButtonPresentation.LightSideColor;
            var flatColor = ButtonPresentation.FlatSideColor;
            var darkColor = ButtonPresentation.DarkSideColor;

            if (element.IsHandleHovered || element.IsHandleGrabbed)
            {
                lightColor = lightColor.Multiply(0.9f);
                //flatColor = flatColor.Multiply(0.9f);
                //darkColor = darkColor.Multiply(0.9f);
            }

            view.PushMatrix();

            view.Translate(center);

            view.Context.Primitives.DrawRectangle(
                view,
                elementBox,
                flatColor, filled: true);

            var scaleModification = GetHandleBox(element, element.Mode);

            view.Context.Primitives.DrawRectangle(
                view,
                handleBox.Inflated(scaleModification),
                lightColor, filled: true);

            view.PopMatrix();
        }

        #endregion Protected Methods

        #region Private Methods

        private Vector2 GetHandleBox(IScrollbar element, ScrollbarMode mode)
        {
            switch (element.Mode)
            {
                case Abstractions.Constants.ScrollbarMode.Horizontal:
                    return new Vector2(0.0f, -element.HandleBox.Size.Y * 0.1f);

                case Abstractions.Constants.ScrollbarMode.Vertical:
                    return new Vector2(-element.HandleBox.Size.X * 0.1f, 0.0f);

                default:
                    throw new NotImplementedException($"Scrollbar mode '{element.Mode}' is not implemented.");
            }
        }

        #endregion Private Methods
    }
}