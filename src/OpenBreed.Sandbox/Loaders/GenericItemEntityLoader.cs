using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Loaders
{
    public class GenericItemEntityLoader : IMapWorldEntityLoader
    {
        #region Private Fields

        private readonly IWorldMan worldMan;
        private readonly IEntityFactory entityFactory;

        #endregion Private Fields

        #region Public Constructors

        public GenericItemEntityLoader(IWorldMan worldMan, IEntityFactory entityFactory)
        {
            this.worldMan = worldMan ?? throw new System.ArgumentNullException(nameof(worldMan));
            this.entityFactory = entityFactory ?? throw new System.ArgumentNullException(nameof(entityFactory));
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntity Load(MapMapper mapper, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, IWorld world)
        {
            var entity = default(IEntity);

            if (!mapper.TryGetFlavor(templateName, gfxValue, out flavor))
                return entity;

            if (flavor is null)
                return entity;

            var split = flavor.Split('/');

            templateName = split[2];

            entity = entityFactory.CreateItem(ix, iy, templateName, mapper.Level, gfxValue, null);
            entity.SetMetadata("Flavor", flavor);
            visited[ix, iy] = true;

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        #endregion Public Methods
    }
}