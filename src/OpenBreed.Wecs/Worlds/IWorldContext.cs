﻿namespace OpenBreed.Wecs.Worlds
{
    public interface IWorldContext
    {
        #region Public Properties

        float Dt { get; }

        float DtMultiplier { get; }

        bool Paused { get; }

        World World { get; }

        #endregion Public Properties
    }
}