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
    public class LandMineEntityLoader : IMapWorldEntityLoader
    {
        #region Private Fields

        private readonly GenericCellHelper genericCellHelper;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public LandMineEntityLoader(GenericCellHelper genericCellHelper, ILogger logger)
        {
            this.genericCellHelper = genericCellHelper;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntity Load(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, World world)
        {
            var entity = default(IEntity);

            switch (templateName)
            {
                case "LandMine":
                    entity = genericCellHelper.AddLandMineCell(world, ix, iy, mapAssets.Level, gfxValue);
                    break;
            }

            visited[ix, iy] = true;
            return entity;
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

        #endregion Private Methods
    }
}