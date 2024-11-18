using Microsoft.Extensions.Logging;
using OpenBreed.Gui.Interface.Builders;
using OpenBreed.Gui.Interface.Elements;
using OpenBreed.Rendering.Interface.Events;
using OpenTK.Mathematics;
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

        private readonly Dictionary<int, IElement> hoveredElements = new Dictionary<int, IElement>();
        private readonly Dictionary<int, Dictionary<CursorKey, IElement>> downElemenets = new Dictionary<int, Dictionary<CursorKey, IElement>>(); 


        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public InteractionCore(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Properties

        public IElement? Root { get; set; }

        #endregion Public Properties

        #region Public Methods

        public IElementBuilder CreatePanel(Action<IPanelBuilder> setter)
        {
            var builder = new PanelBuilder(null);

            setter.Invoke(builder);

            return builder;
        }

        public bool HitTest(float x, float y, out IElement? interactiveElement)
        {
            if (Root is null)
            {
                interactiveElement = null;
                return false;
            }

            return Root.HitTest(new Vector2(x, y), out interactiveElement);
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

            IElement? hoveredElement = null;

            hoveredElements.TryGetValue(cursorId, out hoveredElement);

            if (HitTest(x, y, out IElement? element) && element is not null)
            {
                if (element != hoveredElement)
                {
                    hoveredElements[cursorId] = element;

                    if (hoveredElement is not null)
                    {
                        hoveredElement.OnLeave(cursorId);
                    }

                    logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Enter", element.Tag, cursorId);
                    element.OnEnter(cursorId);
                }

                logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Move ({CursorX}, {CursorY})", element.Tag, cursorId, x, y);
                element.OnMove(cursorId, new Vector2(x, y));
            }
            else
            {
                if (hoveredElement is not null)
                {
                    hoveredElements.Remove(cursorId);

                    logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Leave", hoveredElement.Tag, cursorId);
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

            if (HitTest(x, y, out IElement? element) && element is not null)
            {
                if (!downElemenets.TryGetValue(cursorId, out Dictionary<CursorKey, IElement> keyLookup))
                {
                    keyLookup = new Dictionary<CursorKey, IElement>();
                    downElemenets.Add(cursorId, keyLookup);
                }

                keyLookup[cursorKey] = element;

                logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Down ({CursorKey})", element.Tag, cursorId, cursorKey);
                element.OnDown(cursorId, new Vector2(x, y), cursorKey);
            }
        }

        private bool TryGetDownedElement(int cursorId, CursorKey cursorKey, out Dictionary<CursorKey, IElement>? keyLookup, out IElement? element)
        {
            if (!downElemenets.TryGetValue(cursorId, out keyLookup))
            {
                element = null;
                return false;
            }

            if (!keyLookup.TryGetValue(cursorKey, out element))
            {
                return false;
            }

            return true;
        }

        public void Up(int cursorId, float x, float y, CursorKey cursorKey)
        {
            if (Root is null)
            {
                return;
            }

            if (TryGetDownedElement(cursorId, cursorKey, out Dictionary<CursorKey, IElement>? lookup, out IElement? downedElement))
            {
                lookup.Remove(cursorKey);

                logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Up ({CursorKey})", downedElement.Tag, cursorId, cursorKey);
                downedElement.OnUp(cursorId, new Vector2(x, y), cursorKey);
            }

            if (HitTest(x, y, out IElement? element) && element is not null)
            {
                if (downedElement == element)
                {
                    logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Click ({CursorKey})", element.Tag, cursorId, cursorKey);
                    element.OnClick(cursorId, cursorKey);
                }
            }
        }

        public void Wheel(int cursorId, float x, float y, int delta)
        {
            if (Root is null)
            {
                return;
            }

            if (HitTest(x, y, out IElement? element) && element is not null)
            {
                logger.LogTrace("UI->{ElementTag}: Cursor {CursorId} Wheel ({WheelDelta})", element.Tag, cursorId, delta);
                element.OnWheel(cursorId, delta);
            }
        }

        #endregion Public Methods
    }
}