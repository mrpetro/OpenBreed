using System;

namespace OpenBreed.Wecs.Systems
{
    public interface ISystemFactory
    {
        #region Public Methods

        void Register<TSystem>(Func<ISystem> initializer) where TSystem : ISystem;

        TSystem Create<TSystem>() where TSystem : ISystem;

        #endregion Public Methods
    }
}