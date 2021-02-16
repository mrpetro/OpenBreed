using System;

namespace OpenBreed.Common
{
    public interface IManagerCollection
    {
        #region Public Methods

        TManager GetManager<TManager>();

        object GetManager(Type type);

        void AddSingleton<TInterface>(Func<object> initializer);

        void AddTransient<TInterface>(Func<object> initializer);

        void AddSingleton<TInterface>(TInterface instance);

        #endregion Public Methods
    }
}