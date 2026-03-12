using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Services
{
    internal class DefaultSystemInitializer : ISystemInitializer
    {
        #region Public Methods

        public void Initialize(IServiceProvider serviceProvider, ISystem system)
        {
            var systemRequirementsProvider = serviceProvider.GetRequiredService<ISystemRequirementsProvider>();
            systemRequirementsProvider.RegisterRequirements(system.GetType());

            if (system is IEventSystem eventSystem)
            {
                var eventSystemManager = serviceProvider.GetRequiredService<IEventSystemManager>();
                eventSystemManager.RegisterSystem(eventSystem);
            }
        }

        #endregion Public Methods
    }
}