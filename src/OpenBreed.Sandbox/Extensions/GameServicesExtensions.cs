using OpenBreed.Common.Game.Services;
using OpenBreed.Wecs.Abstractions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Game.Wecs.Extensions;

namespace OpenBreed.Sandbox.Extensions
{
    public static class GameServicesExtensions
    {
        public static void ExecuteHeroEnter(this IGameServices services, IEntity heroEntity, string worldName, int entryId)
        {
            var task = Core.Task.Create((t) => services.AddToWorld(t, heroEntity, worldName));

            task.Then((t) => services.PlayerCharacterEnter(t, heroEntity, entryId));

            task.Start();
        }
    }
}
