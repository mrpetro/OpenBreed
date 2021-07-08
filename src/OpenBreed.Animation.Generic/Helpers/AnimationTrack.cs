using OpenBreed.Animation.Interface;
using OpenBreed.Core;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Animation.Generic.Helpers
{
    internal class AnimationTrack<TValue> : IAnimationTrack<TValue>
    {
        #region Private Fields

        private SortedDictionary<float, TValue> frames = new SortedDictionary<float, TValue>();
        private readonly FrameInterpolation interpolation;
        private FrameUpdater<TValue> frameUpdater;

        #endregion Private Fields

        #region Internal Constructors

        internal AnimationTrack(FrameInterpolation interpolation, FrameUpdater<TValue> frameUpdater, TValue initialValue)
        {
            this.interpolation = interpolation;
            this.frameUpdater = frameUpdater;
            this.frames.Add(0.0f, initialValue);
        }

        #endregion Internal Constructors

        #region Public Methods

        public bool UpdateWithNextFrame(Entity entity, Animator animator)
        {
            //T cf = default(T);

            //if (animator.Frame != null)
            //    cf = (T)animator.Frame;

            var nf = GetFrame(animator.Position, animator.Transition);

            //var update = !cf.Equals(nf);

            //if (update)
            //{
            frameUpdater.Invoke(entity, nf);
            //    animator.Frame = nf;
            //}

            return true;
        }

        public void AddFrame(TValue value, float frameTime)
        {
            frames.Add(frameTime, value);
        }

        #endregion Public Methods

        #region Private Methods

        private TValue GetFrame(float time, FrameInterpolation transition = FrameInterpolation.None)
        {
            switch (interpolation)
            {
                case FrameInterpolation.None:
                    return GetFrameNoTransition(time);

                case FrameInterpolation.Linear:
                    return GetFrameLinearInterpolation(time);

                default:
                    throw new NotImplementedException($"Transition '{transition}' not implemented.");
            }
        }

        private TValue GetFrameNoTransition(float time)
        {
            foreach (var frame in frames)
            {
                if (time <= frame.Key)
                    return frame.Value;
            }

            return frames.Last().Value;
        }

        private void GetFrames(float time, out KeyValuePair<float, TValue> start, out KeyValuePair<float, TValue> end)
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

        private TValue GetFrameLinearInterpolation(float time)
        {
            KeyValuePair<float, TValue> start;
            KeyValuePair<float, TValue> end;

            GetFrames(time, out start, out end);

            var ct = time - start.Key;
            var dt = end.Key - start.Key;

            return (TValue)MathTools.Lerp(start.Value, end.Value, ct / dt);
        }

        #endregion Private Methods
    }
}