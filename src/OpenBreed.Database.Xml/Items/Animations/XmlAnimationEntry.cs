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
        [XmlArrayItem(ElementName = "Track")]
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
    public class XmlAnimationEntryTrack : IAnimationEntryTrack
    {
        #region Public Properties

        [XmlElement("Interpolation")]
        public EntryFrameInterpolation Interpolation { get; set; }

        [XmlElement("AnimatorType")]
        public string AnimatorType { get; set; }

        [XmlArray("AnimatorArguments")]
        [XmlArrayItem(ElementName = "Arg")]
        public List<XmlArgument> XmlAnimatorArguments { get; set; }

        [XmlArray("Frames")]
        [XmlArrayItem(ElementName = "Frame")]
        public List<XmlAnimationFrame> XmlFrames { get; set; }

        [XmlIgnore]
        public ReadOnlyCollection<IArgument> AnimatorArguments
        {
            get
            {
                return new ReadOnlyCollection<IArgument>(XmlAnimatorArguments.Cast<IArgument>().ToList());
            }
        }

        [XmlIgnore]
        public ReadOnlyCollection<IAnimationFrame> Frames
        {
            get
            {
                return new ReadOnlyCollection<IAnimationFrame>(XmlFrames.Cast<IAnimationFrame>().ToList());
            }
        }

        public IEntry Copy()
        {
            throw new NotImplementedException();
        }

        public void ClearFrames()
        {
            XmlFrames.Clear();
        }

        public void AddFrame(int valueIndex, float frameTime)
        {
            XmlFrames.Add(new XmlAnimationFrame { ValueIndex = valueIndex, Time = frameTime });
        }

        #endregion Public Properties
    }
}
