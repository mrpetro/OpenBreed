using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Builders;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Elements
{
    internal abstract class Container : Element, IContainer
    {
        #region Private Fields

        private readonly List<Element> childs = new List<Element>();

        #endregion Private Fields

        #region Protected Constructors

        protected Container(ElementBuilder builder) : base(builder)
        {
        }

        #endregion Protected Constructors

        #region Public Properties

        public IReadOnlyList<IElement> Childs => childs;

        #endregion Public Properties

        #region Public Methods

        public override bool HitTest(Vector2 point, out IElement? interactiveElement)
        {
            if (!base.HitTest(point, out interactiveElement))
            {
                return false;
            }

            var localPoint = point - Position.AsVector();

            IElement? childElement = null;

            for (int i = 0; i < childs.Count; i++)
            {
                if (childs[i].HitTest(localPoint, out childElement))
                {
                    break;
                }
            }

            if (childElement is not null)
            {
                interactiveElement = childElement;
                return true;
            }

            return true;
        }

        public void AddChild(IElement child)
        {
            if (child is not Element internalElement)
            {
                throw new InvalidOperationException($"Expected element of type ({typeof(Element)}).");
            }

            internalElement.SetParent(this);
        }

        public void RemoveChild(IElement child)
        {
            if (child is not Element internalElement)
            {
                throw new InvalidOperationException($"Expected child element of type ({typeof(Element)}).");
            }

            internalElement.SetParent(null);
        }

        internal void AddChild(Element child)
        {
            childs.Add(child);
        }

        internal bool RemoveChild(Element child)
        {
            return childs.Remove(child);
        }

        #endregion Public Methods
    }
}