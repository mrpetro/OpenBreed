using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Components.Physics.Builders;
using OpenBreed.Wecs.Entities;
using OpenTK;
using OpenTK.Mathematics;
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

        /// <summary>
        /// Flag for disabling/enabling this entity body collisions
        /// </summary>
        public bool Inactive { get; set; }

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
            var bodyComponentBuilder = builderFactory.GetBuilder<BodyComponentBuilder>();

            var fixtureBuilder = builderFactory.GetBuilder<BodyFixtureBuilder>();

            bodyComponentBuilder.SetCofFactor(template.CofFactor);
            bodyComponentBuilder.SetCorFactor(template.CorFactor);

            foreach (var fixture in template.Fixtures)
            {
                fixtureBuilder.ClearGroups();

                fixtureBuilder.SetShape(fixture.ShapeName);

                foreach (var groupName in fixture.Groups)
                {
                    fixtureBuilder.AddGroup(groupName);
                }

                bodyComponentBuilder.AddFixture(fixtureBuilder.Build());
            }

            return bodyComponentBuilder.Build();
        }

        #endregion Protected Methods
    }
}