using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Systems.Rendering.Extensions
{
    public static class EntityExtensions
    {
        #region Public Methods

        public static void SetBrightness(this IEntity entity, float brightness)
        {
            var cameraComponent = entity.Get<CameraComponent>();
            cameraComponent.Brightness = brightness;
        }

        public static int GetPaletteId(this IEntity entity)
        {
            return entity.Get<PaletteComponent>().PaletteId;
        }

        public static void SetPaletteId(this IEntity entity, int paletteId)
        {
            entity.Get<PaletteComponent>().PaletteId = paletteId;
        }

        public static void SetPictureColor(this IEntity entity, float r, float g, float b, float a)
        {
            var cmp = entity.Get<PictureComponent>();
            cmp.Color = new Color4(r, g, b, a);
        }

        public static void SetTextColor(this IEntity entity, int textPartId, float r, float g, float b, float a)
        {
            var textCmp = entity.Get<TextComponent>();

            if (textPartId < 0 || textPartId >= textCmp.Parts.Count)
                return;

            textCmp.Parts[textPartId].Color = new Color4(r,g,b,a);
        }

        public static void SetText(this IEntity entity, int textPartId, string text)
        {
            var textCmp = entity.Get<TextComponent>();

            if (textPartId < 0 || textPartId >= textCmp.Parts.Count)
                return;

            textCmp.Parts[textPartId].Text = text;
        }

        public static void SetSpriteOn(this IEntity entity)
        {
            var sprite = entity.Get<SpriteComponent>();
            sprite.Hidden = false;
        }

        public static void SetSpriteOff(this IEntity entity)
        {
            var sprite = entity.Get<SpriteComponent>();
            sprite.Hidden = true;
        }

        public static void SetSpriteImageId(this IEntity entity, int imageId)
        {
            var sprite = entity.Get<SpriteComponent>();
            sprite.ImageId = imageId;
        }

        public static void SetSpriteOriginX(this IEntity entity, float value)
        {
            var sprite = entity.Get<SpriteComponent>();
            sprite.Origin = new Vector2(value, sprite.Origin.Y);
        }

        public static void SetSpriteOriginY(this IEntity entity, float value)
        {
            var sprite = entity.Get<SpriteComponent>();
            sprite.Origin = new Vector2(sprite.Origin.X, value);
        }

        public static void SetSpriteOrigin(this IEntity entity, Vector2 value)
        {
            var sprite = entity.Get<SpriteComponent>();
            sprite.Origin = value;
        }

        public static void SetSpriteScale(this IEntity entity, Vector2 value)
        {
            var sprite = entity.Get<SpriteComponent>();
            sprite.Scale = value;
        }

        public static void SetSpriteAtlas(this IEntity entity, int atlasId)
        {
            var sprite = entity.Get<SpriteComponent>();
            sprite.AtlasId = atlasId;
        }

        public static void SetViewportSize(this IEntity entity, IEventsMan eventsMan, float width, float height)
        {
            var vpc = entity.Get<ViewportComponent>();

            if (vpc.Size.X == width && vpc.Size.Y == height)
                return;

            vpc.Size = new Vector2(width, height);

            eventsMan.Raise(new ViewportResizedEvent(entity.Id, vpc.Size.X, vpc.Size.Y));
        }

        public static void SetViewportCamera(this IEntity entity, int cameraEntityId)
        {
            var vpc = entity.Get<ViewportComponent>();

            vpc.CameraEntityId = cameraEntityId;
        }

        public static void PutStampAtEntityPosition(this IEntity entity, IEntity otherEntity, int stampId, int layerNo)
        {
            var position = otherEntity.Get<PositionComponent>().Value;
            PutStampAtPosition(entity, stampId, layerNo, position);
        }

        public static void PutStampAtPosition(this IEntity entity, int stampId, int layerNo, Vector2 position)
        {
            var items = entity.Get<StampPutterComponent>().Items;
            items.Add(new StampData(stampId, layerNo, position));
        }

        public static void PutTile(this IEntity entity, int atlasId, int tileId, int layerNo, Vector2 position)
        {
            var tp = entity.Get<TilePutterComponent>();
            tp.Items.Add(new TileData(atlasId, tileId, layerNo, position));
        }

        #endregion Public Methods
    }
}