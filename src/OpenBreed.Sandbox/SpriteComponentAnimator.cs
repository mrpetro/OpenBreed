using OpenBreed.Animation.Interface;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Extensions;

namespace OpenBreed.Sandbox
{
    internal class SpriteComponentAnimator
    {
        #region Private Fields

        private readonly ISpriteMan spriteMan;

        #endregion Private Fields

        #region Public Constructors

        public SpriteComponentAnimator(IFrameUpdaterMan<Entity> frameUpdaterMan, ISpriteMan spriteMan)
        {
            this.spriteMan = spriteMan;

            frameUpdaterMan.Register("Sprite.ImageId", (FrameUpdater<Entity, int>)OnImageIdUpdate);
            frameUpdaterMan.Register("Sprite.AtlasId", (FrameUpdater<Entity, string>)OnAtlasIdUpdate);
        }

        #endregion Public Constructors

        #region Private Methods

        private void OnImageIdUpdate(Entity entity, int nextValue)
        {
            entity.SetSpriteImageId(nextValue);
        }

        private void OnAtlasIdUpdate(Entity entity, string nextValue)
        {
            var atlas = spriteMan.GetByName(nextValue);
            entity.SetSpriteAtlas(atlas.Id);
        }

        #endregion Private Methods
    }
}