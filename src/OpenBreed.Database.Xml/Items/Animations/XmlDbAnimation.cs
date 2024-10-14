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
using System.Drawing;
using OpenBreed.Database.Xml.Items.DataSources;

namespace OpenBreed.Database.Xml.Items.Animations
{
    [Serializable]
    public class XmlDbAnimation : XmlDbEntry, IDbAnimation
    {
        #region Public Constructors

        public XmlDbAnimation()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbAnimation(XmlDbAnimation other) : base(other)
        {
            Length = other.Length;
            XmlTracks = other.XmlTracks.Select(item => item.Copy()).Cast<XmlDbAnimationTrack>().ToList();
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("Length")]
        public float Length { get; set; }

        [XmlArray("Tracks")]
        [XmlArrayItem(ElementName = "IntTrack", Type = typeof(XmlDbAnimationTrack<int>))]
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

        #endregion Public Properties

        #region Public Methods

        public IDbAnimationTrack<TValue> AddNewTrack<TValue>()
        {
            var newTrack = new XmlDbAnimationTrack<TValue>();
            XmlTracks.Add(newTrack);
            return newTrack;
        }

        public override IDbEntry Copy() => new XmlDbAnimation(this);

        public bool RemoveTrack(IDbAnimationTrack track)
        {
            return XmlTracks.Remove((XmlDbAnimationTrack)track);
        }

        #endregion Public Methods
    }
}