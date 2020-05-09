using OpenBreed.Core.Common;
using System;

namespace OpenBreed.Core.Events
{
    public class WorldPausedEventArgs : EventArgs
    {
        #region Private Constructors

        public WorldPausedEventArgs(World world)
        {
            World = world;
        }

        #endregion Private Constructors

        #region Public Properties

        public World World { get; }

        #endregion Public Properties
    }
}