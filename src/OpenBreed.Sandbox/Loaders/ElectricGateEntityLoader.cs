using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Common.Game.Services;

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
        private readonly IGameServices gameServices;

        #endregion Private Fields

        #region Public Constructors

        public ElectricGateEntityLoader(IWorldMan worldMan, IEntityFactory entityFactory, IGameServices gameServices)
        {
            this.worldMan = worldMan ?? throw new System.ArgumentNullException(nameof(worldMan));
            this.entityFactory = entityFactory ?? throw new System.ArgumentNullException(nameof(entityFactory));
            this.gameServices = gameServices ?? throw new System.ArgumentNullException(nameof(gameServices));
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
                    entity = gameServices.PutPassUpDown(mapAssets, map, visited, ix, iy, gfxValue, templateName, world);
                    break;

                case PASS_RIGHT:
                case PASS_LEFT:
                    entity = gameServices.PutPassRightLeft(mapAssets, map, visited, ix, iy, gfxValue, templateName, world);
                    break;

                default:
                    break;
            }

            return entity;
        }

        #endregion Public Methods
    }
}