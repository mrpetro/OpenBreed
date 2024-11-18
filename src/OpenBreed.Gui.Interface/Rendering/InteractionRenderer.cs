using OpenBreed.Gui.Interface.Bodies;
using OpenBreed.Gui.Interface.Elements;
using OpenBreed.Gui.Interface.Presentations;
using OpenBreed.Rendering.Interface.Extensions;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace OpenBreed.Gui.Interface.Rendering
{
    public class InteractionRenderer : IInteractionRenderer
    {
        #region Private Fields

        private readonly Dictionary<Type, IElementRenderer> rendererLookup;

        #endregion Private Fields

        #region Public Constructors

        public InteractionRenderer(IEnumerable<IElementRenderer> elementRenderers)
        {
            rendererLookup = elementRenderers.ToDictionary(item => item.ElementType, (item) => item);
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IElement element, IRenderView view)
        {
            var elementType = element.GetType().GetInterfaces().FirstOrDefault(item => item != typeof(IElement));

            if (elementType is not null && rendererLookup.TryGetValue(elementType, out IElementRenderer elementRenderer))
            {
                elementRenderer.Render(element, view);
            }

            view.PushMatrix();

            try
            {
                view.Translate(new Vector3(element.Position.X, element.Position.Y, 0.0f));
                RenderChilds(element, view);
            }
            finally
            {
                view.PopMatrix();
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void RenderChilds(IElement element, IRenderView view)
        {
            for (int i = 0; i < element.Childs.Count; i++)
            {
                Render(element.Childs[i], view);
            }
        }

        #endregion Private Methods
    }
}