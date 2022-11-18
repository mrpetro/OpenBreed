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
        public static void ForEachEntity(this IEntityMan entityMan, int worldId, string entityType, string option, Action<Entity> action)
        {

            var nullOnes = entityMan.Where(entity => entity == null).ToArray();

            var entities = entityMan.Where(entity =>
            {
                if(entity.WorldId != worldId)
                    return false;

                var meta = entity.TryGet<MetadataComponent>();

                if (meta is null)
                    return false;

                if(meta.Name != entityType)
                    return false;

                if(meta.Option != option)
                    return false;

                return true;

            }).ToArray();

            foreach (var entity in entities)
                action.Invoke(entity);
        }

        public static Entity GetSmartCardScreenText(this IEntityMan entityMan)
        {
            return entityMan.GetByTag("SmartCardScreen/Text").FirstOrDefault();
        }

        public static Entity GetHudCamera(this IEntityMan entityMan)
        {
            return entityMan.GetByTag("Camera.GameHud").FirstOrDefault();
        }

        public static Entity GetSmartCardScreenCamera(this IEntityMan entityMan)
        {
            return entityMan.GetByTag("Camera.SmartCardScreen").FirstOrDefault();
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
