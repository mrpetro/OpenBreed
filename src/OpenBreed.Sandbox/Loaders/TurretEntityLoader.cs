using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;

namespace OpenBreed.Sandbox.Loaders
{
    internal class TurretEntryLoader : IMapWorldEntityLoader
    {
        #region Private Fields

        private readonly IWorldMan worldMan;
        private readonly IEntityFactory entityFactory;

        #endregion Private Fields

        #region Internal Constructors

        internal TurretEntryLoader(IWorldMan worldMan, IEntityFactory entityFactory)
        {
            this.worldMan = worldMan ?? throw new System.ArgumentNullException(nameof(worldMan));
            this.entityFactory = entityFactory ?? throw new System.ArgumentNullException(nameof(entityFactory));
        }

        #endregion Internal Constructors

        #region Public Methods

        public IEntity Load(MapMapper mapMapper, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, IWorld world)
        {
            var entity = entityFactory.CreateHeavyTurret((ix + 1) * 16 + 8 , iy * 16 - 8);
            visited[ix, iy] = true;

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        #endregion Public Methods

    }
}
