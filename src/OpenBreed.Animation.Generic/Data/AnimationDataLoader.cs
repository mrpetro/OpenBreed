using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Animations;
using System;

namespace OpenBreed.Animation.Generic.Data
{
    internal class AnimationDataLoader : IDataLoader<IClip>
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly IClipMan clipMan;
        private readonly IFrameUpdaterMan frameUpdaterMan;

        #endregion Private Fields

        #region Public Constructors

        public AnimationDataLoader(IRepositoryProvider repositoryProvider,
                                   IClipMan clipMan,
                                   IFrameUpdaterMan frameUpdaterMan)
        {
            this.repositoryProvider = repositoryProvider;
            this.clipMan = clipMan;
            this.frameUpdaterMan = frameUpdaterMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public object LoadObject(string entryId) => Load(entryId);

        public IClip Load(string entryId, params object[] args)
        {
            var entry = repositoryProvider.GetRepository<IAnimationEntry>().GetById(entryId);
            if (entry == null)
                throw new Exception("Animation error: " + entryId);

            var totalTime = entry.Length;

            var newClip = clipMan.CreateClip(entry.Id, totalTime);

            foreach (var part in entry.Tracks)
                LoadTrack(newClip, part);

            return newClip;
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

        private void LoadTrack<TValue>(IClip clip, IAnimationEntryTrack<TValue> entryTrack)
        {
            var updater = frameUpdaterMan.GetByName<TValue>(entryTrack.Controller);
            var interpolation = GetFrameInterpolation(entryTrack.Interpolation);
            var track = clip.AddTrack<TValue>(interpolation, updater, default(TValue));

            foreach (var frame in entryTrack.Frames)
                track.AddFrame(frame.Value, frame.Time);
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