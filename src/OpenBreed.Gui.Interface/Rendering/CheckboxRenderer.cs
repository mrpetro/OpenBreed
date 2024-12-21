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
    public class CheckboxRenderer : ElementRenderer<ICheckbox>
    {
        #region Protected Methods

        protected override void Render(ICheckbox element, IRenderView view)
        {
            var box = element.LocalBox;

            var borderColor = CheckboxPresentation.BorderColor;
            var backgroundColor = CheckboxPresentation.BackgroundColor;
            var symbolColor = CheckboxPresentation.SymbolColor;

            if (element.IsHovered)
            {
                borderColor = borderColor.Multiply(0.9f);
                backgroundColor = backgroundColor.Multiply(0.9f);
                symbolColor = symbolColor.Multiply(0.9f);
            }

            var elementCenter = element.Position.AsVector();

            view.PushMatrix();
            view.Translate(elementCenter);

            view.Context.Primitives.DrawRectangle(
                view,
                box,
                backgroundColor, filled: true);

            view.Context.Primitives.DrawRectangle(
                view,
                box,
                borderColor, filled: false);

            if (element.IsChecked)
            {
                var points = new Vector2[] {
                    new Vector2(-0.8f * box.HalfSize.X, 0.0f * box.HalfSize.Y ),
                    new Vector2(-0.1f * box.HalfSize.X , -0.8f * box.HalfSize.Y),
                    new Vector2(0.8f * box.HalfSize.X, 0.7f * box.HalfSize.Y) };

                view.Context.Primitives.DrawLines(
                    view,
                    points,
                    symbolColor);
            }

            view.PopMatrix();
        }

        #endregion Protected Methods
    }
}