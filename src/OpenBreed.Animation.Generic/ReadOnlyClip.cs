using OpenBreed.Animation.Generic.Builders;
using OpenBreed.Animation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Animation.Generic
{
    internal class ReadOnlyClip<TObject> : IReadOnlyClip<TObject>
    {
        #region Private Fields

        private readonly List<ITrack<TObject>> tracks;

        #endregion Private Fields

        #region Internal Constructors

        internal ReadOnlyClip(ReadOnlyClipBuilder<TObject> builder)
        {
            Name = builder.Name;
            Length = builder.Length;
            tracks = builder.Tracks.Select(b => b.Build()).ToList();
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Name { get; }
        public float Length { get; }

        #endregion Public Properties

        #region Public Methods

        public bool UpdateWithNextFrame(TObject obj, float time)
        {
            for (int i = 0; i < tracks.Count; i++)
                tracks[i].UpdateWithNextFrame(obj, time);

            return true;
        }

        public override string ToString()
        {
            return $"Clip ({Name})";
        }

        #endregion Public Methods
    }
}