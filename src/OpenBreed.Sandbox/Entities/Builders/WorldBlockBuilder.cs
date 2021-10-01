using OpenBreed.Common;
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
        public int atlasId;
        internal int tileId;
        internal int groupId;

        #endregion Internal Fields

        #region Private Fields

        private readonly ITileMan tileMan;
        private readonly IFixtureMan fixtureMan;
        private readonly IBuilderFactory builderFactory;

        #endregion Private Fields

        #region Internal Constructors

        internal WorldBlockBuilder(ITileMan tileMan, IFixtureMan fixtureMan, IEntityMan entityMan, IBuilderFactory builderFactory) : base(entityMan)
        {
            this.tileMan = tileMan;
            this.fixtureMan = fixtureMan;
            this.builderFactory = builderFactory;
            HasBody = true;
        }

        #endregion Internal Constructors

        #region Public Properties

        public bool HasBody { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void SetPosition(float x, float y)
        {
            this.pos = new Vector2(x, y);
        }

        public void SetTileAtlas(string atlasAlias)
        {
            var atlas = tileMan.GetByAlias(atlasAlias);
            this.atlasId = atlas.Id;
        }

        public void SetTileAtlas(int atlasId)
        {
            this.atlasId = atlasId;
        }

        public void SetTileId(int tileId)
        {
            this.tileId = tileId;
        }

        internal void SetGroupId(int groupId)
        {
            this.groupId = groupId;
        }


        public override Entity Build()
        {
            var entity = entityMan.Create();
            entity.Add(PositionComponent.Create(pos));
            entity.Add(new GroupComponentEx(groupId));

            if (HasBody)
            {
                var fixtureId = fixtureMan.GetByAlias("Fixtures/GridCell").Id;

                var bodyComponentBuilder = builderFactory.GetBuilder<BodyComponentBuilder>();

                bodyComponentBuilder.SetCofFactor(1.0f);
                bodyComponentBuilder.SetCorFactor(1.0f);
                bodyComponentBuilder.SetType("Static");
                bodyComponentBuilder.AddFixture(fixtureId);

                entity.Add(bodyComponentBuilder.Build());
                entity.Add(new ColliderComponent(ColliderTypes.StaticObstacle));
            }

            var tileComponentBuilder = builderFactory.GetBuilder<TileComponentBuilder>();
            tileComponentBuilder.SetAtlasById(atlasId);
            tileComponentBuilder.SetImageIndex(tileId);

            entity.Add(tileComponentBuilder.Build());

            return entity;
        }

        #endregion Public Methods
    }
}