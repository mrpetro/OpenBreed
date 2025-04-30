using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Extensions;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Mathematics;
using System.Collections;

namespace OpenBreed.Gui
{
    internal class InteractionCursor : IInteractionCursor
    {
        #region Private Fields

        private readonly BitArray buttons = new BitArray(16);

        #endregion Private Fields

        #region Internal Constructors

        internal InteractionCursor(int id)
        {
            Id = id;
        }

        #endregion Internal Constructors

        #region Public Properties

        public IRenderView? View { get; internal set; }

        public bool Enabled { get; private set; }

        public Vector2 Position { get; private set; }

        public Vector2 PositionDelta { get; private set; }

        public int WheelDelta { get; private set; }

        public int Id { get; }

        #endregion Public Properties

        #region Internal Properties

        internal IElement? HoveredElement { get; set; }
        internal IElement? DownedElement { get; set; }
        internal Vector2? DownedPosition { get; set; }

        #endregion Internal Properties

        #region Public Methods

        public bool IsPressed(CursorKey key)
        {
            return buttons[(int)key];
        }

        public Vector2 GetPositionRelativeTo(IElement element)
        {
            return element.GetLocalPosition(Position);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void UpdateEnabled(bool enabled)
        {
            Enabled = enabled;
        }

        internal void UpdateButton(CursorKey key, bool pressed)
        {
            buttons[(int)key] = pressed;
        }

        internal void UpdatePosition(Vector2 position)
        {
            PositionDelta = position - Position;
            Position = position;
        }

        internal void UpdateWheel(int wheelDelta)
        {
            WheelDelta = wheelDelta;
        }

        #endregion Internal Methods
    };
}