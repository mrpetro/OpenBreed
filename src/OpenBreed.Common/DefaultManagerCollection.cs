using System;
using System.Collections.Generic;

namespace OpenBreed.Common
{
    public class DefaultManagerCollection : IManagerCollection
    {
        #region Private Fields

        private readonly Dictionary<Type, IManager> managerTypes = new Dictionary<Type, IManager>();
        private readonly DefaultManagerScope scope;

        #endregion Private Fields

        #region Public Constructors

        public DefaultManagerCollection()
        {
        }

        #endregion Public Constructors

        #region Internal Constructors

        internal DefaultManagerCollection(DefaultManagerScope scope)
        {
            this.scope = scope;
        }

        #endregion Internal Constructors

        #region Private Interfaces

        private interface IManager
        {
            #region Public Methods

            object GetInstance();

            #endregion Public Methods
        }

        #endregion Private Interfaces

        #region Public Methods

        public IManagerScope CreateScope()
        {
            return new DefaultManagerScope(this);
        }

        public TManager GetManager<TManager>()
        {
            return (TManager)GetManager(typeof(TManager));
        }

        public object GetManager(Type type)
        {
            if (!managerTypes.TryGetValue(type, out IManager manager))
                throw new InvalidOperationException($"Manager type '{type.Name}' not registered.");

            return manager.GetInstance();
        }

        public void AddSingleton<TInterface>(Func<object> initializer)
        {
            AddManager(typeof(TInterface), new LazySingletonManager(new Lazy<object>(initializer, System.Threading.LazyThreadSafetyMode.PublicationOnly)));
        }

        public void AddTransient<TInterface>(Func<object> initializer)
        {
            AddManager(typeof(TInterface), new TransientManager(initializer));
        }

        public void AddSingleton<TInterface>(TInterface instance)
        {
            AddManager(typeof(TInterface), new SingletonManager(instance));
        }

        public void AddScoped<TInterface>(Func<object> initializer) where TInterface : IDisposable
        {
            AddManager(typeof(TInterface), new ScopedManager(scope, initializer));
        }

        #endregion Public Methods

        #region Private Methods

        private void AddManager(Type managerType, IManager manager)
        {
            if (managerTypes.ContainsKey(managerType))
                throw new InvalidOperationException($"Manager type '{managerType.Name}' already registered.");

            managerTypes.Add(managerType, manager);
        }

        #endregion Private Methods

        #region Private Classes

        private class TransientManager : IManager
        {
            #region Private Fields

            private readonly Func<object> initializer;

            #endregion Private Fields

            #region Public Constructors

            public TransientManager(Func<object> initializer)
            {
                this.initializer = initializer;
            }

            #endregion Public Constructors

            #region Public Methods

            public object GetInstance() => initializer.Invoke();

            #endregion Public Methods
        }

        private class ScopedManager : IManager
        {
            #region Private Fields

            private readonly DefaultManagerScope scope;

            private readonly Func<object> initializer;

            #endregion Private Fields

            #region Public Constructors

            public ScopedManager(DefaultManagerScope scope, Func<object> initializer)
            {
                this.scope = scope;
                this.initializer = initializer;
            }

            #endregion Public Constructors

            #region Public Methods

            public object GetInstance()
            {
                if (scope == null)
                    throw new InvalidOperationException("Scoped manager can only exist within scope.");

                var instance = initializer.Invoke();
                scope.Register((IDisposable)instance);
                return instance;
            }


            #endregion Public Methods
        }

        private class SingletonManager : IManager
        {
            #region Private Fields

            private readonly object instance;

            #endregion Private Fields

            #region Public Constructors

            public SingletonManager(object instance)
            {
                this.instance = instance;
            }

            #endregion Public Constructors

            #region Public Methods

            public object GetInstance() => instance;

            #endregion Public Methods
        }

        private class LazySingletonManager : IManager
        {
            #region Private Fields

            private readonly Lazy<object> lazyInitializer;

            #endregion Private Fields

            #region Public Constructors

            public LazySingletonManager(Lazy<object> initializer)
            {
                this.lazyInitializer = initializer;
            }

            #endregion Public Constructors

            #region Public Methods

            public object GetInstance() => lazyInitializer.Value;

            #endregion Public Methods
        }

        #endregion Private Classes
    }

    internal class DefaultManagerScope : IManagerScope
    {
        #region Private Fields

        private bool disposedValue;
        private IManagerCollection managerCollection;

        private readonly List<IDisposable> toDispose = new List<IDisposable>();

        #endregion Private Fields

        #region Public Constructors

        public DefaultManagerScope(IManagerCollection managerCollection)
        {
            this.managerCollection = managerCollection;

            Provider = new DefaultManagerCollection(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public IManagerCollection Provider { get; }

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    toDispose.ForEach(item => item.Dispose());
                }

                disposedValue = true;
            }
        }

        internal void Register(IDisposable disposable)
        {
            if (toDispose.Contains(disposable))
                throw new InvalidOperationException("Instance already added to dispose list.");

            toDispose.Add(disposable);
        }

        #endregion Protected Methods
    }
}