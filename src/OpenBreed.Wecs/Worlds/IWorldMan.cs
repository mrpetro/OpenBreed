﻿using OpenBreed.Core;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.ObjectModel;

namespace OpenBreed.Wecs.Worlds
{
    public interface IWorldMan
    {
        #region Public Properties

        ReadOnlyCollection<World> Items { get; }

        #endregion Public Properties

        #region Public Methods

        WorldBuilder Create();

        World GetById(int id);

        World GetByName(string name);

        void RaiseEvent<T>(T eventArgs) where T : EventArgs;

        void Remove(World world);

        void Update(float dt);

        void Cleanup();

        void RegisterWorld(World newWorld);

        #endregion Public Methods
    }
}