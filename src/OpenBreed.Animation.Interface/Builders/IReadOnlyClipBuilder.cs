using OpenBreed.Common.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Animation.Interface.Builders
{
    public interface IClipBuilder<TObject>
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
        ITrackBuilder<TObject, TValue> AddTrack<TValue>(string id, FrameInterpolation interpolation, FrameUpdater<TObject, TValue> frameUpdater, TValue initialValue);

        #endregion Public Methods
    }

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
        ITrackBuilder<TObject, TValue> AddTrack<TValue>(string id, FrameInterpolation interpolation, FrameUpdater<TObject, TValue> frameUpdater, TValue initialValue);

        #endregion Public Methods
    }

    public interface ITrackBuilder<TObject>
    {
    }

    public interface IReadOnlyTrackBuilder<TObject>
    {
        IReadOnlyTrack<TObject> Build();
    }

    public interface ITrackBuilder<TObject, TValue> : ITrackBuilder<TObject>
    {
        #region Public Methods

        public void SetId(string id);

        void AddFrame(TValue value, float frameTime);

        #endregion Public Methods
    }

    public interface IReadOnlyTrackBuilder<TObject, TValue> : IReadOnlyTrackBuilder<TObject>
    {
        #region Public Methods

        public void SetId(string id);

        void AddFrame(TValue value, float frameTime);

        #endregion Public Methods
    }
}