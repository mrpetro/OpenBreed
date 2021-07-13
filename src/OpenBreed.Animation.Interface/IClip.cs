using OpenBreed.Wecs.Entities;
using System;

namespace OpenBreed.Animation.Interface
{
    public delegate void FrameUpdater<TValue>(Entity entity, TValue value);

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
    public interface IClip
    {
        #region Public Properties

        int Id { get; }
        string Name { get; }
        float Length { get; }

        #endregion Public Properties

        #region Public Methods

        bool UpdateWithNextFrame(Entity entity, float time);
         
        ITrack<TValue> AddTrack<TValue>(FrameInterpolation interpolation, FrameUpdater<TValue> frameUpdater, TValue initialValue);

        #endregion Public Methods
    }

    public interface ITrack
    {
        #region Public Methods

        bool UpdateWithNextFrame(Entity entity, float time);

        #endregion Public Methods
    }

    public interface ITrack<TValue> : ITrack
    {
        #region Public Methods

        void AddFrame(TValue value, float frameTime);

        #endregion Public Methods
    }
}