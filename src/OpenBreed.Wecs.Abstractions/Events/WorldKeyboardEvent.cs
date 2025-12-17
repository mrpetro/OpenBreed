using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace OpenBreed.Wecs.Abstractions.Events
{
    public class WorldKeyboardEvent : EventArgs
    {
        #region Public Constructors

        public WorldKeyboardEvent(int worldId, KeyboardState oldState, KeyboardState newState)
        {
            WorldId = worldId;
            OldState = oldState;
            NewState = newState;
        }

        #endregion Public Constructors

        #region Public Properties

        public int WorldId { get; }
        public KeyboardState NewState { get; }
        public KeyboardState OldState { get; }

        #endregion Public Properties

        #region Public Methods

        public bool IsKeyDown(Keys key)
        {
            return !OldState[key] && NewState[key];
        }

        public bool IsKeyUp(Keys key)
        {
            return OldState[key] && !NewState[key];
        }

        #endregion Public Methods
    }
}