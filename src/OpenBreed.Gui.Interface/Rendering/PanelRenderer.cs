using OpenBreed.Gui.Interface.Elements;
using OpenBreed.Gui.Interface.Presentations;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
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
            var body = element.Body;
            var elementCenter = element.Position.AsVector();

            view.Context.Primitives.DrawRectangle(
                view,
                elementCenter,
                new Vector2(body.Width, body.Height),
                PanelPresentation.FillColor, filled: true);
            view.Context.Primitives.DrawRectangle(
                view,
                elementCenter,
                new Vector2(body.Width, body.Height),
                PanelPresentation.BorderColor);
        }

        #endregion Protected Methods
    }
}
