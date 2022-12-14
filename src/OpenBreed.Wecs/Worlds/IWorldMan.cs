using OpenBreed.Wecs.Entities;
using System;

namespace OpenBreed.Wecs.Worlds
{
    public interface IWorldMan
    {
        #region Public Events

        #endregion Public Events

        #region Public Methods

        IWorldBuilder Create();

        IWorld GetById(int id);

        IWorld GetByName(string name);

        void Remove(IWorld world);

        void Update(float dt);

        //void RegisterWorld(IWorld newWorld);

        #endregion Public Methods
    }
}