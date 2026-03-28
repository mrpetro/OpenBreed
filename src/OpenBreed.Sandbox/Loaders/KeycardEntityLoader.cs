using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Loaders
{
    public class KeycardEntityLoader : IMapWorldEntityLoader
    {
        private readonly IWorldMan worldMan;
        private readonly IEntityFactory entityFactory;
        #region Private Fields

        #endregion Private Fields

        #region Public Constructors

        public KeycardEntityLoader(IWorldMan worldMan, IEntityFactory entityFactory)
        {
            this.worldMan = worldMan ?? throw new System.ArgumentNullException(nameof(worldMan));
            this.entityFactory = entityFactory ?? throw new System.ArgumentNullException(nameof(entityFactory));
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntity Load(MapMapper mapper, MapModel map, bool[,] visited, int ix, int iy, string actionName, string flavor, int gfxValue, IWorld world)
        {
            if (!mapper.TryGetEntityType(actionName, out string entityType, out string option))
                return null;

            if (!mapper.TryGetFlavor("Keycard", gfxValue, out flavor))
                return null;

            var entity = entityFactory.CreateItem(ix, iy, entityType, mapper.Level, gfxValue, option, flavor);
            visited[ix, iy] = true;

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        #endregion Public Methods
    }
}