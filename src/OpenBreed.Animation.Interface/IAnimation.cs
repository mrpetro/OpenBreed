using OpenBreed.Ecsw.Entities;
using System;

namespace OpenBreed.Animation.Interface
{
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

        bool UpdateWithNextFrame(Entity entity, IAnimator animator);

        IAnimationPart<T> AddPart<T>(Action<Entity, T> frameUpdateAction, T initialValue);

        #endregion Public Methods
    }

    public interface IAnimationPart
    {
        #region Public Methods

        bool UpdateWithNextFrame(Entity entity, IAnimator animator);

        #endregion Public Methods
    }

    public interface IAnimationPart<T> : IAnimationPart
    {
        #region Public Methods

        void AddFrame(T value, float frameTime);

        #endregion Public Methods
    }
}