using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Animation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Animation.Helpers
{
    internal class AnimationData<T> : IAnimationData<T>
    {
        #region Private Fields

        private SortedDictionary<float, T> frames = new SortedDictionary<float, T>();

        #endregion Private Fields

        #region Internal Constructors

        internal AnimationData(int id, string name)
        {
            Id = id;
            Name = name;
        }

        #endregion Internal Constructors

        #region Public Properties

        public int Id { get; }
        public string Name { get; }

        public float Length { get { return frames.Last().Key; } }

        #endregion Public Properties

        #region Public Methods

        private T GetFrameGetFameNoTransition(float time)
        {
            foreach (var frame in frames)
            {
                if (time <= frame.Key)
                    return frame.Value;
            }

            return frames.Last().Value;
        }

        public T GetFrame(float time, FrameTransition transition = FrameTransition.None)
        {
            switch (transition)
            {
                case FrameTransition.None:
                    return GetFrameGetFameNoTransition(time);
                default:
                    throw new NotImplementedException($"Transition '{transition}' not implemented.");
            }
        }

        public void AddFrame(T value, float frameTime)
        {
            if (frames.Any())
                frames.Add(frames.Last().Key + frameTime, value);
            else
                frames.Add(frameTime, value);
        }

        #endregion Public Methods
    }
}