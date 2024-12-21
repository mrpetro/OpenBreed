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

        #endregion Private Fields

        #region Public Constructors

        public InteractionRenderer(IEnumerable<IElementRenderer> elementRenderers)
        {
            rendererLookup = elementRenderers.ToDictionary(item => item.ElementType, (item) => item);
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IInteractionCore interactionCore, IRenderView view)
        {
            if (interactionCore.Root is null)
            {
                return;
            }

            var clipState = new ClipState();
            clipState.ParentIdBits = new System.Collections.BitArray(32);
            clipState.ParentMaskdBits = new System.Collections.BitArray(32);

            Render(interactionCore.Root, view, clipState, 0);
        }

        #endregion Public Methods

        #region Private Methods

        private void Render(IElement element, IRenderView view, ClipState clipState, int bitPosition)
        {
            foreach (var elementType in element.GetType().GetInterfaces().Where(item => item != typeof(IElement)))
            {
                if (elementType is not null && rendererLookup.TryGetValue(elementType, out IElementRenderer? elementRenderer) && elementRenderer is not null)
                {
                    elementRenderer.Render(element, view);
                    break;
                }
            }

            if (element is IContainer container)
            {
                view.PushMatrix();

                void OnRenderFrame(Box2 viewBox, ClipState clipState)
                {
                    view.Translate(new Vector3(element.Position.X, element.Position.Y, 0.0f));
                    RenderChilds(container, view, clipState, bitPosition);
                }

                try
                {
                    //if (bitPosition == 0)
                    //{
                    //    OnRenderFrame(view.Box.AsBox2(), clipState);

                    //}
                    //else
                    //{
                        view.Context.Primitives.DrawNestedEx(view, element.ActualBox, clipState, OnRenderFrame);
                    //}

                    // view.Translate(new Vector3(element.Position.X, element.Position.Y, 0.0f));
                    //RenderChilds(container, view);
                }
                finally
                {
                    view.PopMatrix();
                }
            }
        }

        private void RenderChilds(IContainer container, IRenderView view, ClipState clipState, int bitPosition)
        {
            clipState.ParentId = clipState.Id;
            clipState.ParentMask = clipState.Mask;

            var digits = (int)Math.Ceiling(Math.Log2(container.Childs.Count + 1));

            bitPosition += digits;

            var size = 4;


            //clipState.Mask = clipState.ParentMask + container.Childs.Count + 1;

            clipState.Layer++;

            for (int i = 0; i < container.Childs.Count; i++)
            {


                var child = container.Childs[i];

                var id = (i + 1) << (size - bitPosition);
                var mask = ((2 << digits - 1) - 1) << (size - bitPosition);

                var idStr = Convert.ToString(id, 2).PadLeft(size, '0');
                var maskStr = Convert.ToString(mask, 2).PadLeft(size, '0');

                //Debug.Print($"BTag: {child.Tag} Id: {idStr} Mask: {maskStr}");

                id = clipState.ParentId | id;
                mask = clipState.ParentMask | mask;

                idStr = Convert.ToString(id, 2).PadLeft(size, '0');
                maskStr = Convert.ToString(mask, 2).PadLeft(size, '0');
                //Debug.Print($"Tag: {child.Tag} Id: {idStr} Mask: {maskStr}");


                //Debug.Print(Convert.ToString(clipState.ParentMask, 2).PadLeft(4, '0'));

                //Debug.Print(Convert.ToString((i + 1) * digits, 2).PadLeft(4, '0'));
                clipState.Id =  id;
                clipState.Mask = mask;

                Render(child, view, clipState, bitPosition);
            }

            clipState.Layer--;
        }

        #endregion Private Methods
    }
}