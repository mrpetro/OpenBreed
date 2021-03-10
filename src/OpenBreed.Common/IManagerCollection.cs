using System;

namespace OpenBreed.Common
{
    public interface IManagerScope : IDisposable
    {
        #region Public Properties

        IManagerCollection Provider { get; }

        #endregion Public Properties
    }

    public interface IManagerCollection
    {
        #region Public Methods

        TManager GetManager<TManager>();

        IManagerScope CreateScope();

        object GetManager(Type type);

        void AddSingleton<TInterface>(Func<object> initializer);

        void AddTransient<TInterface>(Func<object> initializer);

        void AddSingleton<TInterface>(TInterface instance);

        void AddScoped<TInterface>(Func<object> initializer) where TInterface : IDisposable;

        #endregion Public Methods
    }
}