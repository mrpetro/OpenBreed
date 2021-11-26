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

        public void Load(MapAssets mapAssets, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            //if (!mapAssets.TileAtlasName.EndsWith("L4"))
            //    return;

            var rightValue = MapWorldDataLoader.GetActionCellValue(map.Layout, ix + 1, iy);

            if (rightValue == DOOR_STANDARD)
            {
                doorHelper.AddHorizontal(world, ix, iy);
                visited[ix, iy] = true;
                visited[ix + 1, iy] = true;
                return;
            }

            var downValue = MapWorldDataLoader.GetActionCellValue(map.Layout, ix, iy + 1);

            if (downValue == DOOR_STANDARD)
            {
                doorHelper.AddVertical(world, ix, iy);
                visited[ix, iy] = true;
                visited[ix, iy + 1] = true;
                return;
            }
        }

        #endregion Public Methods
    }
}