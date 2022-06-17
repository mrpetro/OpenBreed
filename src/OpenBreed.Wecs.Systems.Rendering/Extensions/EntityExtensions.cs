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

        public static void SetViewportSize(this Entity entity, float width, float height)
        {
            var vpc = entity.Get<ViewportComponent>();

            if (vpc.Size.X == width && vpc.Size.Y == height)
                return;

            vpc.Size = new Vector2(width, height);

            entity.RaiseEvent(new ViewportResizedEventArgs(entity.Id, vpc.Size.X, vpc.Size.Y));
        }

        public static void SetViewportCamera(this Entity entity, int cameraEntityId)
        {
            var vpc = entity.Get<ViewportComponent>();

            vpc.CameraEntityId = cameraEntityId;
        }

        public static void PutStamp(this Entity entity, int stampId, int layerNo)
        {
            var position = entity.Get<PositionComponent>().Value;
            PutStampAtPosition(entity, stampId, layerNo, position);
        }

        public static void PutStampAtPosition(this Entity entity, int stampId, int layerNo, Vector2 position)
        {
            entity.Set(new StampPutterComponent(stampId, layerNo, position));
        }

        public static void PutTile(this Entity entity, int atlasId, int tileId, int layerNo, Vector2 position)
        {
            var tp = entity.Get<TilePutterComponent>();
            tp.Items.Add(new TileData(atlasId, tileId, layerNo, position));
        }

        #endregion Public Methods
    }
}