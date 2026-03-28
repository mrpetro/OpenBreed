using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
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

        private IWorldMan worldMan;
        private IEntityFactory entityFactory;

        #endregion Private Fields

        #region Public Constructors

        public GenericCellEntityLoader(IWorldMan worldMan, IEntityFactory entityFactory)
        {
            this.worldMan = worldMan ?? throw new System.ArgumentNullException(nameof(worldMan));
            this.entityFactory = entityFactory ?? throw new System.ArgumentNullException(nameof(entityFactory));
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntity Load(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, IWorld world)
        {
            IEntity entity = null;

            switch (templateName)
            {
                case "FullObstacle":
                    entity = entityFactory.CreateFullObstacleCell(ix, iy, mapAssets.Level, gfxValue);
                    break;

                case "ActorOnlyObstacle":
                    entity = entityFactory.CreateActorOnlyObstacleCell(ix, iy, mapAssets.Level, gfxValue);
                    break;

                case "Void":
                    entity = entityFactory.CreateVoidCell(ix, iy, mapAssets.Level, gfxValue);
                    break;

                case "ObstacleDownLeft":
                    entity = entityFactory.CreateSlopeObstacleCell(ix, iy, mapAssets.Level, gfxValue, "DownLeft");
                    break;

                case "ObstacleDownRight":
                    entity = entityFactory.CreateSlopeObstacleCell(ix, iy, mapAssets.Level, gfxValue, "DownRight");
                    break;

                case "ObstacleUpLeft":
                    entity = entityFactory.CreateSlopeObstacleCell(ix, iy, mapAssets.Level, gfxValue, "UpLeft");
                    break;

                case "ObstacleUpRight":
                    entity = entityFactory.CreateSlopeObstacleCell(ix, iy, mapAssets.Level, gfxValue, "UpRight");
                    break;
            }

            visited[ix, iy] = true;

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        #endregion Public Methods
    }
}