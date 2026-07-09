using OpenBreed.Animation.Generic.Builders;
using OpenBreed.Animation.Abstractions;
using OpenBreed.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Animation.Generic
{
    internal abstract class TrackBase<TObject, TValue>
    {
        #region Private Fields

        private readonly FrameInterpolation interpolation;
        private readonly float[] times;
        private readonly TValue[] values;
        private FrameUpdater<TObject, TValue> frameUpdater;

        #endregion Private Fields

        #region Internal Constructors

        internal TrackBase(TrackBuilderBase<TObject, TValue> builder)
        {
            this.Id = builder.Id;
            this.interpolation = builder.Interpolation;
            this.frameUpdater = builder.FrameUpdater;

            this.times = new float[builder.Frames.Count];
            this.values = new TValue[builder.Frames.Count];

            var counter = 0;

            foreach (var (key, value) in builder.Frames)
            {
                times[counter] = key;
                values[counter] = value;
                counter++;
            }
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Id { get; }
        public abstract IReadOnlyDictionary<float, TValue> Frames { get; }

        #endregion Public Properties

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

        public bool TryFindInRange(float minTime, float maxTime, out IEnumerable<float> keyFrames)
        {
            keyFrames = Frames.SkipWhile(item => item.Key < minTime).TakeWhile(item => item.Key < maxTime).Select(item => item.Key);
            return keyFrames.Any();
        }

        #endregion Public Methods

        #region Protected Methods

        protected int GetKeyFrameIndex(float time)
        {
            var index = Array.BinarySearch(times, time);

            if (index < 0)
            {
                index = Math.Abs(index) - 2;
            }

            return index;
        }


        #endregion Protected Methods

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
            var valueIndex = GetKeyFrameIndex(time);

            return values[valueIndex];
        }

        private void GetFrames(float time, out KeyValuePair<float, TValue> start, out KeyValuePair<float, TValue> end)
        {
            start = Frames.First();

            foreach (var frame in Frames)
            {
                if (time <= frame.Key)
                {
                    end = frame;
                    return;
                }
                else
                    start = frame;
            }

            end = Frames.Last();
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