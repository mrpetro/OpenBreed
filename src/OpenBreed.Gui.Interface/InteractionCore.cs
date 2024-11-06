using OpenBreed.Gui.Interface.Builders;
using OpenBreed.Rendering.Interface.Events;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface
{
    internal class InteractionCore : IInteractionCore
    {
        #region Private Fields

        private readonly Dictionary<int, IInteractiveElement> hoveredElements = new Dictionary<int, IInteractiveElement>();

        #endregion Private Fields

        #region Public Properties

        public IInteractiveElement? Root { get; set; }

        #endregion Public Properties

        #region Public Methods

        public IInteractiveLabelBuilder BeginLabel()
        {
            return new InteractiveLabelBuilder(this, null);
        }

        public IInteractivePanelBuilder BeginPanel()
        {
            return new InteractivePanelBuilder(this, null);
        }

        public bool HitTest(float x, float y, out IInteractiveElement? interactiveElement)
        {
            if (Root is null)
            {
                interactiveElement = null;
                return false;
            }

            return Root.HitTest(x, y, out interactiveElement);
        }

        public void Click(int cursorId, float x, float y, CursorKey cursorKey)
        {
            if (Root is null)
            {
                return;
            }

            Debug.WriteLine("Click");
        }

        public void Enter(int cursorId, float x, float y)
        {
            if (Root is null)
            {
                return;
            }

            Debug.WriteLine("Enter");
        }

        public void Leave(int cursorId)
        {
            if (Root is null)
            {
                return;
            }

            Debug.WriteLine("Leave");
        }

        public void Move(int cursorId, float x, float y)
        {
            if (Root is null)
            {
                return;
            }

            IInteractiveElement? hoveredElement = null;

            hoveredElements.TryGetValue(cursorId, out hoveredElement);

            if (HitTest(x, y, out IInteractiveElement? element) && element is not null)
            {
                if (element != hoveredElement)
                {
                    hoveredElements[cursorId] = element;

                    if (hoveredElement is not null)
                    {
                        hoveredElement.OnLeave(cursorId);
                    }

                    element.OnEnter(cursorId);
                }

                element.OnMove(cursorId, x, y);
            }
            else
            {
                if (hoveredElement is not null)
                {
                    hoveredElements.Remove(cursorId);
                    hoveredElement.OnLeave(cursorId);
                }
            }
        }

        public void Down(int cursorId, float x, float y, CursorKey cursorKey)
        {
            if (Root is null)
            {
                return;
            }

            if (HitTest(x, y, out IInteractiveElement? element) && element is not null)
            {
                element.OnDown(cursorId, cursorKey);
            }
        }

        public void Up(int cursorId, float x, float y, CursorKey cursorKey)
        {
            if (Root is null)
            {
                return;
            }

            if (HitTest(x, y, out IInteractiveElement? element) && element is not null)
            {
                element.OnUp(cursorId, cursorKey);
            }
        }

        public void Wheel(int cursorId, float x, float y, int delta)
        {
            if (Root is null)
            {
                return;
            }

            if (HitTest(x, y, out IInteractiveElement? element) && element is not null)
            {
                element.OnWheel(cursorId, delta);
            }
        }

        #endregion Public Methods
    }
}