using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Presentations;
using OpenBreed.Gui.Abstractions.Rendering;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Extensions;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace OpenBreed.Gui.Rendering
{
    public class InteractionRenderer : IInteractionRenderer
    {
        #region Private Fields

        private readonly Dictionary<Type, IElementRenderer> rendererLookup;
        private readonly ICursorRenderer cursorRenderer;

        #endregion Private Fields

        #region Public Constructors

        public InteractionRenderer(IEnumerable<IElementRenderer> elementRenderers, ICursorRenderer cursorRenderer)
        {
            rendererLookup = elementRenderers.ToDictionary(item => item.ElementType, (item) => item);
            this.cursorRenderer = cursorRenderer;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IDesktop desktop, IRenderView view)
        {
            Render(desktop, view, 0);

            foreach (var cursor in desktop.Cursors)
            {
                cursorRenderer.Render(cursor, view);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Render(IElement element, IRenderView view, int depth)
        {
            //view.EnableAlpha();


            foreach (var elementType in element.GetType().GetInterfaces().Where(item => item != typeof(IElement)).Reverse())
            {
                if (elementType is not null && rendererLookup.TryGetValue(elementType, out IElementRenderer? elementRenderer) && elementRenderer is not null)
                {
                    elementRenderer.Render(element, view);
                    break;
                }
            }

            var worldBox = element.ActualBox;

            var clipBox = new Box2i((Vector2i)worldBox.Min, (Vector2i)worldBox.Max);


            //var clipBox = view.GetWorldToViewCoords(worldBox);


            //view.ClippingEnabled = true;

            //view.SetClipBox(clipBox);
            //view.PushClipBox();


            if (element is IContainer container)
            {
                view.PushMatrix();

                void OnRenderFrame(Box2i clipBox)
                {

                    view.Translate(new Vector3(element.Position.X, element.Position.Y, depth));
                    RenderChilds(container, view, depth);
                }

                try
                {
                    OnRenderFrame(clipBox);
                }
                finally
                {
                    view.PopMatrix();
                }
            }

            //view.PopClipBox();
            //view.ClippingEnabled = false;

            //view.DisableAlpha();
        }

        private void RenderChilds(IContainer container, IRenderView view, int depth)
        {
            depth++;

            for (int i = 0; i < container.Childs.Count; i++)
            {
                var child = container.Childs[i];

                Render(child, view, depth);
            }
        }

        #endregion Private Methods
    }
}