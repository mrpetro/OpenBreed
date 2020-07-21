using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Modules.Physics.Components;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Physics.Builders
{
    public class BodyComponentBuilder : BaseComponentBuilder<BodyComponentBuilder, BodyComponent>
    {
        #region Internal Fields

        internal float CofFactor;

        internal float CorFactor;

        internal string Type;

        internal List<int> Fixtures;

        #endregion Internal Fields

        #region Protected Constructors

        protected BodyComponentBuilder(ICore core) : base(core)
        {
        }

        #endregion Protected Constructors

        #region Public Methods

        public static IComponentBuilder New(ICore core)
        {
            return new BodyComponentBuilder(core);
        }

        public static void Register(ICore core)
        {
            core.Entities.RegisterComponentBuilder(nameof(BodyComponent), New);

            RegisterSetter("CofFactor", (o, value) => { o.CofFactor = Convert.ToSingle(value); });
            RegisterSetter("CorFactor", (o, value) => { o.CorFactor = Convert.ToSingle(value); });
            RegisterSetter("Type", (o, value) => { o.Type = Convert.ToString(value); });
            RegisterSetter("Fixtures", (o, value) => { o.Fixtures = o.ToFixtureIdList(value); });
        }

        public override IEntityComponent Build()
        {
            return new BodyComponent(this);
        }

        #endregion Public Methods

        #region Private Methods

        private List<int> ToFixtureIdList(object value)
        {
            if (value is List<int>)
                return (List<int>)value;

            var fixtureNames = ToStringArray(value);
            var fixtureIds = new List<int>();

            foreach (var fixtureName in fixtureNames)
            {
                var fixture = Core.GetModule<PhysicsModule>().Fixturs.GetByAlias(fixtureName);

                if (fixture != null)
                    fixtureIds.Add(fixture.Id);
            }

            return fixtureIds;
        }

        #endregion Private Methods
    }
}