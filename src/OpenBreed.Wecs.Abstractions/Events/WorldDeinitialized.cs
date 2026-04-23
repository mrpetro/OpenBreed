using System;

namespace OpenBreed.Wecs.Abstractions.Events
{
    /// <summary>
    /// Event that occurs when world is deinitialized
    /// </summary>
    public class WorldDeinitialized : WorldEvent
    {
        #region Public Constructors

        public WorldDeinitialized(int worldId) : base(worldId)
        {
        }

        #endregion Public Constructors
    }
}