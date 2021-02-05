using OpenBreed.Common;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core
{
    public class DefaultManagerCollection : IManagerCollection
    {
        #region Private Fields

        private readonly Dictionary<Type, object> managers = new Dictionary<Type, object>();

        #endregion Private Fields

        #region Public Methods

        public TManager GetManager<TManager>()
        {
            if (!managers.TryGetValue(typeof(TManager), out object manager))
                throw new InvalidOperationException($"Manager type '{typeof(TManager).Name}' not registered.");

            if (manager is Lazy<object>)
                return (TManager)((Lazy<object>)manager).Value;
            else if (manager is TManager)
                return (TManager)manager;
            else
                throw new InvalidOperationException();
        }

        public void AddSingleton<TInterface>(Func<object> initializer)
        {
            var managerType = typeof(TInterface);

            //if (!managerType.IsInterface)
            //    throw new InvalidOperationException("TInterface must be an IManager interface.");

            if (managers.ContainsKey(managerType))
                throw new InvalidOperationException($"Manager type '{managerType.Name}' already registered.");

            managers.Add(managerType, new Lazy<object>(initializer, System.Threading.LazyThreadSafetyMode.PublicationOnly));
        }

        public void AddSingleton<TInterface>(TInterface instance)
        {
            var managerType = typeof(TInterface);

            if (!managerType.IsInterface)
                throw new InvalidOperationException("TInterface must be an IManager interface.");

            if (managers.ContainsKey(managerType))
                throw new InvalidOperationException($"Manager type '{managerType.Name}' already registered.");

            managers.Add(managerType, instance);
        }

        #endregion Public Methods
    }
}