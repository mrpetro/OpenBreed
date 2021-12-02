using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Loaders
{
    public class GenericCellEntityLoader : IMapWorldEntityLoader
    {
        #region Public Fields

        public const int ACTOR_ONLY_OBSTACLE_CODE = 60;
        public const int FULL_OBSTACLE_CODE = 63;
        public const int VOID_CODE = 0;

        #endregion Public Fields

        #region Private Fields

        private readonly GenericCellHelper genericCellHelper;

        #endregion Private Fields

        #region Public Constructors

        public GenericCellEntityLoader(GenericCellHelper genericCellHelper)
        {
            this.genericCellHelper = genericCellHelper;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Load(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, World world)
        {
            switch (templateName)
            {
                case "FullObstacle":
                    genericCellHelper.AddFullObstacleCell(world, ix, iy, mapAssets.Level, gfxValue);
                    break;
                case "ActorOnlyObstacle":
                    genericCellHelper.AddActorOnlyObstacleCell(world, ix, iy, mapAssets.Level, gfxValue);
                    break;
                case "Void":
                    genericCellHelper.AddVoidCell(world, ix, iy, mapAssets.Level, gfxValue);
                    break;
                case "ObstacleDownLeft":
                    genericCellHelper.AddSlopeObstacleCell(world, ix, iy, mapAssets.Level, gfxValue, "DownLeft");
                    break;
                case "ObstacleDownRight":
                    genericCellHelper.AddSlopeObstacleCell(world, ix, iy, mapAssets.Level, gfxValue, "DownRight");
                    break;
                case "ObstacleUpLeft":
                    genericCellHelper.AddSlopeObstacleCell(world, ix, iy, mapAssets.Level, gfxValue, "UpLeft");
                    break;
                case "ObstacleUpRight":
                    genericCellHelper.AddSlopeObstacleCell(world, ix, iy, mapAssets.Level, gfxValue, "UpRight");
                    break;
            }

            visited[ix, iy] = true;
        }

        #endregion Public Methods
    }
}