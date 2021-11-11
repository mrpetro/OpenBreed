using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Loaders
{
    public class DoorCellEntityLoader : IMapWorldEntityLoader
    {
        #region Public Fields

        public const int DOOR_STANDARD = 62;
        public const int DOOR_RED = 28;
        public const int DOOR_GREEN = 29;
        public const int DOOR_BLUE = 30;

        #endregion Public Fields

        #region Private Fields

        private readonly DoorHelper doorHelper;

        #endregion Private Fields

        #region Public Constructors

        public DoorCellEntityLoader(DoorHelper doorHelper)
        {
            this.doorHelper = doorHelper;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Load(WorldBlockBuilder worldBlockBuilder, MapLayoutModel layout, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            PutDoor(layout, visited, world, ix, iy, gfxValue);
        }

        #endregion Public Methods

        #region Private Methods

        private void PutDoor(MapLayoutModel layout, bool[,] visited, World world, int ix, int iy, int gfxValue)
        {
            var rightValue = MapWorldDataLoader.GetActionCellValue(layout, ix + 1, iy);

            if (rightValue == DOOR_STANDARD)
            {
                doorHelper.AddHorizontalDoor(world, ix, iy);
                visited[ix, iy] = true;
                visited[ix + 1, iy] = true;
                return;
            }

            var downValue = MapWorldDataLoader.GetActionCellValue(layout, ix, iy + 1);

            if (downValue == DOOR_STANDARD)
            {
                doorHelper.AddVerticalDoor(world, ix, iy);
                visited[ix, iy] = true;
                visited[ix, iy + 1] = true;
                return;
            }
        }

        #endregion Private Methods
    }
}