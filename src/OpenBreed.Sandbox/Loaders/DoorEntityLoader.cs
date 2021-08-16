using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Loaders
{
    public class DoorEntityLoader : IMapWorldEntityLoader
    {
        #region Private Fields

        private readonly DoorHelper doorHelper;

        #endregion Private Fields

        #region Public Constructors

        public DoorEntityLoader(DoorHelper doorHelper)
        {
            this.doorHelper = doorHelper;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Load(MapLayoutModel layout, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            PutDoor(layout, visited, world, ix, iy, gfxValue);
        }

        #endregion Public Methods

        #region Private Methods

        private void PutDoor(MapLayoutModel layout, bool[,] visited, World world, int ix, int iy, int gfxValue)
        {
            var rightValue = MapWorldDataLoader.GetActionCellValue(layout, ix + 1, iy);

            if (rightValue == 62)
            {
                doorHelper.AddHorizontalDoor(world, ix, iy);
                visited[ix, iy] = true;
                visited[ix + 1, iy] = true;
                return;
            }

            var downValue = MapWorldDataLoader.GetActionCellValue(layout, ix, iy + 1);

            if (downValue == 62)
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