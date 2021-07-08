using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Door
{
    public interface IAnimationPartLoader
    {
        void Load(IAnimation animation, IAnimationEntryTrack animationEntryPart);
    }

    public class SpriteAnimationPartLoader : IAnimationPartLoader
    {    
        private readonly IFrameUpdaterMan frameUpdaterMan;

        public SpriteAnimationPartLoader(IFrameUpdaterMan frameUpdaterMan)
        {
            this.frameUpdaterMan = frameUpdaterMan;
        }

        private readonly Dictionary<Type, Action> adders = new Dictionary<Type, Action>();

        private void AddFrameKey()
        {

        }

        private FrameInterpolation GetFrameInterpolation(EntryFrameInterpolation interpolation)
        {
            switch (interpolation)
            {
                case EntryFrameInterpolation.None:
                    return FrameInterpolation.None;
                case EntryFrameInterpolation.Linear:
                    return FrameInterpolation.Linear;
                default:
                    return FrameInterpolation.None;
            }
        }

        public void Load(IAnimation animation, IAnimationEntryTrack entryTrack)
        {
            var updater = frameUpdaterMan.GetByName<int>(entryTrack.AnimatorType);
            var interpolation = GetFrameInterpolation(entryTrack.Interpolation);
            var animationTrack = animation.AddTrack(interpolation, updater, 0);

            foreach (var frame in entryTrack.Frames)
            {
                animationTrack.AddFrame(frame.ValueIndex, frame.Time);
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

            var totalTime = entry.Length;

            var newAnim = animationMan.Create(entry.Id, totalTime);

            foreach (var part in entry.Tracks)
            {
                var partLoader = animationDataFactory.GetPartLoader(part.AnimatorType);

                partLoader.Load(newAnim, part);
            }

            return newAnim;
        }

        #endregion Public Methods
    }
}