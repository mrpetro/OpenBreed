using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Core.Components.Xml
{
    public abstract class XmlAttributeTemplate : IMetadataAttributeTemplate
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlIgnore]
        public abstract object ValueObject { get; }
    }

    public class XmlAttributeTemplate<TValue> : XmlAttributeTemplate, IMetadataAttributeTemplate<TValue>
    {
        [XmlAttribute("Value")]
        public TValue Value { get; set; }

        public override object ValueObject => Value;
    }

    [XmlRoot("Metadata")]
    public class XmlMetadataComponent : XmlComponentTemplate, IMetadataComponentTemplate
    {
        #region Public Properties

        [XmlElement("Level")]
        public string Level { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Option")]
        public string Option { get; set; }

        [XmlElement("Flavor")]
        public string Flavor { get; set; }

        [XmlIgnore]
        public IEnumerable<IMetadataAttributeTemplate> Attributes => XmlAttributes.Cast<IMetadataAttributeTemplate>();

        [XmlArray("Attributes")]
        [XmlArrayItem(ElementName = "IntAttribute", Type=typeof(XmlAttributeTemplate<int>))]
        [XmlArrayItem(ElementName = "BoolAttribute", Type = typeof(XmlAttributeTemplate<bool>))]
        public XmlAttributeTemplate[] XmlAttributes { get; set; } = Array.Empty<XmlAttributeTemplate>();

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            return new MetadataComponent(Level, Name, Option, Flavor, Attributes);
        }

        #endregion Public Methods
    }
}