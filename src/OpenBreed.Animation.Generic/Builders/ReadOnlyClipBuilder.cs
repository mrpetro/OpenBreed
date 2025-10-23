using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Builders;
using OpenBreed.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Animation.Generic.Builders
{

    internal abstract class ClipBuilderBase<TObject> : IClipBuilder<TObject>
    {
        internal List<ITrackBuilder<TObject>> Tracks { get; } = new List<ITrackBuilder<TObject>>();


        #region Public Methods

        internal string Name { get; private set; }
        internal float Length { get; private set; }


        public void SetName(string name)
        {
            Name = name;
        }

        public void SetLength(float length)
        {
            Length = length;
        }

        protected abstract TrackBuilderBase<TObject, TValue> NewTrackBuilder<TValue>();

        public ITrackBuilder<TObject, TValue> AddTrack<TValue>(string id, FrameInterpolation interpolation, FrameUpdater<TObject, TValue> frameUpdater, TValue initialValue)
        {
            var trackBuilder = NewTrackBuilder<TValue>();
            trackBuilder.SetId(id);
            trackBuilder.SetInterpolation(interpolation);
            trackBuilder.SetFrameUpdater(frameUpdater);
            trackBuilder.SetInitialValue(initialValue);

            Tracks.Add(trackBuilder);

            return trackBuilder;
        }

        #endregion Public Methods
    }

    internal class EditableClipBuilder<TObject> : ClipBuilderBase<TObject>, IBuilder<IEditableClip<TObject>>
    {
        #region Public Methods

        public IEditableClip<TObject> Build()
        {
            return new EditableClip<TObject>(this);
        }

        protected override TrackBuilderBase<TObject, TValue> NewTrackBuilder<TValue>()
        {
            return new EditableTrackBuilder<TObject, TValue>();
        }

        #endregion Public Methods
    }


    internal class ReadOnlyClipBuilder<TObject> : ClipBuilderBase<TObject>, IReadOnlyClipBuilder<TObject>
    {
        #region Public Methods

        public IReadOnlyClip<TObject> Build()
        {
            return new ReadOnlyClip<TObject>(this);
        }

        protected override TrackBuilderBase<TObject, TValue> NewTrackBuilder<TValue>()
        {
            return new ReadOnlyTrackBuilder<TObject, TValue>();
        }

        #endregion Public Methods
    }
}