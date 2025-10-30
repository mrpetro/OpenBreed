using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic.Builders;
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
using System.Xml.Linq;

namespace OpenBreed.Animation.Generic.Data
{
    internal class EditableClipDataLoader<TObject> : IEditableClipDataLoader<TObject>
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly IClipMan<TObject> clipMan;
        private readonly IFrameUpdaterMan<TObject> frameUpdaterMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public EditableClipDataLoader(IRepositoryProvider repositoryProvider,
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

        public IEditableClip<TObject> Load(IDbAnimation dbAnimation)
        {
            if (clipMan.TryGetByName(dbAnimation.Id, out IReadOnlyClip<TObject> clip))
            {
                if (clip is IEditableClip<TObject> existingClip)
                {
                    return existingClip;
                }
            }

            var totalTime = dbAnimation.Length;

            var clipBuilder = new EditableClipBuilder<TObject>();
            clipBuilder.SetName(dbAnimation.Id);
            clipBuilder.SetLength(dbAnimation.Length);

            foreach (var part in dbAnimation.Tracks)
            {
                LoadTrack(clipBuilder, part);
            }

            logger.LogTrace("Animation clip '{0}' loaded.", dbAnimation.Id);

            var editableClip = clipBuilder.Build();

            clipMan.Register(editableClip);

            return editableClip;
        }

        public IEditableClip<TObject> Load(string dbEntryId)
        {
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

        private void LoadTrack<TValue>(IClipBuilder<TObject> clipBuilder, IDbAnimationTrack<TValue> entryTrack)
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

        private void LoadTrack(IClipBuilder<TObject> clipBuilder, IDbAnimationTrack entryTrack)
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