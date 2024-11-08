using OpenBreed.Gui.Interface.Builders;
using OpenBreed.Rendering.Interface.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Elements
{
    internal abstract class InteractiveElement : IInteractiveElement
    {
        #region Private Fields

        private readonly List<InteractiveElement> childs = new List<InteractiveElement>();
        private readonly Action<IInteractiveElement> clickCallback;
        private InteractiveElement? parent;

        #endregion Private Fields

        #region Internal Constructors

        internal InteractiveElement(InteractiveElementBuilder interactiveElementBuilder)
        {
            foreach (var child in interactiveElementBuilder.childBuilders.Select(builder => builder.InternalBuild()))
            {
                child.SetParent(this);
            }

            Tag = interactiveElementBuilder.Tag;
            clickCallback = interactiveElementBuilder.ClickCallback;

            CenterX = interactiveElementBuilder.CenterX;
            CenterY = interactiveElementBuilder.CenterY;
            Width = interactiveElementBuilder.Width;
            Height = interactiveElementBuilder.Height;
        }

        #endregion Internal Constructors

        #region Public Properties

        public float CenterX { get; set; }
        public float CenterY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public string Tag { get; set; }

        public IInteractiveElement? Parent => parent;

        public IReadOnlyList<IInteractiveElement> Childs => childs;

        #endregion Public Properties

        #region Public Methods

        public bool HitTest(float x, float y, out IInteractiveElement? interactiveElement)
        {
            if (!Contains(x, y))
            {
                interactiveElement = null;
                return false;
            }

            var localX = x - CenterX;
            var localY = y - CenterY;

            IInteractiveElement? childElement = null;

            for (int i = 0; i < childs.Count; i++)
            {
                if (childs[i].HitTest(localX, localY, out childElement))
                {
                    break;
                }
            }

            if (childElement is not null)
            {
                interactiveElement = childElement;
                return true;
            }

            interactiveElement = this;
            return true;
        }

        public void OnClick(int cursorId, CursorKey cursorKey)
        {
            clickCallback?.Invoke(this);
        }

        public void OnEnter(int cursorId)
        {
            Debug.WriteLine($"{Tag}: Enter");
        }

        public void OnLeave(int cursorId)
        {
            Debug.WriteLine($"{Tag}: Leave");
        }

        public void OnMove(int cursorId, float x, float y)
        {
            Debug.WriteLine($"{Tag}: Move ({x},{y})");
        }

        public void OnDown(int cursorId, CursorKey cursorKey)
        {
            Debug.WriteLine($"{Tag}: Down ({cursorKey})");
        }

        public void OnUp(int cursorId, CursorKey cursorKey)
        {
            Debug.WriteLine($"{Tag}: Up ({cursorKey})");
        }

        public void OnWheel(int cursorId, int delta)
        {
            Debug.WriteLine($"{Tag}: Wheel ({delta})");
        }

        #endregion Public Methods

        #region Internal Methods

        internal void SetParent(InteractiveElement newParent)
        {
            if (parent != null && newParent == parent)
            {
                return;
            }

            if (parent != null)
            {
                if (!parent.RemoveChild(this))
                {
                    throw new InvalidOperationException("Expected existing child in parent.");
                }
            }

            parent = newParent;

            parent.AddChild(this);
        }

        #endregion Internal Methods

        #region Protected Methods

        protected bool Contains(float x, float y)
        {
            var localX = x - CenterX;
            var localY = y - CenterY;

            if (localX < -Width / 2.0f || localX > Width / 2.0f)
            {
                return false;
            }

            if (localY < -Height / 2.0f || localY > Height / 2.0f)
            {
                return false;
            }

            return true;
        }

        #endregion Protected Methods

        #region Private Methods

        private void AddChild(InteractiveElement child)
        {
            childs.Add(child);
        }

        private bool RemoveChild(InteractiveElement child)
        {
            return childs.Remove(child);
        }

        #endregion Private Methods
    }
}