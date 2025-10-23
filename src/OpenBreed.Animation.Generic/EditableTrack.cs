using OpenBreed.Animation.Generic.Builders;
using OpenBreed.Animation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Animation.Generic
{
    internal class TrackKeyFrame<TValue> : ITrackKeyFrame<TValue>
    {
        #region Public Constructors

        public TrackKeyFrame(float key, TValue value)
        {
            Key = key;
            Value = value;
        }

        #endregion Public Constructors

        #region Public Properties

        public float Key { get; set; }
        public TValue Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        public object GetValue() => Value;

        #endregion Public Methods
    }

    internal class EditableTrack<TObject, TValue> : ReadOnlyTrack<TObject, TValue>, IEditableTrack<TObject, TValue>
    {
        #region Private Fields

        private readonly List<TrackKeyFrame<TValue>> editableFrames = new List<TrackKeyFrame<TValue>>();

        #endregion Private Fields

        #region Internal Constructors

        internal EditableTrack(EditableTrackBuilder<TObject, TValue> builder) : base(builder)
        {
            this.editableFrames = builder.Frames.Select(item => new TrackKeyFrame<TValue>(item.Key, item.Value)).ToList();
        }

        #endregion Internal Constructors

        #region Public Methods

        public bool TryAddKeyFrame(float frameTime, TValue value)
        {
            if (frameTime < 0.0f)
            {
                return false;
            }

            if (frames.ContainsKey(frameTime))
            {
                return false;
            }

            editableFrames.Add(new TrackKeyFrame<TValue>(frameTime, value));
            Rebuild();

            return true;
        }

        public bool TryGetKeyFrame(float key, out ITrackKeyFrame<TValue> keyFrame)
        {
            if (key < 0.0f)
            {
                keyFrame = null;
                return false;
            }

            keyFrame = editableFrames.FirstOrDefault(item => item.Key == key);
            return keyFrame != null;
        }

        public ITrackKeyFrame<TValue> GetKeyFrame(float key)
        {
            return editableFrames.First(item => item.Key == key);
        }

        public void RemoveFrames(IEnumerable<ITrackKeyFrame> keyFrames)
        {
            foreach (var keyFrame in keyFrames)
            {
                editableFrames.Remove((TrackKeyFrame<TValue>)keyFrame);
            }

            Rebuild();
        }

        public float Rebuild()
        {
            frames.Clear();

            var timeMin = 0.0f;
            var timeMax = float.PositiveInfinity;

            foreach (var frame in editableFrames)
            {
                frames.TryAdd(frame.Key, frame.Value);

                timeMin = Math.Max(timeMin, frame.Key);
                timeMax = Math.Min(timeMax, frame.Key);
            }

            return Math.Abs(timeMax - timeMin);
        }

        #endregion Public Methods
    }
}