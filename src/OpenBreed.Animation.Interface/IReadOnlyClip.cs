using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Animation.Interface
{
    public delegate void FrameLoader<TValue>(TValue value);

    public delegate void FrameUpdater<TObject, TValue>(TObject obj, TValue value);

    /// <summary>
    /// Transition methods between frames
    /// </summary>
    public enum FrameInterpolation
    {
        /// <summary>
        /// Value of next frame is being set after specific time has passed. It's not smooth.
        /// </summary>
        None,

        /// <summary>
        /// Value of frame is being set based on linear interpolation between current and next frame and passed time.
        /// </summary>
        Linear
    }

    /// <summary>
    /// Editable animation clip which represents collection of tracks.
    /// </summary>
    public interface IEditableClip<TObject> : IReadOnlyClip<TObject>
    {
        #region Public Properties

        new float Length { get; set; }

        #endregion Public Properties

        #region Public Methods

        IEditableTrack<TObject, TValue> AddTrack<TValue>(string id, FrameInterpolation interpolation, FrameUpdater<TObject, TValue> frameUpdater, TValue initialValue);

        IEditableTrack<TObject> GetTrack(string id);

        bool RemoveTrack(string id);

        #endregion Public Methods
    }

    /// <summary>
    /// Readonly Animation clip which represents collection of tracks
    /// </summary>
    public interface IReadOnlyClip<TObject>
    {
        #region Public Properties

        string Name { get; }
        float Length { get; }
        IReadOnlyCollection<IReadOnlyTrack<TObject>> Tracks { get; }

        #endregion Public Properties

        #region Public Methods

        bool UpdateWithNextFrame(TObject obj, float time);

        #endregion Public Methods
    }

    public interface IEditableTrack<TObject> : IReadOnlyTrack<TObject>
    {
        float Rebuild();
        void RemoveFrames(IEnumerable<ITrackKeyFrame> keyFrame);
    }

    public interface ITrackKeyFrame
    {
        float Key { get; set; }

        object GetValue();
    }

    public interface ITrackKeyFrame<TValue> : ITrackKeyFrame
    {
        TValue Value { get; set; }
    }

    public interface IEditableTrack<TObject, TValue> : IReadOnlyTrack<TObject, TValue>, IEditableTrack<TObject>
    {
        #region Public Methods

        bool TryGetKeyFrame(float key, out ITrackKeyFrame<TValue> keyFrame);

        ITrackKeyFrame<TValue> GetKeyFrame(float key);

        bool TryAddKeyFrame(float frameTime, TValue value);

        #endregion Public Methods
    }

    public interface IReadOnlyTrack<TObject>
    {
        #region Public Properties

        string Id { get; }

        #endregion Public Properties

        #region Public Methods

        bool UpdateWithNextFrame(TObject obj, float time);

        bool TryFindInRange(float minTime, float maxTime, out IEnumerable<float> keyFrames);

        #endregion Public Methods
    }

    public interface IReadOnlyTrack<TObject, TValue> : IReadOnlyTrack<TObject>
    {
        #region Public Properties

        IReadOnlyDictionary<float, TValue> Frames { get; }

        #endregion Public Properties
    }
}