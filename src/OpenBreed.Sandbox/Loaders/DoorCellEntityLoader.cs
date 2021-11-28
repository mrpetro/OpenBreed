using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Loaders
{
    public class DoorCellEntityLoader : IMapWorldEntityLoader
    {
        #region Public Fields

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

        public void Load(MapMapper mapper, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, World world)
        {
            var rightValue = MapWorldDataLoader.GetActionCellValue(map.Layout, ix + 1, iy);
            mapper.Map(rightValue, gfxValue, out string rightTemplateName, out flavor);

            if (rightTemplateName == templateName)
            {
                doorHelper.AddHorizontal(world, ix, iy);
                visited[ix, iy] = true;
                visited[ix + 1, iy] = true;
                return;
            }

            var downValue = MapWorldDataLoader.GetActionCellValue(map.Layout, ix, iy + 1);
            mapper.Map(downValue, gfxValue, out string downTemplateName, out flavor);

            if (downTemplateName == templateName)
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