using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Wecs.Abstractions.Systems;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Physics.Systems
{
    internal class ActionOnTriggerSystemInitializer : ISystemInitializer
    {
        #region Public Methods

        public void Initialize(IServiceProvider serviceProvider, ISystem system)
        {
            if (system is IActionOnTriggerSystem actionOnTriggerSystem)
            {
                var entityTriggerMan = serviceProvider.GetRequiredService<IEntityTriggerMan>();
                entityTriggerMan.RegisterSystem(actionOnTriggerSystem); 
            }
        }

        #endregion Public Methods
    }
}