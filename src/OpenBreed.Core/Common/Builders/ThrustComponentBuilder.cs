using OpenBreed.Core.Common.Systems.Components;
using System;

namespace OpenBreed.Core.Common.Builders
{
    public class ThrustComponentBuilder : IComponentBuilder
    {
        #region Private Fields

        private float x;
        private float y;

        #endregion Private Fields

        #region Public Methods

        public static IComponentBuilder New()
        {
            return new ThrustComponentBuilder();
        }

        public IEntityComponent Build()
        {
            return Thrust.Create(x, y);
        }

        public void SetProperty(object key, object value)
        {
            var index = Convert.ToInt64(key);

            if (index == 1)
                x = Convert.ToSingle(value);
            else if (index == 2)
                y = Convert.ToSingle(value);
            else
                throw new ArgumentException("Too many property keys given.");
        }

        #endregion Public Methods
    }
}