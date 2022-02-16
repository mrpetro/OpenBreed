using System;

namespace OpenBreed.Wecs.Events
{
    /// <summary>
    /// Event args for event that occurs when is removed from world
    /// </summary>
    public class EntityLeftEventArgs : EventArgs
    {
        #region Public Constructors

        public EntityLeftEventArgs(int worldId)
        {
            WorldId = worldId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int WorldId { get; }

        #endregion Public Properties
    }
}