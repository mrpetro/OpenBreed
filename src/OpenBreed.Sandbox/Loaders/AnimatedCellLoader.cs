using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Builders;
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

        public void Load(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, World world)
        {
            switch (templateName)
            {
                case "TVFlickering":
                    PutTVFlickering(mapAssets, map, visited, world, ix, iy, gfxValue);
                    break;
                case "MonsterEating":
                    PutMonsterEating(mapAssets, map, visited, world, ix, iy, gfxValue);
                    break;
                default:
                    break;
            }

        }

        #endregion Public Methods

        #region Private Methods

        private void PutTVFlickering(MapMapper mapAssets, MapModel map, bool[,] visited, World world, int ix, int iy, int gfxValue)
        {
            environmentHelper.AddTVFlickering(world, ix, iy, mapAssets.Level, gfxValue);
            visited[ix, iy] = true;
        }

        private void PutMonsterEating(MapMapper mapAssets, MapModel map, bool[,] visited, World world, int ix, int iy, int gfxValue)
        {
            environmentHelper.AddMonsterEating(world, ix, iy, mapAssets.Level, gfxValue);
            visited[ix, iy] = true;
        }

        #endregion Private Methods
    }
}
