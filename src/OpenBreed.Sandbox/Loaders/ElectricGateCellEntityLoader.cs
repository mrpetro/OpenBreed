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

        public void Load(WorldBlockBuilder worldBlockBuilder, MapLayoutModel layout, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            var rightValue = MapWorldDataLoader.GetActionCellValue(layout, ix + 1, iy);

            if (rightValue == actionValue)
            {
                electricGateHelper.AddHorizontal(world, ix, iy);
                visited[ix, iy] = true;
                visited[ix + 1, iy] = true;
                return;
            }

            var downValue = MapWorldDataLoader.GetActionCellValue(layout, ix, iy + 1);

            if (downValue == actionValue)
            {
                electricGateHelper.AddVertical(world, ix, iy);
                visited[ix, iy] = true;
                visited[ix, iy + 1] = true;
                return;
            }
        }

        #endregion Public Methods
    }
}