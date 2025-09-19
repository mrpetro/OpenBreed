using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Animation.Generic.Builders
{
    internal class ReadOnlyTrackBuilder<TObject, TValue> : IReadOnlyTrackBuilder<TObject, TValue>
    {
        internal FrameInterpolation Interpolation { get; private set; }
        internal FrameUpdater<TObject, TValue> FrameUpdater { get; private set; }
        internal TValue InitialValue { get; private set; }
        internal SortedDictionary<float, TValue> Frames { get; } = new SortedDictionary<float, TValue>();
        public ReadOnlyTrackBuilder()
        {

        }

        public void AddFrame(TValue value, float frameTime)
        {
            Frames.Add(frameTime, value);
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

        public ITrack<TObject> Build()
        {
            return new Track<TObject, TValue>(this);
        }
    }

    internal class ReadOnlyClipBuilder<TObject> : IReadOnlyClipBuilder<TObject>
    {
        internal string Name { get; private set; }
        internal float Length { get; private set; }
        internal List<IReadOnlyTrackBuilder<TObject>> Tracks { get; } = new List<IReadOnlyTrackBuilder<TObject>>();

        public IReadOnlyTrackBuilder<TObject, TValue> AddTrack<TValue>(FrameInterpolation interpolation, FrameUpdater<TObject, TValue> frameUpdater, TValue initialValue)
        {
            var trackBuilder = new ReadOnlyTrackBuilder<TObject, TValue>();
            trackBuilder.SetInterpolation(interpolation);
            trackBuilder.SetFrameUpdater(frameUpdater);
            trackBuilder.SetInitialValue(initialValue);

            Tracks.Add(trackBuilder);

            return trackBuilder;
        }

        public IReadOnlyClip<TObject> Build()
        {
            return new ReadOnlyClip<TObject>(this);
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetLength(float length)
        {
            Length = length;
        }
    }
}
