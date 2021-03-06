﻿using OpenBreed.Core;
using System;

namespace OpenBreed.Wecs.Events
{
    /// <summary>
    /// Event args for event that occurs when world is deinitialized
    /// </summary>
    public class WorldDeinitializedEventArgs : EventArgs
    {
        #region Public Constructors

        public WorldDeinitializedEventArgs(int worldId)
        {
            WorldId = worldId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int WorldId { get; }

        #endregion Public Properties
    }
}