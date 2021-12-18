using System;

namespace OpenBreed.Animation.Interface
{
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
    /// Animation clip which represents collection of tracks
    /// </summary>
    public interface IClip<TObject>
    {
        #region Public Properties

        int Id { get; }
        string Name { get; }
        float Length { get; }

        #endregion Public Properties

        #region Public Methods

        bool UpdateWithNextFrame(TObject obj, float time);
         
        ITrack<TObject, TValue> AddTrack<TValue>(FrameInterpolation interpolation, FrameUpdater<TObject, TValue> frameUpdater, TValue initialValue);

        #endregion Public Methods
    }

    public interface ITrack<TObject>
    {
        #region Public Methods

        bool UpdateWithNextFrame(TObject obj, float time);

        #endregion Public Methods
    }

    public interface ITrack<TObject, TValue> : ITrack<TObject>
    {
        #region Public Methods

        void AddFrame(TValue value, float frameTime);

        #endregion Public Methods
    }
}