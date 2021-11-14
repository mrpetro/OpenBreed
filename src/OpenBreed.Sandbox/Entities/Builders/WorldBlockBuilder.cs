using OpenBreed.Common;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Builders;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
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
        private readonly IShapeMan shapeMan;
        private readonly IBuilderFactory builderFactory;

        #endregion Private Fields

        #region Internal Constructors

        internal WorldBlockBuilder(ITileMan tileMan, IShapeMan shapeMan, IEntityMan entityMan, IBuilderFactory builderFactory) : base(entityMan)
        {
            this.tileMan = tileMan;
            this.shapeMan = shapeMan;
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
                var bodyComponentBuilder = builderFactory.GetBuilder<BodyComponentBuilder>();

                bodyComponentBuilder.SetCofFactor(1.0f);
                bodyComponentBuilder.SetCorFactor(1.0f);
                bodyComponentBuilder.AddFixture("Shapes/Box_0_0_16_16", new string[] { "StaticObstacle" });

                entity.Add(bodyComponentBuilder.Build());
            }

            entity.PutTile(atlasId, tileId, 0, pos);

            return entity;
        }

        #endregion Public Methods
    }
}