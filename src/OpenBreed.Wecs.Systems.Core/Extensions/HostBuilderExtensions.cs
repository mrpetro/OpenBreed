using OpenBreed.Common;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using System;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Common.Interface.Logging;

namespace OpenBreed.Wecs.Systems.Core.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupCoreSystems(this ISystemFactory systemFactory, IServiceProvider sp)
        {
            systemFactory.RegisterSystem<FsmSystem>((world) => new FsmSystem(
                world,
                sp.GetService<IFsmMan>(),
                sp.GetService<ILogger>()));

            systemFactory.RegisterSystem<TextInputSystem>((world) => new TextInputSystem(
                world,
                sp.GetService<IEntityMan>()));
            
            systemFactory.RegisterSystem<TimerSystem>((world) => new TimerSystem(
                world,
                sp.GetService<IEntityMan>(),
                sp.GetService<IEventsMan>(),
                sp.GetService<ILogger>()));

            systemFactory.RegisterSystem<FrameSystem>((world) => new FrameSystem(
                world,
                sp.GetService<IEntityMan>(),
                sp.GetService<IEventsMan>(),
                sp.GetService<ILogger>()));

            systemFactory.RegisterSystem<PausingSystem>((world) => new PausingSystem(
                world,
                sp.GetService<IWorldMan>(),                                                    
                sp.GetService<IEventsMan>()));

            systemFactory.RegisterSystem<EntityEmitterSystem>((world) => new EntityEmitterSystem(
                world,
                sp.GetRequiredService<IEntityFactory>(),
                sp.GetRequiredService<IEventsMan>(),
                sp.GetRequiredService<ITriggerMan>(),
                sp.GetRequiredService<IWorldMan>()));

            systemFactory.RegisterSystem<LifetimeSystem>((world) => new LifetimeSystem(
                world,
                sp.GetRequiredService<IWorldMan>(),
                sp.GetRequiredService<IEntityMan>(),
                sp.GetRequiredService<IEventsMan>()));
        }

        #endregion Public Methods
    }
}