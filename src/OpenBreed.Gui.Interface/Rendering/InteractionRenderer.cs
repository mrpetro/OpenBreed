using OpenBreed.Gui.Interface.Elements;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace OpenBreed.Gui.Interface.Rendering
{
    public class InteractionRenderer : IInteractionRenderer
    {
        #region Public Methods

        public void Render(IInteractiveElement element, IRenderView view)
        {
            switch (element)
            {
                case IInteractiveLabel interactiveLabel:
                    Render(interactiveLabel, view);
                    break;
                case IInteractivePanel interactivePanel:
                    Render(interactivePanel, view);
                    break;
                default:
                    break;
            }
            view.PushMatrix();

            try
            {
                view.Translate(new Vector3(element.CenterX, element.CenterY, 0.0f));
                RenderChilds(element, view);
            }
            finally
            {
                view.PopMatrix();
            }
        }

        public void Render(IInteractivePanel element, IRenderView view)
        {
            view.Context.Primitives.DrawRectangle(
                view,
                new Vector2(element.CenterX, element.CenterY),
                new Vector2(element.Width, element.Height),
                element.FillColor, filled: true);
            view.Context.Primitives.DrawRectangle(
                view,
                new Vector2(element.CenterX, element.CenterY),
                new Vector2(element.Width, element.Height),
                element.BorderColor);
        }

        public void Render(IInteractiveLabel element, IRenderView view)
        {
            view.Context.Primitives.DrawRectangle(
                view,
                new Vector2(element.CenterX, element.CenterY),
                new Vector2(element.Width, element.Height),
                Color4.Yellow);

            var font = view.Context.Fonts.GetOSFont("Arial", 20);

            var viewBox = new Box2(view.Box.Min, view.Box.Max);

            var elementHalfWidth = element.Width / 2.0f;
            var elementHalfHeight = element.Height / 2.0f;

            var textHalfHeight = font.Height / 2.0f;
            var textHalfWidth = font.GetWidth(element.Text) / 2.0f;

            var textPos = new Vector2(element.CenterX - textHalfWidth, element.CenterY - textHalfHeight);

            var offsetX = 0.0f;
            var offsetY = 0.0f;

            switch (element.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    offsetX = textHalfWidth - elementHalfWidth;
                    break;
                case HorizontalAlignment.Right:
                    offsetX = -textHalfWidth + elementHalfWidth;
                    break;
                case HorizontalAlignment.Center:
                default:
                    break;
            }

            switch (element.VerticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    offsetY = textHalfHeight - elementHalfHeight;
                    break;
                case VerticalAlignment.Top:
                    offsetY = -textHalfHeight + elementHalfHeight;
                    break;
                case VerticalAlignment.Center:
                default:
                    break;
            }

            textPos = textPos + new Vector2(offsetX, offsetY);

            view.Context.Fonts.RenderStart(view, textPos);
            view.Context.Fonts.RenderPart(view, font.Id, element.Text, Vector2.Zero, Color4.White, 100, viewBox);
            view.Context.Fonts.RenderEnd(view);

        }

        #endregion Public Methods

        #region Private Methods

        private void RenderChilds(IInteractiveElement element, IRenderView view)
        {
            for (int i = 0; i < element.Childs.Count; i++)
            {
                Render(element.Childs[i], view);
            }
        }

        #endregion Private Methods
    }
}