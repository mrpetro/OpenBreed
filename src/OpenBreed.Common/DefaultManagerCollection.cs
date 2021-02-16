using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common
{
    public class DefaultManagerCollection : IManagerCollection
    {
        #region Private Fields

        private readonly Dictionary<Type, object> singletons = new Dictionary<Type, object>();

        #endregion Private Fields

        #region Public Methods

        public TManager GetManager<TManager>()
        {
            if (!singletons.TryGetValue(typeof(TManager), out object manager))
                throw new InvalidOperationException($"Manager type '{typeof(TManager).Name}' not registered.");

            if (manager is Lazy<object>)
                return (TManager)((Lazy<object>)manager).Value;
            else if (manager is TManager)
                return (TManager)manager;
            else if (manager is Func<object>)
                return (TManager)((Func<object>)manager).Invoke();
            else
                throw new InvalidOperationException();
        }

        public object GetManager(Type type)
        {
            if (!singletons.TryGetValue(type, out object manager))
                throw new InvalidOperationException($"Manager type '{type.Name}' not registered.");

            if (manager is Lazy<object>)
                return ((Lazy<object>)manager).Value;
            else if (manager is Func<object>)
                return ((Func<object>)manager).Invoke();
            else if (manager is object)
                return manager;
            else
                throw new InvalidOperationException();
        }

        public void AddSingleton<TInterface>(Func<object> initializer)
        {
            var managerType = typeof(TInterface);

            //if (!managerType.IsInterface)
            //    throw new InvalidOperationException("TInterface must be an IManager interface.");

            if (singletons.ContainsKey(managerType))
                throw new InvalidOperationException($"Manager type '{managerType.Name}' already registered.");

            singletons.Add(managerType, new Lazy<object>(initializer, System.Threading.LazyThreadSafetyMode.PublicationOnly));
        }

        public void AddTransient<TInterface>(Func<object> initializer)
        {
            var managerType = typeof(TInterface);

            //if (!managerType.IsInterface)
            //    throw new InvalidOperationException("TInterface must be an IManager interface.");

            if (singletons.ContainsKey(managerType))
                throw new InvalidOperationException($"Manager type '{managerType.Name}' already registered.");

            singletons.Add(managerType, initializer);
        }

        public void AddSingleton<TInterface>(TInterface instance)
        {
            var managerType = typeof(TInterface);

            if (!managerType.IsInterface)
                throw new InvalidOperationException("TInterface must be an IManager interface.");

            if (singletons.ContainsKey(managerType))
                throw new InvalidOperationException($"Manager type '{managerType.Name}' already registered.");

            singletons.Add(managerType, instance);
        }

        #endregion Public Methods
    }
}
