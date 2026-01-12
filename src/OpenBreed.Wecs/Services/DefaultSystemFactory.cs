using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenBreed.Wecs.Services
{
    internal class DefaultSystemFactory : ISystemFactory
    {
        #region Private Fields

        private readonly IServiceProvider serviceProvider;
        private readonly ISystemRequirementsProvider systemRequirementsProvider;

        #endregion Private Fields

        #region Public Constructors

        public DefaultSystemFactory(IServiceProvider serviceProvider, ISystemRequirementsProvider systemRequirementsProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.systemRequirementsProvider = systemRequirementsProvider ?? throw new ArgumentNullException(nameof(systemRequirementsProvider));
        }

        #endregion Public Constructors

        #region Public Methods

        public ISystem CreateSystem<TSystem>() where TSystem : ISystem
        {
            var system = serviceProvider.GetService<TSystem>();

            if (system is null)
            {
                throw new InvalidOperationException($"System '{typeof(TSystem)}' not registered.");
            }

            return system;
        }

        #endregion Public Methods
    }
}