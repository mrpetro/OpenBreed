using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Animations;
using System;

namespace OpenBreed.Sandbox.Entities.Door
{
    public interface IAnimationTrackLoader
    {
        #region Public Methods

        void Load(IClip animation, IAnimationEntryTrack animationEntryPart);

        #endregion Public Methods
    }

    public class IntegerAnimationTrackLoader : IAnimationTrackLoader
    {
        #region Private Fields

        private readonly IFrameUpdaterMan frameUpdaterMan;

        #endregion Private Fields

        #region Public Constructors

        public IntegerAnimationTrackLoader(IFrameUpdaterMan frameUpdaterMan)
        {
            this.frameUpdaterMan = frameUpdaterMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Load(IClip animation, IAnimationEntryTrack entryTrack)
        {
            var updater = frameUpdaterMan.GetByName<int>(entryTrack.Controller);
            var interpolation = GetFrameInterpolation(entryTrack.Interpolation);
            var animationTrack = animation.AddTrack(interpolation, updater, 0);

            if (entryTrack is IAnimationEntryTrack<int>)
            {
                var entryIntegerTrack = (IAnimationEntryTrack<int>)entryTrack;

                foreach (var frame in entryIntegerTrack.Frames)
                    animationTrack.AddFrame(frame.Value, frame.Time);
            }
        }

        #endregion Public Methods

        #region Private Methods

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

        #endregion Private Methods
    }

    internal class AnimationDataLoader : IDataLoader<IClip>
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly IClipMan animationMan;
        private readonly IFrameUpdaterMan frameUpdaterMan;
        private readonly AnimationDataFactory animationDataFactory;

        #endregion Private Fields

        #region Public Constructors

        public AnimationDataLoader(IRepositoryProvider repositoryProvider,
                                   IClipMan animationMan,
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

        public IClip Load(string entryId)
        {
            return (IClip)LoadObject(entryId);
        }

        public object LoadObject(string entryId)
        {
            var entry = repositoryProvider.GetRepository<IAnimationEntry>().GetById(entryId);
            if (entry == null)
                throw new Exception("Animation error: " + entryId);

            var totalTime = entry.Length;

            var newAnim = animationMan.CreateClip(entry.Id, totalTime);

            foreach (var part in entry.Tracks)
                LoadTrack(newAnim, part);

            return newAnim;
        }

        #endregion Public Methods

        #region Private Methods

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

        private void LoadTrack<TValue>(IClip animation, IAnimationEntryTrack<TValue> entryTrack)
        {
            var updater = frameUpdaterMan.GetByName<TValue>(entryTrack.Controller);
            var interpolation = GetFrameInterpolation(entryTrack.Interpolation);
            var animationTrack = animation.AddTrack<TValue>(interpolation, updater, default(TValue));

            foreach (var frame in entryTrack.Frames)
                animationTrack.AddFrame(frame.Value, frame.Time);
        }

        private void LoadTrack(IClip animation, IAnimationEntryTrack entryTrack)
        {
            if (entryTrack is IAnimationEntryTrack<int>)
                LoadTrack<int>(animation, (IAnimationEntryTrack<int>)entryTrack);
            else if (entryTrack is IAnimationEntryTrack<float>)
                LoadTrack<float>(animation, (IAnimationEntryTrack<float>)entryTrack);
        }

        #endregion Private Methods
    }
}