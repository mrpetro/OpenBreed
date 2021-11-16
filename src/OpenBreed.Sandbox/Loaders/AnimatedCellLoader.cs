﻿using OpenBreed.Model.Maps;
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

        public const int TV_FLICKERING_CODE = 14;
        public const int MONSTER_EATING_CODE = 42;

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

        public void Load(MapAssets mapAssets, MapLayoutModel layout, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            switch (actionValue)
            {
                case TV_FLICKERING_CODE:
                    PutTVFlickering(mapAssets, layout, visited, world, ix, iy, gfxValue);
                    break;
                case MONSTER_EATING_CODE:
                    PutMonsterEating(mapAssets, layout, visited, world, ix, iy, gfxValue);
                    break;
                default:
                    break;
            }

            
        }

        #endregion Public Methods

        #region Private Methods

        private void PutTVFlickering(MapAssets mapAssets, MapLayoutModel layout, bool[,] visited, World world, int ix, int iy, int gfxValue)
        {
            environmentHelper.AddTVFlickering(world, ix, iy, mapAssets.AtlasId, gfxValue);
            visited[ix, iy] = true;
        }

        private void PutMonsterEating(MapAssets mapAssets, MapLayoutModel layout, bool[,] visited, World world, int ix, int iy, int gfxValue)
        {
            environmentHelper.AddMonsterEating(world, ix, iy, mapAssets.AtlasId, gfxValue);
            visited[ix, iy] = true;
        }

        #endregion Private Methods
    }
}
