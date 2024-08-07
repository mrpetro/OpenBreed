﻿using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Common.Xml
{
    [XmlRoot("AngularVelocity")]
    public class XmlAngularVelocityComponent : XmlComponentTemplate, IAngularPositionTargetComponentTemplate
    {
        [XmlElement("Value")]
        public float Value { get; set; }
    }
}
