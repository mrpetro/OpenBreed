using OpenBreed.Database.Interface.Items.Animations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Animations
{
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

        #endregion Public Properties

        #region Public Methods

        public void ClearFrames()
        {
            XmlFrames.Clear();
        }

        public void AddFrame(TValue value, float frameTime)
        {
            XmlFrames.Add(new XmlDbAnimationFrame<TValue> { Value = value, Time = frameTime });
        }

        public override IDbAnimationTrack Copy()
        {
            return new XmlDbAnimationTrack<TValue>
            {
                Interpolation = this.Interpolation,
                Controller = this.Controller,
                XmlFrames = this.XmlFrames.Select(item => item.Copy()).Cast<XmlDbAnimationFrame<TValue>>().ToList()
            };
        }

        #endregion Public Methods
    }

    [Serializable]
    public abstract class XmlDbAnimationTrack : IDbAnimationTrack
    {
        #region Public Properties

        [XmlElement("Interpolation")]
        public EntryFrameInterpolation Interpolation { get; set; }

        [XmlElement("Controller")]
        public string Controller { get; set; }

        #endregion Public Properties

        #region Public Methods

        public abstract IDbAnimationTrack Copy();

        #endregion Public Methods
    }
}