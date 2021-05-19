using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Door
{
    public interface IAnimationPartLoader
    {
        void Load(IAnimation animation, IAnimationEntry entry);
    }

    public class SpriteAnimationPartLoader : IAnimationPartLoader
    {
        private readonly ICommandsMan commandsMan;

        public SpriteAnimationPartLoader(ICommandsMan commandsMan)
        {
            this.commandsMan = commandsMan;
        }

        private void OnFrameUpdate(Entity entity, int nextValue)
        {
            commandsMan.Post(new SpriteSetCommand(entity.Id, nextValue));
        }

        public void Load(IAnimation animation, IAnimationEntry entry)
        {
            var animationPart = animation.AddPart<int>(OnFrameUpdate, 0);

            var currentFrameTime = 0.0f;
            foreach (var frame in entry.Frames)
            {
                currentFrameTime += frame.FrameTime;
                animationPart.AddFrame(frame.ValueIndex, currentFrameTime);
            }
        }
    }

    internal class AnimationDataLoader : IDataLoader<IAnimation>
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly IAnimationMan animationMan;
        private readonly IFrameUpdaterMan frameUpdaterMan;
        private readonly AnimationDataFactory animationDataFactory;

        #endregion Private Fields

        #region Public Constructors

        public AnimationDataLoader(IRepositoryProvider repositoryProvider,
                                   IAnimationMan animationMan,
                                   IFrameUpdaterMan frameUpdaterMan,
                                   AnimationDataFactory animationDataFactory)
        {
            this.repositoryProvider = repositoryProvider;
            this.animationMan = animationMan;
            this.frameUpdaterMan = frameUpdaterMan;
            this.animationDataFactory = animationDataFactory;
        }

        #endregion Public Constructors

        #region Public Methods

        public IAnimation Load(string entryId)
        {
            return (IAnimation)LoadObject(entryId);
        }

        public object LoadObject(string entryId)
        {
            var entry = repositoryProvider.GetRepository<IAnimationEntry>().GetById(entryId);
            if (entry == null)
                throw new Exception("Animation error: " + entryId);

            var totalTime = entry.Frames.Sum(item => item.FrameTime);

            var newAnim = animationMan.Create(entry.Id, totalTime);

            var partLoader = animationDataFactory.GetPartLoader(entry.AnimatorType);

            partLoader.Load(newAnim, entry);

            return newAnim;
        }

        #endregion Public Methods
    }
}