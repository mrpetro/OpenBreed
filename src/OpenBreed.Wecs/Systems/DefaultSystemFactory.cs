using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems
{
    internal class DefaultSystemFactory : ISystemFactory
    {
        #region Private Fields

        private readonly Dictionary<Type, Func<ISystem>> systemInitializers = new Dictionary<Type, Func<ISystem>>();

        #endregion Private Fields

        #region Internal Constructors

        public DefaultSystemFactory()
        {

        }

        #endregion Internal Constructors

        #region Public Methods

        public void Register<TSystem>(Func<ISystem> initializer) where TSystem : ISystem
        {
            var systemType = typeof(TSystem);

            if (systemInitializers.ContainsKey(systemType))
                throw new InvalidOperationException($"System '{systemType}' already registered.");

            systemInitializers.Add(systemType, initializer);
        }

        public TSystem Create<TSystem>() where TSystem : ISystem
        {
            var systemType = typeof(TSystem);

            if (!systemInitializers.TryGetValue(systemType, out Func<ISystem> initializer))
                throw new InvalidOperationException($"System '{systemType}' not registered.");

            return (TSystem)initializer.Invoke();
        }

        #endregion Public Methods
    }
}