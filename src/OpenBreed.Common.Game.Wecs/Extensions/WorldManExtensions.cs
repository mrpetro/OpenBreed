using OpenBreed.Common.Game.Wecs.Systems;
using OpenBreed.Common.Game.Wecs.Systems.Screen;
using OpenBreed.Wecs.Audio.Systems;
using OpenBreed.Wecs.Core.Systems;
using OpenBreed.Wecs.Rendering.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Extensions
{
    public static class WorldManExtensions
    {
        #region Public Methods

        public static IWorld CreateLimboWorld(this IWorldMan worldMan)
        {
            return worldMan.Create()
                .SetName(WorldNames.Limbo)
                .AddSystem<ResurrectionSystem>()
                .AddSystem<TimerSystem>()
                .Build();
        }

        public static IWorld CreateScreenWorld(this IWorldMan worldMan)
        {
            return worldMan.Create()
            .SetName(WorldNames.ScreenWorld)
            .AddSystem<ActorMovementByPlayerInputsSystem>()
            .AddSystem<ViewportSystem>()
            .AddSystem<SoundSystem>()
            .AddSystem<TimerSystem>()
            .AddSystem<FrameSystem>()
            .AddSystem<ScreenInitSystem>()
            .Build();
        }

        #endregion Public Methods
    }
}