using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core.Common.Builders
{
    public class WorldComponentBuilder : BaseComponentBuilder<WorldComponentBuilder, WorldComponent>
    {
        #region Protected Constructors

        protected WorldComponentBuilder(ICore core) : base(core)
        {
            WorldId = -1;
        }

        #endregion Protected Constructors

        #region Internal Properties

        internal int WorldId { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public static WorldComponentBuilder NewSpec(ICore core)
        {
            return new WorldComponentBuilder(core);
        }

        public static IComponentBuilder New(ICore core)
        {
            return new WorldComponentBuilder(core);
        }

        public override IEntityComponent Build()
        {
            return new WorldComponent(this);
        }

        public int ToWorldId(object value)
        {
            if (value is int)
                return (int)value;

            return Core.Worlds.GetByName(Convert.ToString(value)).Id;
        }

        public override void SetProperty(object key, object value)
        {
            var index = Convert.ToInt64(key);

            if (index == 1)
                WorldId = ToWorldId(value);
            else
                throw new ArgumentException("Too many property keys given.");
        }

        #endregion Public Methods
    }
}