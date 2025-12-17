using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Components.Common;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Extensions
{
    public static class WorldManExtensions
    {
        public static void SetEntityPosition(this IWorldMan worldMan, IEntity target, int entryId)
        {
            var world = worldMan.GetById(target.WorldId);

            var entryEntity = WorldExtensions.GetTopLeftMostEntity(world.FindEntryEntities(entryId));

            if (entryEntity is null)
            {
                entryEntity = WorldExtensions.GetTopLeftMostEntity(world.FindEntryEntities(2));
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
    }
}
