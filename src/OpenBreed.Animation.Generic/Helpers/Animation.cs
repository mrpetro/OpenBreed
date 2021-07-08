using OpenBreed.Animation.Interface;
using OpenBreed.Core;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Animation.Generic.Helpers
{
    internal class Animation : IAnimation
    {
        #region Private Fields

        private readonly List<IAnimationTrack> tracks = new List<IAnimationTrack>();

        #endregion Private Fields

        #region Internal Constructors

        internal Animation(int id, string name, float length)
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

        public IAnimationTrack<TValue> AddTrack<TValue>(FrameInterpolation interpolation, FrameUpdater<TValue> frameUpdater, TValue initialValue)
        {
            var newPart = new AnimationTrack<TValue>(interpolation, frameUpdater, initialValue);
            tracks.Add(newPart);
            return newPart;
        }

        public bool UpdateWithNextFrame(Entity entity, Animator animator)
        {
            for (int i = 0; i < tracks.Count; i++)
                tracks[i].UpdateWithNextFrame(entity, animator);

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