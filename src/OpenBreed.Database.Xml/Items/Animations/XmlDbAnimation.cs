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
    public class XmlDbAnimation : XmlDbEntry, IDbAnimation
    {
        #region Public Properties

        [XmlElement("Length")]
        public float Length { get; set; }

        [XmlArray("Tracks")]
        [XmlArrayItem(ElementName = "IntegerTrack", Type = typeof(XmlDbAnimationTrack<int>))]
        [XmlArrayItem(ElementName = "FloatTrack", Type = typeof(XmlDbAnimationTrack<float>))]
        [XmlArrayItem(ElementName = "StringTrack", Type = typeof(XmlDbAnimationTrack<string>))]
        public List<XmlDbAnimationTrack> XmlTracks { get; set; }

        [XmlIgnore]
        public ReadOnlyCollection<IDbAnimationTrack> Tracks
        {
            get
            {
                return new ReadOnlyCollection<IDbAnimationTrack>(XmlTracks.Cast<IDbAnimationTrack>().ToList());
            }
        }

        public override IDbEntry Copy()
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }


    [Serializable]
    public class XmlDbAnimationTrack<TValue> : XmlDbAnimationTrack, IDbAnimationTrack<TValue>
    {
        #region Public Properties

        [XmlArray("Frames")]
        [XmlArrayItem(ElementName = "Frame")]
        public List<XmlDbAnimationFrame<TValue>> XmlFrames { get; set; }

        [XmlIgnore]
        public ReadOnlyCollection<IDbAnimationFrame<TValue>> Frames
        {
            get
            {
                return new ReadOnlyCollection<IDbAnimationFrame<TValue>>(XmlFrames.Cast<IDbAnimationFrame<TValue>>().ToList());
            }
        }

        public void ClearFrames()
        {
            XmlFrames.Clear();
        }

        public void AddFrame(TValue value, float frameTime)
        {
            XmlFrames.Add(new XmlDbAnimationFrame<TValue> { Value = value, Time = frameTime });
        }

        #endregion Public Properties
    }

    [Serializable]
    public abstract class XmlDbAnimationTrack : IDbAnimationTrack
    {
        #region Public Properties

        [XmlElement("Interpolation")]
        public EntryFrameInterpolation Interpolation { get; set; }

        [XmlElement("Controller")]
        public string Controller { get; set; }

        public IDbEntry Copy()
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}
