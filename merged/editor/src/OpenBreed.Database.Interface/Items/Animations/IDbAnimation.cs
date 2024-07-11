using OpenBreed.Database.Interface.Items.Sprites;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenBreed.Database.Interface.Items.Animations
{
    /// <summary>
    /// Transition methods between frames
    /// </summary>
    public enum EntryFrameInterpolation
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

    public interface IDbAnimation : IDbEntry
    {
        #region Public Properties

        float Length { get; set; }

        ReadOnlyCollection<IDbAnimationTrack> Tracks { get; }

        #endregion Public Properties
    }

    public interface IDbAnimationTrack
    {
        #region Public Properties

        EntryFrameInterpolation Interpolation { get; }

        string Controller { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Copy this object
        /// </summary>
        /// <returns>Copy of this object</returns>
        IDbAnimationTrack Copy();

        #endregion Public Methods
    }

    public interface IDbAnimationTrack<TValue> : IDbAnimationTrack
    {
        #region Public Properties

        ReadOnlyCollection<IDbAnimationFrame<TValue>> Frames { get; }

        #endregion Public Properties

        #region Public Methods

        void ClearFrames();

        void AddFrame(TValue value, float frameTime);

        #endregion Public Methods
    }
}