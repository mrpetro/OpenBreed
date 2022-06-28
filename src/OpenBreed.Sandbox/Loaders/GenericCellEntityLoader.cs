using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Wecs.Entities;
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

        public Entity Load(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, World world)
        {
            Entity entity = null;

            switch (templateName)
            {
                case "FullObstacle":
                    entity = genericCellHelper.AddFullObstacleCell(world, ix, iy, mapAssets.Level, gfxValue);
                    break;
                case "ActorOnlyObstacle":
                    entity = genericCellHelper.AddActorOnlyObstacleCell(world, ix, iy, mapAssets.Level, gfxValue);
                    break;
                case "Void":
                    entity = genericCellHelper.AddVoidCell(world, ix, iy, mapAssets.Level, gfxValue);
                    break;
                case "ObstacleDownLeft":
                    entity = genericCellHelper.AddSlopeObstacleCell(world, ix, iy, mapAssets.Level, gfxValue, "DownLeft");
                    break;
                case "ObstacleDownRight":
                    entity = genericCellHelper.AddSlopeObstacleCell(world, ix, iy, mapAssets.Level, gfxValue, "DownRight");
                    break;
                case "ObstacleUpLeft":
                    entity = genericCellHelper.AddSlopeObstacleCell(world, ix, iy, mapAssets.Level, gfxValue, "UpLeft");
                    break;
                case "ObstacleUpRight":
                    entity = genericCellHelper.AddSlopeObstacleCell(world, ix, iy, mapAssets.Level, gfxValue, "UpRight");
                    break;
            }

            visited[ix, iy] = true;
            return entity;
        }

        #endregion Public Methods
    }
}