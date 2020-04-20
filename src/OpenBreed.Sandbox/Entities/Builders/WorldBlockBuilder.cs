using OpenBreed.Core;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Entities.Builders;
using OpenBreed.Core.Modules.Physics;
using OpenBreed.Core.Modules.Physics.Builders;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Builders
{
    public class WorldBlockBuilder : EntityBuilder
    {
        #region Internal Fields

        internal Vector2 pos;
        internal int atlasId;
        internal int tileId;

        #endregion Internal Fields

        #region Public Constructors

        public WorldBlockBuilder(ICore core) : base(core)
        {
            HasBody = true;

            physics = core.GetModule<PhysicsModule>();
        }

        private PhysicsModule physics;

        #endregion Public Constructors

        #region Public Methods

        public void SetPosition(Vector2 pos )
        {
            this.pos = pos;
        }

        public void SetTileAtlas(string atlasAlias)
        {
            var atlas = Core.Rendering.Tiles.GetByAlias(atlasAlias);
            this.atlasId = atlas.Id;
        }

        public void SetTileId(int tileId)
        {
            this.tileId = tileId;
        }

        public bool HasBody { get; set; }

        public override IEntity Build()
        {
            var entity = Core.Entities.Create();

            entity.Add(PositionComponent.Create(pos));


            if (HasBody)
            {
                var bodyComponentBuilder = BodyComponentBuilder.New(Core);

                var fixtureId = physics.Fixturs.GetByAlias("Fixtures/GridCell").Id;

                bodyComponentBuilder.SetProperty("CofFactor", 1.0f);
                bodyComponentBuilder.SetProperty("CorFactor", 1.0f);
                bodyComponentBuilder.SetProperty("Type", "Static");
                bodyComponentBuilder.SetProperty("Fixtures", new List<int> (new int[] { fixtureId }));

                entity.Add(bodyComponentBuilder.Build());
            }


            var tileComponentBuilder = TileComponentBuilder.New(Core);
            tileComponentBuilder.SetProperty("AtlasId", atlasId);
            tileComponentBuilder.SetProperty("ImageId", tileId);

            entity.Add(tileComponentBuilder.Build());

            return entity;
        }

        #endregion Public Methods
    }
}