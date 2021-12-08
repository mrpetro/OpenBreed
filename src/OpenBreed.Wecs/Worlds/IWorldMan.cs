using OpenBreed.Core;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.ObjectModel;

namespace OpenBreed.Wecs.Worlds
{
    public interface IWorldMan
    {
        #region Public Properties

        #endregion Public Properties

        #region Public Methods

        WorldBuilder Create();

        World GetById(int id);

        World GetByName(string name);

        void RaiseEvent<T>(T eventArgs) where T : EventArgs;

        void Remove(World world);

        void Update(float dt);

        void RegisterWorld(World newWorld);

        #endregion Public Methods
    }
}