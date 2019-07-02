using OpenBreed.Core.Modules.Animation;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Systems.Animation.Components
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

        public T GetFrame(float time)
        {
            foreach (var frame in frames)
            {
                if (time <= frame.Key)
                    return frame.Value;
            }

            return frames.Last().Value;
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