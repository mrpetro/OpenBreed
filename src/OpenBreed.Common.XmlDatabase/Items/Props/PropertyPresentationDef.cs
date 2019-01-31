using OpenBreed.Common.Props;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Props
{
    public class PropertyPresentationDef : IPropertyPresentation
    {
        #region Public Properties

        [XmlElement("Color")]
        public string Color { get; set; }
        [XmlElement("Image")]
        public string Image { get; set; }
        [XmlElement("Visibility")]
        public bool Visibility { get; set; }

        #endregion Public Properties
    }
}
