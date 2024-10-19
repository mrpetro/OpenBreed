using OpenBreed.Core;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Game;
using OpenBreed.Sandbox.Worlds;

namespace OpenBreed.Sandbox.Extensions
{
    public static class EntityManExtensions
    {
        public static void ForEachEntity(this IEntityMan entityMan, int worldId, string entityType, string option, Action<IEntity> action)
        {

            var nullOnes = entityMan.Where(entity => entity == null).ToArray();

            var entities = entityMan.Where(entity =>
            {
                if (entity.WorldId != worldId)
                    return false;

                var meta = entity.TryGet<MetadataComponent>();

                if (meta is null)
                    return false;

                if (meta.Name != entityType)
                    return false;

                if (meta.Option != option)
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
            return entityMan.GetByTag($"{WorldNames.SMARTCARD_SCREEN}/Text").FirstOrDefault();
        }

        public static IEntity GetEntityByDataGrid(this IEntityMan entityMan, IWorldMan worldMan, IEntity entity, int ox, int oy)
        {
            var pos = entity.Get<PositionComponent>();
            var world = worldMan.GetById(entity.WorldId);
            var mapEntity = entityMan.GetMapEntity(entity.WorldId);
            var dataGrid = mapEntity.Get<DataGridComponent>().Grid;
            var indexPos = new Vector2i((int)pos.Value.X / 16, (int)pos.Value.Y / 16);
            var thisEntity = dataGrid.Get(indexPos);
            var indexIndexPos = Vector2i.Add(indexPos, new Vector2i(ox, oy));
            var resultEntityId = dataGrid.Get(indexIndexPos);
            return entityMan.GetById(resultEntityId);
        }

        public static IEntity FindVerticalDoorCell(this IEntityMan entityMan, IWorldMan worldMan, IEntity entity)
        {
            var foundCell = entity;

            var thisData = entity.Get<MetadataComponent>();

            var nextCell = foundCell;

            while (nextCell is not null)
            {
                var nextCellMeta = nextCell.TryGet<MetadataComponent>();

                if (nextCellMeta is null)
                    break;

                if (!(nextCellMeta.Name == thisData.Name && nextCellMeta.Option == thisData.Option))
                    break;

                foundCell = nextCell;

                nextCell = entityMan.GetEntityByDataGrid(worldMan, nextCell, 0, -1);
            }

            return foundCell;
        }

        public static bool IsSameCellType(this IEntityMan entityMan, IWorldMan worldMan, IEntity entity, int ox, int oy)
        {
            var nextCell = entityMan.GetEntityByDataGrid(worldMan, entity, ox, oy);

            var nextCellMeta = nextCell.TryGet<MetadataComponent>();

            if (nextCellMeta is null)
                return false;

            var thisData = entity.Get<MetadataComponent>();

            return nextCellMeta.Name == thisData.Name && nextCellMeta.Option == thisData.Option;
        }

        public static IEntity FindHorizontalDoorCell(this IEntityMan entityMan, IWorldMan worldMan, IEntity entity)
        {
            var foundCell = entity;

            var thisData = entity.Get<MetadataComponent>();

            var nextCell = foundCell;

            while (nextCell is not null)
            {
                var nextCellMeta = nextCell.TryGet<MetadataComponent>();

                if (nextCellMeta is null)
                    break;

                if (!(nextCellMeta.Name == thisData.Name && nextCellMeta.Option == thisData.Option))
                    break;

                foundCell = nextCell;

                nextCell = entityMan.GetEntityByDataGrid(worldMan, nextCell, -1, 0);
            }

            return foundCell;
        }

        public static IEntity GetMissionScreenText(this IEntityMan entityMan)
        {
            return entityMan.GetByTag($"{WorldNames.MISSION_SCREEN}/Text").FirstOrDefault();
        }

        public static IEntity GetMissionScreenBackground(this IEntityMan entityMan)
        {
            return entityMan.GetByTag($"{WorldNames.MISSION_SCREEN}/Background").FirstOrDefault();
        }

        public static IEntity GetHudCamera(this IEntityMan entityMan)
        {
            return entityMan.GetByTag($"Camera.{WorldNames.GAME_HUD}").FirstOrDefault();
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

        public static IEntity GetControllerPlayer(this IEntityMan entityMan, IEntity controlledEntity)
        {
            var player1Entity = entityMan.GetByTag("Players/P1").FirstOrDefault();

            if (player1Entity is not null)
            {
                if (player1Entity.Get<ControllerComponent>().ControlledEntityId == controlledEntity.Id)
                    return player1Entity;
            }

            var player2Entity = entityMan.GetByTag("Players/P2").FirstOrDefault();

            if (player2Entity is not null)
            {
                if (player2Entity.Get<ControllerComponent>().ControlledEntityId == controlledEntity.Id)
                    return player2Entity;
            }

            return null;
        }

        public static IEntity GetMapEntity(this IEntityMan entityMan, int worldId)
        {
            return entityMan.GetByTag("Maps").Where(e => e.WorldId == worldId).FirstOrDefault();
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
