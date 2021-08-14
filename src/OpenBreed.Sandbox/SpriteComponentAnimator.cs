using OpenBreed.Animation.Interface;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Commands;

namespace OpenBreed.Sandbox
{
    internal class SpriteComponentAnimator
    {
        #region Private Fields

        private readonly ISpriteMan spriteMan;
        private readonly ICommandsMan commandsMan;

        #endregion Private Fields

        #region Public Constructors

        public SpriteComponentAnimator(IFrameUpdaterMan frameUpdaterMan, ISpriteMan spriteMan, ICommandsMan commandsMan)
        {
            this.spriteMan = spriteMan;
            this.commandsMan = commandsMan;

            frameUpdaterMan.Register("Sprite.ImageId", (FrameUpdater<int>)OnImageIdUpdate);
            frameUpdaterMan.Register("Sprite.AtlasId", (FrameUpdater<string>)OnAtlasIdUpdate);
        }

        #endregion Public Constructors

        #region Private Methods

        private void OnImageIdUpdate(Entity entity, int nextValue)
        {
            commandsMan.Post(new SpriteSetCommand(entity.Id, nextValue));
        }

        private void OnAtlasIdUpdate(Entity entity, string nextValue)
        {
            var atlas = spriteMan.GetByName(nextValue);
            commandsMan.Post(new SpriteSetAtlasCommand(entity.Id, atlas.Id));
        }

        #endregion Private Methods
    }
}