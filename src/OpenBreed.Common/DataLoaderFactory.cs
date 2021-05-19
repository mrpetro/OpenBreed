using System;
using System.Collections.Generic;

namespace OpenBreed.Common
{
    public class DataLoaderFactory : IDataLoaderFactory
    {
        #region Private Fields

        private readonly Dictionary<Type, IDataLoader> loaders = new Dictionary<Type, IDataLoader>();

        #endregion Private Fields

        #region Public Constructors

        public DataLoaderFactory()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public IDataLoader<TInterface> GetLoader<TInterface>()
        {
            if (loaders.TryGetValue(typeof(TInterface), out IDataLoader loader))
                return (IDataLoader<TInterface>)loader;
            else
                throw new InvalidOperationException($"Loader for type '{typeof(TInterface)}' is not registered");
        }

        public void Register<TInterface>(IDataLoader<TInterface> dataLoader)
        {
            loaders.Add(typeof(TInterface), dataLoader);
        }

        #endregion Public Methods
    }
}