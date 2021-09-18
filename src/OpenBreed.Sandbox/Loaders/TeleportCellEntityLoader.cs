using OpenBreed.Common.Logging;
using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Loaders
{
    public class TeleportCellEntityLoader : IMapWorldEntityLoader
    {
        #region Public Fields

        public const int ENTRY_CODE = 31;
        public const int EXIT_CODE = 36;

        #endregion Public Fields

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

        public void Load(WorldBlockBuilder worldBlockBuilder, MapLayoutModel layout, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            if (actionValue == ENTRY_CODE)
            {
                if (!FindFarthestExit(layout, visited, ix, iy, out (int X, int Y) found))
                    return;

                var layerIndex = layout.GetLayerIndex(MapLayerType.Group);

                var groupId = layout.GetCellValue(layerIndex, ix, iy);


                var cells = layout.FindCellsWithValue(layerIndex, groupId);

                foreach (var cell in cells)
                {
                    teleportHelper.AddTeleportEntry(world, cell.X, cell.Y, ix);
                    visited[cell.X, cell.Y] = true;
                }

                teleportHelper.AddTeleportExit(world, found.X, found.Y, ix);
                visited[found.X, found.Y] = true;
            }

            return;
        }



        private bool FindFarthestExit(MapLayoutModel layout, bool[,] visited, int inX, int inY, out (int X, int Y) found)
        {
            found = (0, 0);
            var layerIndex = layout.GetLayerIndex(MapLayerType.Action);

            var cells = layout.FindCellsWithValue(layerIndex, EXIT_CODE).ToArray();

            cells = cells.Where(item => !visited[item.X, item.Y]).ToArray();

            //Found all not visited exits
            var foundExits = cells;

            //No exit was found which is not expected
            if (!foundExits.Any())
            {
                logger.Warning($"No teleport exit could be found for entry at ({inX},{inY})");

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

        #endregion Public Methods
    }
}