using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using System;

namespace OpenBreed.Core.Modules.Physics.Builders
{
    public class SpriteComponentBuilder : BaseComponentBuilder
    {
        #region Private Fields

        internal int AtlasId { get; private set; }
        internal string AtlasAlias
        {
            set
            {
                AtlasId = Core.Rendering.Sprites.GetByAlias(value).Id;
            }
        }

        internal int ImageId { get; private set; }
        internal float Order { get; private set; }

        #endregion Private Fields

        #region Private Constructors

        private SpriteComponentBuilder(ICore core) : base(core)
        {
        }

        #endregion Private Constructors

        #region Public Methods

        public static IComponentBuilder New(ICore core)
        {
            return new SpriteComponentBuilder(core);
        }

        public override IEntityComponent Build()
        {
            return SpriteComponent.Create(AtlasId, ImageId, Order);
        }

        public override void SetProperty(object key, object value)
        {
            var propertyName = Convert.ToString(key);
            switch (propertyName)
            {
                case nameof(AtlasAlias):
                    AtlasAlias = Convert.ToString(value);
                    break;

                case nameof(AtlasId):
                    AtlasId = Convert.ToInt32(value);
                    break;

                case nameof(ImageId):
                    ImageId = Convert.ToInt32(value);
                    break;

                case nameof(Order):
                    Order = Convert.ToSingle(value);
                    break;

                default:
                    break;
            }
        }

        #endregion Public Methods
    }
}