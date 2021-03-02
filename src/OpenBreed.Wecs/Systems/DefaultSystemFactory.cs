using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems
{
    public class DefaultSystemFactory : ISystemFactory
    {
        #region Private Fields

        private readonly Dictionary<Type, Delegate> systemInitializers = new Dictionary<Type, Delegate>();

        #endregion Private Fields

        #region Internal Constructors

        internal DefaultSystemFactory()
        {
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Register<TSystem>(Func<TSystem> initializer) where TSystem : ISystem
        {
            var systemType = typeof(TSystem);

            if (systemInitializers.ContainsKey(systemType))
                throw new InvalidOperationException($"System '{systemType}' already registered.");

            systemInitializers.Add(systemType, initializer);
        }

        public TSystem Create<TSystem>() where TSystem : ISystem
        {
            var systemType = typeof(TSystem);

            if (!systemInitializers.TryGetValue(systemType, out Delegate initializer))
                throw new InvalidOperationException($"System '{systemType}' not registered.");

            return (TSystem)initializer.DynamicInvoke();
        }

        #endregion Public Methods
    }
}