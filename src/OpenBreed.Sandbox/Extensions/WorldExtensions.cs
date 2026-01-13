using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Extensions
{
    public static class WorldExtensions
    {
        #region Public Methods

        public static IEnumerable<IEntity> FindEntryEntities(this IWorld world, int entryId)
        {
            foreach (var entity in world.Entities.Where(e => e.Contains<MetadataComponent>()))
            {
                var cmpClass = entity.Get<MetadataComponent>();

                if (cmpClass.Name != "WorldEntry")
                    continue;

                if (cmpClass.Flavor != entryId.ToString())
                    continue;

                yield return entity;
            }
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

        #endregion Public Methods
    }
}