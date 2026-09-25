using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Text;
using OpenBreed.Wecs.Core.Components.Extensions;

namespace OpenBreed.Sandbox.Loaders
{
    public class ActorLoader : IMapWorldEntityLoader
    {
        #region Private Fields

        private readonly IWorldMan worldMan;
        private readonly IEntityFactory entityFactory;

        #endregion Private Fields

        #region Internal Constructors

        internal ActorLoader(IWorldMan worldMan, IEntityFactory entityFactory)
        {
            this.worldMan = worldMan ?? throw new System.ArgumentNullException(nameof(worldMan));
            this.entityFactory = entityFactory ?? throw new System.ArgumentNullException(nameof(entityFactory));
        }

        #endregion Internal Constructors

        #region Public Methods

        public IEntity Load(MapMapper mapMapper, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, IWorld world)
        {
            IEntity entity = null;

            switch (templateName)
            {
                case "L1/Lizard":
                    entity = PutActor(mapMapper, map, visited, world, ix, iy, gfxValue, "L1", "Lizard");
                    break;

                default:
                    break;
            }

            entity.SetMetadata("Level", mapMapper.Level);

            return entity;
        }

        #endregion Public Methods

        #region Private Methods

        private IEntity PutActor(MapMapper mapMapper, MapModel map, bool[,] visited, IWorld world, int ix, int iy, int gfxValue, string level, string templateName)
        {
            var entity = entityFactory.Create($@"ABTA\Templates\{level}\{templateName}")
                .SetParameter("startX", 16 * ix)
                .SetParameter("startY", 16 * iy)
                .Build();

            visited[ix, iy] = true;
            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        #endregion Private Methods
    }
}