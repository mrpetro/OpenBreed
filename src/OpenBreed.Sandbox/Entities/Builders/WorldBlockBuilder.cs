using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Modules;
using OpenBreed.Physics.Interface;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Rendering.Interface;
using OpenTK;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Wecs.Entities.Builders;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;

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

            physics = core.GetModule<IPhysicsModule>();
        }

        private IPhysicsModule physics;

        #endregion Public Constructors

        #region Public Methods

        public void SetPosition(Vector2 pos )
        {
            this.pos = pos;
        }

        public void SetTileAtlas(string atlasAlias)
        {
            var atlas = Core.GetModule<IRenderModule>().Tiles.GetByAlias(atlasAlias);
            this.atlasId = atlas.Id;
        }

        public void SetTileId(int tileId)
        {
            this.tileId = tileId;
        }

        public bool HasBody { get; set; }

        public override Entity Build()
        {
            var entity = Core.GetManager<IEntityMan>().Create();

            entity.Add(PositionComponent.Create(pos));


            if (HasBody)
            {
                var bodyComponentBuilder = BodyComponentBuilderEx.New(Core);

                var fixtureId = physics.Fixturs.GetByAlias("Fixtures/GridCell").Id;

                bodyComponentBuilder.SetCofFactor(1.0f);
                bodyComponentBuilder.SetCorFactor(1.0f);
                bodyComponentBuilder.SetType("Static");
                bodyComponentBuilder.AddFixture(fixtureId);

                entity.Add(bodyComponentBuilder.Build());
                entity.Add(new CollisionComponent(ColliderTypes.StaticObstacle));
            }


            var tileComponentBuilder = TileComponentBuilder.New(Core);
            tileComponentBuilder.SetAtlasById(atlasId);
            tileComponentBuilder.SetImageIndex(tileId);

            entity.Add(tileComponentBuilder.Build());

            return entity;
        }

        #endregion Public Methods
    }
}