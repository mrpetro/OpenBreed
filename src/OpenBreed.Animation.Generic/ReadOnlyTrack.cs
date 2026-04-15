using OpenBreed.Animation.Generic.Builders;
using OpenBreed.Animation.Abstractions;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace OpenBreed.Animation.Generic
{

    internal class ReadOnlyTrack<TObject, TValue> : TrackBase<TObject, TValue>, IReadOnlyTrack<TObject, TValue>
    {
        #region Protected Fields

        protected readonly SortedList<float, TValue> frames;

        #endregion Protected Fields

        #region Private Fields

        public override IReadOnlyDictionary<float, TValue> Frames => frames;

        #endregion Private Fields

        #region Internal Constructors

        internal ReadOnlyTrack(TrackBuilderBase<TObject, TValue> builder) : base(builder)
        {
            this.frames = new SortedList<float, TValue>(builder.Frames);
        }

        #endregion Internal Constructors
    }
}