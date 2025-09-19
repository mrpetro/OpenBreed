using OpenBreed.Common.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Animation.Interface.Builders
{
    public interface IReadOnlyClipBuilder<TObject> : IBuilder<IReadOnlyClip<TObject>>
    {
        #region Public Methods

        /// <summary>
        /// Set name of this clip.
        /// </summary>
        /// <param name="length">Name of the clip.</param>
        void SetName(string name);

        /// <summary>
        /// Set clip length(in seconds).
        /// </summary>
        /// <param name="length">Length (in seconds).</param>
        void SetLength(float length);

        /// <summary>
        /// Add new track.
        /// </summary>
        /// <returns>New track builder.</returns>
        IReadOnlyTrackBuilder<TObject, TValue> AddTrack<TValue>(FrameInterpolation interpolation, FrameUpdater<TObject, TValue> frameUpdater, TValue initialValue);

        #endregion Public Methods
    }

    public interface IReadOnlyTrackBuilder<TObject>
    {
        ITrack<TObject> Build();
    }

    public interface IReadOnlyTrackBuilder<TObject, TValue> : IReadOnlyTrackBuilder<TObject>
    {
        #region Public Methods

        void AddFrame(TValue value, float frameTime);


        #endregion Public Methods
    }
}