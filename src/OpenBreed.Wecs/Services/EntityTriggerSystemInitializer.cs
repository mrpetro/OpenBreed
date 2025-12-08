using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Wecs.Systems;
using System;

namespace OpenBreed.Wecs.Services
{
    internal class EntityTriggerSystemInitializer : ISystemInitializer
    {
        #region Public Methods

        public void Initialize(IServiceProvider serviceProvider, ISystem system)
        {
            if (system is not IEntityOnTriggerActionSystem entityOnTriggerSystem)
            {
                return;
            }

            var entityTriggerMan = serviceProvider.GetRequiredService<IEntityTriggerMan>();
            entityTriggerMan.RegisterCallback(entityOnTriggerSystem.TriggerName, entityOnTriggerSystem.ActionName, entityOnTriggerSystem.OnTrigger);
        }

        #endregion Public Methods
    }
}