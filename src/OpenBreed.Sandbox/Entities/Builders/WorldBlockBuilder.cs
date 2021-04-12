using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Builders;
using OpenTK;

namespace OpenBreed.Sandbox.Entities.Builders
{
    public class WorldBlockBuilder : EntityBuilder
    {
        #region Internal Fields

        internal Vector2 pos;
        internal int atlasId;
        internal int tileId;

        #endregion Internal Fields

        #region Private Fields

        private readonly ITileMan tileMan;
        private readonly IFixtureMan fixtureMan;
        private readonly BodyComponentBuilder bodyComponentBuilder;

        #endregion Private Fields

        #region Internal Constructors

        internal WorldBlockBuilder(ITileMan tileMan, IFixtureMan fixtureMan, IEntityMan entityMan, BodyComponentBuilder bodyComponentBuilder) : base(entityMan)
        {
            this.tileMan = tileMan;
            this.fixtureMan = fixtureMan;
            this.bodyComponentBuilder = bodyComponentBuilder;

            HasBody = true;
        }

        #endregion Internal Constructors

        #region Public Properties

        public bool HasBody { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void SetPosition(Vector2 pos)
        {
            this.pos = pos;
        }

        public void SetTileAtlas(string atlasAlias)
        {
            var atlas = tileMan.GetByAlias(atlasAlias);
            this.atlasId = atlas.Id;
        }

        public void SetTileId(int tileId)
        {
            this.tileId = tileId;
        }

        public override Entity Build()
        {
            var entity = entityMan.Create();
            entity.Add(PositionComponent.Create(pos));

            if (HasBody)
            {
                var fixtureId = fixtureMan.GetByAlias("Fixtures/GridCell").Id;

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