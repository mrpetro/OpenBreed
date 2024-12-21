using Microsoft.Extensions.Logging;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Builders;
using OpenBreed.Rendering.Interface.Events;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui
{
    internal class InteractionCore : IInteractionCore
    {
        #region Private Fields

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

        public ICursorInputHandler CreateCursorInput()
        {
            return new CursorInput(this);
        }

        public IElementBuilder CreateDockPanel(Action<IDockPanelBuilder> setter)
        {
            var builder = new DockPanelBuilder(null);

            setter.Invoke(builder);

            return builder;
        }

        public bool HitTest(Vector2 position, out IElement? interactiveElement)
        {
            if (Root is null)
            {
                interactiveElement = null;
                return false;
            }

            return Root.HitTest(position, out interactiveElement);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void OnCursorEnter(IInteractionCursor cursor)
        {
            if (Root is null)
            {
                return;
            }

            Debug.WriteLine("Enter");
        }

        internal void OnCursorLeave(InteractionCursor cursor)
        {
            if (Root is null)
            {
                return;
            }

            Debug.WriteLine("Leave");
        }

        internal void OnCursorMove(InteractionCursor cursor)
        {
            if (Root is null)
            {
                return;
            }

            var cursorId = cursor.Id;
            var position = cursor.Position;

            if (cursor.DownedElement is not null && cursor.DownedElement.IsMovable)
            {
                cursor.DownedElement.MoveBy(cursor.PositionDelta);
            }

            if (HitTest(position, out IElement? element) && element is not null)
            {
                if (element != cursor.HoveredElement)
                {
                    var hoveredElement = cursor.HoveredElement;

                    cursor.HoveredElement = element;

                    if (hoveredElement is not null)
                    {
                        hoveredElement.OnLeave(cursor);
                    }

                    logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Enter", element.Tag, cursorId);
                    element.OnEnter(cursor);
                }

                logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Move ({Position})", element.Tag, cursorId, position);
                element.OnCursorMove(cursor);

                return;
            }

            if (cursor.HoveredElement is not null)
            {
                var hoveredElement = cursor.HoveredElement;

                cursor.HoveredElement = null;

                logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Leave", hoveredElement.Tag, cursorId);
                hoveredElement.OnLeave(cursor);
            }
        }

        internal void OnCursorDown(InteractionCursor cursor, CursorKey key)
        {
            if (Root is null)
            {
                return;
            }

            var cursorId = cursor.Id;
            var position = cursor.Position;

            if (HitTest(position, out IElement? element) && element is not null)
            {
                cursor.DownedElement = element;

                logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Down ({CursorKey})", element.Tag, cursorId, key);
                element.OnDown(cursor, key);
            }
        }

        internal void OnCursorUp(InteractionCursor cursor, CursorKey cursorKey)
        {
            if (Root is null)
            {
                return;
            }

            var cursorId = cursor.Id;
            var position = cursor.Position;

            var downedElement = cursor.DownedElement;

            if (cursor.DownedElement is not null)
            {
                logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Up ({CursorKey})", cursor.DownedElement.Tag, cursorId, cursorKey);

                cursor.DownedElement = null;

                downedElement?.OnUp(cursor, cursorKey);
            }

            if (HitTest(position, out IElement? element) && element is not null)
            {
                if (downedElement == element)
                {
                    logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Click ({CursorKey})", element.Tag, cursorId, cursorKey);
                    element.OnClick(cursor, cursorKey);
                }
            }
        }

        internal void OnCursorWheel(InteractionCursor cursor)
        {
            if (Root is null)
            {
                return;
            }

            var cursorId = cursor.Id;
            var position = cursor.Position;

            if (HitTest(position, out IElement? element) && element is not null)
            {
                logger.LogTrace("UI->{ElementTag}: Cursor {CursorId} Wheel ({WheelDelta})", element.Tag, cursorId, cursor.WheelDelta);
                element.OnWheel(cursor);
            }
        }

        #endregion Internal Methods
    }
}