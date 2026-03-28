using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Loaders
{
    public class AnimatedCellLoader : IMapWorldEntityLoader
    {
        #region Public Fields

        #endregion Public Fields

        #region Private Fields

        private readonly IWorldMan worldMan;
        private readonly IEntityFactory entityFactory;

        #endregion Private Fields

        #region Public Constructors

        public AnimatedCellLoader(IWorldMan worldMan, IEntityFactory entityFactory)
        {
            this.worldMan = worldMan ?? throw new ArgumentNullException(nameof(worldMan));
            this.entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntity Load(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, IWorld world)
        {
            IEntity entity = null;

            switch (templateName)
            {
                case "TVFlickering":
                    entity = PutTVFlickering(mapAssets, map, visited, world, ix, iy, gfxValue);
                    break;
                case "MonsterEating":
                    entity = PutMonsterEating(mapAssets, map, visited, world, ix, iy, gfxValue);
                    break;
                case "L1/ShipSmoke":
                    entity = PutEngineSmoke(mapAssets, map, visited, world, ix, iy, gfxValue);
                    break;
                default:
                    break;
            }

            return entity;
        }

        #endregion Public Methods

        #region Private Methods

        private IEntity PutTVFlickering(MapMapper mapAssets, MapModel map, bool[,] visited, IWorld world, int ix, int iy, int gfxValue)
        {
            var entity = entityFactory.CreateTVFlickering(ix, iy, mapAssets.Level, gfxValue);
            visited[ix, iy] = true;

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        private IEntity PutMonsterEating(MapMapper mapAssets, MapModel map, bool[,] visited, IWorld world, int ix, int iy, int gfxValue)
        {
            var entity = entityFactory.CreateMonsterEating(ix, iy, mapAssets.Level, gfxValue);
            visited[ix, iy] = true;

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        private IEntity PutEngineSmoke(MapMapper mapAssets, MapModel map, bool[,] visited, IWorld world, int ix, int iy, int gfxValue)
        {
            var entity = entityFactory.CreateShipSmoke(ix, iy, mapAssets.Level, gfxValue);
            visited[ix, iy] = true;

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        #endregion Private Methods
    }
}
