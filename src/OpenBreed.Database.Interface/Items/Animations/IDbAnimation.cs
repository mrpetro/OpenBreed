using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Interface.Items.TileStamps;
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

        IDbAnimationTrack<TValue> AddNewTrack<TValue>();

        bool RemoveTrack(IDbAnimationTrack track);

        #endregion Public Properties
    }
}