using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Presentations;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Rendering
{
    public class GridPanelRenderer : ElementRenderer<IGridPanel>
    {
        #region Protected Methods

        protected override void Render(IGridPanel element, IRenderView view)
        {
            var box = element.LocalBox;

            var elementCenter = element.Position.AsVector();

            view.PushMatrix();
            view.Translate(elementCenter);

            view.Context.Primitives.DrawRectangle(
                view,
                box,
                PanelPresentation.FillColor, filled: true);
            view.Context.Primitives.DrawRectangle(
                view,
                box,
                PanelPresentation.BorderColor);

            var x = 0.0f;

            for (int i = 0; i < element.Columns.Count; i++)
            {
                x += element.Columns[i];

                var start = new Vector2(box.Min.X + x, box.Min.Y);
                var end = new Vector2(box.Min.X + x, box.Max.Y);

                view.Context.Primitives.DrawLine(
                    view, start, end,
                    PanelPresentation.BorderColor);
            }

            var y = 0.0f;

            for (int i = 0; i < element.Rows.Count; i++)
            {
                y += element.Rows[i];

                var start = new Vector2(box.Min.X, box.Min.Y + y);
                var end = new Vector2(box.Max.X, box.Min.Y + y);

                view.Context.Primitives.DrawLine(
                    view, start, end,
                    PanelPresentation.BorderColor);
            }


            view.PopMatrix();
        }

        #endregion Protected Methods
    }
}
