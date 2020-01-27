using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Animation.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Animation.Builders
{
    public class AnimatorComponentBuilder : IComponentBuilder
    {
        #region Private Fields

        private float speed;
        private bool loop;

        #endregion Private Fields

        #region Public Methods

        public static IComponentBuilder New()
        {
            return new AnimatorComponentBuilder();
        }

        public IEntityComponent Build()
        {
            return new Animator(speed, loop);
        }

        public void SetProperty(object key, object value)
        {
            var propertyName = Convert.ToString(key);

            if (propertyName == "Speed")
                speed = Convert.ToSingle(value);
            else if (propertyName == "Loop")
                loop = Convert.ToBoolean(value);
            else
                throw new ArgumentException("Too many property keys given.");
        }

        #endregion Public Methods
    }
}
