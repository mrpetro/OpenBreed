using OpenBreed.Wecs.Worlds;
using System;

namespace OpenBreed.Wecs.Systems
{
    /// <summary>
    /// Interface of system factory
    /// </summary>
    public interface ISystemFactory
    {
        #region Public Methods

        ISystem CreateSystem<TSystem>() where TSystem : ISystem;

        void RegisterSystem<TSystem>(Func<ISystem> initializer) where TSystem : ISystem;

        #endregion Public Methods
    }
}