using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Gui.Abstractions.Constants;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Extensions;
using OpenBreed.Gui.Elements;
using OpenBreed.Gui.Presentations;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OpenBreed.Gui.Rendering
{
    public class TextFieldRenderer : ElementRenderer<ITextField>
    {
        #region Protected Methods

        protected override void Render(ITextField element, IRenderView view)
        {
            var box = element.LocalBox;
            var size = box.Size;
            var font = element.Font;
            var textPos = element.StartPos();
            var textScrollBox = element.GetScrollBox();

            view.Context.Primitives.DrawRectangle(view, box, TextFieldPresentation.BackgroundColor, filled: true);
            view.Context.Primitives.DrawRectangle(view, box, TextFieldPresentation.BorderColor);

            view.PushMatrix();

            var startLine = element.ScrollPosition.Y / font.Height;
            var endLine = (box.Size.Y + element.ScrollPosition.Y) / font.Height;

            var startLineIndex = (int)(startLine);
            var endLineIndex = (int)(endLine);

            startLineIndex = Math.Max(0, startLineIndex);
            endLineIndex = Math.Min(endLineIndex + 1, element.LinesCount);

            view.PushMatrix();
            view.Translate(textPos);
            view.Translate(new Vector2(0.0f, -startLineIndex * font.Height));
            view.Translate(element.ScrollPosition);

            for (int lineIndex = startLineIndex; lineIndex < endLineIndex; lineIndex++)
            {
                var chars = element.GetCharacters(lineIndex);

                var text = new string(chars.ToArray());

                font.Draw(view, text, Color4.Black, textScrollBox, ignoreScale: true);

                if (element.IsFocused)
                {
                    if (lineIndex == element.Pointer.LineIndex)
                    {
                        RenderPointer(element, view, textScrollBox, font);
                    }
                }

                view.Translate(new Vector2(0.0f, -font.Height));
            }

            view.PopMatrix();

            view.PopMatrix();
        }

        #endregion Protected Methods

        #region Private Methods

        private void RenderPointer(ITextField element, IRenderView view, Box2 viewBox, OpenBreed.Rendering.Abstractions.IFont font)
        {
            var cursorXPosition = element.Pointer.GetXPosition();

            if (cursorXPosition < viewBox.Min.X)
            {
                return;
            }

            if (cursorXPosition > viewBox.Max.X)
            {
                return;
            }

            var sp = new Vector2(cursorXPosition, font.Height / 2);

            if (element.Pointer.Blink())
            {
                view.Context.Primitives.DrawRectangle(view, sp, new Vector2(2, font.Height), TextFieldPresentation.PointerColor, filled: true);
            }
        }

        #endregion Private Methods
    }
}