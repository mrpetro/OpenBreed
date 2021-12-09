using OpenBreed.Wecs.Entities;
using System;

namespace OpenBreed.Wecs.Worlds
{
    public delegate void EntitiyLeft(Entity entity, World world);

    public delegate void EntitiyEntered(Entity entity, World world);

    public interface IWorldMan
    {
        #region Public Events

        event EntitiyEntered EntitiyEntered;

        event EntitiyLeft EntitiyLeft;

        #endregion Public Events

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