using OpenBreed.Wecs.Components.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Animation.Xml
{
    [XmlRoot("Animation")]
    public class XmlAnimationComponent : XmlComponentTemplate, IAnimationComponentTemplate
    {
        #region Public Properties

        [XmlArray("States")]
        [XmlArrayItem(ElementName = "State")]
        public List<XmlAnimationState> XmlStates { get; set; }

        [XmlIgnore]
        public ReadOnlyCollection<IAnimationStateTemplate> States
        {
            get
            {
                return new ReadOnlyCollection<IAnimationStateTemplate>(XmlStates.Cast<IAnimationStateTemplate>().ToList());
            }
        }

        #endregion Public Properties
    }

    public class XmlAnimationState : IAnimationStateTemplate
    {
        #region Public Properties

        [XmlElement("Speed")]
        public float Speed { get; set; }

        [XmlElement("Loop")]
        public bool Loop { get; set; }

        [XmlElement("ClipName")]
        public string ClipName { get; set; }

        #endregion Public Properties
    }

}