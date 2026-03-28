using OpenBreed.Model.Maps;
using OpenBreed.Model.Maps.Blocks;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Worlds;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Loaders
{
    internal class LevelExitCellLoader : IMapWorldEntityLoader
    {
        private readonly IWorldMan worldMan;
        private readonly IEntityFactory entityFactory;
        #region Internal Constructors

        internal LevelExitCellLoader(IWorldMan worldMan, IEntityFactory entityFactory)
        {
            this.worldMan = worldMan ?? throw new System.ArgumentNullException(nameof(worldMan));
            this.entityFactory = entityFactory ?? throw new System.ArgumentNullException(nameof(entityFactory));
        }

        #endregion Internal Constructors

        #region Public Methods

        public IEntity Load(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, IWorld world)
        {
            var missonBlock = map.Blocks.OfType<MapMissionBlock>().FirstOrDefault();

            int exitId;

            switch (templateName)
            {
                case "MapExit1":
                    exitId = missonBlock.EXC1;
                    break;

                case "MapExit2":
                    exitId = missonBlock.EXC2;
                    break;

                case "MapExit3":
                    exitId = missonBlock.EXC3;
                    break;

                default:
                    throw new NotImplementedException("Exit type not implemented");
            }

            var entity = entityFactory.CreateMapExit(ix, iy, exitId, mapAssets.Level, gfxValue);
            visited[ix, iy] = true;

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        #endregion Public Methods
    }
}