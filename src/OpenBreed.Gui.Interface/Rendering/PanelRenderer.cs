using OpenBreed.Gui.Interface.Elements;
using OpenBreed.Gui.Interface.Presentations;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Rendering
{
    public class PanelRenderer : ElementRenderer<IPanel>
    {
        #region Protected Methods

        protected override void Render(IPanel element, IRenderView view)
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

            view.PopMatrix();
        }

        #endregion Protected Methods
    }
}
