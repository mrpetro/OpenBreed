using OpenBreed.Database.Interface.Items.Animations;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Animations
{
    public class XmlAnimationFrame : IAnimationFrame
    {
        #region Public Properties

        [XmlAttribute]
        public int ValueIndex { get; set; }

        [XmlAttribute]
        public float FrameTime { get; set; }

        #endregion Public Properties
    }
}