using OpenBreed.Sandbox.Worlds;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Extensions
{
    public static class EntityManExtensions
    {
        public static Entity GetHudCamera(this IEntityMan entityMan)
        {
            return entityMan.GetByTag("Camera.GameHud").FirstOrDefault();
        }

        public static Entity GetSmartcardReaderCamera(this IEntityMan entityMan)
        {
            return entityMan.GetByTag("Camera.SmartcardReader").FirstOrDefault();
        }

        public static Entity GetPlayerCamera(this IEntityMan entityMan, Entity playerEntity)
        {
            return playerEntity.Get<FollowedComponent>().FollowerIds.Select(item => entityMan.GetById(item)).
                                                                              FirstOrDefault(item => item.Tag is "Camera.Player");
        }

        public static Entity GetHudViewport(this IEntityMan entityMan)
        {
            return entityMan.GetByTag(ScreenWorldHelper.GAME_HUD_VIEWPORT).First();
        }

        public static Entity GetGameViewport(this IEntityMan entityMan)
        {
            return entityMan.GetByTag(ScreenWorldHelper.GAME_VIEWPORT).First();
        }
    }
}
