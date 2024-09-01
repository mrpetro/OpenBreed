using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Database.Interface.Items.TileStamps;
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

        public IClip<TObject> Load(IDbAnimation dbAnimation, params object[] args)
        {
            if (clipMan.TryGetByName(dbAnimation.Id, out IClip<TObject> clip))
            {
                return clip;
            }

            var totalTime = dbAnimation.Length;

            clip = clipMan.CreateClip(dbAnimation.Id, totalTime);

            foreach (var part in dbAnimation.Tracks)
            {
                LoadTrack(clip, part);
            }

            logger.LogTrace("Animation clip '{0}' loaded.", dbAnimation.Id);

            return clip;
        }

        public IClip<TObject> Load(string dbEntryId)
        {
            if (clipMan.TryGetByName(dbEntryId, out IClip<TObject> clip))
            {
                return clip;
            }

            var entry = repositoryProvider.GetRepository<IDbAnimation>().GetById(dbEntryId);

            if (entry is null)
            {
                throw new Exception("Animation clip error: " + dbEntryId);
            }

            return Load(entry);
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