using OpenBreed.Common;
using OpenBreed.Physics.Interface.Managers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Components.Physics
{
    public interface IBodyFixtureTemplate
    {
        string ShapeName { get; set; }
        IEnumerable<string> Groups { get; }
    }

    public interface IBodyComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        float CofFactor { get; set; }
        float CorFactor { get; set; }
        IEnumerable<IBodyFixtureTemplate> Fixtures { get; }

        #endregion Public Properties
    }

    /// <summary>
    /// Physical Body data
    /// </summary>
    public class BodyComponent : IEntityComponent
    {
        #region Public Constructors

        public BodyComponent(BodyComponentBuilder builder)
        {
            CofFactor = builder.CofFactor;
            CorFactor = builder.CorFactor;
            Fixtures = builder.Fixtures;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Axis-aligned bounding box of this body
        /// </summary>
        public Box2 Aabb { get; set; }

        /// <summary>
        /// Coefficient of friction factor for this body.
        /// </summary>
        public float CofFactor { get; set; }

        /// <summary>
        /// Coefficient of restitution factor for this body.
        /// </summary>
        public float CorFactor { get; set; }

        /// <summary>
        /// List of Fixture IDs
        /// </summary>
        public List<BodyFixture> Fixtures { get; }

        #endregion Public Properties
    }

    public sealed class BodyComponentFactory : ComponentFactoryBase<IBodyComponentTemplate>
    {
        #region Private Fields

        private readonly IBuilderFactory builderFactory;

        #endregion Private Fields

        #region Public Constructors

        public BodyComponentFactory(IBuilderFactory builderFactory)
        {
            this.builderFactory = builderFactory;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IBodyComponentTemplate template)
        {
            var builder = builderFactory.GetBuilder<BodyComponentBuilder>();

            builder.SetCofFactor(template.CofFactor);
            builder.SetCorFactor(template.CorFactor);

            foreach (var fixture in template.Fixtures)
                builder.AddFixture(fixture.ShapeName, fixture.Groups);

            return builder.Build();
        }

        #endregion Protected Methods
    }

    public class BodyComponentBuilder : IBuilder<BodyComponent>
    {
        #region Internal Fields

        internal readonly List<BodyFixture> Fixtures = new List<BodyFixture>();
        internal float CofFactor;
        internal float CorFactor;

        #endregion Internal Fields

        #region Private Fields

        private readonly IShapeMan shapeMan;
        private readonly ICollisionMan collisionMan;

        #endregion Private Fields

        #region Internal Constructors

        internal BodyComponentBuilder(IShapeMan shapeMan, ICollisionMan collisionMan)
        {
            this.shapeMan = shapeMan;
            this.collisionMan = collisionMan;
        }

        #endregion Internal Constructors

        #region Public Methods

        public BodyComponent Build()
        {
            return new BodyComponent(this);
        }

        public void SetCofFactor(float cofFactor)
        {
            CofFactor = cofFactor;
        }

        public void SetCorFactor(float corFactor)
        {
            CorFactor = corFactor;
        }

        public void AddFixture(string shapeName, IEnumerable<string> groupNames)
        {
            var shapeId = shapeMan.GetIdByTag(shapeName);
            var groupIds = groupNames.Select(item => collisionMan.GetGroupId(item));
            Fixtures.Add(new BodyFixture(shapeId, groupIds));
        }

        #endregion Public Methods
    }
}