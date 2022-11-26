using OpenBreed.Wecs.Entities;
using System;

namespace OpenBreed.Wecs.Worlds
{
    public delegate void EntitiyLeft(IEntity entity, IWorld world);

    public delegate void EntitiyEntered(IEntity entity, IWorld world);

    public interface IWorldMan
    {
        #region Public Events

        event EntitiyEntered EntitiyEntered;

        event EntitiyLeft EntitiyLeft;

        #endregion Public Events

        #region Public Methods

        WorldBuilder Create();

        IWorld GetById(int id);

        IWorld GetByName(string name);

        void Remove(IWorld world);

        void Update(float dt);

        //void RegisterWorld(IWorld newWorld);

        #endregion Public Methods
    }
}