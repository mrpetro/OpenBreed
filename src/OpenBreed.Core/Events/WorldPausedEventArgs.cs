﻿using System;

namespace OpenBreed.Core.Events
{
    public class WorldUnpausedEventArgs : EventArgs
    {
        #region Public Constructors

        public WorldUnpausedEventArgs(int worldId)
        {
            WorldId = worldId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int WorldId { get; }

        #endregion Public Properties
    }
}