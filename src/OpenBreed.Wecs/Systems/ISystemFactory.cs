using OpenBreed.Wecs.Worlds;
using System;

namespace OpenBreed.Wecs.Systems
{
    public interface ISystemFactory
    {
        #region Public Methods

        void Register<TSystem>(Func<IWorld, ISystem> initializer) where TSystem : ISystem;

        ISystem Create<TSystem>(IWorld world) where TSystem : ISystem;

        #endregion Public Methods
    }
}