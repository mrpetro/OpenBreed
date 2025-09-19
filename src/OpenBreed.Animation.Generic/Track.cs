using OpenBreed.Animation.Generic.Builders;
using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace OpenBreed.Animation.Generic
{
    internal class Track<TObject, TValue> : ITrack<TObject, TValue>
    {
        #region Private Fields

        private SortedDictionary<float, TValue> frames = new SortedDictionary<float, TValue>();
        private readonly FrameInterpolation interpolation;
        private FrameUpdater<TObject, TValue> frameUpdater;

        #endregion Private Fields

        #region Internal Constructors

        internal Track(ReadOnlyTrackBuilder<TObject, TValue> builder)
        {
            this.interpolation = builder.Interpolation;
            this.frameUpdater = builder.FrameUpdater;
            this.frames = new SortedDictionary<float, TValue>(builder.Frames);
        }

        #endregion Internal Constructors

        #region Public Methods

        public bool UpdateWithNextFrame(TObject obj, float time)
        {
            //T cf = default(T);

            //if (animator.Frame != null)
            //    cf = (T)animator.Frame;

            var nf = SampleFrame(time);

            //var update = !cf.Equals(nf);

            //if (update)
            //{
            frameUpdater.Invoke(obj, nf);
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

        private TValue SampleFrame(float time)
        {
            switch (interpolation)
            {
                case FrameInterpolation.None:
                    return SampleNoInterpolation(time);

                case FrameInterpolation.Linear:
                    return SampleLinearInterpolation(time);

                default:
                    throw new NotImplementedException($"Interpolation type '{interpolation}' not implemented.");
            }
        }

        private TValue SampleNoInterpolation(float time)
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

        private TValue SampleLinearInterpolation(float time)
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