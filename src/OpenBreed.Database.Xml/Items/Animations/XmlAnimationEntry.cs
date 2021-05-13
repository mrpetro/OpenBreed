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

namespace OpenBreed.Database.Xml.Items.Animations
{
    [Serializable]
    public class XmlAnimationEntry : XmlDbEntry, IAnimationEntry
    {
        #region Public Properties

        [XmlElement("ValueSetRef")]
        public string ValueSetRef { get; set; }


        [XmlArray("Frames")]
        [XmlArrayItem(ElementName = "Frame")]
        public List<XmlAnimationFrame> XmlFrames { get; set; }

        [XmlIgnore]
        public ReadOnlyCollection<IAnimationFrame> Frames
        {
            get
            {
                return new ReadOnlyCollection<IAnimationFrame>(XmlFrames.Cast<IAnimationFrame>().ToList());
            }
        }

        public override IEntry Copy()
        {
            throw new NotImplementedException();
        }

        public void ClearFrames()
        {
            XmlFrames.Clear();
        }

        public void AddFrame(int valueIndex, float frameTime)
        {
            XmlFrames.Add(new XmlAnimationFrame { ValueIndex = valueIndex, FrameTime = frameTime });
        }

        #endregion Public Properties
    }
}
