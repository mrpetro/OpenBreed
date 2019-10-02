namespace OpenBreed.Core.Modules.Animation.Helpers
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

        bool TryGetNextFrame(float time, object currentFrame, out object nextFrame, FrameTransition transition = FrameTransition.None);

        #endregion Public Properties
    }

    public interface IAnimation<T> : IAnimation
    {
        #region Public Methods

        T GetFrame(float time, FrameTransition transition = FrameTransition.None);

        void AddFrame(T value, float frameTime);

        #endregion Public Methods
    }
}