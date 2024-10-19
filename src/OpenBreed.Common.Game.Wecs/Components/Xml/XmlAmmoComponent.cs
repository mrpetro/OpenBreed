using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.Game.Wecs.Components.Xml
{
    [XmlRoot("Ammo")]
    public class XmlAmmoComponent : XmlComponentTemplate, IAmmoComponentTemplate
    {
        [XmlElement("MaximumRoundsCount")]
        public int MaximumRoundsCount { get; set; }

        [XmlElement("RoundsCount")]
        public int RoundsCount { get; set; }

        [XmlElement("MagazinesCount")]
        public int MagazinesCount { get; set; }
    }
}
