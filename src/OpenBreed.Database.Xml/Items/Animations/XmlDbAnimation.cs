using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using OpenBreed.Common;
using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Database.Xml.Items.DataSources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Net.WebRequestMethods;

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
        public List<XmlDbAnimationTrack> XmlTracks { get; set; } = new List<XmlDbAnimationTrack>();

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

        public IDbAnimationTrack<TValue> GetTrack<TValue>(string id)
        {
            return XmlTracks.FirstOrDefault(item => item.Controller == id) as IDbAnimationTrack<TValue>;
        }

        public IDbAnimationTrack<TValue> AddNewTrack<TValue>(string controller)
        {
            var newTrack = new XmlDbAnimationTrack<TValue>()
            {
                Controller = controller, 
            };
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