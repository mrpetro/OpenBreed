using Microsoft.Extensions.Hosting;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Extensions;

namespace OpenBreed.Common.Game.Wecs
{
    internal class SpriteComponentAnimator
    {
        #region Private Fields

        private readonly ISpriteMan spriteMan;
        private readonly ISpriteAtlasDataLoader spriteAtlasDataLoader;

        #endregion Private Fields

        #region Public Constructors

        public SpriteComponentAnimator(IFrameUpdaterMan<IEntity> frameUpdaterMan, ISpriteMan spriteMan, IDataLoaderFactory dataLoaderFactory)
        {
            this.spriteMan = spriteMan;

            spriteAtlasDataLoader = dataLoaderFactory.GetLoader<ISpriteAtlasDataLoader>();

            frameUpdaterMan.Register("Sprite.ImageId", (FrameUpdater<IEntity, int>)OnImageIdUpdate);
            frameUpdaterMan.Register("Sprite.AtlasId", (FrameUpdater<IEntity, string>)OnAtlasIdUpdate, OnAtlasIdLoad);
            frameUpdaterMan.Register("Sprite.OriginX", (FrameUpdater<IEntity, float>)OnOriginXUpdate);
            frameUpdaterMan.Register("Sprite.OriginY", (FrameUpdater<IEntity, float>)OnOriginYUpdate);
        }

        #endregion Public Constructors

        #region Private Methods

        private void OnImageIdUpdate(IEntity entity, int nextValue)
        {
            entity.SetSpriteImageId(nextValue);
        }

        private void OnOriginXUpdate(IEntity entity, float nextValue)
        {
            entity.SetSpriteOriginX(nextValue);
        }

        private void OnOriginYUpdate(IEntity entity, float nextValue)
        {
            entity.SetSpriteOriginY(nextValue);
        }

        private void OnAtlasIdLoad(string nextValue)
        {
            var atlas = spriteMan.GetByName(nextValue);

            if (atlas is null)
            {
                atlas = spriteAtlasDataLoader.Load(nextValue);
            }
        }

        private void OnAtlasIdUpdate(IEntity entity, string nextValue)
        {
            var atlas = spriteMan.GetByName(nextValue);

            entity.SetSpriteAtlas(atlas.Id);
        }

        #endregion Private Methods
    }
}