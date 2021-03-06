﻿using OpenBreed.Wecs.Components.Xml;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Common.Xml
{
    [XmlRoot("Position")]
    public class XmlPositionComponent : XmlComponentTemplate, IPositionComponentTemplate
    {
        #region Public Properties

        [XmlAttribute("X")]
        public float X { get; set; }

        [XmlAttribute("Y")]
        public float Y { get; set; }

        #endregion Public Properties
    }
}