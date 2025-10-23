using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Builders;
using OpenBreed.Common.Interface;
using System.Collections.Generic;

namespace OpenBreed.Animation.Generic.Builders
{
    internal class EditableTrackBuilder<TObject, TValue> : TrackBuilderBase<TObject, TValue>, IBuilder<IEditableTrack<TObject>>
    {
        public IEditableTrack<TObject> Build()
        {
            return new EditableTrack<TObject, TValue>(this);
        }
    }

    internal class ReadOnlyTrackBuilder<TObject, TValue> : TrackBuilderBase<TObject, TValue>, IBuilder<IReadOnlyTrack<TObject>>
{
        public IReadOnlyTrack<TObject> Build()
        {
            return new ReadOnlyTrack<TObject, TValue>(this);
        }
    }

    internal abstract class TrackBuilderBase<TObject, TValue> : ITrackBuilder<TObject, TValue>
    {
        internal string Id { get; private set; }
        internal FrameInterpolation Interpolation { get; private set; }
        internal FrameUpdater<TObject, TValue> FrameUpdater { get; private set; }
        internal TValue InitialValue { get; private set; }
        internal SortedDictionary<float, TValue> Frames { get; } = new SortedDictionary<float, TValue>();


        #region Public Constructors

        public TrackBuilderBase()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddFrame(TValue value, float frameTime)
        {
            Frames.Add(frameTime, value);
        }

        public void SetId(string id)
        {
            Id = id;
        }

        internal void SetInterpolation(FrameInterpolation interpolation)
        {
            Interpolation = interpolation;

        }

        internal void SetFrameUpdater(FrameUpdater<TObject, TValue> frameUpdater)
        {
            FrameUpdater = frameUpdater;
        }

        internal void SetInitialValue(TValue initialValue)
        {
            InitialValue = initialValue;
        }


        #endregion Public Methods
    }
}