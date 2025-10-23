using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Builders;
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
    internal class ReadOnlyClipDataLoader<TObject> : IReadOnlyClipDataLoader<TObject>
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly IClipMan<TObject> clipMan;
        private readonly IFrameUpdaterMan<TObject> frameUpdaterMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public ReadOnlyClipDataLoader(IRepositoryProvider repositoryProvider,
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

        public IReadOnlyClip<TObject> Load(IDbAnimation dbAnimation, bool reload = false)
        {
            if (clipMan.TryGetByName(dbAnimation.Id, out IReadOnlyClip<TObject> clip))
            {
                if (!reload)
                {
                    return clip;
                }
            }

            var totalTime = dbAnimation.Length;

            var clipBuilder = clipMan.NewClip(dbAnimation.Id, totalTime);

            foreach (var part in dbAnimation.Tracks)
            {
                LoadTrack(clipBuilder, part);
            }

            logger.LogTrace("Animation clip '{0}' loaded.", dbAnimation.Id);

            clip = clipBuilder.Build();

            clipMan.Register(clip);

            return clip;
        }

        public IReadOnlyClip<TObject> Load(string dbEntryId)
        {
            if (clipMan.TryGetByName(dbEntryId, out IReadOnlyClip<TObject> clip))
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

        private void LoadTrack<TValue>(IReadOnlyClipBuilder<TObject> clipBuilder, IDbAnimationTrack<TValue> entryTrack)
        {
            var updater = frameUpdaterMan.GetByName<TValue>(entryTrack.Controller);
            var loader = frameUpdaterMan.GetLoaderByName<TValue>(entryTrack.Controller);
            var interpolation = GetFrameInterpolation(entryTrack.Interpolation);
            var trackBuilder = clipBuilder.AddTrack<TValue>(entryTrack.Controller, interpolation, updater, default(TValue));

            foreach (var frame in entryTrack.Frames)
            {
                loader?.Invoke(frame.Value);
                trackBuilder.AddFrame(frame.Value, frame.Time);
            }
        }

        private void LoadTrack(IReadOnlyClipBuilder<TObject> clipBuilder, IDbAnimationTrack entryTrack)
        {
            if (entryTrack is IDbAnimationTrack<int>)
                LoadTrack<int>(clipBuilder, (IDbAnimationTrack<int>)entryTrack);
            else if (entryTrack is IDbAnimationTrack<float>)
                LoadTrack<float>(clipBuilder, (IDbAnimationTrack<float>)entryTrack);
            else if (entryTrack is IDbAnimationTrack<string>)
                LoadTrack<string>(clipBuilder, (IDbAnimationTrack<string>)entryTrack);
        }

        #endregion Private Methods
    }
}