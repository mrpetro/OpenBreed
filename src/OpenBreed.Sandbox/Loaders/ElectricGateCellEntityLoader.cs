using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.ElectricGate;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Loaders
{
    public class ElectricGateCellEntityLoader : IMapWorldEntityLoader
    {
        #region Public Fields

        public const int PASS_UP = 7;
        public const int PASS_DOWN = 8;
        public const int PASS_RIGHT = 12;
        public const int PASS_LEFT = 13;

        #endregion Public Fields

        #region Private Fields

        private readonly ElectricGateHelper electricGateHelper;

        #endregion Private Fields

        #region Public Constructors

        public ElectricGateCellEntityLoader(ElectricGateHelper electricGateHelper)
        {
            this.electricGateHelper = electricGateHelper;
        }

        #endregion Public Constructors

        #region Public Methods

        private void PutPassUpDown(MapAssets mapAssets, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            var rightValue = MapWorldDataLoader.GetActionCellValue(map.Layout, ix + 1, iy);

            if (actionValue == rightValue)
            {
                electricGateHelper.AddHorizontal(world, ix, iy);
                visited[ix, iy] = true;
                visited[ix + 1, iy] = true;
                return;
            }
            else
            {
                electricGateHelper.AddHorizontal(world, ix - 1, iy);
                visited[ix, iy] = true;
                visited[ix - 1, iy] = true;
                return;
            }
        }

        private void PutPassRightLeft(MapAssets mapAssets, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            var downValue = MapWorldDataLoader.GetActionCellValue(map.Layout, ix, iy + 1);

            if (downValue == actionValue)
            {
                electricGateHelper.AddVertical(world, ix, iy);
                visited[ix, iy] = true;
                visited[ix, iy + 1] = true;
                return;
            }
            else
            {
                electricGateHelper.AddVertical(world, ix, iy - 1);
                visited[ix, iy] = true;
                visited[ix, iy - 1] = true;
                return;
            }
        }

        public void Load(MapAssets mapAssets, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            switch (actionValue)
            {
                case PASS_UP:
                case PASS_DOWN:
                    PutPassUpDown(mapAssets, map, visited, ix, iy, gfxValue, actionValue, world);
                    break;
                case PASS_RIGHT:
                case PASS_LEFT:
                    PutPassRightLeft(mapAssets, map, visited, ix, iy, gfxValue, actionValue, world);
                    break;
                default:
                    break;
            }
        }

        #endregion Public Methods
    }
}