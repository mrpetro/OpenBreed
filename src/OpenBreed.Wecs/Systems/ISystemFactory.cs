using System;

namespace OpenBreed.Wecs.Systems
{
    public interface ISystemFactory
    {
        #region Public Methods

        void Register<TSystem>(Func<TSystem> initializer) where TSystem : ISystem;

        TSystem Create<TSystem>() where TSystem : ISystem;

        #endregion Public Methods
    }
}