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
        public static void ForEachEntity(this IEntityMan entityMan, int worldId, string entityType, string option, Action<IEntity> action)
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

        public static IEntity GetFirstFound(this IEntityMan entityMan, string tag)
        {
            return entityMan.GetByTag(tag).FirstOrDefault();
        }

        public static IEntity GetSmartCardScreenText(this IEntityMan entityMan)
        {
            return entityMan.GetByTag("SmartCardScreen/Text").FirstOrDefault();
        }

        public static IEntity GetMissionScreenText(this IEntityMan entityMan)
        {
            return entityMan.GetByTag("MissionScreen/Text").FirstOrDefault();
        }

        public static IEntity GetMissionScreenBackground(this IEntityMan entityMan)
        {
            return entityMan.GetByTag("MissionScreen/Background").FirstOrDefault();
        }

        public static IEntity GetHudCamera(this IEntityMan entityMan)
        {
            return entityMan.GetByTag("Camera.GameHud").FirstOrDefault();
        }

        public static IEntity GetMission(this IEntityMan entityMan, int worldId)
        {
            return entityMan.GetByTag("Mission").FirstOrDefault(entity => entity.WorldId == worldId);
        }

        public static IEntity GetDirector(this IEntityMan entityMan, int worldId)
        {
            return entityMan.GetByTag("Director").FirstOrDefault(entity => entity.WorldId == worldId);
        }

        public static IEntity GetSmartCardScreenCamera(this IEntityMan entityMan)
        {
            return entityMan.GetByTag("Camera.SmartCardScreen").FirstOrDefault();
        }

        public static IEntity GetMissionScreenCamera(this IEntityMan entityMan)
        {
            return entityMan.GetByTag("Camera.MissionScreen").FirstOrDefault();
        }

        public static IEntity GetPlayerCamera(this IEntityMan entityMan, IEntity playerEntity)
        {
            return playerEntity.Get<FollowedComponent>().FollowerIds.Select(item => entityMan.GetById(item)).
                                                                              FirstOrDefault(item => item.Tag is "Camera.Player");
        }

        public static IEntity GetHudViewport(this IEntityMan entityMan)
        {
            return entityMan.GetByTag(ScreenWorldHelper.GAME_HUD_VIEWPORT).First();
        }

        public static IEntity GetGameViewport(this IEntityMan entityMan)
        {
            return entityMan.GetByTag(ScreenWorldHelper.GAME_VIEWPORT).First();
        }
    }
}
