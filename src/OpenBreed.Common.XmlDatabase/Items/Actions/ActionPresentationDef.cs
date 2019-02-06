using OpenBreed.Common.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Actions
{
    public class ActionPresentationDef : IActionPresentation
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
