using OpenBreed.Gui.Abstractions.Constants;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OpenBreed.Gui.Rendering
{
    public class LabelRenderer : ElementRenderer<ILabelField>
    {
        #region Protected Methods

        protected override void Render(ILabelField element, IRenderView view)
        {
            var box = element.LocalBox;
            var size = box.Size;
            var font = element.Font;

            view.Context.Primitives.DrawRectangle(
                view,
                element.ActualBox,
                Color4.Yellow);

            var viewBox = new Box2(view.Box.Min, view.Box.Max);

            var elementHalfWidth = size.X / 2.0f;
            var elementHalfHeight = size.Y / 2.0f;

            var textHalfHeight = font.Height / 2.0f;
            var textHalfWidth = font.GetWidth(element.Text) / 2.0f;

            var textPos = element.Position.AsVector() - new Vector2(textHalfWidth, textHalfHeight);

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

            textPos += new Vector2(offsetX, offsetY);

            view.PushMatrix();
            view.Translate(textPos);

            font.Draw(view, element.Text, Color4.Black, element.ActualBox, ignoreScale: true);

            view.PopMatrix();
        }

        #endregion Protected Methods
    }
}
