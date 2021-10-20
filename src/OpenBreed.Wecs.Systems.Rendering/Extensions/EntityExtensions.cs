using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;

namespace OpenBreed.Wecs.Systems.Rendering.Extensions
{
    public static class EntityExtensions
    {
        #region Public Methods

        public static void SetText(this Entity entity, int textPartId, string text)
        {
            var textCmp = entity.Get<TextComponent>();

            if (textPartId < 0 || textPartId >= textCmp.Parts.Count)
                return;

            textCmp.Parts[textPartId].Text = text;
        }

        public static void SetSpriteOn(this Entity entity)
        {
            var sprite = entity.Get<SpriteComponent>();
            sprite.Hidden = false;
        }

        public static void SetSpriteOff(this Entity entity)
        {
            var sprite = entity.Get<SpriteComponent>();
            sprite.Hidden = true;
        }

        public static void SetSpriteImageId(this Entity entity, int imageId)
        {
            var sprite = entity.Get<SpriteComponent>();
            sprite.ImageId = imageId;
        }

        public static void SetSpriteAtlas(this Entity entity, int atlasId)
        {
            var sprite = entity.Get<SpriteComponent>();
            sprite.AtlasId = atlasId;
        }

        #endregion Public Methods
    }
}