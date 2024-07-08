using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Mathematics;
using System.Linq;

namespace OpenBreed.Sandbox.Loaders
{
    public class TeleportCellEntityLoader : IMapWorldEntityLoader
    {
        #region Private Fields

        private readonly TeleportHelper teleportHelper;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public TeleportCellEntityLoader(TeleportHelper teleportHelper, ILogger logger)
        {
            this.teleportHelper = teleportHelper;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntity Load(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, IWorld world)
        {
            var layout = map.Layout;

            if (templateName == "TeleportEntry")
            {
                if (!FindFarthestExit(layout, visited, ix, iy, out (int X, int Y) found))
                    return null;

                var groupLayerIdx = layout.GetLayerIndex(MapLayerType.Group);
                var gfxLayerIdx = layout.GetLayerIndex(MapLayerType.Gfx);

                var groupId = layout.GetCellValue(groupLayerIdx, ix, iy);

                var cells = layout.FindCellsWithValue(groupLayerIdx, groupId);

                foreach (var cell in cells)
                {
                    var cellGfxValue = layout.GetCellValue(gfxLayerIdx, cell.X, cell.Y);
                    teleportHelper.AddTeleportEntry(world, cell.X, cell.Y, ix, mapAssets.Level, cellGfxValue);
                    visited[cell.X, cell.Y] = true;
                }

                var exitGfxValue = layout.GetCellValue(gfxLayerIdx, found.X, found.Y);

                var entity = teleportHelper.AddTeleportExit(world, found.X, found.Y, ix, mapAssets.Level, exitGfxValue);
                visited[found.X, found.Y] = true;

                return entity;
            }
            //else if (templateName == "TeleportExit")
            //{
            //}
            //visited[ix, iy] = true;

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private bool FindFarthestExit(MapLayoutModel layout, bool[,] visited, int inX, int inY, out (int X, int Y) found)
        {
            found = (0, 0);
            var layerIndex = layout.GetLayerIndex(MapLayerType.Action);

            var cells = layout.FindCellsWithValue(layerIndex, 36).ToArray();

            cells = cells.Where(item => !visited[item.X, item.Y]).ToArray();

            //Found all not visited exits
            var foundExits = cells;

            //No exit was found which is not expected
            if (!foundExits.Any())
            {
                logger.LogWarning("No teleport exit could be found for entry at ({0},{1}).", inX, inY);

                return false;
            }

            var inP = new Vector2(inX, inY);
            var farthestIndex = 0;
            var farthestDistance = Vector2.Distance(inP, new Vector2(foundExits[0].X, foundExits[0].Y));

            for (var i = 1; i < foundExits.Length; i++)
            {
                var outP = new Vector2(foundExits[i].X, foundExits[i].Y);

                var distance = Vector2.Distance(inP, outP);
                if (distance > farthestDistance)
                {
                    farthestIndex = i;
                    farthestDistance = distance;
                }
            }

            found = foundExits[farthestIndex];
            return true;
        }

        #endregion Private Methods
    }
}