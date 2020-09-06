using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Animation.Helpers
{
    internal class AnimationPart<T> : IAnimationPart<T>
    {
        #region Private Fields

        private SortedDictionary<float, T> frames = new SortedDictionary<float, T>();
        private Action<Entity, T> frameUpdateAction;

        #endregion Private Fields

        #region Internal Constructors

        internal AnimationPart(Action<Entity, T> frameUpdateAction, T initialValue)
        {
            this.frameUpdateAction = frameUpdateAction;
            frames.Add(0.0f, initialValue);
        }

        #endregion Internal Constructors

        #region Public Properties

        #endregion Public Properties

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
                frameUpdateAction.Invoke(entity, nf);
            //    animator.Frame = nf;
            //}

            return true;
        }

        private T GetFrame(float time, FrameTransition transition = FrameTransition.None)
        {
            switch (transition)
            {
                case FrameTransition.None:
                    return GetFrameNoTransition(time);

                case FrameTransition.LinearInterpolation:
                    return GetFrameLinearInterpolation(time);

                default:
                    throw new NotImplementedException($"Transition '{transition}' not implemented.");
            }
        }

        public void AddFrame(T value, float frameTime)
        {
            frames.Add(frameTime, value);
        }

        #endregion Public Methods

        #region Private Methods

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

        #endregion Private Methods
    }
}
