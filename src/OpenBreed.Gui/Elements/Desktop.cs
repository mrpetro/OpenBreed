using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenBreed.Core.Interface;
using OpenBreed.Core.Interface.Extensions;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Extensions;
using OpenBreed.Gui.Abstractions.Helpers;
using OpenBreed.Gui.Abstractions.Rendering;
using OpenBreed.Gui.Builders;
using OpenBreed.Gui.Rendering;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Mathematics;

namespace OpenBreed.Gui.Elements
{
    internal class Desktop : DockPanel, IDesktop
    {
        #region Private Fields

        private readonly Dictionary<int, InteractionCursor> cursors = new Dictionary<int, InteractionCursor>();
        private readonly IRenderView renderView;
        private readonly IInteractionRenderer interactionRenderer;
        private readonly ILogger logger;
        private readonly IRenderLayer renderLayer;

        #endregion Private Fields

        #region Internal Constructors

        internal Desktop(DesktopBuilder builder) : base(builder)
        {
            renderView = builder.RenderView;
            interactionRenderer = builder.InteractionRenderer;
            logger = builder.Logger;

            renderLayer = renderView.CreateLayer(OnViewRender);
            renderView.Resized += RenderView_Resized;
            renderView.CursorMove += RenderView_CursorMove;
            renderView.CursorDown += RenderView_CursorDown;
            renderView.CursorUp += RenderView_CursorUp;
            renderView.CursorEnter += RenderView_CursorEnter;
            renderView.CursorLeave += RenderView_CursorLeave;
            renderView.CursorWheel += RenderView_CursorWheel;
            renderView.TextInput += RenderView_TextInput;
            renderView.KeyDown += RenderView_KeyDown;
            renderView.KeyUp += RenderView_KeyUp;
        }

        #endregion Internal Constructors

        #region Public Properties

        public IElement? FocussedElement { get; private set; }
        public IReadOnlyCollection<IInteractionCursor> Cursors => cursors.Values;

        #endregion Public Properties

        #region Internal Methods

        internal InteractionCursor GetCursor(int cursorId)
        {
            if (!cursors.TryGetValue(cursorId, out InteractionCursor? cursor))
            {
                cursor = new InteractionCursor(cursorId);

                cursors.Add(cursorId, cursor);
            }

            return cursor;
        }

        internal void ResolveCursorEnter(IInteractionCursor cursor)
        {
            //Debug.WriteLine("Enter");
        }

        internal void ResolveCursorLeave(InteractionCursor cursor)
        {
            //Debug.WriteLine("Leave");
        }

        internal void ResolveCursorMove(InteractionCursor cursor)
        {
            var cursorId = cursor.Id;
            var position = cursor.Position;

            if (cursor.DownedElement is not null)
            {
                var downedPosition  = cursor.DownedPosition ?? Vector2.Zero;

                cursor.DownedElement.InputController.OnCursorDrag(cursor.DownedElement, cursor);
            }

            if (HitTest(position, out IElement? element) && element is not null)
            {
                if (element != cursor.HoveredElement)
                {
                    var hoveredElement = cursor.HoveredElement;

                    cursor.HoveredElement = element;

                    if (hoveredElement is not null)
                    {
                        hoveredElement.InputController.OnCursorLeave(hoveredElement, cursor);
                    }

                    logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Enter", element.Tag, cursorId);
                    element.InputController.OnCursorEnter(element, cursor);
                }

                logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Move ({Position})", element.Tag, cursorId, position);

                var worldBox = element.ToWorld(element.Position.AsVector());

                logger.LogTrace("WorldPos ({Box})", worldBox);

                element.InputController.OnCursorMove(element, cursor);

                return;
            }

            if (cursor.HoveredElement is not null)
            {
                var hoveredElement = cursor.HoveredElement;
                cursor.HoveredElement = null;

                logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Leave", hoveredElement.Tag, cursorId);
                hoveredElement.InputController.OnCursorLeave(hoveredElement, cursor);
            }
        }

        internal void ResolveCursorDown(InteractionCursor cursor, CursorKey key)
        {
            var cursorId = cursor.Id;
            var position = cursor.Position;

            if (HitTest(position, out IElement? element) && element is not null)
            {
                cursor.DownedElement = element;
                cursor.DownedPosition = position;

                logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Down ({CursorKey})", element.Tag, cursorId, key);
                element.InputController.OnCursorDown(element, cursor, key);
            }
        }

