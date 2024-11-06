using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Color4.Green, filled: true);
            view.Context.Primitives.DrawRectangle(
                view,
                new Vector2(element.CenterX, element.CenterY),
                new Vector2(element.Width, element.Height),
                Color4.Yellow);
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