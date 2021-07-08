using OpenBreed.Database.Interface.Items.Animations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Animations
{
    public class XmlAnimationFrame : IAnimationFrame
    {
        #region Public Properties

        [XmlAttribute]
        public float Time { get; set; }

        [XmlAttribute]
        public int ValueIndex { get; set; }

        #endregion Public Properties
    }
}