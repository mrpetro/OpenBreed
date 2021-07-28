using OpenBreed.Database.Xml.Items.Assets;
using OpenBreed.Common.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;
using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Animations;
using System.Collections.ObjectModel;
using OpenBreed.Database.Interface;

namespace OpenBreed.Database.Xml.Items.Animations
{
    [Serializable]
    public class XmlAnimationEntry : XmlDbEntry, IAnimationEntry
    {
        #region Public Properties

        [XmlElement("Length")]
        public float Length { get; set; }

        [XmlArray("Tracks")]
        [XmlArrayItem(ElementName = "IntegerTrack", Type = typeof(XmlAnimationEntryTrack<int>))]
        [XmlArrayItem(ElementName = "FloatTrack", Type = typeof(XmlAnimationEntryTrack<float>))]
        [XmlArrayItem(ElementName = "StringTrack", Type = typeof(XmlAnimationEntryTrack<string>))]
        public List<XmlAnimationEntryTrack> XmlTracks { get; set; }

        [XmlIgnore]
        public ReadOnlyCollection<IAnimationEntryTrack> Tracks
        {
            get
            {
                return new ReadOnlyCollection<IAnimationEntryTrack>(XmlTracks.Cast<IAnimationEntryTrack>().ToList());
            }
        }

        public override IEntry Copy()
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }


    [Serializable]
    public class XmlAnimationEntryTrack<TValue> : XmlAnimationEntryTrack, IAnimationEntryTrack<TValue>
    {
        #region Public Properties

        [XmlArray("Frames")]
        [XmlArrayItem(ElementName = "Frame")]
        public List<XmlAnimationFrame<TValue>> XmlFrames { get; set; }

        [XmlIgnore]
        public ReadOnlyCollection<IAnimationFrame<TValue>> Frames
        {
            get
            {
                return new ReadOnlyCollection<IAnimationFrame<TValue>>(XmlFrames.Cast<IAnimationFrame<TValue>>().ToList());
            }
        }

        public void ClearFrames()
        {
            XmlFrames.Clear();
        }

        public void AddFrame(TValue value, float frameTime)
        {
            XmlFrames.Add(new XmlAnimationFrame<TValue> { Value = value, Time = frameTime });
        }

        #endregion Public Properties
    }

    [Serializable]
    public abstract class XmlAnimationEntryTrack : IAnimationEntryTrack
    {
        #region Public Properties

        [XmlElement("Interpolation")]
        public EntryFrameInterpolation Interpolation { get; set; }

        [XmlElement("Controller")]
        public string Controller { get; set; }

        public IEntry Copy()
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}
