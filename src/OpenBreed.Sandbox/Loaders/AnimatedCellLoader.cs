using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Wecs.Entities;
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

        private readonly EnvironmentHelper environmentHelper;

        #endregion Private Fields

        #region Public Constructors

        public AnimatedCellLoader(EnvironmentHelper environmentHelper)
        {
            this.environmentHelper = environmentHelper;
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntity Load(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, World world)
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
                default:
                    break;
            }

            return entity;
        }

        #endregion Public Methods

        #region Private Methods

        private IEntity PutTVFlickering(MapMapper mapAssets, MapModel map, bool[,] visited, World world, int ix, int iy, int gfxValue)
        {
            var entity = environmentHelper.AddTVFlickering(world, ix, iy, mapAssets.Level, gfxValue);
            visited[ix, iy] = true;
            return entity;
        }

        private IEntity PutMonsterEating(MapMapper mapAssets, MapModel map, bool[,] visited, World world, int ix, int iy, int gfxValue)
        {
            var entity = environmentHelper.AddMonsterEating(world, ix, iy, mapAssets.Level, gfxValue);
            visited[ix, iy] = true;
            return entity;
        }

        #endregion Private Methods
    }
}
