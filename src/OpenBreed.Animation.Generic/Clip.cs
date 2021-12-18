using OpenBreed.Animation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Animation.Generic
{
    internal class Clip<TObject> : IClip<TObject>
    {
        #region Private Fields

        private readonly List<ITrack<TObject>> tracks = new List<ITrack<TObject>>();

        #endregion Private Fields

        #region Internal Constructors

        internal Clip(int id, string name, float length)
        {
            Id = id;
            Name = name;
            Length = length;
        }

        #endregion Internal Constructors

        #region Public Properties

        public int Id { get; }
        public string Name { get; }
        public float Length { get; set; }

        #endregion Public Properties

        #region Public Methods

        public ITrack<TObject, TValue> AddTrack<TValue>(FrameInterpolation interpolation, FrameUpdater<TObject, TValue> frameUpdater, TValue initialValue)
        {
            var newPart = new Track<TObject, TValue>(interpolation, frameUpdater, initialValue);
            tracks.Add(newPart);
            return newPart;
        }

        public bool UpdateWithNextFrame(TObject obj, float time)
        {
            for (int i = 0; i < tracks.Count; i++)
                tracks[i].UpdateWithNextFrame(obj, time);

            return true;
        }

        public override string ToString()
        {
            return $"{Name} ({Id})";
        }
        #endregion Public Methods

        #region Private Methods


        #endregion Private Methods
    }
}