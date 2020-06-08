using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Rendering.Builders
{
    public class CameraComponentBuilder : BaseComponentBuilder<CameraComponentBuilder, CameraComponent>
    {

        #region Private Constructors

        private CameraComponentBuilder(ICore core) : base(core)
        {
            Brightness = 1.0f;
        }

        #endregion Private Constructors

        #region Internal Properties

        internal float Width { get; private set; }
        internal float Height { get; private set; }
        internal float Brightness { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public static IComponentBuilder New(ICore core)
        {
            return new CameraComponentBuilder(core);
        }

        public override IEntityComponent Build()
        {
            return new CameraComponent(this);
        }

        public override void SetProperty(object key, object value)
        {
            var propertyName = Convert.ToString(key);
            switch (propertyName)
            {
                case nameof(Width):
                    Width = Convert.ToSingle(value);
                    break;

                case nameof(Height):
                    Height = Convert.ToSingle(value);
                    break;

                case nameof(Brightness):
                    Brightness = Convert.ToSingle(value);
                    break;

                default:
                    break;
            }
        }

        #endregion Public Methods

    }
}
