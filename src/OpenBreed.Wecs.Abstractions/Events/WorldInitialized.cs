using System;

namespace OpenBreed.Wecs.Abstractions.Events
{
    /// <summary>
    /// Event that occurs when world is initialized
    /// </summary>
    public class WorldInitialized : WorldEvent
    {
        #region Public Constructors

        public WorldInitialized(int worldId) : base(worldId)
        {
        }

        #endregion Public Constructors
    }
}