using OpenBreed.Gui.Abstractions.Elements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Extensions
{
    public static class TextBoxExtensions
    {
        public static Vector2 StartPos(this ITextBox textBox)
        {
            var halfWidth = textBox.Size.X / 2.0f;
            var halfHeight = textBox.Size.Y / 2.0f;

            var topLeftPos = textBox.Position.AsVector();

            var offsetX =  - halfWidth;
            var offsetY = halfHeight - textBox.Font.Height;

            return topLeftPos + new Vector2(offsetX, offsetY);
        }

        public static Box2 GetScrollBox(this ITextBox textBox)
        {
            var textPos = textBox.StartPos();

            var scrollBox = textBox.LocalBox.Translated((textPos + textBox.ScrollPosition) * new Vector2(-1.0f, 1.0f));

            return scrollBox;
        }

    }
}
