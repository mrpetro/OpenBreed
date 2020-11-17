using OpenBreed.Database.Interface.Items.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Actions
{
    public class XmlActionPresentationDef : IActionPresentation
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
