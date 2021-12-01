using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Loaders
{
    public class ElectricGateEntityLoader : IMapWorldEntityLoader
    {
        #region Public Fields

        public const string PASS_UP = "ElectricGateUp";
        public const string PASS_DOWN = "ElectricGateDown";
        public const string PASS_RIGHT = "ElectricGateRight";
        public const string PASS_LEFT = "ElectricGateLeft";

        #endregion Public Fields

        #region Private Fields

        private readonly ElectricGateHelper electricGateHelper;

        #endregion Private Fields

        #region Public Constructors

        public ElectricGateEntityLoader(ElectricGateHelper electricGateHelper)
        {
            this.electricGateHelper = electricGateHelper;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Load(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, World world)
        {
            switch (templateName)
            {
                case PASS_UP:
                case PASS_DOWN:
                    PutPassUpDown(mapAssets, map, visited, ix, iy, gfxValue, templateName, world);
                    break;

                case PASS_RIGHT:
                case PASS_LEFT:
                    PutPassRightLeft(mapAssets, map, visited, ix, iy, gfxValue, templateName, world);
                    break;

                default:
                    break;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void PutPassUpDown(MapMapper mapper, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, string templateName, World world)
        {
            var rightValue = MapWorldDataLoader.GetActionCellValue(map.Layout, ix + 1, iy);
            var rightAction = map.GetAction(rightValue);

            if (rightAction?.Name == templateName)
            {
                electricGateHelper.AddHorizontal(world, ix, iy, mapper.Level);
                visited[ix, iy] = true;
                visited[ix + 1, iy] = true;
                return;
            }
            else
            {
                electricGateHelper.AddHorizontal(world, ix - 1, iy, mapper.Level);
                visited[ix, iy] = true;
                visited[ix - 1, iy] = true;
                return;
            }
        }

        private void PutPassRightLeft(MapMapper mapper, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, string templateName, World world)
        {
            var downValue = MapWorldDataLoader.GetActionCellValue(map.Layout, ix, iy + 1);
            var downAction = map.GetAction(downValue);

            if (downAction?.Name == templateName)
            {
                electricGateHelper.AddVertical(world, ix, iy, mapper.Level);
                visited[ix, iy] = true;
                visited[ix, iy + 1] = true;
                return;
            }
            else
            {
                electricGateHelper.AddVertical(world, ix, iy - 1, mapper.Level);
                visited[ix, iy] = true;
                visited[ix, iy - 1] = true;
                return;
            }
        }

        #endregion Private Methods
    }
}