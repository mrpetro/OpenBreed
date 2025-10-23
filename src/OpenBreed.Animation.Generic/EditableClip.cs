using OpenBreed.Animation.Generic.Builders;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenBreed.Animation.Generic
{
    internal class EditableClip<TObject> : IEditableClip<TObject>
    {
        #region Private Fields

        private readonly List<IEditableTrack<TObject>> tracks;

        #endregion Private Fields

        #region Internal Constructors

        internal EditableClip(EditableClipBuilder<TObject> builder)
        {
            Name = builder.Name;
            Length = builder.Length;
            tracks = builder.Tracks.Select(b => ((IBuilder<IEditableTrack<TObject>>)b).Build()).ToList();
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Name { get; }
        public float Length { get; set; }

        public IReadOnlyCollection<IReadOnlyTrack<TObject>> Tracks => tracks;

        #endregion Public Properties

        #region Public Methods

        public IEditableTrack<TObject, TValue> AddTrack<TValue>(string id, FrameInterpolation interpolation, FrameUpdater<TObject, TValue> frameUpdater, TValue initialValue)
        {
            var trackBuilder = new EditableTrackBuilder<TObject, TValue>();
            trackBuilder.SetId(id);
            trackBuilder.SetInterpolation(interpolation);
            trackBuilder.SetFrameUpdater(frameUpdater);
            trackBuilder.SetInitialValue(initialValue);

            var newEditableTract = trackBuilder.Build();

            tracks.Add(newEditableTract);

            return (IEditableTrack<TObject, TValue>)newEditableTract;
        }

        public bool UpdateWithNextFrame(TObject obj, float time)
        {
            for (int i = 0; i < tracks.Count; i++)
                tracks[i].UpdateWithNextFrame(obj, time);

            return true;
        }

        public IEditableTrack<TObject> GetTrack(string id)
        {
            return tracks.FirstOrDefault(item => item.Id == id);
        }

        public bool RemoveTrack(string id)
        {
            return tracks.RemoveAll(item => item.Id == id) > 0;
        }

        public override string ToString()
        {
            return $"Editable clip ({Name})";
        }

        #endregion Public Methods
    }
}