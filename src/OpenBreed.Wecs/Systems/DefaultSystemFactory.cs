using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems
{
    internal class DefaultSystemFactory : ISystemFactory
    {
        #region Private Fields

        private readonly ISystemRequirementsProvider systemRequirementsProvider;
        private readonly Dictionary<Type, Func<IWorld, ISystem>> systemInitializers = new Dictionary<Type, Func<IWorld, ISystem>>();

        #endregion Private Fields

        #region Public Constructors

        public DefaultSystemFactory(ISystemRequirementsProvider systemRequirementsProvider)
        {
            this.systemRequirementsProvider = systemRequirementsProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public ISystem CreateSystem<TSystem>(IWorld world) where TSystem : ISystem
        {
            var systemType = typeof(TSystem);

            if (!systemInitializers.TryGetValue(systemType, out Func<IWorld, ISystem> initializer))
                throw new InvalidOperationException($"System '{systemType}' not registered.");

            return (TSystem)initializer.Invoke(world);
        }

        public void RegisterSystem<TSystem>(Func<IWorld, ISystem> initializer) where TSystem : ISystem
        {
            var systemType = typeof(TSystem);

            if (systemInitializers.ContainsKey(systemType))
                throw new InvalidOperationException($"System '{systemType}' already registered.");

            systemInitializers.Add(systemType, initializer);
            systemRequirementsProvider.RegisterRequirements(systemType);
        }

        #endregion Public Methods
    }
}