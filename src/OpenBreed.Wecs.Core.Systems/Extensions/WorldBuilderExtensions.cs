using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Core.Systems.Extensions
{
    public static class WorldBuilderExtensions
    {
        #region Public Methods

        public static IWorldBuilder AddCoreSystems(this IWorldBuilder builder)
        {
            builder.AddSystem<LifetimeSystem>();
            builder.AddSystem<EntityEmitterSystem>();
            builder.AddSystem<TimerSystem>();
            builder.AddSystem<FrameSystem>();
            builder.AddSystem<PausingSystem>();
            builder.AddSystem<FsmSystem>();

            return builder;
        }

        #endregion Public Methods
    }
}