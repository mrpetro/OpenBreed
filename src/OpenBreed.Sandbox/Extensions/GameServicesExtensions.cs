using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Core.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Core.Components;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xaml;
using OpenBreed.Wecs.Core.Components.Extensions;

namespace OpenBreed.Sandbox.Extensions
{
    public static class GameServicesExtensions
    {
        public static IEntity GetEntityByDataGrid(this IGameServices gameServices, IEntity entity, int ox, int oy)
        {
            var pos = entity.Get<PositionComponent>();
            var world = gameServices.Worlds.GetById(entity.WorldId);
            var mapEntity = gameServices.Entities.GetMapEntity(entity.WorldId);
            var dataGrid = mapEntity.Get<DataGridComponent>().Grid;
            var indexPos = pos.Value.ToCellIndex(cellSize: 16);
            var thisEntity = dataGrid.Get(indexPos);
            var indexIndexPos = Vector2i.Add(indexPos, new Vector2i(ox, oy));
            var resultEntityId = dataGrid.Get(indexIndexPos);
            return gameServices.Entities.GetById(resultEntityId);
        }

        public static IEntity FindVerticalDoorCell(this IGameServices gameServices, IEntity entity)
        {
            var foundCell = entity;

            var thisOption = entity.GetMetadata("Option");
            var thisClassId = entity.ClassId;

            var nextCell = foundCell;

            while (nextCell is not null)
            {
                var nextClassId = nextCell.ClassId;

                if (!nextCell.TryGetMetadata("Option", out var option))
                {
                    break;
                }

                if (!(nextClassId == thisClassId && option == thisOption))
                    break;

                foundCell = nextCell;

                nextCell = gameServices.GetEntityByDataGrid(nextCell, 0, -1);
            }

            return foundCell;
        }

        public static bool TryGetCellGfxFlavor(this IGameServices services, ITileCell cell, string findPattern, out string level)
        {
            var regex = new Regex(findPattern);

            var result = services.Stamps.FindAll(item => regex.IsMatch(item.Name));
            var existingStamp = result.Where(item => item.Cells.Any(c => c.AtlasId == cell.AtlasId)).FirstOrDefault();

            if (existingStamp is null)
            {
                level = null;
                return false;
            }

            var res = regex.Match(existingStamp.Name);

            if (!res.Success)
            {
                level = null;
                return false;
            }

            if (!res.Groups.TryGetValue("level", out var levelGroup))
            {
                level = null;
                return false;
            }

            level = levelGroup.Value;
            return true;
        }

        public static bool TryGetCellGfxFlavor(this IGameServices services, ITileCell cell, string findPattern, out string level, out string flavor)
        {
            var regex = new Regex(findPattern);

            var result = services.Stamps.FindAll(item => regex.IsMatch(item.Name));
            var existingStamp = result.Where(item => item.Cells.Any(c => c.AtlasId == cell.AtlasId && c.ImageId == cell.ImageId)).FirstOrDefault();

            if (existingStamp is null)
            {
                level = null;
                flavor = null;
                return false;
            }

            var res = regex.Match(existingStamp.Name);

            if (!res.Success)
            {
                level = null;
                flavor = null;
                return false;
            }

            if (!res.Groups.TryGetValue("flavor", out var flavorGroup) || !res.Groups.TryGetValue("level", out var levelGroup))
            {
                level = null;
                flavor = null;
                return false;
            }

            level = levelGroup.Value;
            flavor = flavorGroup.Value;
            return true;
        }

        public static void SetEntityPosition(this IGameServices gameServices, IEntity target, int entryId)
        {
            var world = gameServices.Worlds.GetById(target.WorldId);

            var entryEntity = GetTopLeftMostEntity(gameServices.FindEntryEntities(world, entryId));

            if (entryEntity is null)
            {
                entryEntity = GetTopLeftMostEntity(gameServices.FindEntryEntities(world,2));
            }

            if (entryEntity is null)
            {
                throw new Exception($"No entry with ID '{entryId}' found.");
            }

            var entryPos = entryEntity.Get<PositionComponent>();
            var targetPos = target.Get<PositionComponent>();

            var newPosition = entryPos.Value;

            targetPos.Value = newPosition;

            var velocityCmp = target.Get<VelocityComponent>();
            velocityCmp.Value = Vector2.Zero;

            var thrustCmp = target.Get<ThrustComponent>();
            thrustCmp.Value = Vector2.Zero;

            target.State = null;
        }

        /// <summary>
        /// This function should emulate scanline method from vanilla ABTA for searching
        /// Entities
        /// </summary>
        /// <param name="entities">Entities to check coordinates</param>
        /// <returns></returns>
        public static IEntity GetTopLeftMostEntity(IEnumerable<IEntity> entities)
        {
            IEntity topMostEntity = null;
            var topMostPosX = float.MaxValue;
            var topMostPosY = 0.0f;

            foreach (var entity in entities)
            {
                var pos = entity.Get<PositionComponent>().Value;

                if (pos.Y < topMostPosY)
                    continue;

                if (pos.Y == topMostPosY)
                {
                    if (pos.X > topMostPosX)
                        continue;
                }

                topMostPosX = pos.X;
                topMostPosY = pos.Y;
                topMostEntity = entity;
            }

            return topMostEntity;
        }

        public static bool IsSameCellType(this IGameServices gameServices, IEntity entity, int ox, int oy)
        {
            var nextCell = gameServices.GetEntityByDataGrid(entity, ox, oy);
            var nextClassId = nextCell.ClassId;

            if (!nextCell.TryGetMetadata("Option", out var option))
            {
                return false;
            }

            var thisOption = entity.GetMetadata("Option");
            var thisClassId = entity.ClassId;

            return nextClassId == thisClassId && option == thisOption;
        }

        public static IEntity FindHorizontalDoorCell(this IGameServices gameServices, IEntity entity)
        {
            var foundCell = entity;

            if (!entity.TryGetMetadata("Option", out var thisOption))
            {

            }

            var thisClassId = entity.ClassId;

            var nextCell = foundCell;

            while (nextCell is not null)
            {
                if (!nextCell.TryGetMetadata("Option", out var nextOption))
                {
                    break;
                }

                var nextClassId = nextCell.ClassId;

                if (!(nextClassId == thisClassId && nextOption == thisOption))
                {
                    break;
                }

                foundCell = nextCell;

                nextCell = gameServices.GetEntityByDataGrid(nextCell, -1, 0);
            }

            return foundCell;
        }

        public static IEnumerable<IEntity> FindEntryEntities(this IGameServices gameServices, IWorld world, int entryId)
        {
            var searchClassId = gameServices.Classes.GetByName("MapEntry").Id;

            foreach (var entity in world.Entities.Where(e => e.Contains<MetadataComponent>()))
            {
                if (entity.ClassId != searchClassId)
                {
                    continue;
                }

                var flavor = entity.GetMetadata("Flavor");

                if (flavor != entryId.ToString())
                {
                    continue;
                }

                yield return entity;
            }
        }

        public static void ExecuteHeroEnter(this IGameServices services, IEntity heroEntity, string worldName, int entryId)
        {
            var task = Core.Task.Create((t) => services.AddToWorld(t, heroEntity, worldName));

            task.Then((t) => services.PlayerCharacterEnter(t, heroEntity, entryId));

            task.Start();
        }
    }
}
