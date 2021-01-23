using OpenBreed.Core;
using OpenBreed.Components.Common;
using OpenBreed.Core.Modules;
using OpenBreed.Physics.Interface;
using OpenTK;
using System;
using System.Collections.Generic;
using OpenBreed.Ecsw.Components;

namespace OpenBreed.Components.Physics
{
    public interface IBodyComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        float CofFactor { get; set; }
        float CorFactor { get; set; }
        string Type { get; set; }
        string[] Fixtures { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    /// Physical Body data
    /// </summary>
    public class BodyComponent : IEntityComponent
    {
        #region Public Constructors

        public BodyComponent(BodyComponentBuilderEx builder)
        {
            CofFactor = builder.CofFactor;
            CorFactor = builder.CorFactor;
            Fixtures = builder.Fixtures;
            Tag = builder.Type;
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
        public List<int> Fixtures { get; }

        /// <summary>
        /// User defined tag
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// DEBUG only
        /// </summary>
        ///
        public List<Tuple<int, int>> Boxes { get; set; }

        /// <summary>
        /// DEBUG only
        /// </summary>
        ///
        public Vector2 Projection { get; set; }

        /// <summary>
        /// OldPosition used for verlet integration
        /// </summary>
        public Vector2 OldPosition { get; set; }

        #endregion Public Properties
    }

    public sealed class BodyComponentFactory : ComponentFactoryBase<IBodyComponentTemplate>
    {
        #region Public Constructors

        public BodyComponentFactory(ICore core) : base(core)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IBodyComponentTemplate template)
        {
            var builder = BodyComponentBuilderEx.New(core);
            builder.SetCofFactor(template.CofFactor);
            builder.SetCorFactor(template.CorFactor);
            builder.SetType(template.Type);

            foreach (var fixtureName in template.Fixtures)
                builder.AddFixtureByName(fixtureName);

            return builder.Build();
        }

        #endregion Protected Methods
    }

    public class BodyComponentBuilderEx
    {
        #region Internal Fields

        internal float CofFactor;
        internal float CorFactor;
        internal string Type;
        internal readonly List<int> Fixtures = new List<int>();

        #endregion Internal Fields

        #region Private Fields

        private ICore core;

        #endregion Private Fields

        #region Private Constructors

        private BodyComponentBuilderEx(ICore core)
        {
            this.core = core;
        }

        #endregion Private Constructors

        #region Public Methods

        public static BodyComponentBuilderEx New(ICore core)
        {
            return new BodyComponentBuilderEx(core);
        }

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

        public void AddFixture(int fixtureId)
        {
            Fixtures.Add(fixtureId);
        }

        public void AddFixtureByName(string fixtureName)
        {
            var fixture = core.GetModule<IPhysicsModule>().Fixturs.GetByAlias(fixtureName);

            if (fixture != null)
                Fixtures.Add(fixture.Id);
        }

        public void SetType(string type)
        {
            Type = type;
        }

        #endregion Public Methods
    }
}