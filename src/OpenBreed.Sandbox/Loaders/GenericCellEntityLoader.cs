using OpenBreed.Core.Managers;
using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Loaders
{
    public class GenericCellEntityLoader : IMapWorldEntityLoader
    {
        #region Public Fields

        public const int OBSTACLE_CODE = 63;
        public const int VOID_CODE = 0;
        private readonly GenericCellHelper genericCellHelper;

        #endregion Public Fields

        #region Protected Fields

        #endregion Protected Fields

        #region Private Fields

        #endregion Private Fields

        #region Public Constructors

        public GenericCellEntityLoader(GenericCellHelper genericCellHelper)
        {
            this.genericCellHelper = genericCellHelper;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Load(WorldBlockBuilder worldBlockBuilder, MapLayoutModel layout, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            switch (actionValue)
            {
                case OBSTACLE_CODE:
                    PutObstacleCell(worldBlockBuilder, layout, world, ix, iy, gfxValue);
                    break;

                case VOID_CODE:
                    PutVoidCell(worldBlockBuilder, layout, world, ix, iy, gfxValue);
                    break;

                //default:
                //    PutGenericCell(worldBlockBuilder, layout, world, ix, iy, gfxValue, actionValue, hasBody: false, unknown: true);
                //    break;
            }

            visited[ix, iy] = true;
        }

        #endregion Public Methods

        #region Private Methods

        private void PutVoidCell(WorldBlockBuilder worldBlockBuilder, MapLayoutModel layout, World world, int ix, int iy, int gfxValue)
        {
            genericCellHelper.AddVoidCell(world, ix, iy, worldBlockBuilder.atlasId, gfxValue);
        }

        private void PutObstacleCell(WorldBlockBuilder worldBlockBuilder, MapLayoutModel layout, World world, int ix, int iy, int gfxValue)
        {
            genericCellHelper.AddObstacleCell(world, ix, iy, worldBlockBuilder.atlasId, gfxValue);
        }

        #endregion Private Methods
    }
}