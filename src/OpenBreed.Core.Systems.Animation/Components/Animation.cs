using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Systems.Animation.Components
{
    public class Animation<T>
    {
        #region Private Fields

        private SortedDictionary<float, T> frames = new SortedDictionary<float, T>();

        #endregion Private Fields

        #region Public Constructors

        public Animation()
        {
        }

        #endregion Public Constructors

        #region Public Properties

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