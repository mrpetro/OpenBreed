using OpenBreed.Core;
using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Loaders
{
    public class DoorEntityLoader : IMapWorldEntityLoader
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

        public DoorEntityLoader(DoorHelper doorHelper)
        {
            this.doorHelper = doorHelper;
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntity Load(MapMapper mapper, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, World world)
        {
            var key = default(string);

            switch (templateName)
            {
                case "DoorStandard":
                    key = "";
                    break;
                case "DoorRed":
                    key = "Keycard1";
                    break;
                case "DoorGreen":
                    key = "Keycard2";
                    break;
                case "DoorBlue":
                    key = "Keycard3";
                    break;
            }

            var entity = doorHelper.AddDoor(world, ix, iy, mapper.Level, key);
            visited[ix, iy] = true;

            return entity;
        }

        public IEntity LoadOld(MapMapper mapper, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, World world)
        {
            var entity = default(IEntity);
            var key = default(string);

            switch (templateName)
            {
                case "DoorStandard":
                    key = "";
                    break;
                case "DoorRed":
                    key = "Keycard1";
                    break;
                case "DoorGreen":
                    key = "Keycard2";
                    break;
                case "DoorBlue":
                    key = "Keycard3";
                    break;
            }

            var rightValue = MapLegacyDataLoader.GetActionCellValue(map.Layout, ix + 1, iy);
            var rightAction = map.GetAction(rightValue);

            if (rightAction?.Name == templateName)
            {
                entity = doorHelper.AddHorizontal(world, ix, iy, mapper.Level, key);
                visited[ix, iy] = true;
                visited[ix + 1, iy] = true;
                return entity;
            }

            var downValue = MapLegacyDataLoader.GetActionCellValue(map.Layout, ix, iy + 1);
            var downAction = map.GetAction(downValue);

            if (downAction?.Name == templateName)
            {
                entity = doorHelper.AddVertical(world, ix, iy, mapper.Level, key);
                visited[ix, iy] = true;
                visited[ix, iy + 1] = true;
                return entity;
            }

            return entity;
        }

        #endregion Public Methods
    }
}