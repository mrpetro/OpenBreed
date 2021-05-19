using OpenBreed.Wecs.Entities;
using System;

namespace OpenBreed.Animation.Interface
{
    public delegate void FrameUpdater<TValue>(Entity entity, TValue value);

    /// <summary>
    /// Transition methods between frames
    /// </summary>
    public enum FrameTransition
    {
        /// <summary>
        /// Value of next frame is being set after specific time has passed. It's not smooth.
        /// </summary>
        None,

        /// <summary>
        /// Value of frame is being set based on linear interpolation between current and next frame and passed time.
        /// </summary>
        LinearInterpolation
    }

    public interface IAnimation
    {
        #region Public Properties

        int Id { get; }
        string Name { get; }
        float Length { get; }

        #endregion Public Properties

        #region Public Methods

        bool UpdateWithNextFrame(Entity entity, Animator animator);

        IAnimationPart<TValue> AddPart<TValue>(FrameUpdater<TValue> frameUpdater, TValue initialValue);

        #endregion Public Methods
    }

    public interface IAnimationPart
    {
        #region Public Methods

        bool UpdateWithNextFrame(Entity entity, Animator animator);

        #endregion Public Methods
    }

    public interface IAnimationPart<TValue> : IAnimationPart
    {
        #region Public Methods

        void AddFrame(TValue value, float frameTime);

        #endregion Public Methods
    }
}