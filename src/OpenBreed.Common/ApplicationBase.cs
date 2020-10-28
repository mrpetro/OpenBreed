using System;
using System.Collections.Generic;

namespace OpenBreed.Common
{
    public delegate IApplicationInterface InterfaceInitializer();

    public abstract class ApplicationBase : IApplication
    {
        #region Private Fields

        private Dictionary<Type, InterfaceContainer> interfaces = new Dictionary<Type, InterfaceContainer>();

        #endregion Private Fields

        #region Protected Constructors

        protected ApplicationBase()
        {
            ServiceLocator = ServiceLocator.Instance;
        }

        #endregion Protected Constructors

        #region Public Properties

        public ServiceLocator ServiceLocator { get; }

        #endregion Public Properties

        #region Public Methods

        public T GetInterface<T>() where T : IApplicationInterface
        {
            var interfaceType = typeof(T);

            if (!interfaces.TryGetValue(interfaceType, out InterfaceContainer interfacePair))
                throw new InvalidOperationException($"Interface with type {interfaceType} not registered.");

            if (interfacePair.Instance is null)
                interfacePair.Instance = interfacePair.Initializer();

            return (T)interfacePair.Instance;
        }

        public void UnregisterInterface<T>() where T : IApplicationInterface
        {
            var interfaceType = typeof(T);

            if (!interfaces.ContainsKey(interfaceType))
                throw new InvalidOperationException($"Interface with type {interfaceType} not registered.");

            interfaces.Remove(interfaceType);
        }

        public void RegisterInterface<T>(InterfaceInitializer interfaceInitializer) where T : IApplicationInterface
        {
            var interfaceType = typeof(T);

            if (interfaces.ContainsKey(interfaceType))
                throw new InvalidOperationException($"Interface with type {interfaceType} already registered.");

            interfaces.Add(typeof(T), new InterfaceContainer(interfaceInitializer));
        }

        public bool InterfaceExists<T>() where T : IApplicationInterface
        {
            return interfaces.ContainsKey(typeof(T));
        }

        #endregion Public Methods

        #region Private Classes

        private class InterfaceContainer
        {
            #region Internal Constructors

            internal InterfaceContainer(InterfaceInitializer initializer)
            {
                Initializer = initializer;
            }

            #endregion Internal Constructors

            #region Internal Properties

            internal InterfaceInitializer Initializer { get; set; }
            internal IApplicationInterface Instance { get; set; }

            #endregion Internal Properties
        }

        #endregion Private Classes
    }
}