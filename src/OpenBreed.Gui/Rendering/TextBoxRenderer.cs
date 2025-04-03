using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Gui.Abstractions.Constants;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Extensions;
using OpenBreed.Gui.Elements;
using OpenBreed.Rendering.Interface.Managers;
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
    public class TextBoxRenderer : ElementRenderer<ITextBox>
    {
        #region Protected Methods

        private void RenderPointer(ITextBox element, IRenderView view, Box2 viewBox, OpenBreed.Rendering.Interface.IFont font)
        {
            var cursorXPosition = element.Pointer.GetXPosition();

            var sp = new Vector2(cursorXPosition, font.Height / 2);

            if (element.Pointer.Blink())
            {
                view.Context.Primitives.DrawRectangle(view, sp, new Vector2(2, font.Height), Color4.White, filled: true);
            }
        }

        protected override void Render(ITextBox element, IRenderView view)
        {
            var box = element.LocalBox;
            var size = box.Size;
            var font = element.Font;
            var textPos = element.StartPos();

            view.Context.Primitives.DrawRectangle(view, box, Color4.Yellow);

            var viewBox = new Box2(view.Box.Min, view.Box.Max);

            view.PushMatrix();

            var d = box.Translated(textPos * new Vector2(-1.0f, 1.0f));


            var startLineIndex = (int)(d.Min.Y / font.Height);
            var endLineIndex = (int)(d.Max.Y / font.Height) + 2;
            startLineIndex = Math.Max(0, startLineIndex);
            endLineIndex = Math.Min(endLineIndex, element.LinesCount);

            for (int lineIndex = startLineIndex; lineIndex < endLineIndex; lineIndex++)
            {
                var chars = element.GetCharacters(lineIndex);

                var text = new string(chars.ToArray());

                view.PushMatrix();
                view.Translate(textPos);

               
                font.Draw(view, text, Color4.White, d, ignoreScale: true);

                if (lineIndex == element.Pointer.LineIndex)
                {
                    RenderPointer(element, view, viewBox, font);
                }

                view.PopMatrix();

                view.Translate(new Vector2(0.0f, -font.Height));
            }

            view.PopMatrix();
        }

        #endregion Protected Methods
    }
}
