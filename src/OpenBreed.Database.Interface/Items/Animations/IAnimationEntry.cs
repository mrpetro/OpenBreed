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

    public interface IAnimationEntry : IEntry
    {
        float Length { get; set; }

        ReadOnlyCollection<IAnimationEntryTrack> Tracks { get; }
    }

    public interface IAnimationEntryTrack
    {
        EntryFrameInterpolation Interpolation { get; }

        string Controller { get; set; }
    }

    public interface IAnimationEntryTrack<TValue> : IAnimationEntryTrack
    {
        ReadOnlyCollection<IAnimationFrame<TValue>> Frames { get; }

        void ClearFrames();

        void AddFrame(TValue value, float frameTime);
    }

}