using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Animation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Animation.Helpers
{
    internal class Animation<T> : IAnimation<T>
    {
        #region Private Fields

        private SortedDictionary<float, T> frames = new SortedDictionary<float, T>();

        #endregion Private Fields

        #region Internal Constructors

        internal Animation(int id, string name)
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

        private T GetFrameNoTransition(float time)
        {
            foreach (var frame in frames)
            {
                if (time <= frame.Key)
                    return frame.Value;
            }

            return frames.Last().Value;
        }

        private void GetFrames(float time, out KeyValuePair<float, T> start, out KeyValuePair<float, T> end)
        {
            start = frames.First();

            foreach (var frame in frames)
            {
                if (time <= frame.Key)
                {
                    end = frame;
                    return;
                }
                else
                    start = frame;
            }

            end = frames.Last();
        }

        private T GetFrameLinearInterpolation(float time)
        {
            KeyValuePair<float, T> start;
            KeyValuePair<float, T> end;

            GetFrames(time, out start, out end);

            var ct = time - start.Key;
            var dt = end.Key - start.Key;

            return (T)MathTools.Lerp(start.Value, end.Value, ct / dt);
        }

        public bool TryGetNextFrame(float time, object currentFrame, out object nextFrame, FrameTransition transition = FrameTransition.None)
        {
            T cf = default(T);

            if(currentFrame != null)
                cf = (T)currentFrame;

            var nf = GetFrame(time, transition);

            nextFrame = nf;

            return !cf.Equals(nf);
        }

        public T GetFrame(float time, FrameTransition transition = FrameTransition.None)
        {
            switch (transition)
            {
                case FrameTransition.None:
                    return GetFrameNoTransition(time);
                case FrameTransition.LinearInterpolation:
                    {
                        var g = GetFrameLinearInterpolation(time);
                        Console.WriteLine(g);
                        return g;
                    }
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