using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common.Game.Wecs.Services;
using OpenBreed.Common.Game.Wecs.Systems.Actor;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Physics
{
    internal class ActorTriggerSystemInitializer : ISystemInitializer
    {
        #region Public Methods

        public void Initialize(IServiceProvider serviceProvider, ISystem system)
        {
            if (system is not IActorOnTriggerSystem onActorTriggerSystem)
            {
                return;
            }

            var actorTriggerMan = serviceProvider.GetRequiredService<IActorTriggerMan>();
            actorTriggerMan.RegisterCallback(onActorTriggerSystem.ActionName, onActorTriggerSystem.OnTrigger);
        }

        #endregion Public Methods
    }
}