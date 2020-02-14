using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Physics.Builders
{
    public class BodyComponentBuilder : BaseComponentBuilder
    {
        #region Private Fields

        private static Dictionary<string, Action<BodyComponentBuilder, object>> setters = new Dictionary<string, Action<BodyComponentBuilder, object>>();
        private float cofFactor;

        private float corFactor;

        private string type;

        private List<string> fixtures;

        #endregion Private Fields

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
            core.Entities.RegisterComponentBuilder("BodyComponent", New);

            RegisterSetter("CofFactor", (o, value) => { o.cofFactor = Convert.ToSingle(value); });
            RegisterSetter("CorFactor", (o, value) => { o.corFactor = Convert.ToSingle(value); });
            RegisterSetter("Type", (o, value) => { o.type = Convert.ToString(value); });
            RegisterSetter("Fixtures", (o, value) => { o.fixtures = ToStringArray(value); });
        }

        public override IEntityComponent Build()
        {
            var fixtureIds = new List<int>();

            if (fixtures != null)
            {
                foreach (var item in fixtures)
                {
                    var fixture = Core.GetModule<PhysicsModule>().Fixturs.GetByAlias(item);

                    if (fixture != null)
                        fixtureIds.Add(fixture.Id);
                }
            }

            return new BodyComponent() { CofFactor = cofFactor, CorFactor = corFactor, Fixtures = fixtureIds, Tag = type };
        }

        public override void SetProperty(object key, object value)
        {
            var propertyName = Convert.ToString(key);
            setters[propertyName].Invoke(this, value);
        }

        #endregion Public Methods

        #region Protected Methods

        protected static void RegisterSetter(string name, Action<BodyComponentBuilder, object> setter)
        {
            setters.Add(name, setter);
        }

        #endregion Protected Methods

        #region Private Methods

        #endregion Private Methods
    }
}