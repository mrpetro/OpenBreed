using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Animations;
using System;

namespace OpenBreed.Animation.Generic.Data
{
    internal class AnimationClipDataLoader<TObject> : IAnimationClipDataLoader<TObject>
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly IClipMan<TObject> clipMan;
        private readonly IFrameUpdaterMan<TObject> frameUpdaterMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public AnimationClipDataLoader(IRepositoryProvider repositoryProvider,
                                   IClipMan<TObject> clipMan,
                                   IFrameUpdaterMan<TObject> frameUpdaterMan,
                                   ILogger logger)
        {
            this.repositoryProvider = repositoryProvider;
            this.clipMan = clipMan;
            this.frameUpdaterMan = frameUpdaterMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public object LoadObject(string entryId) => Load(entryId);

        public IClip<TObject> Load(string clipName, params object[] args)
        {
            var clip = clipMan.GetByName(clipName);

            if (clip != null)
                return clip;

            var entry = repositoryProvider.GetRepository<IDbAnimation>().GetById(clipName);
            if (entry == null)
                throw new Exception("Animation clip error: " + clipName);

            var totalTime = entry.Length;

            clip = clipMan.CreateClip(entry.Id, totalTime);

            foreach (var part in entry.Tracks)
                LoadTrack(clip, part);

            logger.Verbose($"Animation clip '{clipName}' loaded.");

            return clip;
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

        private void LoadTrack<TValue>(IClip<TObject> clip, IDbAnimationTrack<TValue> entryTrack)
        {
            var updater = frameUpdaterMan.GetByName<TValue>(entryTrack.Controller);
            var interpolation = GetFrameInterpolation(entryTrack.Interpolation);
            var track = clip.AddTrack<TValue>(interpolation, updater, default(TValue));

            foreach (var frame in entryTrack.Frames)
                track.AddFrame(frame.Value, frame.Time);
        }

        private void LoadTrack(IClip<TObject> animation, IDbAnimationTrack entryTrack)
        {
            if (entryTrack is IDbAnimationTrack<int>)
                LoadTrack<int>(animation, (IDbAnimationTrack<int>)entryTrack);
            else if (entryTrack is IDbAnimationTrack<float>)
                LoadTrack<float>(animation, (IDbAnimationTrack<float>)entryTrack);
            else if (entryTrack is IDbAnimationTrack<string>)
                LoadTrack<string>(animation, (IDbAnimationTrack<string>)entryTrack);
        }

        #endregion Private Methods
    }
}