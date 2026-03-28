using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Sandbox.Extensions;

namespace OpenBreed.Sandbox.Loaders
{
    public class ElectricGateEntityLoader : IMapWorldEntityLoader
    {
        #region Public Fields

        public const string PASS_UP = "ElectricGateUp";
        public const string PASS_DOWN = "ElectricGateDown";
        public const string PASS_RIGHT = "ElectricGateRight";
        public const string PASS_LEFT = "ElectricGateLeft";
        private readonly IWorldMan worldMan;

        #endregion Public Fields

        #region Private Fields

        private readonly IEntityFactory entityFactory;

        #endregion Private Fields

        #region Public Constructors

        public ElectricGateEntityLoader(IWorldMan worldMan, IEntityFactory entityFactory)
        {
            this.worldMan = worldMan ?? throw new System.ArgumentNullException(nameof(worldMan));
            this.entityFactory = entityFactory ?? throw new System.ArgumentNullException(nameof(entityFactory));
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntity Load(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, IWorld world)
        {
            var entity = default(IEntity);

            switch (templateName)
            {
                case PASS_UP:
                case PASS_DOWN:
                    entity = PutPassUpDown(mapAssets, map, visited, ix, iy, gfxValue, templateName, world);
                    break;

                case PASS_RIGHT:
                case PASS_LEFT:
                    entity = PutPassRightLeft(mapAssets, map, visited, ix, iy, gfxValue, templateName, world);
                    break;

                default:
                    break;
            }

            return entity;
        }

        #endregion Public Methods

        #region Private Methods

        private IEntity PutPassUpDown(MapMapper mapper, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, string templateName, IWorld world)
        {
            var rightValue = MapLegacyDataLoader.GetActionCellValue(map.Layout, ix + 1, iy);
            var rightAction = map.GetAction(rightValue);

            if (rightAction?.Name == templateName)
            {
                var entity = entityFactory.CreateElectricGateHorizontal(ix, iy, mapper.Level);
                visited[ix, iy] = true;
                visited[ix + 1, iy] = true;

                worldMan.RequestAddEntity(entity, world.Id);

                return entity;
            }
            else
            {
                var entity = entityFactory.CreateElectricGateHorizontal(ix - 1, iy, mapper.Level);
                visited[ix, iy] = true;
                visited[ix - 1, iy] = true;

                worldMan.RequestAddEntity(entity, world.Id);

                return entity;
            }

            return null;
        }

        private IEntity PutPassRightLeft(MapMapper mapper, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, string templateName, IWorld world)
        {
            var downValue = MapLegacyDataLoader.GetActionCellValue(map.Layout, ix, iy + 1);
            var downAction = map.GetAction(downValue);

            if (downAction?.Name == templateName)
            {
                var entity = entityFactory.CreateElectricGateVertical(ix, iy, mapper.Level);
                visited[ix, iy] = true;
                visited[ix, iy + 1] = true;

                worldMan.RequestAddEntity(entity, world.Id);

                return entity;
            }
            else
            {
                var entity = entityFactory.CreateElectricGateVertical(ix, iy - 1, mapper.Level);
                visited[ix, iy] = true;
                visited[ix, iy - 1] = true;

                worldMan.RequestAddEntity(entity, world.Id);

                return entity;
            }
        }

        #endregion Private Methods
    }
}