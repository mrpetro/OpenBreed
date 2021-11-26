using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Loaders
{
    public class GenericCellEntityLoader : IMapWorldEntityLoader
    {
        #region Public Fields

        public const int ACTOR_ONLY_OBSTACLE_CODE = 60;
        public const int FULL_OBSTACLE_CODE = 63;

        public const int VOID_CODE = 0;

        #endregion Public Fields

        #region Private Fields

        private readonly GenericCellHelper genericCellHelper;

        #endregion Private Fields

        #region Public Constructors

        public GenericCellEntityLoader(GenericCellHelper genericCellHelper)
        {
            this.genericCellHelper = genericCellHelper;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Load(MapAssets mapAssets, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            switch (actionValue)
            {
                case FULL_OBSTACLE_CODE:
                    genericCellHelper.AddFullObstacleCell(world, ix, iy, mapAssets.TileAtlasName, gfxValue);
                    break;
                case ACTOR_ONLY_OBSTACLE_CODE:
                    genericCellHelper.AddActorOnlyObstacleCell(world, ix, iy, mapAssets.TileAtlasName, gfxValue);
                    break;
                case VOID_CODE:
                    genericCellHelper.AddVoidCell(world, ix, iy, mapAssets.TileAtlasName, gfxValue);
                    break;

                    //default:
                    //    PutGenericCell(worldBlockBuilder, layout, world, ix, iy, gfxValue, actionValue, hasBody: false, unknown: true);
                    //    break;
            }

            visited[ix, iy] = true;
        }

        #endregion Public Methods
    }
}