using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using System;

namespace OpenBreed.Core.Modules.Physics.Builders
{
    public class SpriteComponentBuilder : BaseComponentBuilder
    {
        #region Private Fields

        private string atlasAlias;
        private int imageId;
        private float order;

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
            var atlas = Core.Rendering.Sprites.GetByAlias(atlasAlias);
            return SpriteComponent.Create(atlas.Id, imageId, order);
        }

        public override void SetProperty(object key, object value)
        {
            var propertyName = Convert.ToString(key);
            switch (propertyName)
            {
                case "AtlasAlias":
                    atlasAlias = Convert.ToString(value);
                    break;

                case "ImageId":
                    imageId = Convert.ToInt32(value);
                    break;

                case "Order":
                    order = Convert.ToSingle(value);
                    break;

                default:
                    break;
            }
        }

        #endregion Public Methods
    }
}