using OpenBreed.Common;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using System;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Common.Interface.Logging;
using Microsoft.Extensions.Logging;

namespace OpenBreed.Wecs.Systems.Core.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupCoreSystems(this ISystemFactory systemFactory, IServiceProvider sp)
        {
            systemFactory.RegisterSystem<FsmSystem>(() => new FsmSystem(
                sp.GetService<IFsmMan>(),
                sp.GetService<ILogger>()));

            systemFactory.RegisterSystem<TextInputSystem>(() => new TextInputSystem(
                sp.GetService<IEntityMan>(),
                sp.GetService<IEventsMan>()));
            
            systemFactory.RegisterSystem<TimerSystem>(() => new TimerSystem(
                sp.GetService<IEntityMan>(),
                sp.GetService<IEventsMan>(),
                sp.GetService<ILogger>()));

            systemFactory.RegisterSystem<FrameSystem>(() => new FrameSystem(
                sp.GetService<IEntityMan>(),
                sp.GetService<IEventsMan>(),
                sp.GetService<ILogger>()));

            systemFactory.RegisterSystem<PausingSystem>(() => new PausingSystem(
                sp.GetService<IWorldMan>(),                                                    
                sp.GetService<IEventsMan>()));

            systemFactory.RegisterSystem<EntityEmitterSystem>(() => new EntityEmitterSystem(
                sp.GetRequiredService<IEntityFactory>(),
                sp.GetRequiredService<IEventsMan>(),
                sp.GetRequiredService<ITriggerMan>(),
                sp.GetRequiredService<IWorldMan>()));

            systemFactory.RegisterSystem<LifetimeSystem>(() => new LifetimeSystem(
                sp.GetRequiredService<IWorldMan>(),
                sp.GetRequiredService<IEntityMan>(),
                sp.GetRequiredService<IEventsMan>()));
        }

        #endregion Public Methods
    }
}