        internal void ResolveCursorUp(InteractionCursor cursor, CursorKey cursorKey)
        {
            var cursorId = cursor.Id;
            var position = cursor.Position;

            var downedElement = cursor.DownedElement;

            if (cursor.DownedElement is not null)
            {
                logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Up ({CursorKey})", cursor.DownedElement.Tag, cursorId, cursorKey);

                cursor.DownedElement = null;

                downedElement?.InputController.OnCursorUp(downedElement, cursor, cursorKey);
            }

            if (HitTest(position, out IElement? element) && element is not null)
            {
                if (downedElement == element)
                {
                    logger.LogTrace("UI->{ElementTag}: Cursor.{CursorId} Click ({CursorKey})", element.Tag, cursorId, cursorKey);
                    element.InputController.OnCursorClick(element, cursor, cursorKey);
                }
            }
        }

        internal void ResolveCursorWheel(InteractionCursor cursor)
        {
            var cursorId = cursor.Id;
            var position = cursor.Position;

            if (HitTest(position, out IElement? element) && element is not null)
            {
                logger.LogTrace("UI->{ElementTag}: Cursor {CursorId} Wheel ({WheelDelta})", element.Tag, cursorId, cursor.WheelDelta);
                element.InputController.OnCursorWheel(element, cursor);
            }
        }

        internal void ResolveTextInput(string text)
        {
            FocussedElement?.InputController.OnKeyboardTextInput(FocussedElement, text);
        }

        internal void ResolveKeyDown(Keys keys, KeyModifiers modifiers)
        {
            FocussedElement?.InputController.OnKeyboardKeyDown(FocussedElement, keys, modifiers);
        }

        internal void ResolveKeyUp(Keys keys, KeyModifiers modifiers)
        {
            FocussedElement?.InputController.OnKeyboardKeyUp(FocussedElement, keys, modifiers);
        }

        internal void OnFocus(IElement element)
        {
            if (FocussedElement is not null)
            {
                FocussedElement.Unfocus();
            }

            FocussedElement = element;
        }

        #endregion Internal Methods

        #region Private Methods

        private void OnViewRender(IRenderView view, float dt)
        {  
            view.Reset();

            interactionRenderer.Render(this, view);
        }

        private void RenderView_Resized(IRenderView view, float width, float height)
        {
            this.Position.X = view.Box.Center.X;
            this.Position.Y = view.Box.Center.Y;

            Resize(new Vector2(width, height));
        }

        private void RenderView_CursorWheel(IRenderView view, int cursorId, Vector2i position, int wheelDelta)
        {
            var cursor = GetCursor(cursorId);

            cursor.UpdateWheel(wheelDelta);

            ResolveCursorWheel(cursor);
        }

        private void RenderView_CursorLeave(IRenderView view, int cursorId, Vector2i position)
        {
            var cursor = GetCursor(cursorId);
            cursor.View = null;
            cursor.UpdateEnabled(enabled: false);

            ResolveCursorLeave(cursor);
        }

        private void RenderView_CursorEnter(IRenderView view, int cursorId, Vector2i position)
        {
            var cursor = GetCursor(cursorId);
            cursor.View = view;
            cursor.UpdateEnabled(enabled: true);

            ResolveCursorEnter(cursor);
        }

        private void RenderView_CursorUp(IRenderView view, int cursorId, Vector2i position, CursorKey cursorKey)
        {
            var cursor = GetCursor(cursorId);

            cursor.UpdateButton(cursorKey, pressed: false);

            ResolveCursorUp(cursor, cursorKey);
        }

        private void RenderView_CursorDown(IRenderView view, int cursorId, Vector2i position, CursorKey cursorKey)
        {
            var cursor = GetCursor(cursorId);

            cursor.UpdateButton(cursorKey, pressed: true);

            ResolveCursorDown(cursor, cursorKey);
        }

        private void RenderView_CursorMove(IRenderView view, int cursorId, Vector2i position, BitArray cursorKeyStates, KeyModifiers modifiers)
        {
            var cursor = GetCursor(cursorId);

            cursor.UpdatePosition(position);

            ResolveCursorMove(cursor);
        }

        private void RenderView_TextInput(IRenderView view, string text)
        {
            ResolveTextInput(text);
        }

        private void RenderView_KeyUp(IRenderView view, Keys key, KeyModifiers modifiers)
        {
            ResolveKeyUp(key, modifiers);
        }

        private void RenderView_KeyDown(IRenderView view, Keys key, KeyModifiers modifiers)
        {
            ResolveKeyDown(key, modifiers);
        }

        #endregion Private Methods
    }
}